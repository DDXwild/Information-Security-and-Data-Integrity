namespace Lab06;

public partial class AccessCheckForm : Form
{
    private UserAccount? _currentUser;

    public AccessCheckForm()
    {
        InitializeComponent();
        cmbObject.Items.AddRange(
            SecurityPolicy.ObjectIds
                .Select((id, i) => $"{id} - {SecurityPolicy.ObjectNames[i]}")
                .ToArray());
        cmbObject.SelectedIndex = 0;

        cmbOperation.Items.AddRange(
        [
            "1 - читання",
            "2 - редагування",
            "3 - видалення"
        ]);
        cmbOperation.SelectedIndex = 0;
    }

    private void btnAuthenticate_Click(object sender, EventArgs e)
    {
        if (!SecurityPolicy.TryAuthenticate(txtLogin.Text, txtPassword.Text, out var account))
        {
            _currentUser = null;
            lblUserInfo.Text = "Аутентифікацію не пройдено.";
            lblUserInfo.ForeColor = Color.DarkRed;
            MessageBox.Show("Невірний ідентифікатор або пароль.", "Аутентифікація",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _currentUser = account;
        lblUserInfo.ForeColor = Color.DarkGreen;
        lblUserInfo.Text =
            $"Користувач: {account!.FullName} ({account.Position}), суб'єкт {account.SubjectId}";
    }

    private void btnCheckAccess_Click(object sender, EventArgs e)
    {
        if (_currentUser is null)
        {
            MessageBox.Show("Спочатку пройдіть аутентифікацію.", "Перевірка доступу",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        int objectIndex = cmbObject.SelectedIndex;
        int requestedRight = cmbOperation.SelectedIndex + 1;
        int granted = SecurityPolicy.GetAccessRight(_currentUser.SubjectIndex, objectIndex);
        bool allowed = SecurityPolicy.IsAccessAllowed(_currentUser.SubjectIndex, objectIndex, requestedRight);

        string objectId = SecurityPolicy.ObjectIds[objectIndex];
        string operation = SecurityPolicy.DescribeRight(requestedRight);
        string grantedText = SecurityPolicy.DescribeRight(granted);

        lblResult.ForeColor = allowed ? Color.DarkGreen : Color.DarkRed;
        lblResult.Text = allowed
            ? $"Доступ ДОЗВОЛЕНО: {operation} для {objectId} (надано: {grantedText})."
            : $"Доступ ЗАБОРОНЕНО: {operation} для {objectId} (надано: {grantedText}).";
    }
}
