namespace Lab06;

partial class AccessCheckForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblLogin;
    private TextBox txtLogin;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnAuthenticate;
    private Label lblUserInfo;
    private Label lblObject;
    private ComboBox cmbObject;
    private Label lblOperation;
    private ComboBox cmbOperation;
    private Button btnCheckAccess;
    private Label lblResult;
    private Button btnClose;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblLogin = new Label();
        txtLogin = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        btnAuthenticate = new Button();
        lblUserInfo = new Label();
        lblObject = new Label();
        cmbObject = new ComboBox();
        lblOperation = new Label();
        cmbOperation = new ComboBox();
        btnCheckAccess = new Button();
        lblResult = new Label();
        btnClose = new Button();
        SuspendLayout();

        Text = "Перевірка прав доступу";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(560, 360);

        lblLogin.Text = "Ідентифікатор:";
        lblLogin.Location = new Point(20, 20);
        lblLogin.AutoSize = true;
        txtLogin.Location = new Point(150, 16);
        txtLogin.Size = new Size(180, 27);

        lblPassword.Text = "Пароль:";
        lblPassword.Location = new Point(20, 56);
        lblPassword.AutoSize = true;
        txtPassword.Location = new Point(150, 52);
        txtPassword.Size = new Size(180, 27);
        txtPassword.PasswordChar = '*';

        btnAuthenticate.Text = "Аутентифікація";
        btnAuthenticate.Location = new Point(350, 36);
        btnAuthenticate.Size = new Size(180, 36);
        btnAuthenticate.Click += btnAuthenticate_Click;

        lblUserInfo.Location = new Point(20, 96);
        lblUserInfo.Size = new Size(520, 40);
        lblUserInfo.Text = "Користувач не автентифікований.";

        lblObject.Text = "Об'єкт (On):";
        lblObject.Location = new Point(20, 148);
        lblObject.AutoSize = true;
        cmbObject.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbObject.Location = new Point(150, 144);
        cmbObject.Size = new Size(380, 28);

        lblOperation.Text = "Операція (Ri):";
        lblOperation.Location = new Point(20, 188);
        lblOperation.AutoSize = true;
        cmbOperation.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOperation.Location = new Point(150, 184);
        cmbOperation.Size = new Size(220, 28);

        btnCheckAccess.Text = "Перевірити доступ";
        btnCheckAccess.Location = new Point(150, 228);
        btnCheckAccess.Size = new Size(180, 36);
        btnCheckAccess.Click += btnCheckAccess_Click;

        lblResult.Location = new Point(20, 276);
        lblResult.Size = new Size(520, 48);
        lblResult.Text = "Результат перевірки з'явиться тут.";

        btnClose.Text = "Закрити";
        btnClose.Location = new Point(430, 228);
        btnClose.Size = new Size(100, 36);
        btnClose.Click += (_, _) => Close();

        Controls.AddRange(
        [
            lblLogin, txtLogin, lblPassword, txtPassword, btnAuthenticate,
            lblUserInfo, lblObject, cmbObject, lblOperation, cmbOperation,
            btnCheckAccess, lblResult, btnClose
        ]);

        ResumeLayout(false);
        PerformLayout();
    }
}
