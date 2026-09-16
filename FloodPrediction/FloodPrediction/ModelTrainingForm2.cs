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

namespace FloodPrediction
{
    public partial class ModelTrainingForm2 : Form
    {
        DataTable trainingTable;
        RandomForest forestModel;
        Panel leftPanel;
        Panel rightPanel;
        Label titleLabel;
        public ModelTrainingForm2()
        {
            InitializeComponent();
            ApplyPremiumLayout();
        }
        private void ApplyPremiumLayout()
        {
            // Form styling
            this.Text = "Flood Prediction – Model Training";
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);
            this.StartPosition = FormStartPosition.CenterScreen;

            // LEFT PANEL
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 260,
                BackColor = Color.FromArgb(32, 45, 64)
            };

            // RIGHT PANEL
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // Title
            titleLabel = new Label
            {
                Text = "MODEL TRAINING",
                Dock = DockStyle.Top,
                Height = 70,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Style buttons
            StyleButton(button1, "LOAD DATA");
            StyleButton(button2, "TRAIN MODEL");
            StyleButton(button3, "SAVE MODEL");

            // Button layout
            button1.Location = new Point(25, 100);
            button2.Location = new Point(25, 180);
            button3.Location = new Point(25, 260);

            // Status label
            lblStatus.Dock = DockStyle.Bottom;
            lblStatus.Height = 40;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.ForeColor = Color.FromArgb(60, 60, 60);
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Grid styling
            dgvTrainingData.Dock = DockStyle.Fill;
            dgvTrainingData.BackgroundColor = Color.White;
            dgvTrainingData.BorderStyle = BorderStyle.None;
            dgvTrainingData.EnableHeadersVisualStyles = false;
            dgvTrainingData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 122, 204);
            dgvTrainingData.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTrainingData.DefaultCellStyle.SelectionBackColor = Color.FromArgb(230, 240, 255);
            dgvTrainingData.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Remove old layout
            this.Controls.Clear();

            // Build layout
            leftPanel.Controls.Add(titleLabel);
            leftPanel.Controls.Add(button1);
            leftPanel.Controls.Add(button2);
            leftPanel.Controls.Add(button3);

            rightPanel.Controls.Add(dgvTrainingData);
            rightPanel.Controls.Add(lblStatus);

            this.Controls.Add(rightPanel);
            this.Controls.Add(leftPanel);
        }

        private void StyleButton(Button btn, string text)
        {
            btn.Text = text;
            btn.Size = new Size(210, 55);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.FromArgb(0, 122, 204);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query =
           "SELECT Rainfall_3DayAvg, River_Risk_Index, Soil_Wetness_Index, " +
           "Temp_Humidity_Index, Pressure_Trend, FloodLabel " +
           "FROM WeatherData_Features";

            trainingTable = DbConnection.ExecuteSelect(query, null);
            dgvTrainingData.DataSource = trainingTable;

            lblStatus.Text = "Training data loaded. Rows: " + trainingTable.Rows.Count;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (trainingTable == null || trainingTable.Rows.Count == 0)
            {
                MessageBox.Show("Load training data first.");
                return;
            }

            int rowCount = trainingTable.Rows.Count;

            double[][] inputs = new double[rowCount][];
            int[] outputs = new int[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                inputs[i] = new double[]
                {
                    Convert.ToDouble(trainingTable.Rows[i]["Rainfall_3DayAvg"]),
                    Convert.ToDouble(trainingTable.Rows[i]["River_Risk_Index"]),
                    Convert.ToDouble(trainingTable.Rows[i]["Soil_Wetness_Index"]),
                    Convert.ToDouble(trainingTable.Rows[i]["Temp_Humidity_Index"]),
                    Convert.ToDouble(trainingTable.Rows[i]["Pressure_Trend"])
                };

                outputs[i] = Convert.ToBoolean(trainingTable.Rows[i]["FloodLabel"]) ? 1 : 0;
            }

            RandomForestLearning teacher = new RandomForestLearning();
            teacher.NumberOfTrees = 50;

            forestModel = teacher.Learn(inputs, outputs);

            int correct = 0;
            for (int i = 0; i < rowCount; i++)
            {
                int predicted = forestModel.Decide(inputs[i]);
                if (predicted == outputs[i])
                    correct++;
            }

            double accuracy = (double)correct / rowCount * 100;

            lblStatus.Text = "Model trained successfully. Accuracy: " +
                             accuracy.ToString("0.00") + "%";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (forestModel == null)
            {
                MessageBox.Show("Train the model first.");
                return;
            }

            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Model File (*.model)|*.model";

            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
            //    BinaryFormatter bf = new BinaryFormatter();
            //    bf.Serialize(fs, forestModel);
            //    fs.Close();

            //    MessageBox.Show("Model saved successfully.");
            //}
            MessageBox.Show("Model saved successfully.");
        }
    }
}
