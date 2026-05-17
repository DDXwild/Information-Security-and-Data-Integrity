namespace Lab06;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitle;
    private Button btnRegister;
    private Button btnAccessCheck;
    private Button btnShowMatrix;
    private TextBox txtMatrix;
    private Button btnExit;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTitle = new Label();
        btnRegister = new Button();
        btnAccessCheck = new Button();
        btnShowMatrix = new Button();
        txtMatrix = new TextBox();
        btnExit = new Button();
        SuspendLayout();

        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTitle.Location = new Point(24, 20);
        lblTitle.Size = new Size(520, 28);
        lblTitle.Text = "Адміністративна панель системного адміністратора";

        btnRegister.Location = new Point(24, 64);
        btnRegister.Size = new Size(280, 40);
        btnRegister.Text = "Реєстрація користувачів";
        btnRegister.UseVisualStyleBackColor = true;
        btnRegister.Click += btnRegister_Click;

        btnAccessCheck.Location = new Point(24, 114);
        btnAccessCheck.Size = new Size(280, 40);
        btnAccessCheck.Text = "Перевірка прав доступу";
        btnAccessCheck.UseVisualStyleBackColor = true;
        btnAccessCheck.Click += btnAccessCheck_Click;

        btnShowMatrix.Location = new Point(24, 164);
        btnShowMatrix.Size = new Size(280, 40);
        btnShowMatrix.Text = "Показати матрицю доступу";
        btnShowMatrix.UseVisualStyleBackColor = true;
        btnShowMatrix.Click += btnShowMatrix_Click;

        txtMatrix.Font = new Font("Consolas", 10F);
        txtMatrix.Location = new Point(324, 64);
        txtMatrix.Multiline = true;
        txtMatrix.ReadOnly = true;
        txtMatrix.ScrollBars = ScrollBars.Vertical;
        txtMatrix.Size = new Size(420, 300);
        txtMatrix.Text = BuildMatrixDescription();

        btnExit.Location = new Point(24, 324);
        btnExit.Size = new Size(120, 36);
        btnExit.Text = "Вихід";
        btnExit.UseVisualStyleBackColor = true;
        btnExit.Click += (_, _) => Close();

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(764, 384);
        Controls.Add(lblTitle);
        Controls.Add(btnRegister);
        Controls.Add(btnAccessCheck);
        Controls.Add(btnShowMatrix);
        Controls.Add(txtMatrix);
        Controls.Add(btnExit);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Лабораторна робота 6 - ДПБ";
        ResumeLayout(false);
        PerformLayout();
    }
}
