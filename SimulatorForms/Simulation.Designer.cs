using System;
using System.Drawing;
using System.Windows.Forms; 
using FlightLib;

namespace SimulatorForms
{
    partial class Simulation : Form
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
            this.button1 = new System.Windows.Forms.Button();
            this.labelEstado = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewVelocidades = new System.Windows.Forms.DataGridView();
            this.columnaVuelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnaVelocidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonVelocidadesOriginales = new System.Windows.Forms.Button();
            this.labelDistancia = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVelocidades)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSimulacion
            // 
            this.panelSimulacion.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.panelSimulacion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSimulacion.Location = new System.Drawing.Point(12, 12);
            this.panelSimulacion.Name = "panelSimulacion";
            this.panelSimulacion.Size = new System.Drawing.Size(826, 576);
            this.panelSimulacion.TabIndex = 0;
            this.panelSimulacion.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSimulacion_Paint);
            this.panelSimulacion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelSimulacion_MouseClick);
            // 
            // buttonMover
            // 
            this.buttonMover.Location = new System.Drawing.Point(857, 49);
            this.buttonMover.Name = "buttonMover";
            this.buttonMover.Size = new System.Drawing.Size(143, 61);
            this.buttonMover.TabIndex = 1;
            this.buttonMover.Text = "Mover";
            this.buttonMover.UseVisualStyleBackColor = true;
            this.buttonMover.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonIniciar
            // 
            this.buttonIniciar.Location = new System.Drawing.Point(857, 128);
            this.buttonIniciar.Name = "buttonIniciar";
            this.buttonIniciar.Size = new System.Drawing.Size(143, 63);
            this.buttonIniciar.TabIndex = 2;
            this.buttonIniciar.Text = "Iniciar";
            this.buttonIniciar.UseVisualStyleBackColor = true;
            this.buttonIniciar.Click += new System.EventHandler(this.buttonIniciar_Click);
            // 
            // buttonDetener
            // 
            this.buttonDetener.Enabled = false;
            this.buttonDetener.Location = new System.Drawing.Point(857, 216);
            this.buttonDetener.Name = "buttonDetener";
            this.buttonDetener.Size = new System.Drawing.Size(143, 64);
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
            this.buttonVerDatos.Location = new System.Drawing.Point(857, 305);
            this.buttonVerDatos.Name = "buttonVerDatos";
            this.buttonVerDatos.Size = new System.Drawing.Size(143, 64);
            this.buttonVerDatos.TabIndex = 4;
            this.buttonVerDatos.Text = "Ver datos";
            this.buttonVerDatos.UseVisualStyleBackColor = true;
            this.buttonVerDatos.Click += new System.EventHandler(this.buttonVerDatos_Click_1);
            // 
            // Conflicte
            // 
            this.Conflicte.Location = new System.Drawing.Point(857, 392);
            this.Conflicte.Name = "Conflicte";
            this.Conflicte.Size = new System.Drawing.Size(143, 68);
            this.Conflicte.TabIndex = 5;
            this.Conflicte.Text = "Conflicto";
            this.Conflicte.UseVisualStyleBackColor = true;
            this.Conflicte.Click += new System.EventHandler(this.Conflicte_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(857, 482);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 61);
            this.button1.TabIndex = 6;
            this.button1.Text = "Restart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Restart_Click);
            // 
            // labelEstado
            // 
            this.labelEstado.AutoSize = true;
            this.labelEstado.Location = new System.Drawing.Point(293, 640);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.Size = new System.Drawing.Size(50, 16);
            this.labelEstado.TabIndex = 7;
            this.labelEstado.Text = "Estado";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1028, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Vuelos y velocidades";
            // 
            // dataGridViewVelocidades
            // 
            this.dataGridViewVelocidades.AllowUserToAddRows = false;
            this.dataGridViewVelocidades.AllowUserToDeleteRows = false;
            this.dataGridViewVelocidades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewVelocidades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVelocidades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnaVuelo,
            this.columnaVelocidad});
            this.dataGridViewVelocidades.Location = new System.Drawing.Point(1031, 87);
            this.dataGridViewVelocidades.Name = "dataGridViewVelocidades";
            this.dataGridViewVelocidades.RowHeadersVisible = false;
            this.dataGridViewVelocidades.RowHeadersWidth = 51;
            this.dataGridViewVelocidades.RowTemplate.Height = 24;
            this.dataGridViewVelocidades.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewVelocidades.TabIndex = 9;
            this.dataGridViewVelocidades.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewVelocidades_CellEndEdit);
            // 
            // columnaVuelo
            // 
            this.columnaVuelo.HeaderText = "Vuelo";
            this.columnaVuelo.MinimumWidth = 6;
            this.columnaVuelo.Name = "columnaVuelo";
            // 
            // columnaVelocidad
            // 
            this.columnaVelocidad.HeaderText = "Velocidad";
            this.columnaVelocidad.MinimumWidth = 6;
            this.columnaVelocidad.Name = "columnaVelocidad";
            // 
            // buttonVelocidadesOriginales
            // 
            this.buttonVelocidadesOriginales.Location = new System.Drawing.Point(1088, 263);
            this.buttonVelocidadesOriginales.Name = "buttonVelocidadesOriginales";
            this.buttonVelocidadesOriginales.Size = new System.Drawing.Size(143, 64);
            this.buttonVelocidadesOriginales.TabIndex = 10;
            this.buttonVelocidadesOriginales.Text = "Originales";
            this.buttonVelocidadesOriginales.UseVisualStyleBackColor = true;
            this.buttonVelocidadesOriginales.Click += new System.EventHandler(this.buttonVelocidadesOriginales_Click);
            // 
            // labelDistancia
            // 
            this.labelDistancia.AutoSize = true;
            this.labelDistancia.Location = new System.Drawing.Point(122, 601);
            this.labelDistancia.Name = "labelDistancia";
            this.labelDistancia.Size = new System.Drawing.Size(128, 16);
            this.labelDistancia.TabIndex = 11;
            this.labelDistancia.Text = "Pareja más cercana";
            this.labelDistancia.Click += new System.EventHandler(this.label2_Click);
            // 
            // Simulation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1332, 686);
            this.Controls.Add(this.labelDistancia);
            this.Controls.Add(this.buttonVelocidadesOriginales);
            this.Controls.Add(this.dataGridViewVelocidades);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelEstado);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Conflicte);
            this.Controls.Add(this.buttonVerDatos);
            this.Controls.Add(this.buttonDetener);
            this.Controls.Add(this.buttonIniciar);
            this.Controls.Add(this.buttonMover);
            this.Controls.Add(this.panelSimulacion);
            this.Name = "Simulation";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Simulation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVelocidades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelSimulacion;
        private System.Windows.Forms.Button buttonMover;
        private System.Windows.Forms.Button buttonIniciar;
        private System.Windows.Forms.Button buttonDetener;
        private System.Windows.Forms.Timer timerSimulacion;
        private System.Windows.Forms.Button buttonVerDatos;
        private System.Windows.Forms.Button Conflicte;
        private Button button1;
        private Label labelEstado;
        private Label label1;
        private DataGridView dataGridViewVelocidades;
        private DataGridViewTextBoxColumn columnaVuelo;
        private DataGridViewTextBoxColumn columnaVelocidad;
        private Button buttonVelocidadesOriginales;
        private Label labelDistancia;
    }
}