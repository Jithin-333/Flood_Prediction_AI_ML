namespace FloodPrediction
{
    partial class ModelTrainingForm
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
            this.btnLoadData = new System.Windows.Forms.Button();
            this.lblTotalRecords = new System.Windows.Forms.Label();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.numTrainSplit = new System.Windows.Forms.NumericUpDown();
            this.btnTrainModel = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.lblPrecision = new System.Windows.Forms.Label();
            this.lblRecall = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numTrainSplit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(32, 62);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(165, 44);
            this.btnLoadData.TabIndex = 0;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // lblTotalRecords
            // 
            this.lblTotalRecords.AutoSize = true;
            this.lblTotalRecords.Location = new System.Drawing.Point(29, 34);
            this.lblTotalRecords.Name = "lblTotalRecords";
            this.lblTotalRecords.Size = new System.Drawing.Size(44, 16);
            this.lblTotalRecords.TabIndex = 1;
            this.lblTotalRecords.Text = "label1";
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Items.AddRange(new object[] {
            "Decision Tree",
            "Random Forest"});
            this.cmbAlgorithm.Location = new System.Drawing.Point(273, 62);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(121, 24);
            this.cmbAlgorithm.TabIndex = 2;
            // 
            // numTrainSplit
            // 
            this.numTrainSplit.Location = new System.Drawing.Point(297, 121);
            this.numTrainSplit.Name = "numTrainSplit";
            this.numTrainSplit.Size = new System.Drawing.Size(120, 22);
            this.numTrainSplit.TabIndex = 3;
            // 
            // btnTrainModel
            // 
            this.btnTrainModel.Location = new System.Drawing.Point(52, 182);
            this.btnTrainModel.Name = "btnTrainModel";
            this.btnTrainModel.Size = new System.Drawing.Size(165, 44);
            this.btnTrainModel.TabIndex = 4;
            this.btnTrainModel.Text = "Train";
            this.btnTrainModel.UseVisualStyleBackColor = true;
            this.btnTrainModel.Click += new System.EventHandler(this.btnTrainModel_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(41, 328);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 44);
            this.button2.TabIndex = 5;
            this.button2.Text = "Save Model";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(157, 248);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(237, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Location = new System.Drawing.Point(555, 215);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(44, 16);
            this.lblAccuracy.TabIndex = 7;
            this.lblAccuracy.Text = "label1";
            // 
            // lblPrecision
            // 
            this.lblPrecision.AutoSize = true;
            this.lblPrecision.Location = new System.Drawing.Point(555, 255);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(44, 16);
            this.lblPrecision.TabIndex = 8;
            this.lblPrecision.Text = "label2";
            // 
            // lblRecall
            // 
            this.lblRecall.AutoSize = true;
            this.lblRecall.Location = new System.Drawing.Point(555, 302);
            this.lblRecall.Name = "lblRecall";
            this.lblRecall.Size = new System.Drawing.Size(44, 16);
            this.lblRecall.TabIndex = 9;
            this.lblRecall.Text = "label3";
            // 
            // ModelTrainingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRecall);
            this.Controls.Add(this.lblPrecision);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnTrainModel);
            this.Controls.Add(this.numTrainSplit);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.lblTotalRecords);
            this.Controls.Add(this.btnLoadData);
            this.Name = "ModelTrainingForm";
            this.Text = "ModelTrainingForm";
            this.Load += new System.EventHandler(this.ModelTrainingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numTrainSplit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.ComboBox cmbAlgorithm;
        private System.Windows.Forms.NumericUpDown numTrainSplit;
        private System.Windows.Forms.Button btnTrainModel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.Label lblPrecision;
        private System.Windows.Forms.Label lblRecall;
    }
}