# SSL-Cert-Viewer

SSL Cert Viewer is a small Windows desktop utility for opening certificate files and inspecting their contents quickly.

## Supported file types

- `.pem`
- `.crt`
- `.cer`
- `.der`
- `.pfx`
- `.p12`

## What it does

- opens certificate files from disk
- supports drag and drop
- prompts for passwords on protected `PFX` and `P12` files
- lists every certificate found in the file
- shows summary certificate fields
- shows the full certificate detail dump for the selected certificate

## Solution layout

- `SslCertViewer.App` - WinForms desktop application
- `SslCertViewer.Core` - certificate parsing and loading logic
- `SslCertViewer.Core.Tests` - automated tests for supported certificate formats

## Run locally

```powershell
dotnet build .\SslCertViewer.slnx
.\SslCertViewer.App\bin\Debug\net10.0-windows\SslCertViewer.App.exe
```

## Published executable

The repository now includes a published Windows build here:

```text
dist/win-x64/SslCertViewer.App.exe
```

This published `.exe` is **framework-dependent**, so the target machine needs the **.NET 10 Windows Desktop Runtime (x64)** installed.

## Test

```powershell
dotnet test .\SslCertViewer.Core.Tests\SslCertViewer.Core.Tests.csproj --nologo
```
