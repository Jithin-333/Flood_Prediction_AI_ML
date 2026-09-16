namespace FloodPrediction
{
    partial class ModelEvaluationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.lblPrecision = new System.Windows.Forms.Label();
            this.lblRecall = new System.Windows.Forms.Label();
            this.lblF1 = new System.Windows.Forms.Label();
            this.dgvConfusion = new System.Windows.Forms.DataGridView();
            this.chartMetrics = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfusion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMetrics)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 60);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load Test Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(195, 36);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 60);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load Model";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(380, 36);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(219, 60);
            this.button3.TabIndex = 2;
            this.button3.Text = "Evaluate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Location = new System.Drawing.Point(63, 141);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(44, 16);
            this.lblAccuracy.TabIndex = 3;
            this.lblAccuracy.Text = "label1";
            // 
            // lblPrecision
            // 
            this.lblPrecision.AutoSize = true;
            this.lblPrecision.Location = new System.Drawing.Point(63, 183);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(44, 16);
            this.lblPrecision.TabIndex = 4;
            this.lblPrecision.Text = "label1";
            // 
            // lblRecall
            // 
            this.lblRecall.AutoSize = true;
            this.lblRecall.Location = new System.Drawing.Point(63, 217);
            this.lblRecall.Name = "lblRecall";
            this.lblRecall.Size = new System.Drawing.Size(44, 16);
            this.lblRecall.TabIndex = 5;
            this.lblRecall.Text = "label1";
            // 
            // lblF1
            // 
            this.lblF1.AutoSize = true;
            this.lblF1.Location = new System.Drawing.Point(63, 283);
            this.lblF1.Name = "lblF1";
            this.lblF1.Size = new System.Drawing.Size(44, 16);
            this.lblF1.TabIndex = 6;
            this.lblF1.Text = "label1";
            // 
            // dgvConfusion
            // 
            this.dgvConfusion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfusion.Location = new System.Drawing.Point(195, 111);
            this.dgvConfusion.Name = "dgvConfusion";
            this.dgvConfusion.RowHeadersWidth = 51;
            this.dgvConfusion.RowTemplate.Height = 24;
            this.dgvConfusion.Size = new System.Drawing.Size(605, 150);
            this.dgvConfusion.TabIndex = 7;
            // 
            // chartMetrics
            // 
            chartArea3.Name = "ChartArea1";
            this.chartMetrics.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartMetrics.Legends.Add(legend3);
            this.chartMetrics.Location = new System.Drawing.Point(240, 298);
            this.chartMetrics.Name = "chartMetrics";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartMetrics.Series.Add(series3);
            this.chartMetrics.Size = new System.Drawing.Size(549, 245);
            this.chartMetrics.TabIndex = 8;
            this.chartMetrics.Text = "chart1";
            // 
            // ModelEvaluationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 566);
            this.Controls.Add(this.chartMetrics);
            this.Controls.Add(this.dgvConfusion);
            this.Controls.Add(this.lblF1);
            this.Controls.Add(this.lblRecall);
            this.Controls.Add(this.lblPrecision);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ModelEvaluationForm";
            this.Text = "ModelEvaluationForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfusion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMetrics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.Label lblPrecision;
        private System.Windows.Forms.Label lblRecall;
        private System.Windows.Forms.Label lblF1;
        private System.Windows.Forms.DataGridView dgvConfusion;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMetrics;
    }
}