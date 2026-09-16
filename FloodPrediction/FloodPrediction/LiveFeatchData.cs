using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace FloodPrediction
{
    public partial class LiveFeatchData : Form
    {
        private Panel mainPanel;
        private Panel inputPanel;
        private Label headerLabel;
        private Button fetchButton;
        public LiveFeatchData()
        {
            InitializeComponent();
            BuildPremiumLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FetchAndStoreWeather();
        }
        private void BuildPremiumLayout()
        {
            // MAIN PANEL
            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(20)
            };
            this.Controls.Add(mainPanel);

            // HEADER
            headerLabel = label2; // reuse existing label2
            headerLabel.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            headerLabel.ForeColor = Color.FromArgb(30, 60, 90);
            headerLabel.Dock = DockStyle.Top;
            headerLabel.Height = 50;
            mainPanel.Controls.Add(headerLabel);

            // INPUT PANEL
            inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.White,
                Padding = new Padding(20),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(inputPanel);

            // Label + TextBox (Location)
            label1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(50, 50, 50);
            label1.Dock = DockStyle.Top;
            label1.Padding = new Padding(0, 0, 0, 5);

            textBox1.Font = new Font("Segoe UI", 12);
            textBox1.Dock = DockStyle.Top;
            textBox1.Height = 35;
            textBox1.BorderStyle = BorderStyle.FixedSingle;

            inputPanel.Controls.Add(textBox1);
            inputPanel.Controls.Add(label1);

            // Fetch Button Styling
            fetchButton = button1; // reuse existing button
            fetchButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            fetchButton.BackColor = Color.FromArgb(30, 120, 220);
            fetchButton.ForeColor = Color.White;
            fetchButton.FlatStyle = FlatStyle.Flat;
            fetchButton.FlatAppearance.BorderSize = 0;
            fetchButton.Height = 50;
            fetchButton.Dock = DockStyle.Top;
            fetchButton.Margin = new Padding(0, 15, 0, 0);

            mainPanel.Controls.Add(fetchButton);
        }
        private void FetchAndStoreWeather()
        {
            string apiKey = "a38d6bbb8947ebc1cc98c3f3aa376066"; // VALID KEY
            string city = textBox1.Text;

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={apiKey}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.UserAgent = "FloodPredictionSystem";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer serializer =
                        new DataContractJsonSerializer(typeof(WeatherRoot));

                    WeatherRoot data = (WeatherRoot)serializer.ReadObject(stream);

                    double temperature = data.Main.Temp;
                    double humidity = data.Main.Humidity;
                    double pressure = data.Main.Pressure;
                    double rainfall = data.Rain != null ? data.Rain.OneHour : 0;
                    MessageBox.Show("Temperature" + temperature + "  Humidity" + humidity + "  pressure" + pressure + "  Rainfall" + rainfall);
                    SaveToDatabase(city, temperature, humidity, pressure, rainfall);
                }

                MessageBox.Show("Live weather data saved successfully!");
            }
            catch (WebException ex)
            {
                MessageBox.Show("API Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("System Error: " + ex.Message);
            }
        }

        private void SaveToDatabase(string location, double temp,
                                    double humidity, double pressure, double rainfall)
        {
            string query = @"INSERT INTO WeatherData_Live
                            (Timestamp, Location, Rainfall_mm,
                             Temperature_C, Humidity_percent, Pressure_hPa)
                             VALUES (@ts, @loc, @rain, @temp, @hum, @pres)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@ts", DateTime.Now),
                new SqlParameter("@loc", location),
                new SqlParameter("@rain", rainfall),
                new SqlParameter("@temp", temp),
                new SqlParameter("@hum", humidity),
                new SqlParameter("@pres", pressure)
            };

            DbConnection.ExecuteNonQuery(query, parameters);
        }
    }
}

