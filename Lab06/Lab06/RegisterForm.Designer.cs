namespace Lab06;

partial class RegisterForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblLogin;
    private TextBox txtLogin;
    private Label lblPassword;
    private TextBox txtPassword;
    private Label lblPasswordConfirm;
    private TextBox txtPasswordConfirm;
    private Label lblFullName;
    private TextBox txtFullName;
    private Label lblPosition;
    private TextBox txtPosition;
    private Label lblRole;
    private ComboBox cmbRole;
    private Button btnRegister;
    private ListBox lstUsers;
    private Label lblUsers;
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
        lblPasswordConfirm = new Label();
        txtPasswordConfirm = new TextBox();
        lblFullName = new Label();
        txtFullName = new TextBox();
        lblPosition = new Label();
        txtPosition = new TextBox();
        lblRole = new Label();
        cmbRole = new ComboBox();
        btnRegister = new Button();
        lstUsers = new ListBox();
        lblUsers = new Label();
        btnClose = new Button();
        SuspendLayout();

        Text = "Реєстрація користувачів";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(640, 420);

        lblLogin.Text = "Ідентифікатор:";
        lblLogin.Location = new Point(20, 20);
        lblLogin.AutoSize = true;
        txtLogin.Location = new Point(160, 16);
        txtLogin.Size = new Size(200, 27);

        lblPassword.Text = "Пароль:";
        lblPassword.Location = new Point(20, 56);
        lblPassword.AutoSize = true;
        txtPassword.Location = new Point(160, 52);
        txtPassword.Size = new Size(200, 27);
        txtPassword.PasswordChar = '*';

        lblPasswordConfirm.Text = "Підтвердження:";
        lblPasswordConfirm.Location = new Point(20, 92);
        lblPasswordConfirm.AutoSize = true;
        txtPasswordConfirm.Location = new Point(160, 88);
        txtPasswordConfirm.Size = new Size(200, 27);
        txtPasswordConfirm.PasswordChar = '*';

        lblFullName.Text = "ПІБ:";
        lblFullName.Location = new Point(20, 128);
        lblFullName.AutoSize = true;
        txtFullName.Location = new Point(160, 124);
        txtFullName.Size = new Size(280, 27);

        lblPosition.Text = "Посада:";
        lblPosition.Location = new Point(20, 164);
        lblPosition.AutoSize = true;
        txtPosition.Location = new Point(160, 160);
        txtPosition.Size = new Size(280, 27);

        lblRole.Text = "Роль (Sm):";
        lblRole.Location = new Point(20, 200);
        lblRole.AutoSize = true;
        cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbRole.Location = new Point(160, 196);
        cmbRole.Size = new Size(280, 28);

        btnRegister.Text = "Зареєструвати";
        btnRegister.Location = new Point(160, 236);
        btnRegister.Size = new Size(140, 36);
        btnRegister.Click += btnRegister_Click;

        btnClose.Text = "Закрити";
        btnClose.Location = new Point(310, 236);
        btnClose.Size = new Size(100, 36);
        btnClose.Click += (_, _) => Close();

        lblUsers.Text = "Зареєстровані користувачі:";
        lblUsers.Location = new Point(20, 284);
        lblUsers.AutoSize = true;
        lstUsers.Font = new Font("Consolas", 9F);
        lstUsers.Location = new Point(20, 308);
        lstUsers.Size = new Size(600, 96);

        Controls.AddRange(
        [
            lblLogin, txtLogin, lblPassword, txtPassword,
            lblPasswordConfirm, txtPasswordConfirm,
            lblFullName, txtFullName, lblPosition, txtPosition,
            lblRole, cmbRole, btnRegister, btnClose, lblUsers, lstUsers
        ]);

        ResumeLayout(false);
        PerformLayout();
    }
}
