using Accord.MachineLearning;
using Accord.MachineLearning.DecisionTrees;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FloodPrediction
{
    public partial class RealTimePredictionForm : Form
    {
        RandomForest model;
        DataTable liveData;
        public RealTimePredictionForm()
        {
            InitializeComponent();
            timerPredict.Interval = 10000; 
        }

        private void RealTimePredictionForm_Load(object sender, EventArgs e)
        {
            
            // Load trained model once
            FileStream fs = new FileStream(Application.StartupPath+"//flood.model", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            model = (RandomForest)bf.Deserialize(fs);
            fs.Close();

            lblStatus.Text = "Model loaded. Ready for live prediction.";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timerPredict.Start();
            lblStatus.Text = "Real-time prediction started...";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerPredict.Stop();
            lblStatus.Text = "Prediction stopped.";
        }

        private void timerPredict_Tick(object sender, EventArgs e)
        {
            string query =
            "SELECT TOP 1 * FROM WeatherData_Live ORDER BY Timestamp DESC";

            liveData = DbConnection.ExecuteSelect(query, null);

            if (liveData.Rows.Count == 0)
                return;

            dgvLiveData.DataSource = liveData;

            DataRow row = liveData.Rows[0];

            double[] input =
            {
                Convert.ToDouble(row["Rainfall_3DayAvg"]),
                Convert.ToDouble(row["River_Risk_Index"]),
                Convert.ToDouble(row["Soil_Wetness_Index"]),
                Convert.ToDouble(row["Temp_Humidity_Index"]),
                Convert.ToDouble(row["Pressure_Trend"])
            };

            int result = model.Decide(input);

            string risk = result == 1 ? "High" : "Low";
            lblLastPrediction.Text =
                "Last Prediction (" + DateTime.Now + "): " + risk;

            // STORE GIS UPDATE
            SaveToGIS(row["Location"].ToString(), risk);

            // TRIGGER ALERT
            if (risk == "High")
                TriggerAlert(row["Location"].ToString());
        }
        private void SaveToGIS(string location, string risk)
        {
            string query =
            "INSERT INTO FloodRiskLocations " +
            "(LocationName, Latitude, Longitude, RiskLevel, LastUpdated) " +
            "VALUES (@loc, 10.10, 76.30, @risk, GETDATE())";

            DbConnection.ExecuteNonQuery(query, new SqlParameter[]
            {
                new SqlParameter("@loc", location),
                new SqlParameter("@risk", risk)
            });
        }

        // ALERT TRIGGER
        private void TriggerAlert(string location)
        {
            MessageBox.Show(
                "⚠ FLOOD ALERT!\nLocation: " + location,
                "Authority Alert");
        }
    }
}
