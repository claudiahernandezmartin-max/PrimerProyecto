namespace SimulatorForms
{
    partial class FlightDataForm
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
            this.dataGridViewVols = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVols)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewVols
            // 
            this.dataGridViewVols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVols.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewVols.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewVols.Name = "dataGridViewVols";
            this.dataGridViewVols.RowHeadersWidth = 51;
            this.dataGridViewVols.RowTemplate.Height = 24;
            this.dataGridViewVols.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewVols.TabIndex = 0;
            // 
            // FlightDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewVols);
            this.Name = "FlightDataForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVols)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewVols;
    }
}