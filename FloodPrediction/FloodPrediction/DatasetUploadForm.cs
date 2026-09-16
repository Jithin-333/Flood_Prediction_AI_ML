using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FloodPrediction
{
    public partial class DatasetUploadForm : Form
    {

        DataTable dtPreview = new DataTable();
        public DatasetUploadForm()
        {
            InitializeComponent();
            ApplyPremiumDesign();
            HookEvents();
        }
        private void ApplyPremiumDesign()
        {
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;

            panelTop.BackColor = Color.FromArgb(30, 60, 114);
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);

            label2.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(70, 70, 70);

            txtFilePath.Font = new Font("Segoe UI", 11);

            //StyleButton(btnBrowse);
            //StyleButton(btnPreview);
            //StyleButton(btnUpload);
            //StyleButton(btnClear);

            StyleButton(btnBrowse);
            StyleButton(btnPreview);
            StyleButton(btnUpload);
            StyleButton(btnClear);

            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblStatus.ForeColor = Color.Gray;

            StyleGrid();
        }

        private void StyleButton(System.Windows.Forms.Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(0, 120, 215);
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Width = 160;
            btn.Height = 45;
            btn.Cursor = Cursors.Hand;
        }

        private void StyleGrid()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ===================== EVENTS =====================
        private void HookEvents()
        {
            btnBrowse.Click += btnBrowse_Click;
            btnPreview.Click += btnPreview_Click;
            btnUpload.Click += btnUpload_Click;
            btnClear.Click += btnClear_Click;
        }
        private void DatasetUploadForm_Load(object sender, EventArgs e)
        {

        }

       
        
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV Files (*.csv)|*.csv";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                lblStatus.Text = "File selected: " + ofd.FileName;
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilePath.Text))
            {
                MessageBox.Show("Select a CSV file first.");
                return;
            }

            try
            {
                dtPreview.Clear();
                string[] lines = File.ReadAllLines(txtFilePath.Text);

                if (lines.Length > 0)
                {
                    // Create columns
                    string[] headers = lines[0].Split(',');
                    foreach (string header in headers)
                        dtPreview.Columns.Add(header);

                    // Load rows
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] rows = lines[i].Split(',');
                        dtPreview.Rows.Add(rows);
                    }

                    dataGridView1.DataSource = dtPreview;
                    lblStatus.Text = "Preview loaded. Rows: " + dtPreview.Rows.Count;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (dtPreview.Rows.Count == 0)
            {
                MessageBox.Show("Preview data first.");
                return;
            }

            try
            {
                foreach (DataRow row in dtPreview.Rows)
                {
                    bool floodLabel = row["FloodLabel"].ToString().Trim() == "1";

                    string query = @"INSERT INTO WeatherData
        (Timestamp, Location, Rainfall_mm, RiverLevel_m, ReservoirLevel_percent,
         SoilMoisture_percent, Temperature_C, Humidity_percent, Pressure_hPa, FloodLabel)
        VALUES
        (@ts, @loc, @rain, @river, @res, @soil, @temp, @hum, @pres, @label)";

                    SqlParameter[] param =
                    {
        new SqlParameter("@ts", DateTime.Parse(row["Timestamp"].ToString())),
        new SqlParameter("@loc", row["Location"].ToString()),
        new SqlParameter("@rain", Convert.ToDouble(row["Rainfall_mm"])),
        new SqlParameter("@river", Convert.ToDouble(row["RiverLevel_m"])),
        new SqlParameter("@res", Convert.ToDouble(row["ReservoirLevel_percent"])),
        new SqlParameter("@soil", Convert.ToDouble(row["SoilMoisture_percent"])),
        new SqlParameter("@temp", Convert.ToDouble(row["Temperature_C"])),
        new SqlParameter("@hum", Convert.ToDouble(row["Humidity_percent"])),
        new SqlParameter("@pres", Convert.ToDouble(row["Pressure_hPa"])),
        new SqlParameter("@label", floodLabel)
    };

                    // DbConnection.ExecuteNonQuery(query, param);
                }

                MessageBox.Show("Dataset uploaded successfully!");
                lblStatus.Text = "Upload completed. Total rows: " + dtPreview.Rows.Count;
                dtPreview.Clear();
                dataGridView1.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Upload failed: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFilePath.Clear();
            dtPreview.Clear();
            dataGridView1.DataSource = null;
            lblStatus.Text = "Cleared.";
        }
    }
}
