using Accord.Collections;
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
    public partial class LiveFloodPrediction : Form
    {
        Panel leftPanel;
        Panel rightPanel;
        Panel headerPanel;
        Panel resultCard;
        public LiveFloodPrediction()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BuildLayout();
            StyleControls();
            MoveControls();
            LoadLocations();
        }
        private void BuildLayout()
        {
            // HEADER
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(30, 60, 114)
            };
            this.Controls.Add(headerPanel);

            label1.Parent = headerPanel;
            label1.Text = "Live Flood Prediction";
            label1.ForeColor = Color.White;
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.AutoSize = true;
            label1.Location = new Point(20, 15);

            // LEFT PANEL
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 350,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(leftPanel);
            leftPanel.BringToFront();

            // RIGHT PANEL
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };
            this.Controls.Add(rightPanel);

            // RESULT CARD
            resultCard = new Panel
            {
                Size = new Size(300, 140),
                Location = new Point(25, 230),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            leftPanel.Controls.Add(resultCard);
        }

        // ================= STYLING =================
        private void StyleControls()
        {
            Font labelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 11);

            foreach (Control c in this.Controls)
            {
                if (c is Label lbl && lbl != label1)
                {
                    lbl.Font = labelFont;
                    lbl.ForeColor = Color.FromArgb(60, 60, 60);
                }
                if (c is TextBox txt)
                {
                    txt.Font = textFont;
                    txt.ReadOnly = true;
                }
                if (c is ComboBox cmb)
                {
                    cmb.Font = textFont;
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                if (c is Button btn)
                {
                    btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.BackColor = Color.FromArgb(30, 120, 200);
                    btn.ForeColor = Color.White;
                }
            }

            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ================= MOVE CONTROLS =================
        private void MoveControls()
        {
            // LEFT PANEL CONTROLS
            label2.Parent = leftPanel;
            label2.Location = new Point(25, 20);

            label3.Parent = leftPanel;
            label3.Location = new Point(25, 60);

            cmbLocation.Parent = leftPanel;
            cmbLocation.Location = new Point(25, 85);
            cmbLocation.Width = 280;

            button1.Parent = leftPanel;
            button1.Location = new Point(25, 130);
            button1.Width = 280;
            button1.Text = "Predict Flood Risk";

            // RESULT CARD CONTROLS
            label4.Parent = resultCard;
            label4.Text = "Flood Probability";
            label4.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label4.Location = new Point(20, 20);

            textBox1.Parent = resultCard;
            textBox1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.Location = new Point(20, 50);
            textBox1.Width = 250;

            lblStatu.Parent = resultCard;
            lblStatu.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblStatu.Location = new Point(20, 100);
            lblStatu.AutoSize = true;

            // RIGHT PANEL GRID
            dataGridView1.Parent = rightPanel;
            dataGridView1.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string location = cmbLocation.SelectedItem.ToString();
            DataTable dt = FetchLast5WeatherData(location);

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data found for selected location!");
                return;
            }

            // Weighted flood calculation
            double totalFloodScore = 0;

            foreach (DataRow row in dt.Rows)
            {
                double rainfall = Convert.ToDouble(row["Rainfall_mm"]);
                double humidity = Convert.ToDouble(row["Humidity_percent"]);
                double temp = Convert.ToDouble(row["Temperature_C"]);
                double pressure = Convert.ToDouble(row["Pressure_hPa"]);

                // Simple weighted probability
                double rainScore = Math.Min(rainfall * 5, 100);
                double humidityScore = Math.Min(humidity, 100);
                double tempScore = Math.Max(0, 100 - Math.Abs(temp - 25) * 4);
                double pressureScore = Math.Max(0, 100 - (1013 - pressure) * 2);

                double floodChance = (rainScore * 0.5) + (humidityScore * 0.2) +
                                     (tempScore * 0.1) + (pressureScore * 0.2);

                totalFloodScore += floodChance;
            }

            // Average of last 5 rows
            double averageFloodChance = totalFloodScore / dt.Rows.Count;

            textBox1.Text = Math.Round(averageFloodChance, 2) + " %";
            if(averageFloodChance>=80)
            {
                lblStatu.Text = "Chance of Flooding";

            }
            else
            {
                lblStatu.Text = "Low Chance";
            }
        }
        private void LoadLocations()
        {
            cmbLocation.Items.Clear();

            string connString = "Data Source =.; Initial Catalog = SmartFloodPredictionDB; Integrated Security = True";

            using (SqlConnection con = new SqlConnection(connString))
            {
                string query = "SELECT DISTINCT Location FROM WeatherData_Live WHERE Location IS NOT NULL ORDER BY Location";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbLocation.Items.Add(reader["Location"].ToString());
                    }

                    reader.Close();
                }
            }
        }

        private void LiveFloodPrediction_Load(object sender, EventArgs e)
        {
            LoadLocations();
        }
        //private void LoadLocations()
        //{
        //    cmbLocation.Items.Clear();
        //    cmbLocation.Items.Add("All");
        //    cmbLocation.Items.Add("Trivandrum");
        //    cmbLocation.Items.Add("Kochi");
        //    cmbLocation.Items.Add("Kollam");
        //    cmbLocation.SelectedIndex = 0;
        //}
        private DataTable FetchLast5WeatherData(string location)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT TOP 5 * FROM WeatherData_Live
                             WHERE Location=@loc
                             ORDER BY Timestamp DESC";

            SqlParameter[] param = { new SqlParameter("@loc", location) };

            dt = DbConnection.ExecuteSelect(query, param);
            return dt;
        }
    }
}
