namespace Lab06;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void btnRegister_Click(object sender, EventArgs e)
    {
        using var form = new RegisterForm();
        form.ShowDialog(this);
    }

    private void btnAccessCheck_Click(object sender, EventArgs e)
    {
        using var form = new AccessCheckForm();
        form.ShowDialog(this);
    }

    private void btnShowMatrix_Click(object sender, EventArgs e)
    {
        txtMatrix.Text = BuildMatrixDescription();
    }

    private static string BuildMatrixDescription()
    {
        var header = "Матриця доступу (таблиця 6.5):" + Environment.NewLine + SecurityPolicy.FormatMatrix();
        var legend = Environment.NewLine + Environment.NewLine +
                     "S1 - директор; S2 - секретар; S3 - оператор; S4 - менеджер; S5 - адміністратор." +
                     Environment.NewLine +
                     "О1 - комерційна/конфіденційна інформація; О2 - робітники; О3 - споживачі; О4 - клієнти." +
                     Environment.NewLine +
                     "R: 1 - читання; 2 - редагування; 3 - видалення; 4 - немає доступу.";
        return header + legend;
    }
}
