using System.Collections.Generic;
using System.Drawing;

namespace LFA_Proyecto_1
{
    class Nodo
    {
        public string simbolo { get; set; }
        public Nodo HijoIzquierdo { get; set; }
        public Nodo HijoDerecho { get; set; }
        public bool Nullable = false;
        public List<int> First = new List<int>();
        public List<int> Last = new List<int>();

        public string[] FuncionesFLN()
        {
            if (simbolo == "|")
            {
                //First
                foreach (var item in HijoIzquierdo.First)
                {
                    First.Add(item);
                }
                foreach (var item in HijoDerecho.First)
                {
                    First.Add(item);
                }

                //Last
                foreach (var item in HijoIzquierdo.Last)
                {
                    Last.Add(item);
                }
                foreach (var item in HijoDerecho.Last)
                {
                    Last.Add(item);
                }

                //Nullable
                if (HijoIzquierdo.Nullable || HijoDerecho.Nullable)
                {
                    Nullable = true;
                }
            }
            else if (simbolo == ".")
            {
                //First
                if (HijoIzquierdo.Nullable)
                {
                    foreach (var item in HijoIzquierdo.First)
                    {
                        First.Add(item);
                    }
                    foreach (var item in HijoDerecho.First)
                    {
                        First.Add(item);
                    }
                }
                else
                {
                    First = HijoIzquierdo.First;
                }

                //Last
                if (HijoDerecho.Nullable)
                {
                    foreach (var item in HijoIzquierdo.Last)
                    {
                        Last.Add(item);
                    }
                    foreach (var item in HijoDerecho.Last)
                    {
                        Last.Add(item);
                    }
                }
                else
                {
                    Last = HijoDerecho.Last;
                }

                //Nullable
                if (HijoIzquierdo.Nullable && HijoDerecho.Nullable)
                {
                    Nullable = true;
                }

                //Follow
                foreach (var L in HijoIzquierdo.Last)
                {
                    if (Logica.Follows.ContainsKey(L))
                    {
                        foreach (var First in HijoDerecho.First)
                        {
                            if (!Logica.Follows[L].Contains(First))
                            {
                                Logica.Follows[L].Add(First);
                            }
                        }
                    }
                    else
                    {
                        Logica.Follows.Add(L, HijoDerecho.First);
                    }
                }
            }
            else if (simbolo == "*" || simbolo == "?" || simbolo == "+")
            {
                //First
                First = HijoIzquierdo.First;
                //Last
                Last = HijoIzquierdo.Last;

                //Nullable
                if (simbolo == "*" || simbolo == "?")
                {
                    Nullable = true;
                }

                //Follow
                if (simbolo == "*" || simbolo == "+")
                {
                    foreach (var L in HijoIzquierdo.Last)
                    {
                        if (Logica.Follows.ContainsKey(L))
                        {
                            foreach (var First in HijoIzquierdo.First)
                            {
                                if (!Logica.Follows[L].Contains(First))
                                {
                                    Logica.Follows[L].Add(First);
                                }
                            }
                        }
                        else
                        {
                            Logica.Follows.Add(L, HijoIzquierdo.First);
                        }
                    }
                }
            }
            else
            {
                First.Add(Logica.cont);
                Last.Add(Logica.cont);

                Logica.listTerminales.Add(simbolo);

                Logica.cont++;
            }

            var fila = new List<string> { simbolo };

            var aux = string.Empty;

            foreach (var item in First)
            {
                aux += $"{item},";
            }
            fila.Add(aux.TrimEnd(','));

            aux = string.Empty;

            foreach (var item in Last)
            {
                aux += $"{item},";
            }
            fila.Add(aux.TrimEnd(','));

            fila.Add(Nullable ? "True" : "False");

            return fila.ToArray();
        }

        #region GRAFICAR ARBOL
        private const int Radio = 30;
        private const int DistanciaH = 80;
        private const int DistanciaV = 10;

        private int CoordenadaX;
        private int CoordenadaY;

        public void PosNodo(ref int x, int y)
        {
            int aux1, aux2;
            CoordenadaY = (int)(y + Radio / 2);

            if (HijoIzquierdo != null)
            {
                HijoIzquierdo.PosNodo(ref x, y + Radio + DistanciaV);
                if (y + Radio + DistanciaV > Logica.altura)
                {
                    Logica.altura = y + Radio + DistanciaV;
                }
            }

            if (HijoIzquierdo != null && HijoDerecho != null)
            {
                x += DistanciaH;
            }

            if (HijoDerecho != null)
            {
                HijoDerecho.PosNodo(ref x, y + Radio + DistanciaV);
                if (y + Radio + DistanciaV > Logica.altura)
                {
                    Logica.altura = y + Radio + DistanciaV;
                }
            }

            if (HijoIzquierdo != null && HijoDerecho != null)
            {
                CoordenadaX = (int)((HijoIzquierdo.CoordenadaX + HijoDerecho.CoordenadaX) / 2);
            }
            else if (HijoIzquierdo != null)
            {
                aux1 = HijoIzquierdo.CoordenadaX;
                HijoIzquierdo.CoordenadaX = CoordenadaX - 80;
                CoordenadaX = aux1;
            }
            else if (HijoDerecho != null)
            {
                aux2 = HijoDerecho.CoordenadaX;
                HijoDerecho.CoordenadaX = CoordenadaX + 80;
                CoordenadaX = aux2;
            }
            else
            {
                CoordenadaX = (int)(x + Radio / 2);
                x += Radio;
            }
        }

        public void MostrarRamas(Graphics grafo, Pen lapiz)
        {
            if (HijoIzquierdo != null)
            {
                grafo.DrawLine(lapiz, CoordenadaX, CoordenadaY, HijoIzquierdo.CoordenadaX, HijoIzquierdo.CoordenadaY);
                HijoIzquierdo.MostrarRamas(grafo, lapiz);
            }

            if (HijoDerecho != null)
            {
                grafo.DrawLine(lapiz, CoordenadaX, CoordenadaY, HijoDerecho.CoordenadaX, HijoDerecho.CoordenadaY);
                HijoDerecho.MostrarRamas(grafo, lapiz);
            }
        }

        public void MostrarNodo(Graphics grafo, Font fuente, Brush relleno, Brush rellenoFuente, Pen lapiz, Brush encuentro)
        {
            var rect = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            grafo.FillEllipse(encuentro, rect);
            grafo.FillEllipse(relleno, rect);
            grafo.DrawEllipse(lapiz, rect);

            var formato = new StringFormat();
            formato.Alignment = StringAlignment.Center;
            formato.LineAlignment = StringAlignment.Center;
            grafo.DrawString(simbolo, fuente, rellenoFuente, CoordenadaX, CoordenadaY, formato);

            if (HijoIzquierdo != null)
            {
                HijoIzquierdo.MostrarNodo(grafo, fuente, relleno, rellenoFuente, lapiz, encuentro);
            }
            if (HijoDerecho != null)
            {
                HijoDerecho.MostrarNodo(grafo, fuente, relleno, rellenoFuente, lapiz, encuentro);

            }
        }

        public void colorear(Graphics grafo, Font fuente, Brush relleno, Brush rellenoFuente, Pen lapiz)
        {
            var rect = new Rectangle((int)(CoordenadaX - Radio / 2), (int)(CoordenadaY - Radio / 2), Radio, Radio);
            grafo.FillEllipse(relleno, rect);
            grafo.DrawEllipse(lapiz, rect);

            var formato = new StringFormat();

            formato.Alignment = StringAlignment.Center;
            formato.LineAlignment = StringAlignment.Center;
            grafo.DrawString(simbolo, fuente, rellenoFuente, CoordenadaX, CoordenadaY, formato);
        }
        #endregion
    }
}
