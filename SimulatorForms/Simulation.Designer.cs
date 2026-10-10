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
            this.components = new System.ComponentModel.Container();
            this.panelSimulacion = new System.Windows.Forms.Panel();
            this.buttonMover = new System.Windows.Forms.Button();
            this.buttonIniciar = new System.Windows.Forms.Button();
            this.buttonDetener = new System.Windows.Forms.Button();
            this.timerSimulacion = new System.Windows.Forms.Timer(this.components);
            this.buttonVerDatos = new System.Windows.Forms.Button();
            this.Conflicte = new System.Windows.Forms.Button();
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
            this.buttonMover.Location = new System.Drawing.Point(539, 12);
            this.buttonMover.Name = "buttonMover";
            this.buttonMover.Size = new System.Drawing.Size(135, 54);
            this.buttonMover.TabIndex = 1;
            this.buttonMover.Text = "Mover";
            this.buttonMover.UseVisualStyleBackColor = true;
            this.buttonMover.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonIniciar
            // 
            this.buttonIniciar.Location = new System.Drawing.Point(539, 91);
            this.buttonIniciar.Name = "buttonIniciar";
            this.buttonIniciar.Size = new System.Drawing.Size(135, 56);
            this.buttonIniciar.TabIndex = 2;
            this.buttonIniciar.Text = "Iniciar";
            this.buttonIniciar.UseVisualStyleBackColor = true;
            this.buttonIniciar.Click += new System.EventHandler(this.buttonIniciar_Click);
            // 
            // buttonDetener
            // 
            this.buttonDetener.Enabled = false;
            this.buttonDetener.Location = new System.Drawing.Point(539, 179);
            this.buttonDetener.Name = "buttonDetener";
            this.buttonDetener.Size = new System.Drawing.Size(135, 57);
            this.buttonDetener.TabIndex = 3;
            this.buttonDetener.Text = "Detener";
            this.buttonDetener.UseVisualStyleBackColor = true;
            this.buttonDetener.Click += new System.EventHandler(this.buttonDetener_Click);
            // 
            // timerSimulacion
            // 
            this.timerSimulacion.Tick += new System.EventHandler(this.timerSimulacion_Tick);
            // 
            // buttonVerDatos
            // 
            this.buttonVerDatos.Location = new System.Drawing.Point(539, 268);
            this.buttonVerDatos.Name = "buttonVerDatos";
            this.buttonVerDatos.Size = new System.Drawing.Size(135, 57);
            this.buttonVerDatos.TabIndex = 4;
            this.buttonVerDatos.Text = "Ver datos";
            this.buttonVerDatos.UseVisualStyleBackColor = true;
            // 
            // Conflicte
            // 
            this.Conflicte.Location = new System.Drawing.Point(539, 355);
            this.Conflicte.Name = "Conflicte";
            this.Conflicte.Size = new System.Drawing.Size(135, 57);
            this.Conflicte.TabIndex = 5;
            this.Conflicte.Text = "Conflicto";
            this.Conflicte.UseVisualStyleBackColor = true;
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 453);
            this.Controls.Add(this.Conflicte);
            this.Controls.Add(this.buttonVerDatos);
            this.Controls.Add(this.buttonDetener);
            this.Controls.Add(this.buttonIniciar);
            this.Controls.Add(this.buttonMover);
            this.Controls.Add(this.panelSimulacion);
            this.Name = "Simulation";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSimulacion;
        private System.Windows.Forms.Button buttonMover;
        private System.Windows.Forms.Button buttonIniciar;
        private System.Windows.Forms.Button buttonDetener;
        private System.Windows.Forms.Timer timerSimulacion;
        private System.Windows.Forms.Button buttonVerDatos;
        private System.Windows.Forms.Button Conflicte;
    }
}