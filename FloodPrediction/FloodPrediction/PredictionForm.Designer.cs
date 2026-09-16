namespace FloodPrediction
{
    partial class PredictionForm
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
            this.txtRainfall = new System.Windows.Forms.TextBox();
            this.txtRiver = new System.Windows.Forms.TextBox();
            this.txtSoil = new System.Windows.Forms.TextBox();
            this.txtTempHumidity = new System.Windows.Forms.TextBox();
            this.txtPressure = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblRisk = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtRainfall
            // 
            this.txtRainfall.Location = new System.Drawing.Point(76, 59);
            this.txtRainfall.Name = "txtRainfall";
            this.txtRainfall.Size = new System.Drawing.Size(196, 22);
            this.txtRainfall.TabIndex = 0;
            // 
            // txtRiver
            // 
            this.txtRiver.Location = new System.Drawing.Point(76, 106);
            this.txtRiver.Name = "txtRiver";
            this.txtRiver.Size = new System.Drawing.Size(186, 22);
            this.txtRiver.TabIndex = 1;
            // 
            // txtSoil
            // 
            this.txtSoil.Location = new System.Drawing.Point(76, 158);
            this.txtSoil.Name = "txtSoil";
            this.txtSoil.Size = new System.Drawing.Size(186, 22);
            this.txtSoil.TabIndex = 2;
            // 
            // txtTempHumidity
            // 
            this.txtTempHumidity.Location = new System.Drawing.Point(76, 217);
            this.txtTempHumidity.Name = "txtTempHumidity";
            this.txtTempHumidity.Size = new System.Drawing.Size(186, 22);
            this.txtTempHumidity.TabIndex = 3;
            // 
            // txtPressure
            // 
            this.txtPressure.Location = new System.Drawing.Point(76, 264);
            this.txtPressure.Name = "txtPressure";
            this.txtPressure.Size = new System.Drawing.Size(186, 22);
            this.txtPressure.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rainfall Level";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "River Level";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Soil Moisture";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 223);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Humidity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(139, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Atmospheric Pressure";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 69);
            this.button1.TabIndex = 10;
            this.button1.Text = "Load Model";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(269, 318);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(224, 69);
            this.button2.TabIndex = 11;
            this.button2.Text = "Prediction";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(398, 223);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 16);
            this.lblResult.TabIndex = 12;
            // 
            // lblRisk
            // 
            this.lblRisk.AutoSize = true;
            this.lblRisk.Location = new System.Drawing.Point(330, 412);
            this.lblRisk.Name = "lblRisk";
            this.lblRisk.Size = new System.Drawing.Size(0, 16);
            this.lblRisk.TabIndex = 13;
            // 
            // PredictionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRisk);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPressure);
            this.Controls.Add(this.txtTempHumidity);
            this.Controls.Add(this.txtSoil);
            this.Controls.Add(this.txtRiver);
            this.Controls.Add(this.txtRainfall);
            this.Name = "PredictionForm";
            this.Text = "PredictionForm";
            this.Load += new System.EventHandler(this.PredictionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRainfall;
        private System.Windows.Forms.TextBox txtRiver;
        private System.Windows.Forms.TextBox txtSoil;
        private System.Windows.Forms.TextBox txtTempHumidity;
        private System.Windows.Forms.TextBox txtPressure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblRisk;
    }
}