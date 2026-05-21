#nullable enable

namespace SslCertViewer.App;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private Button openButton = null!;
    private Label headingLabel = null!;
    private Label subheadingLabel = null!;
    private Label filePathLabel = null!;
    private Label filePathValueLabel = null!;
    private Label formatLabel = null!;
    private Label formatValueLabel = null!;
    private Label countLabel = null!;
    private Label countValueLabel = null!;
    private Label openHintLabel = null!;
    private ListBox certificateListBox = null!;
    private Label statusLabel = null!;
    private TableLayoutPanel detailsTable = null!;
    private Label subjectNameLabel = null!;
    private Label subjectValueLabel = null!;
    private Label issuerNameLabel = null!;
    private Label issuerValueLabel = null!;
    private Label serialNameLabel = null!;
    private Label serialValueLabel = null!;
    private Label thumbprintNameLabel = null!;
    private Label thumbprintValueLabel = null!;
    private Label signatureNameLabel = null!;
    private Label signatureValueLabel = null!;
    private Label privateKeyNameLabel = null!;
    private Label privateKeyValueLabel = null!;
    private Label notBeforeNameLabel = null!;
    private Label notBeforeValueLabel = null!;
    private Label notAfterNameLabel = null!;
    private Label notAfterValueLabel = null!;
    private TextBox detailsTextBox = null!;
    private SplitContainer splitContainer = null!;
    private Panel topPanel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        openButton = new Button();
        headingLabel = new Label();
        subheadingLabel = new Label();
        filePathLabel = new Label();
        filePathValueLabel = new Label();
        formatLabel = new Label();
        formatValueLabel = new Label();
        countLabel = new Label();
        countValueLabel = new Label();
        openHintLabel = new Label();
        certificateListBox = new ListBox();
        statusLabel = new Label();
        detailsTable = new TableLayoutPanel();
        subjectNameLabel = new Label();
        subjectValueLabel = new Label();
        issuerNameLabel = new Label();
        issuerValueLabel = new Label();
        serialNameLabel = new Label();
        serialValueLabel = new Label();
        thumbprintNameLabel = new Label();
        thumbprintValueLabel = new Label();
        signatureNameLabel = new Label();
        signatureValueLabel = new Label();
        privateKeyNameLabel = new Label();
        privateKeyValueLabel = new Label();
        notBeforeNameLabel = new Label();
        notBeforeValueLabel = new Label();
        notAfterNameLabel = new Label();
        notAfterValueLabel = new Label();
        detailsTextBox = new TextBox();
        splitContainer = new SplitContainer();
        topPanel = new Panel();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        topPanel.SuspendLayout();
        detailsTable.SuspendLayout();
        SuspendLayout();
        // 
        // topPanel
        // 
        topPanel.Controls.Add(openButton);
        topPanel.Controls.Add(headingLabel);
        topPanel.Controls.Add(subheadingLabel);
        topPanel.Controls.Add(filePathLabel);
        topPanel.Controls.Add(filePathValueLabel);
        topPanel.Controls.Add(formatLabel);
        topPanel.Controls.Add(formatValueLabel);
        topPanel.Controls.Add(countLabel);
        topPanel.Controls.Add(countValueLabel);
        topPanel.Controls.Add(openHintLabel);
        topPanel.Dock = DockStyle.Top;
        topPanel.Padding = new Padding(16);
        topPanel.Height = 170;
        // 
        // openButton
        // 
        openButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        openButton.Location = new Point(20, 84);
        openButton.Size = new Size(170, 36);
        openButton.Text = "Open certificate file...";
        openButton.UseVisualStyleBackColor = true;
        // 
        // openHintLabel
        // 
        openHintLabel.AutoSize = true;
        openHintLabel.Location = new Point(204, 93);
        openHintLabel.Text = "or drag and drop a certificate file anywhere into this window";
        // 
        // headingLabel
        // 
        headingLabel.AutoSize = true;
        headingLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        headingLabel.Location = new Point(16, 14);
        headingLabel.Text = "SSL Cert Viewer";
        // 
        // subheadingLabel
        // 
        subheadingLabel.AutoSize = true;
        subheadingLabel.Location = new Point(20, 55);
        subheadingLabel.Text = "Open PEM, CRT, CER, DER, PFX, or P12 files to inspect their certificate details.";
        // 
        // filePathLabel
        // 
        filePathLabel.AutoSize = true;
        filePathLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        filePathLabel.Location = new Point(20, 133);
        filePathLabel.Text = "File";
        // 
        // filePathValueLabel
        // 
        filePathValueLabel.AutoEllipsis = true;
        filePathValueLabel.Location = new Point(68, 133);
        filePathValueLabel.Size = new Size(560, 20);
        filePathValueLabel.Text = "-";
        // 
        // formatLabel
        // 
        formatLabel.AutoSize = true;
        formatLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        formatLabel.Location = new Point(650, 133);
        formatLabel.Text = "Format";
        // 
        // formatValueLabel
        // 
        formatValueLabel.AutoSize = true;
        formatValueLabel.Location = new Point(706, 133);
        formatValueLabel.Text = "-";
        // 
        // countLabel
        // 
        countLabel.AutoSize = true;
        countLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        countLabel.Location = new Point(790, 133);
        countLabel.Text = "Certificates";
        // 
        // countValueLabel
        // 
        countValueLabel.AutoSize = true;
        countValueLabel.Location = new Point(873, 133);
        countValueLabel.Text = "0";
        // 
        // splitContainer
        // 
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Location = new Point(0, 170);
        // 
        // splitContainer.Panel1
        // 
        splitContainer.Panel1.Controls.Add(certificateListBox);
        // 
        // splitContainer.Panel2
        // 
        splitContainer.Panel2.Controls.Add(detailsTextBox);
        splitContainer.Panel2.Controls.Add(detailsTable);
        // 
        // certificateListBox
        // 
        certificateListBox.Dock = DockStyle.Fill;
        certificateListBox.IntegralHeight = false;
        // 
        // detailsTable
        // 
        detailsTable.ColumnCount = 2;
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        detailsTable.Controls.Add(subjectNameLabel, 0, 0);
        detailsTable.Controls.Add(subjectValueLabel, 1, 0);
        detailsTable.Controls.Add(issuerNameLabel, 0, 1);
        detailsTable.Controls.Add(issuerValueLabel, 1, 1);
        detailsTable.Controls.Add(serialNameLabel, 0, 2);
        detailsTable.Controls.Add(serialValueLabel, 1, 2);
        detailsTable.Controls.Add(thumbprintNameLabel, 0, 3);
        detailsTable.Controls.Add(thumbprintValueLabel, 1, 3);
        detailsTable.Controls.Add(signatureNameLabel, 0, 4);
        detailsTable.Controls.Add(signatureValueLabel, 1, 4);
        detailsTable.Controls.Add(privateKeyNameLabel, 0, 5);
        detailsTable.Controls.Add(privateKeyValueLabel, 1, 5);
        detailsTable.Controls.Add(notBeforeNameLabel, 0, 6);
        detailsTable.Controls.Add(notBeforeValueLabel, 1, 6);
        detailsTable.Controls.Add(notAfterNameLabel, 0, 7);
        detailsTable.Controls.Add(notAfterValueLabel, 1, 7);
        detailsTable.Dock = DockStyle.Top;
        detailsTable.Padding = new Padding(16);
        detailsTable.RowCount = 8;
        detailsTable.AutoSize = true;
        detailsTable.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        for (var i = 0; i < 8; i++)
        {
            detailsTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }
        // 
        // detailsTextBox
        // 
        detailsTextBox.Dock = DockStyle.Fill;
        detailsTextBox.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
        detailsTextBox.Multiline = true;
        detailsTextBox.ReadOnly = true;
        detailsTextBox.ScrollBars = ScrollBars.Both;
        detailsTextBox.WordWrap = false;
        // 
        // detail labels
        // 
        ConfigureNameLabel(subjectNameLabel, "Subject");
        ConfigureValueLabel(subjectValueLabel);
        ConfigureNameLabel(issuerNameLabel, "Issuer");
        ConfigureValueLabel(issuerValueLabel);
        ConfigureNameLabel(serialNameLabel, "Serial number");
        ConfigureValueLabel(serialValueLabel);
        ConfigureNameLabel(thumbprintNameLabel, "Thumbprint");
        ConfigureValueLabel(thumbprintValueLabel);
        ConfigureNameLabel(signatureNameLabel, "Signature");
        ConfigureValueLabel(signatureValueLabel);
        ConfigureNameLabel(privateKeyNameLabel, "Private key");
        ConfigureValueLabel(privateKeyValueLabel);
        ConfigureNameLabel(notBeforeNameLabel, "Valid from");
        ConfigureValueLabel(notBeforeValueLabel);
        ConfigureNameLabel(notAfterNameLabel, "Valid to");
        ConfigureValueLabel(notAfterValueLabel);
        // 
        // statusLabel
        // 
        statusLabel.AutoSize = false;
        statusLabel.Dock = DockStyle.Bottom;
        statusLabel.Height = 34;
        statusLabel.Padding = new Padding(16, 8, 16, 8);
        statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        statusLabel.Text = "Drop a certificate file here or click Open file.";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(960, 640);
        Controls.Add(splitContainer);
        Controls.Add(statusLabel);
        Controls.Add(topPanel);
        MinimumSize = new Size(840, 520);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SSL Cert Viewer";
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        topPanel.ResumeLayout(false);
        topPanel.PerformLayout();
        detailsTable.ResumeLayout(false);
        detailsTable.PerformLayout();
        ResumeLayout(false);
    }

    private static void ConfigureNameLabel(Label label, string text)
    {
        label.AutoSize = true;
        label.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        label.Margin = new Padding(3, 8, 3, 8);
        label.Text = text;
    }

    private static void ConfigureValueLabel(Label label)
    {
        label.AutoSize = true;
        label.MaximumSize = new Size(420, 0);
        label.Margin = new Padding(3, 8, 3, 8);
        label.Text = "-";
    }
}
