using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFA_Proyecto_1
{
    class ExpresionRegular
    {
        public static void EvaluarSet(string linea)
        {



            foreach (var item in linea.Trim(new char[] { '\t', ' ' }))
            {
                if (char.IsUpper(item))
                {
                    Console.WriteLine(item + " - mayu");
                }
                else
                {
                    Console.WriteLine(item + " - minus");
                }
            }
        }

        void arbolExpresion(Nodo raiz, string operacion, string dato1, string dato2 = null)
        {
            if (raiz == null)
            {
                raiz.simbolo = operacion;
                raiz.Hijo1 = new Nodo { simbolo = dato1};
                raiz.Hijo2 = new Nodo { simbolo = dato2 };
            }
            else
            {

            }
        }

        void insertar(Nodo actual, string operacion )
        {

        }

        public static void EvaluarToken()
        {

        }
    }
}
