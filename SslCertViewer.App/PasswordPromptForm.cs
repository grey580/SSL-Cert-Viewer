namespace SslCertViewer.App;

internal sealed class PasswordPromptForm : Form
{
    private readonly TextBox passwordTextBox;

    public PasswordPromptForm()
    {
        Text = "Certificate password";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(360, 150);

        var instructionLabel = new Label
        {
            AutoSize = true,
            Location = new Point(16, 16),
            Text = "Enter the password for this PFX/P12 file."
        };

        passwordTextBox = new TextBox
        {
            Location = new Point(16, 48),
            Size = new Size(320, 23),
            UseSystemPasswordChar = true
        };

        var okButton = new Button
        {
            DialogResult = DialogResult.OK,
            Location = new Point(180, 96),
            Size = new Size(75, 28),
            Text = "OK"
        };

        var cancelButton = new Button
        {
            DialogResult = DialogResult.Cancel,
            Location = new Point(261, 96),
            Size = new Size(75, 28),
            Text = "Cancel"
        };

        Controls.Add(instructionLabel);
        Controls.Add(passwordTextBox);
        Controls.Add(okButton);
        Controls.Add(cancelButton);

        AcceptButton = okButton;
        CancelButton = cancelButton;
    }

    public string Password => passwordTextBox.Text;
}
