using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SslCertViewer.Core.Tests;

public sealed class CertificateDocumentLoaderTests
{
    [Fact]
    public void LoadFromFile_reads_a_single_pem_certificate()
    {
        using var certificate = CreateCertificate("CN=Pem Leaf");
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pem");

        try
        {
            var pem = certificate.ExportCertificatePem();
            File.WriteAllText(tempPath, pem, Encoding.ASCII);

            var document = InvokeLoadFromFile(tempPath);

            Assert.Equal("Pem", GetStringProperty(document, "DetectedFormat"));

            var certificates = GetListProperty(document, "Certificates");
            Assert.Single(certificates);

            var firstCertificate = certificates[0];
            Assert.Equal("CN=Pem Leaf", GetStringProperty(firstCertificate, "Subject"));
            Assert.Equal(certificate.Thumbprint, GetStringProperty(firstCertificate, "Thumbprint"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void LoadFromFile_reads_a_binary_cer_certificate()
    {
        using var certificate = CreateCertificate("CN=Binary Cer");
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.cer");

        try
        {
            File.WriteAllBytes(tempPath, certificate.Export(X509ContentType.Cert));

            var document = InvokeLoadFromFile(tempPath);

            Assert.Equal("Der", GetStringProperty(document, "DetectedFormat"));
            var certificates = GetListProperty(document, "Certificates");
            Assert.Single(certificates);
            Assert.Equal("CN=Binary Cer", GetStringProperty(certificates[0], "Subject"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void LoadFromFile_reads_a_password_protected_pfx_certificate()
    {
        using var certificate = CreateCertificateWithPrivateKey("CN=Pfx Leaf");
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pfx");
        const string password = "DstcDemo!234";

        try
        {
            File.WriteAllBytes(tempPath, certificate.Export(X509ContentType.Pfx, password));

            var document = InvokeLoadFromFile(tempPath, password);

            Assert.Equal("Pkcs12", GetStringProperty(document, "DetectedFormat"));
            var certificates = GetListProperty(document, "Certificates");
            Assert.Single(certificates);
            Assert.Equal("CN=Pfx Leaf", GetStringProperty(certificates[0], "Subject"));
            Assert.Equal(certificate.Issuer, GetStringProperty(certificates[0], "Issuer"));
            Assert.Equal(certificate.SerialNumber, GetStringProperty(certificates[0], "SerialNumber"));
            Assert.Equal(certificate.SignatureAlgorithm.FriendlyName, GetStringProperty(certificates[0], "SignatureAlgorithm"));
            Assert.True(GetBoolProperty(certificates[0], "HasPrivateKey"));
            var detailedText = GetStringProperty(certificates[0], "DetailedText");
            Assert.Contains("[Subject]", detailedText);
            Assert.Contains("CN=Pfx Leaf", detailedText);
            Assert.Contains("Thumbprint", detailedText);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void LoadFromFile_reads_multiple_certificates_from_a_pem_bundle()
    {
        using var firstCertificate = CreateCertificate("CN=Pem Bundle One");
        using var secondCertificate = CreateCertificate("CN=Pem Bundle Two");
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.pem");

        try
        {
            var pemBundle = string.Concat(
                firstCertificate.ExportCertificatePem(),
                secondCertificate.ExportCertificatePem());

            File.WriteAllText(tempPath, pemBundle, Encoding.ASCII);

            var document = InvokeLoadFromFile(tempPath);

            Assert.Equal("Pem", GetStringProperty(document, "DetectedFormat"));
            var certificates = GetListProperty(document, "Certificates");
            Assert.Equal(2, certificates.Count);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void LoadFromFile_reads_a_pem_certificate_even_with_a_crt_extension()
    {
        using var certificate = CreateCertificate("CN=Crt Pem");
        var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.crt");

        try
        {
            File.WriteAllText(tempPath, certificate.ExportCertificatePem(), Encoding.ASCII);

            var document = InvokeLoadFromFile(tempPath);

            Assert.Equal("Pem", GetStringProperty(document, "DetectedFormat"));
            var certificates = GetListProperty(document, "Certificates");
            Assert.Single(certificates);
            Assert.Equal("CN=Crt Pem", GetStringProperty(certificates[0], "Subject"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    private static object InvokeLoadFromFile(string path, string? password = null)
    {
        var loaderType = typeof(CertificateDocument).Assembly.GetType("SslCertViewer.Core.CertificateDocumentLoader");
        Assert.NotNull(loaderType);

        var method = loaderType.GetMethod(
            "LoadFromFile",
            BindingFlags.Public | BindingFlags.Static,
            binder: null,
            [typeof(string), typeof(string)],
            modifiers: null);

        Assert.NotNull(method);
        return method.Invoke(null, [path, password])!;
    }

    private static string GetStringProperty(object target, string propertyName)
    {
        var property = target.GetType().GetProperty(propertyName);
        Assert.NotNull(property);
        return Assert.IsType<string>(property.GetValue(target));
    }

    private static IReadOnlyList<object> GetListProperty(object target, string propertyName)
    {
        var property = target.GetType().GetProperty(propertyName);
        Assert.NotNull(property);
        return Assert.IsAssignableFrom<IReadOnlyList<object>>(property.GetValue(target));
    }

    private static bool GetBoolProperty(object target, string propertyName)
    {
        var property = target.GetType().GetProperty(propertyName);
        Assert.NotNull(property);
        return Assert.IsType<bool>(property.GetValue(target));
    }

    private static X509Certificate2 CreateCertificate(string subjectName)
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest(
            new X500DistinguishedName(subjectName),
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false));
        request.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

        var cert = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        return X509CertificateLoader.LoadCertificate(cert.Export(X509ContentType.Cert));
    }

    private static X509Certificate2 CreateCertificateWithPrivateKey(string subjectName)
    {
        using var rsa = RSA.Create(2048);
        var request = new CertificateRequest(
            new X500DistinguishedName(subjectName),
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        request.CertificateExtensions.Add(
            new X509BasicConstraintsExtension(false, false, 0, false));
        request.CertificateExtensions.Add(
            new X509SubjectKeyIdentifierExtension(request.PublicKey, false));

        return request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
    }
}
