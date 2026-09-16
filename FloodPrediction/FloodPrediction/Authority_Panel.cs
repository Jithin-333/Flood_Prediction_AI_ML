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
    public partial class Authority_Panel : Form
    {
        Panel panelHeader, panelMenu, panelContent, panelFooter;
        FlowLayoutPanel menuContainer;

        // Labels
        Label lblTitle, lblUser, lblStatus, lblDateTime;

        // Buttons
        Button btnDashboard, btnUsers, btnDataset, btnModels, btnReports, btnLogout, btnReports1;

        // Grid
        DataGridView dgvRecentPredictions;

        // Theme Colors
        Color HeaderColor = Color.FromArgb(25, 35, 55);
        Color SidebarColor = Color.FromArgb(30, 40, 60);
        Color ContentColor = Color.FromArgb(245, 247, 250);
        Color AccentColor = Color.FromArgb(0, 153, 255);
        Color BorderColor = Color.FromArgb(60, 80, 110);

        public Authority_Panel()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI()
        {
            // ================= FORM =================
            this.Text = "Authority Dashboard";
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;

            // ================= HEADER =================
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = HeaderColor
            };

            lblTitle = new Label
            {
                Text = "AI-Based Smart Flood Prediction System",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 25),
                AutoSize = true
            };

            lblUser = new Label
            {
                Text = "Authority ",
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = Color.LightGray,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            panelHeader.Controls.Add(lblTitle);
            panelHeader.Controls.Add(lblUser);

            // ================= SIDEBAR =================
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = SidebarColor
            };

            menuContainer = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(5)
            };

            btnDashboard = CreateMenuButton("Dashboard", Properties.Resources.speedometer);
            btnUsers = CreateMenuButton("Prediction", Properties.Resources.UsersIcon);
            btnDataset = CreateMenuButton("Live Prediction", Properties.Resources.dataset);
            btnModels = CreateMenuButton("GIS Visualization", Properties.Resources.ModelsIcon);
            btnReports = CreateMenuButton("Alert", Properties.Resources.ReportsIcon);
            btnReports1 = CreateMenuButton("Live Analysis", Properties.Resources.training);
            btnLogout = CreateMenuButton("Logout", Properties.Resources.LogoutIcon);

            menuContainer.Controls.Add(btnDashboard);
            menuContainer.Controls.Add(btnUsers);
            menuContainer.Controls.Add(btnDataset);
            menuContainer.Controls.Add(btnModels);
            menuContainer.Controls.Add(btnReports);
            menuContainer.Controls.Add(btnReports1);
            menuContainer.Controls.Add(btnLogout);

            panelMenu.Controls.Add(menuContainer);

            // ================= CONTENT =================
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ContentColor,
                Padding = new Padding(20)
            };

            dgvRecentPredictions = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            StyleGrid(dgvRecentPredictions);
            LoadSampleData();

            panelContent.Controls.Add(dgvRecentPredictions);

            // ================= FOOTER =================
            panelFooter = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = HeaderColor
            };

            lblStatus = new Label
            {
                Text = "System Status : Online",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.LightGreen,
                Location = new Point(20, 12),
                AutoSize = true
            };

            lblDateTime = new Label
            {
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                AutoSize = true
            };

            panelFooter.Controls.Add(lblStatus);
            panelFooter.Controls.Add(lblDateTime);

            // ================= ADD CONTROLS =================
            this.Controls.Add(panelContent);
            this.Controls.Add(panelMenu);
            this.Controls.Add(panelFooter);
            this.Controls.Add(panelHeader);

            this.Load += AdminDashboardForm_Load;
        }

        // ================= ICON + TEXT BUTTON =================
        private Button CreateMenuButton(string text, Image icon)
        {
            Button btn = new Button
            {
                Text = "   " + text,
                Height = 45,
                Width = 200,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = SidebarColor,
                Image = icon,
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(15, 0, 0, 0),
                Margin = new Padding(5, 6, 5, 0)
            };

            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = BorderColor;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = AccentColor;
                btn.FlatAppearance.BorderColor = Color.White;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = SidebarColor;
                btn.FlatAppearance.BorderColor = BorderColor;
            };

            return btn;
        }

        // ================= GRID STYLE =================
        private void StyleGrid(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = AccentColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            dgv.RowTemplate.Height = 30;
            dgv.GridColor = Color.LightGray;
        }

        // ================= SAMPLE DATA =================
        private void LoadSampleData()
        {
            string conStr = @"Data Source=.;Initial Catalog=SmartFloodPredictionDB;Integrated Security=True";

            using (SqlConnection con = new SqlConnection(conStr))
            {
                string query = @"
        SELECT f1.LocationName, f1.RiskLevel, f1.LastUpdated
        FROM FloodRiskLocations f1
        INNER JOIN
        (
            SELECT LocationName, MAX(LastUpdated) AS LatestDate
            FROM FloodRiskLocations
            GROUP BY LocationName
        ) f2
        ON f1.LocationName = f2.LocationName 
        AND f1.LastUpdated = f2.LatestDate
        ORDER BY f1.LastUpdated DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvRecentPredictions.DataSource = dt;

                // Optional: Rename headers
                //dgvRecentPredictions.Columns["LocationName"].HeaderText = "Location";
                //dgvRecentPredictions.Columns["RiskLevel"].HeaderText = "Risk Level";
                //dgvRecentPredictions.Columns["LastUpdated"].HeaderText = "Last Updated";
            }
        }

        // ================= LOAD EVENT =================
        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            btnDashboard.Click += BtnDashboard_Click;
            btnUsers.Click += BtnUsers_Click;
            btnDataset.Click += BtnDataset_Click;
            btnModels.Click += BtnModels_Click;
            btnReports.Click += BtnReports_Click;
            btnReports1.Click += BtnReports1_Click;
            btnLogout.Click += BtnLogout_Click;

            lblUser.Left = this.Width - 100;

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += (s, ev) =>
            {
                lblDateTime.Text = DateTime.Now.ToString("dd MMM yyyy  hh:mm:ss tt");
                lblDateTime.Left = this.Width - 220;
            };
            timer.Start();
        }
        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            //Admin_Panel obj = new Admin_Panel();

            // MessageBox.Show("Dashboard clicked");
            // Load dashboard data
        }
        private void OpenChildForm(Form childForm)
        {
            panelContent.Controls.Clear();          // Remove old content
            childForm.TopLevel = false;              // Make it a child
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;         // Full area
            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            childForm.Show();
        }
        private void BtnUsers_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PredictionForm());

        }

        private void BtnDataset_Click(object sender, EventArgs e)
        {

            OpenChildForm(new LiveFloodPrediction());
            // Upload / view datasets
        }

        private void BtnModels_Click(object sender, EventArgs e)
        {

            OpenChildForm(new GISVisualizationForm());
            // ML model management
        }

        private void BtnReports_Click(object sender, EventArgs e)
        {

            OpenChildForm(new AuthorityAlertForm());
            // Generate reports
        }
        private void BtnReports1_Click(object sender, EventArgs e)
        {

            OpenChildForm(new LiveFeatchData());
            // Generate reports
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            //  MessageBox.Show("Logging out...");
            this.Close();
        }
    }
}
