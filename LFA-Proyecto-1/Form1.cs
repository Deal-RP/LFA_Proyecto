using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LFA_Proyecto_1
{
    public partial class Form1 : Form
    {
        Graphics g;
        Nodo Arbol_ER;
        bool limpieza;

        public Form1()
        {
            InitializeComponent();
        }

        private void Btn_subir_Click(object sender, EventArgs e)
        {
            limpieza = true;
            Logica.ER = string.Empty;
            Refresh();
            limpieza = false;
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.Filter = "txt files (*.txt)|*.txt";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    TB_Ruta.Text = open.FileName;
                    var fileStream = open.OpenFile();
                    lResultado.Text = Logica.Lectura(fileStream);
                    lResultado.Visible = true;
                    if (lResultado.Text == "FORMATO CORRECTO")
                    {
                        Arbol_ER = Logica.CreacionArbol();
                        lResultado.ForeColor = Color.Black;
                        Refresh();

                        #region CREACION DE LOS DATAGRIDVIEW
                        //First, Last y Nullable
                        var TablaFLN = new DataGridView();
                        TablaFLN.Columns.Add("sim", "SIMBOLO");
                        TablaFLN.Columns.Add("first", "FIRST");
                        TablaFLN.Columns.Add("last", "LAST");
                        TablaFLN.Columns.Add("null", "NULLABLE");
                        TablaFLN.Location = new Point(0, 300);
                        TablaFLN.RowHeadersVisible = false;
                        TablaFLN.Width = 500;
                        TablaFLN.Height = 300;
                        TablaFLN.ScrollBars = ScrollBars.Vertical;
                        TablaFLN.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        panel1.Controls.Add(TablaFLN);

                        //Follows
                        var TablaF = new DataGridView();
                        TablaF.Columns.Add("sim", "SIMBOLO");
                        TablaF.Columns.Add("first", "FOLLOW");
                        TablaF.Location = new Point(525, 300);
                        TablaF.RowHeadersVisible = false;
                        TablaF.Width = 500;
                        TablaF.Height = 300;
                        TablaF.ScrollBars = ScrollBars.Vertical;
                        TablaF.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        panel1.Controls.Add(TablaF);

                        //Estados
                        var TablaS = new DataGridView();
                        TablaS.Columns.Add("sim", "SIMBOLO");
                        TablaS.Location = new Point(1050, 300);
                        TablaS.RowHeadersVisible = false;
                        TablaS.Width = 500;
                        TablaS.Height = 300;
                        TablaS.ScrollBars = ScrollBars.Both;
                        panel1.Controls.Add(TablaS);

                        //Gestion de Tabla(First, Last y Nullable)
                        Logica.cont = 1;
                        Logica.listTerminales = new List<string>();
                        Logica.Follows = new Dictionary<int, List<int>>();
                        Logica.TablaFLN(Arbol_ER, TablaFLN);
                        Logica.TablaF(TablaF);
                        Logica.TablaEstados(Arbol_ER, TablaS);
                        #endregion

                        Generar.CrearPrograma();
                    }
                    else
                    {
                        lResultado.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (limpieza)
            {
                e.Graphics.Clear(this.BackColor);
                panel1.Controls.Clear();
                TB_Ruta.Clear();
                lResultado.Text = string.Empty;
                lResultado.Visible = false;
            }
            else
            {
                //Grafica el arbol en el panel
                e.Graphics.Clear(this.BackColor);
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g = e.Graphics;
                if (Logica.ER != null && Logica.ER != "" && Arbol_ER != null)
                {
                    var x = Logica.MostrarArbol(Arbol_ER, g, this.Font, Brushes.LightGray, Brushes.Black, Pens.Black, Brushes.White);
                    var lbl = new Label();
                    lbl.Text = "";
                    lbl.Location = new Point(x, Logica.altura + 200);
                    panel1.Controls.Add(lbl);
                }
            }
        }
    }
}
