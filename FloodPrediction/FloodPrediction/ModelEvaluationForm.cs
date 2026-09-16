using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FloodPrediction
{
    public partial class ModelEvaluationForm : Form
    {
        DataTable testData;
        RandomForest model;
        Panel leftPanel;
        Panel rightPanel;
        Panel metricsPanel;
        public ModelEvaluationForm()
        {
            InitializeComponent();
            this.Shown += (s, e) => BuildSafePremiumLayout();
            //StyleControls();
        }
        private void BuildSafePremiumLayout()
        {
            this.SuspendLayout();

            // ================= FORM =================
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.None;

            // ================= LEFT PANEL =================
            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(30, 60, 114),
                Padding = new Padding(15)
            };

            // ================= RIGHT PANEL =================
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            // ================= TABLE LAYOUT (KEY FIX) =================
            TableLayoutPanel table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1
            };

            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 120)); // Metrics
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 200)); // Confusion
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // Chart

            // ================= REMOVE FROM FORM =================
            foreach (Control c in new Control[]
            {
        button1, button2, button3,
        lblAccuracy, lblPrecision, lblRecall, lblF1,
        dgvConfusion, chartMetrics
            })
            {
                this.Controls.Remove(c);
            }

            // ================= BUTTONS =================
            ConfigureButton(button1, "Load Test Data");
            ConfigureButton(button2, "Load Model");
            ConfigureButton(button3, "Evaluate");

            leftPanel.Controls.Add(button3);
            leftPanel.Controls.Add(button2);
            leftPanel.Controls.Add(button1);

            // ================= METRICS PANEL =================
            Panel metricsPanel = new Panel { Dock = DockStyle.Fill };

            StyleMetric(lblAccuracy, "Accuracy", 20);
            StyleMetric(lblPrecision, "Precision", 220);
            StyleMetric(lblRecall, "Recall", 420);
            StyleMetric(lblF1, "F1-Score", 620);

            metricsPanel.Controls.AddRange(new Control[]
            {
        lblAccuracy, lblPrecision, lblRecall, lblF1
            });

            // ================= GRID =================
            dgvConfusion.Dock = DockStyle.Fill;
            dgvConfusion.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvConfusion.BackgroundColor = Color.White;
            dgvConfusion.BorderStyle = BorderStyle.None;

            // ================= CHART =================
            chartMetrics.Dock = DockStyle.Fill;

            // ================= ADD TO TABLE =================
            table.Controls.Add(metricsPanel, 0, 0);
            table.Controls.Add(dgvConfusion, 0, 1);
            table.Controls.Add(chartMetrics, 0, 2);

            rightPanel.Controls.Add(table);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);

            this.ResumeLayout(true);
        }
        private void ConfigureButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Dock = DockStyle.Top;
            btn.Height = 60;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(30, 60, 114);
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Margin = new Padding(0, 0, 0, 10);
        }

        private void StyleMetric(Label lbl, string title, int x)
        {
            lbl.Text = $"{title}: --";
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(30, 60, 114);
            lbl.Location = new Point(x, 40);
            lbl.AutoSize = true;
        }
        // ===================== STYLING =====================
        private void StyleControls()
        {
            StyleButton(button1, "Load Test Data");
            StyleButton(button2, "Load Model");
            StyleButton(button3, "Evaluate Model");

            StyleMetricLabel(lblAccuracy);
            StyleMetricLabel(lblPrecision);
            StyleMetricLabel(lblRecall);
            StyleMetricLabel(lblF1);

            StyleGrid(dgvConfusion);
            StyleChart(chartMetrics);
        }

        private void StyleButton(Button btn, string text)
        {
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.FromArgb(30, 60, 114);
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Margin = new Padding(0, 0, 0, 15);
            btn.Cursor = Cursors.Hand;
        }

        private void StyleMetricLabel(Label lbl)
        {
            lbl.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lbl.ForeColor = Color.FromArgb(30, 60, 114);
            lbl.AutoSize = true;
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
        }

        private void StyleChart(Chart chart)
        {
            chart.ChartAreas[0].BackColor = Color.White;
            chart.Legends[0].Font = new Font("Segoe UI", 10);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (testData == null || model == null)
            {
                MessageBox.Show("Load test data and model first.");
                return;
            }

            int TP = 0, TN = 0, FP = 0, FN = 0;

            foreach (DataRow row in testData.Rows)
            {
                double[] input =
                {
                    Convert.ToDouble(row["Rainfall_3DayAvg"]),
                    Convert.ToDouble(row["River_Risk_Index"]),
                    Convert.ToDouble(row["Soil_Wetness_Index"]),
                    Convert.ToDouble(row["Temp_Humidity_Index"]),
                    Convert.ToDouble(row["Pressure_Trend"])
                };

                int actual = Convert.ToBoolean(row["FloodLabel"]) ? 1 : 0;
                int predicted = model.Decide(input);

                if (predicted == 1 && actual == 1) TP++;
                else if (predicted == 0 && actual == 0) TN++;
                else if (predicted == 1 && actual == 0) FP++;
                else if (predicted == 0 && actual == 1) FN++;
            }

            double accuracy = (double)(TP + TN) / (TP + TN + FP + FN);
            double precision = TP + FP == 0 ? 0 : (double)TP / (TP + FP);
            double recall = TP + FN == 0 ? 0 : (double)TP / (TP + FN);
            double f1 = precision + recall == 0 ? 0 :
                        2 * precision * recall / (precision + recall);

            lblAccuracy.Text = "Accuracy: " + (accuracy * 100).ToString("0.00") + "%";
            lblPrecision.Text = "Precision: " + precision.ToString("0.00");
            lblRecall.Text = "Recall: " + recall.ToString("0.00");
            lblF1.Text = "F1-Score: " + f1.ToString("0.00");

            LoadConfusionMatrix(TP, FP, FN, TN);
            LoadChart(accuracy, precision, recall, f1);
        }
        private void LoadConfusionMatrix(int TP, int FP, int FN, int TN)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(" ");
            dt.Columns.Add("Predicted Flood");
            dt.Columns.Add("Predicted No Flood");

            dt.Rows.Add("Actual Flood", TP, FN);
            dt.Rows.Add("Actual No Flood", FP, TN);

            dgvConfusion.DataSource = dt;
        }

        // METRIC BAR CHART
        private void LoadChart(double acc, double prec, double rec, double f1)
        {
            chartMetrics.Series.Clear();
            Series s = new Series("Metrics");
            s.ChartType = SeriesChartType.Column;

            s.Points.AddXY("Accuracy", acc * 100);
            s.Points.AddXY("Precision", prec * 100);
            s.Points.AddXY("Recall", rec * 100);
            s.Points.AddXY("F1-Score", f1 * 100);

            chartMetrics.Series.Add(s);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string query =
           "SELECT Rainfall_3DayAvg, River_Risk_Index, Soil_Wetness_Index, " +
           "Temp_Humidity_Index, Pressure_Trend, FloodLabel " +
           "FROM WeatherData_Features";

            testData = DbConnection.ExecuteSelect(query, null);

            MessageBox.Show("Test data loaded: " + testData.Rows.Count + " rows");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
           // OpenFileDialog ofd = new OpenFileDialog();
           // ofd.Filter = "Model File (*.model)|*.model";

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
                FileStream fs = new FileStream(Application.StartupPath+"//flood.model", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                model = (RandomForest)bf.Deserialize(fs);
                fs.Close();

                MessageBox.Show("Model loaded successfully.");
            //}
        }
    }
}
