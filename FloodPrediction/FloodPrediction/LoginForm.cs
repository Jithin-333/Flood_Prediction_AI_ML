using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloodPrediction
{
    public partial class LoginForm : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;
        private Label lblMessage;
        public LoginForm()
        {
            //InitializeComponent();
            InitializeComponent1();
        }
        private void InitializeComponent1()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Background
            this.BackgroundImage = global::FloodPrediction.Properties.Resources._1cc11700_06da_4553_a0bd_441ce745835f_md; // high-res image
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Semi-transparent login panel
            Panel panelLogin = new Panel();
            panelLogin.Size = new Size(400, 400);
            panelLogin.BackColor = Color.FromArgb(180, 0, 0, 0); // semi-transparent black
            panelLogin.Location = new Point((this.Width - panelLogin.Width) / 2, (this.Height - panelLogin.Height) / 2);
            panelLogin.Anchor = AnchorStyles.None;
            panelLogin.BorderStyle = BorderStyle.None;
            this.Controls.Add(panelLogin);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "Smart Flood Prediction System";
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 60;
            panelLogin.Controls.Add(lblTitle);

            // Username
            Label lblUser = new Label();
            lblUser.Text = "Username";
            lblUser.ForeColor = Color.White;
            lblUser.BackColor = Color.Transparent;
            lblUser.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblUser.Top = 80;
            lblUser.Left = 20;
            panelLogin.Controls.Add(lblUser);

            txtUser = new TextBox();
            txtUser.Top = lblUser.Bottom + 5;
            txtUser.Left = 20;
            txtUser.Width = 360;
            txtUser.Height = 35;
            txtUser.Font = new Font("Segoe UI", 12);
            panelLogin.Controls.Add(txtUser);

            // Password
            Label lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.ForeColor = Color.White;
            lblPass.BackColor = Color.Transparent;
            lblPass.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            lblPass.Top = txtUser.Bottom + 20;
            lblPass.Left = 20;
            panelLogin.Controls.Add(lblPass);

            txtPass = new TextBox();
            txtPass.Top = lblPass.Bottom + 5;
            txtPass.Left = 20;
            txtPass.Width = 360;
            txtPass.Height = 35;
            txtPass.Font = new Font("Segoe UI", 12);
            txtPass.UseSystemPasswordChar = true;
            panelLogin.Controls.Add(txtPass);

            // Buttons
            Button btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.BackColor = Color.FromArgb(0, 120, 215);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Top = txtPass.Bottom + 30;
            btnLogin.Left = 20;
            btnLogin.Width = 160;
            btnLogin.Height = 40;
            panelLogin.Controls.Add(btnLogin);

            Button btnClear = new Button();
            btnClear.Text = "Close";
            btnClear.BackColor = Color.Gray;
            btnClear.ForeColor = Color.White;
            btnClear.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Top = txtPass.Bottom + 30;
            btnClear.Left = 220;
            btnClear.Width = 160;
            btnClear.Height = 40;
            panelLogin.Controls.Add(btnClear);
            btnLogin.Click += button1_Click;
            btnClear.Click += button2_Click;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text) ||
         string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Please enter username and password");
                return;
            }

            string query = @"SELECT Role FROM Users 
                     WHERE Username=@u AND PasswordHash=@p AND IsActive=1";

            SqlParameter[] param =
            {
        new SqlParameter("@u", txtUser.Text),
        new SqlParameter("@p", txtPass.Text)
    };

            object result = DbConnection.ExecuteScalar(query, param);

            if (result != null)
            {
                string role = result.ToString();

                this.Hide();

                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    new Admin_Panel().Show();
                else if (role.Equals("Authority", StringComparison.OrdinalIgnoreCase))
                    new Authority_Panel().Show();
                else
                    MessageBox.Show("Unknown role");
            }
            else
            {
                MessageBox.Show( "Invalid login credentials");
            }
        }
        
        private void OpenDashboard(string role)
        {
            this.Hide();

            if (role == "Admin")
                new Admin_Panel().Show();
            //else if (role == "Authority")
               // new AuthorityDashboardForm().Show();
            else
               // new AnalystDashboardForm().Show();
                MessageBox.Show("error");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
           // cmbRole.Items.AddRange(new string[] { "Admin", "Authority"});
           // cmbRole.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
