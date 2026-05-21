namespace SslCertViewer.Core;

public sealed record CertificateInfo(
    string Subject,
    string Issuer,
    string SerialNumber,
    string Thumbprint,
    string SignatureAlgorithm,
    bool HasPrivateKey,
    DateTime NotBefore,
    DateTime NotAfter,
    string DetailedText);
