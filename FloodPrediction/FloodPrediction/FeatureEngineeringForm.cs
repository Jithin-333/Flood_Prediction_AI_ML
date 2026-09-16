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
    public partial class FeatureEngineeringForm : Form
    {
        Panel panelLeft;
        Panel panelRight;

        DataTable dtFeatures = new DataTable();
        public FeatureEngineeringForm()
        {
            InitializeComponent();
            InitializeFeatureTable();
      

            BuildLayout();
            ApplyPremiumDesign();
        }
        private void BuildLayout()
        {
            // LEFT PANEL
            panelLeft = new Panel
            {
                Dock = DockStyle.Left,
                Width = 320,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(20)
            };

            // RIGHT PANEL
            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            this.Controls.Add(panelRight);
            this.Controls.Add(panelLeft);

            // Move controls
            panelLeft.Controls.Add(label1);
            panelLeft.Controls.Add(button1);
            panelLeft.Controls.Add(button2);
            panelLeft.Controls.Add(lblStatus);

            panelRight.Controls.Add(label2);
            panelRight.Controls.Add(dgvFeatures);

            ArrangeLeftPanel();
            ArrangeRightPanel();
        }
        private void ArrangeLeftPanel()
        {
            int top = 20;

            label1.Location = new Point(20, top);
            top += 60;

            button1.Location = new Point(20, top);
            button1.Width = 260;
            top += 70;

            button2.Location = new Point(20, top);
            button2.Width = 260;
            top += 70;

            lblStatus.Location = new Point(20, top);
        }
        private void ArrangeRightPanel()
        {
            label2.Location = new Point(10, 10);

            dgvFeatures.Dock = DockStyle.Fill;
            dgvFeatures.Top = 40;
        }
        private void ApplyPremiumDesign()
        {
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;

            // Title
            label1.Text = "Feature Engineering";
            label1.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(30, 60, 114);

            label2.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label2.ForeColor = Color.Gray;

            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblStatus.ForeColor = Color.DarkSlateGray;

            StyleButton(button1, Color.FromArgb(0, 120, 215));
            StyleButton(button2, Color.FromArgb(46, 204, 113));

            StyleGrid();
        }
        private void StyleButton(Button btn, Color accent)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = accent;
            btn.BackColor = Color.White;
            btn.ForeColor = accent;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Height = 45;
            btn.Cursor = Cursors.Hand;
        }
        private void StyleGrid()
        {
            dgvFeatures.BorderStyle = BorderStyle.None;
            dgvFeatures.EnableHeadersVisualStyles = false;

            dgvFeatures.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 60, 114);
            dgvFeatures.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFeatures.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvFeatures.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            dgvFeatures.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvFeatures.RowHeadersVisible = false;
        }
        private void InitializeFeatureTable()
        {
            dtFeatures.Columns.Add("Timestamp", typeof(DateTime));
            dtFeatures.Columns.Add("Location", typeof(string));

            dtFeatures.Columns.Add("Rainfall_3DayAvg", typeof(double));
            dtFeatures.Columns.Add("River_Risk_Index", typeof(double));
            dtFeatures.Columns.Add("Soil_Wetness_Index", typeof(double));
            dtFeatures.Columns.Add("Temp_Humidity_Index", typeof(double));
            dtFeatures.Columns.Add("Pressure_Trend", typeof(double));

            dtFeatures.Columns.Add("FloodLabel", typeof(bool));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM WeatherData_Processed ORDER BY Timestamp";

            DataTable dt = DbConnection.ExecuteSelect(query, null);

            dtFeatures.Rows.Clear();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                double rainfallAvg = CalculateRainfallAverage(dt, i);
                double riverRisk = Convert.ToDouble(row["RiverLevel_m"]) *
                                   Convert.ToDouble(row["Rainfall_mm"]);

                double soilWetness = Convert.ToDouble(row["SoilMoisture_percent"]) *
                                     Convert.ToDouble(row["Rainfall_mm"]);

                double tempHumidity = Convert.ToDouble(row["Temperature_C"]) *
                                      Convert.ToDouble(row["Humidity_percent"]);

                double pressureTrend = Convert.ToDouble(row["Pressure_hPa"]) - 1013.0;

                dtFeatures.Rows.Add(
                    Convert.ToDateTime(row["Timestamp"]),
                    row["Location"].ToString(),
                    rainfallAvg,
                    riverRisk,
                    soilWetness,
                    tempHumidity,
                    pressureTrend,
                    Convert.ToBoolean(row["FloodLabel"])
                );
            }

            dgvFeatures.DataSource = dtFeatures;
            lblStatus.Text = "Feature engineering completed. Rows: " + dtFeatures.Rows.Count;
        }
        private double CalculateRainfallAverage(DataTable dt, int index)
        {
            double sum = 0;
            int count = 0;

            for (int i = index; i >= 0 && i > index - 3; i--)
            {
                sum += Convert.ToDouble(dt.Rows[i]["Rainfall_mm"]);
                count++;
            }

            return sum / count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int inserted = 0;

            foreach (DataRow row in dtFeatures.Rows)
            {
                string query =
                "IF NOT EXISTS (" +
                "SELECT 1 FROM WeatherData_Features WHERE Timestamp=@ts AND Location=@loc" +
                ") " +
                "INSERT INTO WeatherData_Features " +
                "(Timestamp, Location, Rainfall_3DayAvg, River_Risk_Index, Soil_Wetness_Index, " +
                " Temp_Humidity_Index, Pressure_Trend, FloodLabel) " +
                "VALUES (@ts,@loc,@ravg,@risk,@soil,@temp,@pres,@label)";

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ts", row["Timestamp"]),
                    new SqlParameter("@loc", row["Location"]),
                    new SqlParameter("@ravg", row["Rainfall_3DayAvg"]),
                    new SqlParameter("@risk", row["River_Risk_Index"]),
                    new SqlParameter("@soil", row["Soil_Wetness_Index"]),
                    new SqlParameter("@temp", row["Temp_Humidity_Index"]),
                    new SqlParameter("@pres", row["Pressure_Trend"]),
                    new SqlParameter("@label", row["FloodLabel"])
                };

                DbConnection.ExecuteNonQuery(query, param);
                inserted++;
            }

            MessageBox.Show("Feature data saved successfully.\nRecords: " + inserted);
        }
    }
    
}
