namespace SimulatorForms
{
    partial class Menu
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.planesDeVueloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simulacionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.planesDeVueloToolStripMenuItem,
            this.distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem,
            this.simulacionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1654, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // planesDeVueloToolStripMenuItem
            // 
            this.planesDeVueloToolStripMenuItem.Name = "planesDeVueloToolStripMenuItem";
            this.planesDeVueloToolStripMenuItem.Size = new System.Drawing.Size(202, 38);
            this.planesDeVueloToolStripMenuItem.Text = "Planes de vuelo";
            this.planesDeVueloToolStripMenuItem.Click += new System.EventHandler(this.planesDeVueloToolStripMenuItem_Click);
            // 
            // distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem
            // 
            this.distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem.Name = "distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem";
            this.distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem.Size = new System.Drawing.Size(467, 38);
            this.distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem.Text = "Distancia de seguridad y tiempo de ciclo";
            // 
            // simulacionToolStripMenuItem
            // 
            this.simulacionToolStripMenuItem.Name = "simulacionToolStripMenuItem";
            this.simulacionToolStripMenuItem.Size = new System.Drawing.Size(151, 38);
            this.simulacionToolStripMenuItem.Text = "Simulacion";
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1654, 794);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Menu";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem planesDeVueloToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem distanciaDeSeguridadYTiempoDeCicloToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem simulacionToolStripMenuItem;
    }
}

