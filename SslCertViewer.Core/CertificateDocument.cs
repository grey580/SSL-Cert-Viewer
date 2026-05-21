namespace SslCertViewer.Core;

public sealed record CertificateDocument(
    string FilePath,
    string DetectedFormat,
    IReadOnlyList<CertificateInfo> Certificates);
