namespace FloodPrediction
{
    partial class RealTimePredictionForm
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
            this.components = new System.ComponentModel.Container();
            this.dgvLiveData = new System.Windows.Forms.DataGridView();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblLastPrediction = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.timerPredict = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiveData)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLiveData
            // 
            this.dgvLiveData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLiveData.Location = new System.Drawing.Point(50, 42);
            this.dgvLiveData.Name = "dgvLiveData";
            this.dgvLiveData.RowHeadersWidth = 51;
            this.dgvLiveData.RowTemplate.Height = 24;
            this.dgvLiveData.Size = new System.Drawing.Size(698, 223);
            this.dgvLiveData.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(63, 303);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 16);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "label1";
            // 
            // lblLastPrediction
            // 
            this.lblLastPrediction.AutoSize = true;
            this.lblLastPrediction.Location = new System.Drawing.Point(63, 384);
            this.lblLastPrediction.Name = "lblLastPrediction";
            this.lblLastPrediction.Size = new System.Drawing.Size(44, 16);
            this.lblLastPrediction.TabIndex = 2;
            this.lblLastPrediction.Text = "label2";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(562, 303);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(142, 61);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(728, 303);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(142, 61);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // timerPredict
            // 
            this.timerPredict.Interval = 10000;
            this.timerPredict.Tick += new System.EventHandler(this.timerPredict_Tick);
            // 
            // RealTimePredictionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 583);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblLastPrediction);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.dgvLiveData);
            this.Name = "RealTimePredictionForm";
            this.Text = "RealTimePredictionForm";
            this.Load += new System.EventHandler(this.RealTimePredictionForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiveData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLiveData;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblLastPrediction;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timerPredict;
    }
}