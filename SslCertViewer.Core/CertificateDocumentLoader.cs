using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace SslCertViewer.Core;

public static class CertificateDocumentLoader
{
    public static CertificateDocument LoadFromFile(string path, string? password)
    {
        var format = DetectFormat(path);
        return format switch
        {
            "Pem" => BuildDocument(path, "Pem", LoadPemCertificates(path)),
            "Pkcs12" => BuildDocument(path, "Pkcs12", LoadPkcs12Certificates(path, password)),
            "Der" => BuildDocument(path, "Der", [X509CertificateLoader.LoadCertificateFromFile(path)]),
            _ => throw new NotSupportedException($"The file '{path}' is not a supported certificate format.")
        };
    }

    private static string DetectFormat(string path)
    {
        var bytes = File.ReadAllBytes(path);
        if (LooksLikePem(bytes))
        {
            return "Pem";
        }

        var contentType = X509Certificate2.GetCertContentType(bytes);
        return contentType switch
        {
            X509ContentType.Pkcs12 => "Pkcs12",
            X509ContentType.Cert => "Der",
            _ => throw new NotSupportedException($"The file '{path}' is not a supported certificate format.")
        };
    }

    private static X509Certificate2Collection LoadPemCertificates(string path)
    {
        var pemText = File.ReadAllText(path, Encoding.ASCII);
        var certificates = new X509Certificate2Collection();

        foreach (Match match in Regex.Matches(
                     pemText,
                     "-----BEGIN CERTIFICATE-----(?<content>.*?)-----END CERTIFICATE-----",
                     RegexOptions.Singleline | RegexOptions.CultureInvariant))
        {
            var base64 = Regex.Replace(match.Groups["content"].Value, "\\s+", string.Empty);
            var rawBytes = Convert.FromBase64String(base64);
            certificates.Add(X509CertificateLoader.LoadCertificate(rawBytes));
        }

        return certificates;
    }

    private static X509Certificate2Collection LoadPkcs12Certificates(string path, string? password)
    {
        return X509CertificateLoader.LoadPkcs12CollectionFromFile(
            path,
            password,
            X509KeyStorageFlags.EphemeralKeySet);
    }

    private static CertificateDocument BuildDocument(string path, string format, IEnumerable<X509Certificate2> certificates)
    {
        return new CertificateDocument(
            path,
            format,
            certificates
                .Select(certificate => new CertificateInfo(
                    certificate.Subject,
                    certificate.Issuer,
                    certificate.SerialNumber,
                    certificate.Thumbprint,
                    certificate.SignatureAlgorithm.FriendlyName ?? certificate.SignatureAlgorithm.Value ?? "Unknown",
                    certificate.HasPrivateKey,
                    certificate.NotBefore,
                    certificate.NotAfter,
                    certificate.ToString(true)))
                .ToArray());
    }

    private static bool LooksLikePem(byte[] bytes)
    {
        var preamble = Encoding.ASCII.GetString(bytes, 0, Math.Min(bytes.Length, 32));
        return preamble.StartsWith("-----BEGIN", StringComparison.Ordinal);
    }
}
