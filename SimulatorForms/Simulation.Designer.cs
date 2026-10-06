namespace SimulatorForms
{
    partial class Simulation
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
            this.panelSimulacion = new System.Windows.Forms.Panel();
            this.buttonMover = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelSimulacion
            // 
            this.panelSimulacion.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelSimulacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSimulacion.Location = new System.Drawing.Point(12, 12);
            this.panelSimulacion.Name = "panelSimulacion";
            this.panelSimulacion.Size = new System.Drawing.Size(490, 400);
            this.panelSimulacion.TabIndex = 0;
            this.panelSimulacion.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSimulacion_Paint);
            this.panelSimulacion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelSimulacion_MouseClick);
            // 
            // buttonMover
            // 
            this.buttonMover.Location = new System.Drawing.Point(539, 33);
            this.buttonMover.Name = "buttonMover";
            this.buttonMover.Size = new System.Drawing.Size(75, 23);
            this.buttonMover.TabIndex = 1;
            this.buttonMover.Text = "Mover";
            this.buttonMover.UseVisualStyleBackColor = true;
            this.buttonMover.Click += new System.EventHandler(this.button1_Click);
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 453);
            this.Controls.Add(this.buttonMover);
            this.Controls.Add(this.panelSimulacion);
            this.Name = "Simulation";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSimulacion;
        private System.Windows.Forms.Button buttonMover;
    }
}