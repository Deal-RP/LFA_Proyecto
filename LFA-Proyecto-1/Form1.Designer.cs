namespace LFA_Proyecto_1
{
    partial class Form1
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
            this.TB_Ruta = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lResultado = new System.Windows.Forms.Label();
            this.Btn_subir = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BTN_Generar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TB_Ruta
            // 
            this.TB_Ruta.Location = new System.Drawing.Point(33, 52);
            this.TB_Ruta.Name = "TB_Ruta";
            this.TB_Ruta.Size = new System.Drawing.Size(163, 20);
            this.TB_Ruta.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "ARCHIVO SUBIDO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lResultado
            // 
            this.lResultado.AutoSize = true;
            this.lResultado.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lResultado.Location = new System.Drawing.Point(251, 29);
            this.lResultado.Name = "lResultado";
            this.lResultado.Size = new System.Drawing.Size(0, 20);
            this.lResultado.TabIndex = 3;
            this.lResultado.Visible = false;
            // 
            // Btn_subir
            // 
            this.Btn_subir.Location = new System.Drawing.Point(33, 79);
            this.Btn_subir.Name = "Btn_subir";
            this.Btn_subir.Size = new System.Drawing.Size(163, 24);
            this.Btn_subir.TabIndex = 4;
            this.Btn_subir.Text = "Subir archivo";
            this.Btn_subir.UseVisualStyleBackColor = true;
            this.Btn_subir.Click += new System.EventHandler(this.Btn_subir_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.Location = new System.Drawing.Point(234, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // BTN_Generar
            // 
            this.BTN_Generar.Enabled = false;
            this.BTN_Generar.Location = new System.Drawing.Point(33, 110);
            this.BTN_Generar.Name = "BTN_Generar";
            this.BTN_Generar.Size = new System.Drawing.Size(163, 23);
            this.BTN_Generar.TabIndex = 6;
            this.BTN_Generar.Text = "Generar programa";
            this.BTN_Generar.UseVisualStyleBackColor = true;
            this.BTN_Generar.Click += new System.EventHandler(this.BTN_Generar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(703, 328);
            this.Controls.Add(this.BTN_Generar);
            this.Controls.Add(this.Btn_subir);
            this.Controls.Add(this.lResultado);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_Ruta);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TB_Ruta;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lResultado;
        private System.Windows.Forms.Button Btn_subir;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BTN_Generar;
    }
}

