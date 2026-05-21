using System.Security.Cryptography;
using SslCertViewer.Core;

namespace SslCertViewer.App;

public partial class Form1 : Form
{
    private static readonly Color AppBackground = Color.FromArgb(10, 14, 24);
    private static readonly Color SurfaceBackground = Color.FromArgb(18, 24, 39);
    private static readonly Color ElevatedBackground = Color.FromArgb(24, 31, 49);
    private static readonly Color AccentColor = Color.FromArgb(86, 156, 214);
    private static readonly Color AccentSoft = Color.FromArgb(50, 76, 112);
    private static readonly Color ForegroundPrimary = Color.FromArgb(235, 241, 255);
    private static readonly Color ForegroundMuted = Color.FromArgb(146, 158, 184);
    private readonly PictureBox logoPictureBox = new();
    private CertificateDocument? currentDocument;

    public Form1()
    {
        InitializeComponent();
        Shown += Form1_Shown;
        certificateListBox.DisplayMember = nameof(CertificateInfo.Subject);
        certificateListBox.SelectedIndexChanged += CertificateListBox_SelectedIndexChanged;
        openButton.Click += OpenButton_Click;
        certificateListBox.DrawMode = DrawMode.OwnerDrawFixed;
        certificateListBox.DrawItem += CertificateListBox_DrawItem;
        AllowDrop = true;
        DragEnter += Form1_DragEnter;
        DragDrop += Form1_DragDrop;
        ApplyTheme();
        AddBranding();
        AddPanelHeaders();
        ShowCertificateDetails(null);
    }

    private void Form1_Shown(object? sender, EventArgs e)
    {
        var desiredDistance = Math.Max(220, splitContainer.Width / 4);
        var maxDistance = Math.Max(120, splitContainer.Width - 180);
        splitContainer.SplitterDistance = Math.Min(desiredDistance, maxDistance);
    }

    private void OpenButton_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Open certificate file",
            Filter = "Certificate files|*.pem;*.crt;*.cer;*.der;*.pfx;*.p12|All files|*.*"
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            LoadDocument(dialog.FileName);
        }
    }

    private void Form1_DragEnter(object? sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
            ? DragDropEffects.Copy
            : DragDropEffects.None;
    }

    private void Form1_DragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] files && files.Length > 0)
        {
            LoadDocument(files[0]);
        }
    }

    private void LoadDocument(string filePath)
    {
        try
        {
            currentDocument = TryLoadDocument(filePath);
            filePathValueLabel.Text = currentDocument.FilePath;
            formatValueLabel.Text = currentDocument.DetectedFormat;
            countValueLabel.Text = currentDocument.Certificates.Count.ToString();
            certificateListBox.DataSource = currentDocument.Certificates.ToList();
            statusLabel.Text = "Certificate file loaded.";

            if (currentDocument.Certificates.Count > 0)
            {
                certificateListBox.SelectedIndex = 0;
            }
            else
            {
                ShowCertificateDetails(null);
            }
        }
        catch (Exception ex) when (ex is NotSupportedException or CryptographicException or InvalidOperationException)
        {
            currentDocument = null;
            certificateListBox.DataSource = null;
            filePathValueLabel.Text = filePath;
            formatValueLabel.Text = "-";
            countValueLabel.Text = "0";
            ShowCertificateDetails(null);
            statusLabel.Text = ex.Message;
            MessageBox.Show(this, ex.Message, "Unable to load certificate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private CertificateDocument TryLoadDocument(string filePath)
    {
        if (!RequiresPassword(filePath))
        {
            return CertificateDocumentLoader.LoadFromFile(filePath, null);
        }

        while (true)
        {
            using var passwordDialog = new PasswordPromptForm();
            if (passwordDialog.ShowDialog(this) != DialogResult.OK)
            {
                throw new InvalidOperationException("Loading the certificate was cancelled.");
            }

            try
            {
                return CertificateDocumentLoader.LoadFromFile(filePath, passwordDialog.Password);
            }
            catch (CryptographicException)
            {
                MessageBox.Show(this, "The password was not accepted. Try again.", "Incorrect password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    private static bool RequiresPassword(string filePath)
    {
        var extension = Path.GetExtension(filePath);
        return extension.Equals(".pfx", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".p12", StringComparison.OrdinalIgnoreCase);
    }

    private void CertificateListBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        ShowCertificateDetails(certificateListBox.SelectedItem as CertificateInfo);
    }

    private void CertificateListBox_DrawItem(object? sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || e.Index >= certificateListBox.Items.Count)
        {
            return;
        }

        var certificate = (CertificateInfo)certificateListBox.Items[e.Index];
        var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        var background = isSelected ? AccentSoft : SurfaceBackground;
        var foreground = isSelected ? ForegroundPrimary : Color.FromArgb(210, 222, 247);

        using var backgroundBrush = new SolidBrush(background);
        using var foregroundBrush = new SolidBrush(foreground);
        using var captionFont = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        using var metaFont = new Font("Cascadia Code", 8.5F, FontStyle.Regular, GraphicsUnit.Point);

        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
        e.Graphics.DrawString(certificate.Subject, captionFont, foregroundBrush, e.Bounds.X + 12, e.Bounds.Y + 8);
        e.Graphics.DrawString(certificate.Thumbprint, metaFont, new SolidBrush(ForegroundMuted), e.Bounds.X + 12, e.Bounds.Y + 30);
        e.DrawFocusRectangle();
    }

    private void ShowCertificateDetails(CertificateInfo? certificate)
    {
        subjectValueLabel.Text = certificate?.Subject ?? "-";
        issuerValueLabel.Text = certificate?.Issuer ?? "-";
        serialValueLabel.Text = certificate?.SerialNumber ?? "-";
        thumbprintValueLabel.Text = certificate?.Thumbprint ?? "-";
        signatureValueLabel.Text = certificate?.SignatureAlgorithm ?? "-";
        privateKeyValueLabel.Text = certificate is null ? "-" : (certificate.HasPrivateKey ? "Yes" : "No");
        notBeforeValueLabel.Text = certificate?.NotBefore.ToString("yyyy-MM-dd HH:mm:ss") ?? "-";
        notAfterValueLabel.Text = certificate?.NotAfter.ToString("yyyy-MM-dd HH:mm:ss") ?? "-";
        detailsTextBox.Text = certificate?.DetailedText ?? "Select a certificate on the left to inspect its full contents.";
    }

    private void ApplyTheme()
    {
        BackColor = AppBackground;
        ForeColor = ForegroundPrimary;

        topPanel.BackColor = ElevatedBackground;
        splitContainer.BackColor = AppBackground;
        splitContainer.Panel1.BackColor = SurfaceBackground;
        splitContainer.Panel2.BackColor = SurfaceBackground;
        splitContainer.SplitterWidth = 1;

        headingLabel.ForeColor = ForegroundPrimary;
        headingLabel.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
        subheadingLabel.ForeColor = ForegroundMuted;
        openHintLabel.ForeColor = ForegroundMuted;

        openButton.BackColor = AccentColor;
        openButton.ForeColor = Color.FromArgb(7, 11, 19);
        openButton.FlatStyle = FlatStyle.Flat;
        openButton.FlatAppearance.BorderSize = 0;
        openButton.Cursor = Cursors.Hand;

        filePathLabel.ForeColor = ForegroundMuted;
        formatLabel.ForeColor = ForegroundMuted;
        countLabel.ForeColor = ForegroundMuted;
        filePathValueLabel.ForeColor = ForegroundPrimary;
        formatValueLabel.ForeColor = ForegroundPrimary;
        countValueLabel.ForeColor = ForegroundPrimary;

        certificateListBox.BackColor = SurfaceBackground;
        certificateListBox.ForeColor = ForegroundPrimary;
        certificateListBox.BorderStyle = BorderStyle.None;
        certificateListBox.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        certificateListBox.ItemHeight = 54;
        certificateListBox.IntegralHeight = false;

        detailsTable.BackColor = SurfaceBackground;
        foreach (var control in detailsTable.Controls.OfType<Label>())
        {
            control.BackColor = SurfaceBackground;
        }

        foreach (var valueLabel in new[]
                 {
                     subjectValueLabel, issuerValueLabel, serialValueLabel, thumbprintValueLabel,
                     signatureValueLabel, privateKeyValueLabel, notBeforeValueLabel, notAfterValueLabel
                 })
        {
            valueLabel.ForeColor = ForegroundPrimary;
            valueLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        }

        foreach (var nameLabel in new[]
                 {
                     subjectNameLabel, issuerNameLabel, serialNameLabel, thumbprintNameLabel,
                     signatureNameLabel, privateKeyNameLabel, notBeforeNameLabel, notAfterNameLabel
                 })
        {
            nameLabel.ForeColor = ForegroundMuted;
        }

        detailsTextBox.BackColor = AppBackground;
        detailsTextBox.ForeColor = Color.FromArgb(221, 231, 252);
        detailsTextBox.BorderStyle = BorderStyle.None;
        detailsTextBox.Margin = new Padding(16, 0, 16, 16);

        statusLabel.BackColor = ElevatedBackground;
        statusLabel.ForeColor = ForegroundMuted;
    }

    private void AddBranding()
    {
        var assetsPath = Path.Combine(AppContext.BaseDirectory, "Assets");
        var logoPath = Path.Combine(assetsPath, "SslCertViewerLogo.png");
        var iconPath = Path.Combine(assetsPath, "SslCertViewer.ico");

        if (File.Exists(iconPath))
        {
            Icon = new Icon(iconPath);
        }

        if (File.Exists(logoPath))
        {
            logoPictureBox.Image = Image.FromFile(logoPath);
        }

        logoPictureBox.Location = new Point(20, 18);
        logoPictureBox.Size = new Size(58, 58);
        logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        logoPictureBox.BackColor = Color.Transparent;
        topPanel.Controls.Add(logoPictureBox);
        logoPictureBox.BringToFront();

        headingLabel.Location = new Point(96, 14);
        subheadingLabel.Location = new Point(100, 55);
        openButton.Location = new Point(100, 84);
        openHintLabel.Location = new Point(284, 93);
        filePathLabel.Location = new Point(100, 133);
        filePathValueLabel.Location = new Point(148, 133);
    }

    private void AddPanelHeaders()
    {
        splitContainer.Panel1.Controls.Add(BuildHeaderPanel("Loaded certificates", "Select a certificate to inspect it"));
        splitContainer.Panel2.Controls.Add(BuildHeaderPanel("Certificate inspector", "Summary fields above, full raw details below"));
    }

    private Panel BuildHeaderPanel(string title, string subtitle)
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 56,
            Padding = new Padding(14, 12, 14, 8),
            BackColor = ElevatedBackground
        };

        var titleLabel = new Label
        {
            AutoSize = true,
            ForeColor = ForegroundPrimary,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point),
            Text = title
        };

        var subtitleLabel = new Label
        {
            AutoSize = true,
            ForeColor = ForegroundMuted,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point),
            Location = new Point(0, 24),
            Text = subtitle
        };

        panel.Controls.Add(titleLabel);
        panel.Controls.Add(subtitleLabel);
        return panel;
    }
}
