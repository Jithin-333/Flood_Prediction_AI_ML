using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloodPrediction
{
    public partial class UserManagementForm : Form
    {
        int selectedUserId = 0;
        Panel panelLeft;
        Panel panelRight;

        public UserManagementForm()
        {
            InitializeComponent();
            BuildLayout();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            cmbRole.Items.AddRange(new string[] { "Admin", "Authority"});
            cmbRole.SelectedIndex = 0;
            LoadUsers();
            StyleUI();
        }
        private void BuildLayout()
        {
            panelLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 500,
                BackColor = Color.White,
                Padding = new Padding(30)
            };

            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 243, 248),
                Padding = new Padding(20)
            };

            // Move controls
            Control[] leftControls =
            {
                label1,label2,label3,label4,label5,label6,label7,label8,
                txtFullName,txtUsername,txtPassword,cmbRole,
                txtEmail,txtPhone,chkActive,
                button1,button2,button3,button4
            };

            foreach (Control c in leftControls)
            {
                this.Controls.Remove(c);
                panelLeft.Controls.Add(c);
            }

            this.Controls.Remove(dgvUsers);
            panelRight.Controls.Add(dgvUsers);

            this.Controls.Add(panelRight);
            this.Controls.Add(panelLeft);
        }

        // ===================== STYLING =====================
        private void StyleUI()
        {
            StyleLabels();
            StyleTextBoxes();
            StyleButtons();
            StyleGrid();
            ArrangeLeftPanel();
        }

        private void StyleLabels()
        {
            // Main title
            label1.Text = "User Management";
            label1.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(30, 60, 114);
            label1.Location = new Point(120, 10);
            label1.AutoSize = true;  // Let it size itself
            label1.MaximumSize = new Size(panelLeft.Width - 10, 50); // wrap if too long

            foreach (Control c in panelLeft.Controls)
            {
                if (c is Label lbl && lbl != label1)
                {
                    lbl.Font = new Font("Segoe UI", 12, FontStyle.Bold);
                    lbl.ForeColor = Color.FromArgb(80, 80, 80);
                    lbl.TextAlign = ContentAlignment.MiddleRight; // middle vertically
                    lbl.AutoSize = false;
                    lbl.Width = 140;      // wider width for text
                    lbl.Height = 30;      // enough height
                }
            }
        }
        private void StyleTextBoxes()
        {
            TextBox[] txts = {
                txtFullName,txtUsername,txtPassword,txtEmail,txtPhone
            };

            foreach (TextBox txt in txts)
            {
                txt.Font = new Font("Segoe UI", 11);
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.Width = 280;
                txt.TextAlign = HorizontalAlignment.Left;
            }

            txtPassword.UseSystemPasswordChar = true;

            cmbRole.Font = new Font("Segoe UI", 11);
            cmbRole.Width = 280;
            cmbRole.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void StyleButtons()
        {
            Button[] buttons = { button1, button2, button3, button4 };

            foreach (Button btn in buttons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.FromArgb(30, 60, 114);
                btn.BackColor = Color.White;
                btn.ForeColor = Color.FromArgb(30, 60, 114);
                btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                btn.Height = 42;
                btn.Width = 140;
            }

            button1.Text = "➕ Add";
            button2.Text = "✏ Update";
            button3.Text = "🗑 Delete";
            button4.Text = "🧹 Clear";
        }

        private void StyleGrid()
        {
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.RowHeadersVisible = false;

            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 60, 114);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvUsers.EnableHeadersVisualStyles = false;
        }

        private void ArrangeLeftPanel()
        {
            int y = 70;
            int gap = 45;
            int labelX = 20;
            int inputX = 160;

            Label[] labels = {
                label2,label3,label4,label5,label6,label7,label8
            };

            Control[] inputs = {
                txtFullName,txtUsername,txtPassword,
                cmbRole,txtEmail,txtPhone,chkActive
            };

            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Location = new Point(labelX, y);
                inputs[i].Location = new Point(inputX, y);
                y += gap;
            }

            button1.Location = new Point(40, y + 20);
            button2.Location = new Point(220, y + 20);
            button3.Location = new Point(40, y + 75);
            button4.Location = new Point(220, y + 75);
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = DbConnection.ExecuteSelect(
                "SELECT UserID, FullName, Username, Role, Email, Phone, IsActive, CreatedDate FROM Users");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;
            string query = @"INSERT INTO Users
                (FullName, Username, PasswordHash, Role, Email, Phone, IsActive, CreatedDate)
                VALUES
                (@fn, @un, @pw, @role, @em, @ph, @act, GETDATE())";

            SqlParameter[] param =
            {
                new SqlParameter("@fn", txtFullName.Text),
                new SqlParameter("@un", txtUsername.Text),
                new SqlParameter("@pw", txtPassword.Text),
                new SqlParameter("@role", cmbRole.SelectedItem.ToString()),
                new SqlParameter("@em", txtEmail.Text),
                new SqlParameter("@ph", txtPhone.Text),
                new SqlParameter("@act", chkActive.Checked)
            };

            DbConnection.ExecuteNonQuery(query, param);
            MessageBox.Show("User added successfully");
            LoadUsers();
            ClearFields();
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            selectedUserId = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["UserID"].Value);
            txtFullName.Text = dgvUsers.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[e.RowIndex].Cells["Username"].Value.ToString();
            cmbRole.Text = dgvUsers.Rows[e.RowIndex].Cells["Role"].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[e.RowIndex].Cells["Email"].Value.ToString();
            txtPhone.Text = dgvUsers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
            chkActive.Checked = Convert.ToBoolean(dgvUsers.Rows[e.RowIndex].Cells["IsActive"].Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0) return;

            string query = @"UPDATE Users SET
                FullName=@fn,
                Username=@un,
                PasswordHash=@pw,
                Role=@role,
                Email=@em,
                Phone=@ph,
                IsActive=@act
                WHERE UserID=@id";

            SqlParameter[] param =
            {
                new SqlParameter("@fn", txtFullName.Text),
                new SqlParameter("@un", txtUsername.Text),
                new SqlParameter("@pw", txtPassword.Text),
                new SqlParameter("@role", cmbRole.SelectedItem.ToString()),
                new SqlParameter("@em", txtEmail.Text),
                new SqlParameter("@ph", txtPhone.Text),
                new SqlParameter("@act", chkActive.Checked),
                new SqlParameter("@id", selectedUserId)
            };

            DbConnection.ExecuteNonQuery(query, param);
            MessageBox.Show("User updated successfully");
            LoadUsers();
            ClearFields();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedUserId == 0) return;

            DialogResult dr = MessageBox.Show(
                "Are you sure you want to delete this user?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                string query = "DELETE FROM Users WHERE UserID=@id";
                SqlParameter[] param =
                {
                    new SqlParameter("@id", selectedUserId)
                };

                DbConnection.ExecuteNonQuery(query, param);
                MessageBox.Show("User deleted successfully");
                LoadUsers();
                ClearFields();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
        }
        private bool ValidateFields()
        {
            bool isValid = true;

            // Array of all required textboxes
            TextBox[] requiredTextBoxes = { txtFullName, txtUsername, txtPassword, txtEmail, txtPhone };

            foreach (TextBox txt in requiredTextBoxes)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    isValid = false;
                    txt.BackColor = Color.FromArgb(255, 230, 230); // highlight in light red
                }
                else
                {
                    txt.BackColor = Color.White; // reset if filled
                }
            }

            // Validate Email format
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.BackColor = Color.FromArgb(255, 230, 230);
                    isValid = false;
                }
            }

            // Validate Phone number (digits only, 10-15 digits)
            if (!string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtPhone.Text, @"^\d{10,15}$"))
                {
                    MessageBox.Show("Please enter a valid phone number (10-15 digits).", "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.BackColor = Color.FromArgb(255, 230, 230);
                    isValid = false;
                }
            }

            // Role should be selected
            if (cmbRole.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a role.", "Missing Role", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                isValid = false;
            }

            return isValid;
        }

        // Helper method to check valid email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private void StyleLabelsAdvanced()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Label lbl && lbl != label1)
                {
                    lbl.TextAlign = ContentAlignment.MiddleRight;
                    lbl.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lbl.ForeColor = Color.FromArgb(80, 80, 80);
                    lbl.AutoSize = false;
                    lbl.Width = 120;
                }
            }

            // Main title
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(30, 30, 30);
            label1.TextAlign = ContentAlignment.MiddleLeft;
        }
        private void StyleTextBoxAdvanced(TextBox txt)
        {
            txt.Font = new Font("Segoe UI", 11F);
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Height = 36;
            txt.Width = 280;
            txt.TextAlign = HorizontalAlignment.Center;
            txt.BackColor = Color.White;

            txt.GotFocus += (s, e) =>
            {
                txt.BackColor = Color.FromArgb(245, 250, 255);
            };

            txt.LostFocus += (s, e) =>
            {
                txt.BackColor = Color.White;
            };
        }
        private void ClearFields()
        {
            txtFullName.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            chkActive.Checked = true;
            cmbRole.SelectedIndex = 0;
            selectedUserId = 0;
        }
    }
}
