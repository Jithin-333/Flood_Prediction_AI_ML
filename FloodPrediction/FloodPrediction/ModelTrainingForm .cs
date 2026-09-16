using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord.MachineLearning.DecisionTrees;
using Accord.MachineLearning.DecisionTrees.Learning;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



namespace FloodPrediction
{
    public partial class ModelTrainingForm : Form
    {
        DataTable trainingData;
        DecisionTree trainedModel;

        public ModelTrainingForm()
        {
            InitializeComponent();
        }

        private void ModelTrainingForm_Load(object sender, EventArgs e)
        {
            cmbAlgorithm.Items.Add("Decision Tree");
            cmbAlgorithm.Items.Add("Random Forest");
            cmbAlgorithm.SelectedIndex = 0;

            numTrainSplit.Minimum = 50;
            numTrainSplit.Maximum = 90;
            numTrainSplit.Value = 70;

            lblAccuracy.Text = "Accuracy: 0 %";
            lblPrecision.Text = "Precision: 0 %";
            lblRecall.Text = "Recall: 0 %";
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM WeatherData_Processed";
            trainingData = DbConnection.ExecuteSelect(query);

            lblTotalRecords.Text = "Total Records: " + trainingData.Rows.Count;
        }

        private void btnTrainModel_Click(object sender, EventArgs e)
        {
            if (trainingData == null || trainingData.Rows.Count == 0)
            {
                MessageBox.Show("Please load processed data first");
                return;
            }

            double[][] inputs;
            int[] outputs;

            PrepareData(out inputs, out outputs);

            DecisionVariable[] attributes = new DecisionVariable[]
            {
                new DecisionVariable("Rainfall", DecisionVariableKind.Continuous),
                new DecisionVariable("RiverLevel", DecisionVariableKind.Continuous),
                new DecisionVariable("Reservoir", DecisionVariableKind.Continuous),
                new DecisionVariable("SoilMoisture", DecisionVariableKind.Continuous),
                new DecisionVariable("Temperature", DecisionVariableKind.Continuous),
                new DecisionVariable("Humidity", DecisionVariableKind.Continuous),
                new DecisionVariable("Pressure", DecisionVariableKind.Continuous)
            };

            trainedModel = new DecisionTree(attributes, 2);

            var teacher = new C45Learning(trainedModel);
            teacher.Learn(inputs, outputs);

            EvaluateModel(trainedModel, inputs, outputs);

            MessageBox.Show("Model training completed successfully");
        }
        private void PrepareData(out double[][] inputs, out int[] outputs)
        {
            int rows = trainingData.Rows.Count;

            inputs = new double[rows][];
            outputs = new int[rows];

            for (int i = 0; i < rows; i++)
            {
                DataRow r = trainingData.Rows[i];

                inputs[i] = new double[]
                {
                    Convert.ToDouble(r["Rainfall_mm"]),
                    Convert.ToDouble(r["RiverLevel_m"]),
                    Convert.ToDouble(r["ReservoirLevel_percent"]),
                    Convert.ToDouble(r["SoilMoisture_percent"]),
                    Convert.ToDouble(r["Temperature_C"]),
                    Convert.ToDouble(r["Humidity_percent"]),
                    Convert.ToDouble(r["Pressure_hPa"])
                };

                outputs[i] = Convert.ToBoolean(r["FloodLabel"]) ? 1 : 0;
            }
        }

        // ================= EVALUATE MODEL =================
        private void EvaluateModel(DecisionTree model, double[][] inputs, int[] outputs)
        {
            int tp = 0, tn = 0, fp = 0, fn = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                int predicted = model.Decide(inputs[i]);
                int actual = outputs[i];

                if (predicted == 1 && actual == 1) tp++;
                else if (predicted == 0 && actual == 0) tn++;
                else if (predicted == 1 && actual == 0) fp++;
                else if (predicted == 0 && actual == 1) fn++;
            }

            double accuracy = (double)(tp + tn) / (tp + tn + fp + fn) * 100;
            double precision = tp + fp == 0 ? 0 : (double)tp / (tp + fp) * 100;
            double recall = tp + fn == 0 ? 0 : (double)tp / (tp + fn) * 100;

            lblAccuracy.Text = "Accuracy: " + accuracy.ToString("0.00") + " %";
            lblPrecision.Text = "Precision: " + precision.ToString("0.00") + " %";
            lblRecall.Text = "Recall: " + recall.ToString("0.00") + " %";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (trainedModel == null)
            {
                MessageBox.Show("Please train the model first");
                return;
            }

            using (FileStream fs = new FileStream("FloodPredictionModel.bin", FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, trainedModel);
            }

            MessageBox.Show("Model saved successfully");
        }
    }
}
