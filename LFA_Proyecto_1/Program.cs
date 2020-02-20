using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFA_Proyecto_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Console.ReadLine();

            using (var sr = new StreamReader(path))
            {
                var linea = string.Empty;
                var contLinea = 1;
                var set = false;
                var token = false;
                var action = false;
                var error = false;
                var ErrorLexico = false;

                while ((linea = sr.ReadLine()) != null && !ErrorLexico)
                {
                    var prueba = linea.Trim(' ');
                    switch (linea.Trim(' '))
                    {
                        case "SETS":
                            if (!set)
                            {
                                while ((linea = sr.ReadLine()) != null && !ErrorLexico)
                                {
                                    ExpresionRegular.EvaluarSet(linea);
                                    contLinea++;
                                }

                                set = true;
                            }
                            else
                            {
                                ErrorLexico = true;
                            }
                            break;

                        case "TOKENS":
                            if (!token)
                            {
                                while ((linea = sr.ReadLine()) != null && !ErrorLexico)
                                {
                                    ExpresionRegular.EvaluarToken();
                                    contLinea++;
                                }
                                token = true;
                            }
                            else
                            {
                                ErrorLexico = true;
                            }
                            break;

                        case "ACTIONS":
                            if (!action)
                            {
                                while ((linea = sr.ReadLine()) != null && !ErrorLexico)
                                {
                                    ExpresionRegular.EvaluarToken();
                                    contLinea++;
                                }
                                action = true;
                            }
                            else
                            {
                                ErrorLexico = true;
                            }
                            break;

                        case "ERROR":
                            if (!error)
                            {
                                while ((linea = sr.ReadLine()) != null && !ErrorLexico)
                                {
                                    ExpresionRegular.EvaluarToken();
                                    contLinea++;
                                }
                                error = true;
                            }
                            else
                            {
                                ErrorLexico = true;
                            }
                            break;

                        case "":
                            break;

                        default:
                            ErrorLexico = true;
                            break;
                    }

                    ////Evaluar que todo sea mayuscula
                    //var mayuscula = linea.Any(c => char.IsUpper(c));

                    ////Evaluar que todo sea minuscula
                    //var minuscula = linea.Any(c => char.IsLower(c));

                    //Console.WriteLine(linea);

                    contLinea++;
                }
            }

            Console.ReadKey();
        }
    }
}
