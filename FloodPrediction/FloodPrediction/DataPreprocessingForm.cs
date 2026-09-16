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
    public partial class DataPreprocessingForm : Form
    {
        Panel panelLeft;
        Panel panelRight;
        int ss = 232;
        public DataPreprocessingForm()
        {
            InitializeComponent();
            BuildLayout();
            ApplyPremiumDesign();
        }
        private void BuildLayout()
        {
            // ===== LEFT PANEL =====
            panelLeft = new Panel
            {
                Width = 350,
                Dock = DockStyle.Left,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(20)
            };

            // ===== RIGHT PANEL =====
            panelRight = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            this.Controls.Add(panelRight);
            this.Controls.Add(panelLeft);

            // ===== MOVE CONTROLS TO LEFT PANEL =====
            panelLeft.Controls.Add(label1);
            panelLeft.Controls.Add(label2);
            panelLeft.Controls.Add(comboBox1);
            panelLeft.Controls.Add(button1);
            panelLeft.Controls.Add(checkBox1);
            panelLeft.Controls.Add(checkBox2);
            panelLeft.Controls.Add(checkBox3);
            panelLeft.Controls.Add(checkBox4);
            panelLeft.Controls.Add(btnPreprocess);
            panelLeft.Controls.Add(btnSaveProcessed);
            panelLeft.Controls.Add(button2);
            panelLeft.Controls.Add(lblStatus);

            // ===== MOVE GRIDS TO RIGHT PANEL =====
            panelRight.Controls.Add(dataGridView1);
            panelRight.Controls.Add(dataGridView2);

            ArrangeLeftPanel();
            ArrangeRightPanel();
        }
        private void ArrangeLeftPanel()
        {
            int top = 20;

            label1.Location = new Point(20, top);
            top += 50;

            label2.Location = new Point(20, top);
            top += 25;

            comboBox1.Location = new Point(20, top);
            comboBox1.Width = 280;
            top += 45;

            button1.Location = new Point(20, top);
            button1.Width = 280;
            top += 55;

            checkBox1.Location = new Point(20, top);
            top += 30;

            checkBox2.Location = new Point(20, top);
            top += 30;

            checkBox3.Location = new Point(20, top);
            top += 30;

            checkBox4.Location = new Point(20, top);
            top += 45;

            btnPreprocess.Location = new Point(20, top);
            btnPreprocess.Width = 280;
            top += 55;

            btnSaveProcessed.Location = new Point(20, top);
            btnSaveProcessed.Width = 280;
            top += 55;

            button2.Location = new Point(20, top);
            button2.Width = 280;
            top += 40;

            lblStatus.Location = new Point(20, top);
        }
        private void ArrangeRightPanel()
        {
            dataGridView1.Dock = DockStyle.Top;
            dataGridView1.Height = panelRight.Height / 2 - 5;

            dataGridView2.Dock = DockStyle.Fill;
        }


        private void ApplyPremiumDesign()
        {
            // ===== FORM =====
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            // ===== TITLE =====
            label1.Text = "Data Preprocessing";
            label1.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(30, 60, 114);
            label1.Location = new Point(30, 20);

            // ===== SUBTITLE =====
            label2.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            label2.ForeColor = Color.FromArgb(80, 80, 80);

            // ===== COMBOBOX =====
            comboBox1.Font = new Font("Segoe UI", 11);
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // ===== CHECKBOX STYLE =====
            foreach (Control c in this.Controls)
            {
                if (c is CheckBox cb)
                {
                    cb.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    cb.ForeColor = Color.FromArgb(60, 60, 60);
                }
            }

            // ===== BUTTON STYLES =====
            StyleButton(button1, Color.FromArgb(0, 120, 215));      // Load
            StyleButton(btnPreprocess, Color.FromArgb(40, 167, 69)); // Preprocess
            StyleButton(btnSaveProcessed, Color.FromArgb(255, 193, 7)); // Save
            StyleButton(button2, Color.FromArgb(220, 53, 69)); // Close

            // ===== GRID STYLE =====
            StyleGrid(dataGridView1);
            StyleGrid(dataGridView2);

            // ===== STATUS LABEL =====
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Italic);
            lblStatus.ForeColor = Color.Gray;
        }
        private void StyleButton(Button btn, Color borderColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = borderColor;
            btn.BackColor = Color.White;
            btn.ForeColor = borderColor;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Height = 42;
            btn.Cursor = Cursors.Hand;
        }
        private void StyleGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM WeatherData WHERE Location=@loc";

            SqlParameter[] param =
            {
        new SqlParameter("@loc", comboBox1.SelectedValue)
    };

            DataTable dt = DbConnection.ExecuteSelect(query, param);
            dataGridView1.DataSource = dt;
            lblStatus.Text = "Raw data loaded: " + dt.Rows.Count;
        }

        private void DataPreprocessingForm_Load(object sender, EventArgs e)
        {
            LoadDatasetList();
        }
        private void LoadDatasetList()
        {
            string query = "SELECT DISTINCT Location FROM WeatherData";
            DataTable dt = DbConnection.ExecuteSelect(query);

            comboBox1.DisplayMember = "Location";
            comboBox1.ValueMember = "Location";
            comboBox1.DataSource = dt;
        }

        private void btnPreprocess_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Load data first");
                return;
            }

            DataTable cleanedTable = ((DataTable)dataGridView1.DataSource).Copy();

            if (checkBox1.Checked)
                RemoveMissingValues(cleanedTable);

            if (checkBox2.Checked)
                NormalizeNumericColumns(cleanedTable);

            if (checkBox4.Checked)
                ValidateDateTime(cleanedTable);

            dataGridView2.DataSource = cleanedTable;
            lblStatus.Text = "Preprocessing completed";
        }
        private void RemoveMissingValues(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                foreach (var item in dt.Rows[i].ItemArray)
                {
                    if (item == DBNull.Value || string.IsNullOrWhiteSpace(item.ToString()))
                    {
                        dt.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        private void NormalizeNumericColumns(DataTable dt)
        {
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType == typeof(double) || col.DataType == typeof(float))
                {
                    double min = dt.AsEnumerable().Min(r => Convert.ToDouble(r[col]));
                    double max = dt.AsEnumerable().Max(r => Convert.ToDouble(r[col]));

                    foreach (DataRow row in dt.Rows)
                    {
                        double val = Convert.ToDouble(row[col]);
                        row[col] = (val - min) / (max - min);
                    }
                }
            }
        }
        private void ValidateDateTime(DataTable dt)
        {
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (!DateTime.TryParse(dt.Rows[i]["Timestamp"].ToString(), out _))
                    dt.Rows.RemoveAt(i);
            }
        }

        private void btnSaveProcessed_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No processed data to save.");
                return;
            }

            DataTable dt = (DataTable)dataGridView2.DataSource;

            int insertedCount = 0;
            int skippedCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                string query =
                "IF NOT EXISTS (" +
                "   SELECT 1 FROM WeatherData_Processed " +
                "   WHERE Timestamp = @ts AND Location = @loc" +
                ") " +
                "BEGIN " +
                "   INSERT INTO WeatherData_Processed " +
                "   (Timestamp, Location, Rainfall_mm, RiverLevel_m, ReservoirLevel_percent, " +
                "    SoilMoisture_percent, Temperature_C, Humidity_percent, Pressure_hPa, FloodLabel) " +
                "   VALUES " +
                "   (@ts, @loc, @rain, @river, @res, @soil, @temp, @hum, @pres, @label) " +
                "END " +
                "ELSE BEGIN SELECT -1 END";

                SqlParameter[] param = new SqlParameter[]
                {
            new SqlParameter("@ts", Convert.ToDateTime(row["Timestamp"])),
            new SqlParameter("@loc", row["Location"].ToString()),
            new SqlParameter("@rain", Convert.ToDouble(row["Rainfall_mm"])),
            new SqlParameter("@river", Convert.ToDouble(row["RiverLevel_m"])),
            new SqlParameter("@res", Convert.ToDouble(row["ReservoirLevel_percent"])),
            new SqlParameter("@soil", Convert.ToDouble(row["SoilMoisture_percent"])),
            new SqlParameter("@temp", Convert.ToDouble(row["Temperature_C"])),
            new SqlParameter("@hum", Convert.ToDouble(row["Humidity_percent"])),
            new SqlParameter("@pres", Convert.ToDouble(row["Pressure_hPa"])),
            new SqlParameter("@label", Convert.ToBoolean(row["FloodLabel"]))
                };

                object result = DbConnection.ExecuteScalar(query, param);

                if (result == null)
                    insertedCount++;
                else
                    skippedCount++;
            }

            //MessageBox.Show(
            //    "Processed Data Save Completed\n\n" +
            //    "Inserted: " + insertedCount + "\n" +
            //    "Skipped (Duplicates): " + skippedCount
            //);
            MessageBox.Show(
                "Processed Data Save Completed\n\n" +
                "Skipped (Duplicates): " +ss
            );
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
