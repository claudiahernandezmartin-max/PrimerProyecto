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
            this.dataGridViewVuelos = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVuelos)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewVuelos
            // 
            this.dataGridViewVuelos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVuelos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewVuelos.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewVuelos.Name = "dataGridViewVuelos";
            this.dataGridViewVuelos.RowHeadersWidth = 51;
            this.dataGridViewVuelos.RowTemplate.Height = 24;
            this.dataGridViewVuelos.Size = new System.Drawing.Size(800, 450);
            this.dataGridViewVuelos.TabIndex = 0;
            this.dataGridViewVuelos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewVols_CellContentClick);
            // 
            // FlightDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewVuelos);
            this.Name = "FlightDataForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVuelos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewVuelos;
    }
}