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
    public partial class PredictionForm : Form
    {
        Panel leftPanel;
        Panel rightPanel;
        Panel resultCard;
        RandomForest trainedModel;
        public PredictionForm()
        {
            InitializeComponent();
            //StyleResultLabels();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BuildLayout();
            StyleInputs();
            MoveControlsToLeftPanel();
        }
        private void BuildLayout()
        {
            // LEFT PANEL (Inputs)
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 380,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(leftPanel);

            // RIGHT PANEL (Result)
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 243, 248)
            };
            this.Controls.Add(rightPanel);

            // RESULT CARD
            resultCard = new Panel
            {
                Size = new Size(380, 180),
                Location = new Point(400, 60),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            rightPanel.Controls.Add(resultCard);

            SetupResultLabels();
        }

        // ===================== RESULT LABELS =====================
        private void SetupResultLabels()
        {
            lblResult.Parent = resultCard;
            lblRisk.Parent = resultCard;

            lblResult.AutoSize = false;
            lblResult.Size = new Size(340, 50);
            lblResult.Location = new Point(20, 30);
            lblResult.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblResult.TextAlign = ContentAlignment.MiddleCenter;
            lblResult.ForeColor = Color.Black;
            lblResult.Visible = true;

            lblRisk.AutoSize = false;
            lblRisk.Size = new Size(340, 35);
            lblRisk.Location = new Point(20, 100);
            lblRisk.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblRisk.TextAlign = ContentAlignment.MiddleCenter;
            lblRisk.ForeColor = Color.Gray;
            lblRisk.Visible = true;

            lblResult.BringToFront();
            lblRisk.BringToFront();
        }

        // ===================== INPUT STYLING =====================
        private void StyleInputs()
        {
            Font labelFont = new Font("Segoe UI", 11, FontStyle.Bold);
            Font textFont = new Font("Segoe UI", 11);

            foreach (Control c in this.Controls)
            {
                if (c is Label lbl)
                {
                    lbl.Font = labelFont;
                    lbl.ForeColor = Color.FromArgb(60, 60, 60);
                }
                if (c is TextBox txt)
                {
                    txt.Font = textFont;
                    txt.BorderStyle = BorderStyle.FixedSingle;
                }
                if (c is Button btn)
                {
                    btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(30, 120, 200);
                    btn.BackColor = Color.FromArgb(30, 120, 200);
                    btn.ForeColor = Color.White;
                }
            }
        }

        // ===================== MOVE CONTROLS =====================
        private void MoveControlsToLeftPanel()
        {
            Control[] inputs =
            {
                label1, txtRainfall,
                label2, txtRiver,
                label3, txtSoil,
                label4, txtTempHumidity,
                label5, txtPressure,
                button1, button2
            };

            int y = 30;

            foreach (Control c in inputs)
            {
                c.Parent = leftPanel;
                c.Location = new Point(30, y);
                c.Width = 300;
                y += (c is TextBox) ? 45 : 30;
            }

            button1.Height = 45;
            button2.Height = 55;
        }
        private void button1_Click(object sender, EventArgs e)
        {
          //  OpenFileDialog ofd = new OpenFileDialog();
          //  ofd.Filter = "Model File (*.model)|*.model";

           // if (ofd.ShowDialog() == DialogResult.OK)
           // {
                FileStream fs = new FileStream(Application.StartupPath + "//flood.model", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                trainedModel = (RandomForest)bf.Deserialize(fs);
                fs.Close();

                MessageBox.Show("Model loaded successfully!");
           // }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (trainedModel == null)
            {
                MessageBox.Show("Load trained model first.");
                return;
            }

            try
            {
                double rainfall = Convert.ToDouble(txtRainfall.Text);
                double riverRisk = Convert.ToDouble(txtRiver.Text);
                double soilWetness = Convert.ToDouble(txtSoil.Text);
                double tempHumidity = Convert.ToDouble(txtTempHumidity.Text);
                double pressureTrend = Convert.ToDouble(txtPressure.Text);

                double[] input = new double[]
                {
                    rainfall,
                    riverRisk,
                    soilWetness,
                    tempHumidity,
                    pressureTrend
                };

                int prediction = trainedModel.Decide(input);

                if (prediction == 1)
                {
                    lblResult.Text = "Flood Prediction: YES";
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    lblRisk.Text = "Risk Level: HIGH / EXTREME";
                }
                else
                {
                    lblResult.Text = "Flood Prediction: NO";
                    lblResult.ForeColor = System.Drawing.Color.Green;
                    lblRisk.Text = "Risk Level: LOW";
                }
            }
            catch
            {
                MessageBox.Show("Please enter valid numeric values.");
            }
        }

        private void PredictionForm_Load(object sender, EventArgs e)
        {
           // BuildLayout();
           // StyleControls();
        }
       
    }
}
