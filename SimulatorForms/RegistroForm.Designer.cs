namespace SimulatorForms
{
    partial class RegistroForm
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
            this.labelUsuario = new System.Windows.Forms.Label();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelRepetir = new System.Windows.Forms.Label();
            this.textBoxRepetir = new System.Windows.Forms.TextBox();
            this.buttonCrear = new System.Windows.Forms.Button();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labelUsuario
            //
            this.labelUsuario.AutoSize = true;
            this.labelUsuario.Location = new System.Drawing.Point(40, 30);
            this.labelUsuario.Name = "labelUsuario";
            this.labelUsuario.Size = new System.Drawing.Size(54, 16);
            this.labelUsuario.TabIndex = 0;
            this.labelUsuario.Text = "Usuario";
            //
            // textBoxUsuario
            //
            this.textBoxUsuario.Location = new System.Drawing.Point(43, 52);
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.Size = new System.Drawing.Size(250, 22);
            this.textBoxUsuario.TabIndex = 1;
            //
            // labelPassword
            //
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(40, 90);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(76, 16);
            this.labelPassword.TabIndex = 2;
            this.labelPassword.Text = "Contraseña";
            //
            // textBoxPassword
            //
            this.textBoxPassword.Location = new System.Drawing.Point(43, 112);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(250, 22);
            this.textBoxPassword.TabIndex = 3;
            //
            // labelRepetir
            //
            this.labelRepetir.AutoSize = true;
            this.labelRepetir.Location = new System.Drawing.Point(40, 150);
            this.labelRepetir.Name = "labelRepetir";
            this.labelRepetir.Size = new System.Drawing.Size(129, 16);
            this.labelRepetir.TabIndex = 4;
            this.labelRepetir.Text = "Repetir contraseña";
            //
            // textBoxRepetir
            //
            this.textBoxRepetir.Location = new System.Drawing.Point(43, 172);
            this.textBoxRepetir.Name = "textBoxRepetir";
            this.textBoxRepetir.PasswordChar = '*';
            this.textBoxRepetir.Size = new System.Drawing.Size(250, 22);
            this.textBoxRepetir.TabIndex = 5;
            //
            // buttonCrear
            //
            this.buttonCrear.Location = new System.Drawing.Point(43, 225);
            this.buttonCrear.Name = "buttonCrear";
            this.buttonCrear.Size = new System.Drawing.Size(120, 40);
            this.buttonCrear.TabIndex = 6;
            this.buttonCrear.Text = "Crear cuenta";
            this.buttonCrear.UseVisualStyleBackColor = true;
            this.buttonCrear.Click += new System.EventHandler(this.buttonCrear_Click);
            //
            // buttonCancelar
            //
            this.buttonCancelar.Location = new System.Drawing.Point(173, 225);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(120, 40);
            this.buttonCancelar.TabIndex = 7;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click);
            //
            // RegistroForm
            //
            this.AcceptButton = this.buttonCrear;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 295);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.buttonCrear);
            this.Controls.Add(this.textBoxRepetir);
            this.Controls.Add(this.labelRepetir);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxUsuario);
            this.Controls.Add(this.labelUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RegistroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Crear cuenta";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUsuario;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelRepetir;
        private System.Windows.Forms.TextBox textBoxRepetir;
        private System.Windows.Forms.Button buttonCrear;
        private System.Windows.Forms.Button buttonCancelar;
    }
}
