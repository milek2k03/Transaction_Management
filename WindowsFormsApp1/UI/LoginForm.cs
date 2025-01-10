// LoginForm.cs
using System;
using System.Windows.Forms;
using InvoiceApp.Services;
//using WindowsFormsApp1.Services;
//using WindowsFormsApp1.UI;

namespace InvoiceApp.UI
{
    public class LoginForm : Form
    {
        private TextBox emailTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Button registerButton;
        private UserService userService;

        public LoginForm()
        {
            userService = new UserService();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "Login";
            Size = new System.Drawing.Size(300, 200);

            var emailLabel = new Label { Text = "Email", Dock = DockStyle.Top };
            emailTextBox = new TextBox { Dock = DockStyle.Top };

            var passwordLabel = new Label { Text = "Password", Dock = DockStyle.Top };
            passwordTextBox = new TextBox { Dock = DockStyle.Top, PasswordChar = '*' };

            loginButton = new Button { Text = "Login", Dock = DockStyle.Bottom };
            loginButton.Click += LoginButton_Click;

            registerButton = new Button { Text = "Register", Dock = DockStyle.Bottom };
            registerButton.Click += RegisterButton_Click;

            Controls.Add(loginButton);
            Controls.Add(registerButton);
            Controls.Add(passwordTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(emailTextBox);
            Controls.Add(emailLabel);
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            var email = emailTextBox.Text;
            var password = passwordTextBox.Text;

            if (userService.Login(email, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Otwórz MainForm jako dialog
                Hide(); // Ukryj LoginForm
                using (var mainForm = new MainForm(userService.GetCurrentUser()))
                {
                    mainForm.ShowDialog();
                }
                Close(); // Zamknij LoginForm po zamknięciu MainForm
            }
            else
            {
                MessageBox.Show("Invalid credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var email = emailTextBox.Text;
            var password = passwordTextBox.Text;

            if (userService.Register(email, password))
            {
                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Registration failed. User may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}