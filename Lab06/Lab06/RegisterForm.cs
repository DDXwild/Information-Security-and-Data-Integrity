namespace Lab06;

public partial class RegisterForm : Form
{
    public RegisterForm()
    {
        InitializeComponent();
        cmbRole.Items.AddRange(
        [
            "S1 - Директор",
            "S2 - Секретар",
            "S3 - Оператор",
            "S4 - Менеджер",
            "S5 - Адміністратор"
        ]);
        cmbRole.SelectedIndex = 0;
        RefreshUsersList();
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        string login = txtLogin.Text.Trim();
        string password = txtPassword.Text;
        string fullName = txtFullName.Text.Trim();
        string position = txtPosition.Text.Trim();

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Введіть ідентифікатор та пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (password != txtPasswordConfirm.Text)
        {
            MessageBox.Show("Паролі не збігаються.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!SecurityPolicy.RegisterUser(login, password, cmbRole.SelectedIndex, fullName, position))
        {
            MessageBox.Show("Користувача з таким ідентифікатором уже зареєстровано.", "Помилка",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show($"Користувача «{login}» зареєстровано.", "Реєстрація",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        txtLogin.Clear();
        txtPassword.Clear();
        txtPasswordConfirm.Clear();
        RefreshUsersList();
    }

    private void RefreshUsersList()
    {
        lstUsers.Items.Clear();
        foreach (var user in SecurityPolicy.Accounts.Values.OrderBy(u => u.SubjectIndex))
        {
            lstUsers.Items.Add($"{user.Login,-12} {user.FullName,-18} {user.SubjectId}  {user.Position}");
        }
    }
}
