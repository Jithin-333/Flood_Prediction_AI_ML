using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace FloodPrediction
{
    public partial class GISVisualizationForm : Form
    {
        GMapOverlay markersOverlay = new GMapOverlay("markers");
        Panel leftPanel;
        Panel rightPanel;
        Panel headerPanel;
        Panel filterCard;
        public GISVisualizationForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            BuildLayout();
            StyleControls();
            MoveControls();
        }

        // ================= BUILD LAYOUT =================
        private void BuildLayout()
        {
            // HEADER
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(30, 60, 114)
            };
            this.Controls.Add(headerPanel);
            headerPanel.BringToFront();

            label1.Parent = headerPanel;
            label1.ForeColor = Color.White;
            label1.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            label1.Location = new Point(20, 15);

            // LEFT PANEL (Filters & Buttons)
            leftPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 320,
                BackColor = Color.WhiteSmoke
            };
            this.Controls.Add(leftPanel);

            // RIGHT PANEL (Map)
            rightPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
            };
            this.Controls.Add(rightPanel);

            // FILTER CARD
            filterCard = new Panel
            {
                Size = new Size(280, 200),
                Location = new Point(20, 20),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            leftPanel.Controls.Add(filterCard);
        }

        // ================= STYLE CONTROLS =================
        private void StyleControls()
        {
            // Buttons
            btnLoadMap.Parent = filterCard;
            btnLoadMap.BackColor = Color.FromArgb(30, 120, 200);
            btnLoadMap.ForeColor = Color.White;
            btnLoadMap.FlatStyle = FlatStyle.Flat;
            btnLoadMap.FlatAppearance.BorderSize = 0;
            btnLoadMap.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLoadMap.Location = new Point(10, 10);
            btnLoadMap.Width = 260;
            btnLoadMap.Height = 50;
            btnLoadMap.Text = "Load Map";

            // ComboBox
            cmbRiskFilter.Parent = filterCard;
            cmbRiskFilter.Location = new Point(10, 80);
            cmbRiskFilter.Width = 260;
            cmbRiskFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRiskFilter.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Labels
            lblStatus.Parent = filterCard;
            lblStatus.Location = new Point(10, 130);
            lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.DarkBlue;

            label2.Parent = filterCard;
            label2.Location = new Point(10, 60);
            label2.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            label1.Parent = headerPanel;
        }

        // ================= MOVE MAP TO RIGHT PANEL =================
        private void MoveControls()
        {
            gmap.Parent = rightPanel;
            gmap.Dock = DockStyle.Fill;
        }
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            markersOverlay.Markers.Clear();

            string query = "SELECT * FROM FloodRiskLocations";
            DataTable dt = DbConnection.ExecuteSelect(query, null);

            foreach (DataRow row in dt.Rows)
            {
                if (cmbRiskFilter.Text != "All" &&
                    row["RiskLevel"].ToString() != cmbRiskFilter.Text)
                    continue;

                double lat = Convert.ToDouble(row["Latitude"]);
                double lng = Convert.ToDouble(row["Longitude"]);
                string risk = row["RiskLevel"].ToString();
                string name = row["LocationName"].ToString();

                GMarkerGoogleType markerType = GMarkerGoogleType.green;

                if (risk == "Moderate")
                    markerType = GMarkerGoogleType.orange;
                else if (risk == "High")
                    markerType = GMarkerGoogleType.red;

                GMapMarker marker = new GMarkerGoogle(
                    new PointLatLng(lat, lng),
                    markerType
                );

                marker.ToolTipText =
                    "Location: " + name +
                    "\nRisk: " + risk;

                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                markersOverlay.Markers.Add(marker);
            }

            lblStatus.Text = "Map loaded. Locations: " + markersOverlay.Markers.Count;
            gmap.Refresh();
        }

        private void GISVisualizationForm_Load(object sender, EventArgs e)
        {
            gmap.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            gmap.Position = new PointLatLng(20.5937, 78.9629); // India
            gmap.MinZoom = 2;
            gmap.MaxZoom = 18;
            gmap.Zoom = 5;

            gmap.Overlays.Add(markersOverlay);

            cmbRiskFilter.Items.Add("All");
            cmbRiskFilter.Items.Add("Low");
            cmbRiskFilter.Items.Add("Moderate");
            cmbRiskFilter.Items.Add("High");
            cmbRiskFilter.SelectedIndex = 0;

        }
    }
}
