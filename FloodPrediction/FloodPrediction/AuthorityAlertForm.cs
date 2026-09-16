using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace FloodPrediction
{
    public partial class AuthorityAlertForm : Form
    {
        private Panel leftPanel;
        private Panel rightPanel;
        private TableLayoutPanel formLayout;
        private FlowLayoutPanel buttonPanel;

        public AuthorityAlertForm()
        {
            InitializeComponent();
            comboBox1.Items.Add("Low");
            comboBox1.Items.Add("Moderate");
            comboBox1.Items.Add("High");
            comboBox1.SelectedIndex = 2;
            BuildStylishLayout();
        }

        private void BuildStylishLayout()
        {
            // LEFT PANEL - Form inputs
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 450,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            this.Controls.Add(leftPanel);

            // RIGHT PANEL - Status / preview
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };
            this.Controls.Add(rightPanel);

            // Table layout for labels & inputs
            formLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                AutoSize = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None
            };
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            formLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            for (int i = 0; i < 6; i++) formLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

            leftPanel.Controls.Add(formLayout);

            // Add labels & inputs
            formLayout.Controls.Add(label6, 0, 0); // Location
            formLayout.Controls.Add(txtLocation, 1, 0);

            formLayout.Controls.Add(label5, 0, 1); // Level
            formLayout.Controls.Add(comboBox1, 1, 1);

            formLayout.Controls.Add(label4, 0, 2); // Message
            formLayout.Controls.Add(txtMessage, 1, 2);

            formLayout.Controls.Add(label3, 0, 3); // Email
            formLayout.Controls.Add(txtEmail, 1, 3);

            //formLayout.Controls.Add(label2, 0, 4); // Phone
           // formLayout.Controls.Add(txtPhone, 1, 4);

            // Buttons in FlowLayoutPanel
            buttonPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                AutoSize = true
            };
            buttonPanel.Controls.Add(button1);
          //  buttonPanel.Controls.Add(button2);
            formLayout.Controls.Add(buttonPanel, 1, 5);

            // Right panel status label
            lblStatus.Parent = rightPanel;
            lblStatus.Location = new Point(30, 30);
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblStatus.ForeColor = Color.DarkBlue;

            ApplyStyling();
        }

        private void ApplyStyling()
        {
            // Labels
            foreach (Control c in formLayout.Controls)
            {
                if (c is Label lbl)
                {
                    lbl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    lbl.ForeColor = Color.Black;
                    lbl.TextAlign = ContentAlignment.MiddleRight;
                    lbl.Dock = DockStyle.Fill;
                }
            }

            // Inputs
            txtLocation.Dock = DockStyle.Fill;
            txtMessage.Dock = DockStyle.Fill;
            txtEmail.Dock = DockStyle.Fill;
            //txtPhone.Dock = DockStyle.Fill;
            comboBox1.Dock = DockStyle.Fill;
            txtMessage.Multiline = true;

            // Buttons
            button1.BackColor = Color.FromArgb(30, 120, 200);
            button1.ForeColor = Color.White;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button1.Size = new Size(120, 40);

            //button2.BackColor = Color.FromArgb(30, 180, 80);
            //button2.ForeColor = Color.White;
            //button2.FlatStyle = FlatStyle.Flat;
            //button2.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            //button2.Size = new Size(120, 40);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public class GeoResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
        }

        // ================= GET LAT/LON =================
        private async Task<(double lat, double lon)> GetCoordinates(string location)
        {
            string url = "https://nominatim.openstreetmap.org/search?q="
                         + location + "&format=json&limit=1";

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "FloodPredictionApp");

                string json = await client.DownloadStringTaskAsync(url);

                var data = JsonConvert.DeserializeObject<List<GeoResult>>(json);

                if (data != null && data.Count > 0)
                {
                    double lat = Convert.ToDouble(data[0].lat);
                    double lon = Convert.ToDouble(data[0].lon);
                    return (lat, lon);
                }
                else
                {
                    throw new Exception("Location not found");
                }
            }
        }

        // ================= SAVE FLOOD RISK =================
        private void SaveFloodRisk(string location, double lat, double lon, string risk)
        {
            string query = @"INSERT INTO FloodRiskLocations
                            (LocationName, Latitude, Longitude, RiskLevel, LastUpdated)
                            VALUES (@loc, @lat, @lon, @risk, GETDATE())";

            SqlParameter[] param =
            {
                new SqlParameter("@loc", location),
                new SqlParameter("@lat", lat),
                new SqlParameter("@lon", lon),
                new SqlParameter("@risk", risk)
            };

            DbConnection.ExecuteNonQuery(query, param);
        }
        private void SendEmail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("smarttutortvm@gmail.com");
            mail.To.Add(txtEmail.Text);
            mail.Subject = "Flood Alert - " + comboBox1.Text;
            mail.Body = txtMessage.Text;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(
                "smarttutortvm@gmail.com",
                "sdlu ridf twei oyaj"); // App password
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }

        // ================= MAIN BUTTON CLICK =================
        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLocation.Text == "" || txtEmail.Text == "")
                {
                    MessageBox.Show("Please enter Location and Email");
                    return;
                }

                // STEP 1: Get Coordinates
                var result = await GetCoordinates(txtLocation.Text);

                // STEP 2: Save to FloodRiskLocations table
                SaveFloodRisk(
                    txtLocation.Text,
                    result.lat,
                    result.lon,
                    comboBox1.Text
                );

                // STEP 3: Send Email Alert
                SendEmail();

                // SUCCESS MESSAGE
                MessageBox.Show(
                    "Alert Sent & Location Saved!\n\n" +
                    "Latitude: " + result.lat +
                    "\nLongitude: " + result.lon
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string smsText =
            //  "FLOOD ALERT\nLocation: " + txtLocation.Text +
            //  "\nRisk: " + comboBox1.Text +
            //  "\nMessage: " + txtMessage.Text;

            //// Simulate SMS
            //MessageBox.Show(
            //    "SMS SENT TO: " + txtPhone.Text + "\n\n" + smsText,
            //    "SMS Simulation");

            //SaveAlert("SMS", txtPhone.Text);

            //lblStatus.Text = "SMS alert simulated successfully.";
        }
        private void SaveAlert(string type, string sentTo)
        {
            string query =
            "INSERT INTO FloodAlerts VALUES (@loc,@risk,@msg,@type,@to,GETDATE())";

            SqlParameter[] param =
            {
                new SqlParameter("@loc", txtLocation.Text),
                new SqlParameter("@risk", comboBox1.Text),
                new SqlParameter("@msg", txtMessage.Text),
                new SqlParameter("@type", type),
                new SqlParameter("@to", sentTo)
            };

            DbConnection.ExecuteNonQuery(query, param);
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
