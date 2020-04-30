using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace LFA_Proyecto_1
{
    class Generar
    {
        static void BuildExe()
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            //C:\Windows\Microsoft.NET\Framework64\v4.0.30319 Scanner.cs
            //csc Scanner.cs
            process.StandardInput.WriteLine("C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319 Program.cs");
            process.StandardInput.Flush();
            process.StandardInput.Close();
            process.WaitForExit();
        }

        public static List<int> aprobacion = new List<int>();

        public static void CrearPrograma()
        {
            Logica.Sets = new List<string>();
            Logica.listTerminales.Remove("#");
            foreach (var n in Logica.listTerminales)
            {
                if (n[0] == '\'' && !Logica.Sets.Contains($"{(int)n[1]}|"))
                {
                    Logica.Sets.Add($"{Convert.ToString((int)n[1])}|");
                }
                else if(Logica.auxSets.ContainsKey(n))
                {
                    Logica.Sets.Add($"{n}|{Logica.auxSets[n]}");
                    Logica.auxSets.Remove(n);
                }
            }

            if (File.Exists("Generico\\Generico\\Program.cs"))
            {
                File.Delete("Generico\\Generico\\Program.cs");
            }

            using (var writer = new StreamWriter("Generico\\Generico\\Program.cs"))
            {

                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Text;");
                writer.WriteLine("using System.IO;");
                writer.WriteLine("");
                writer.WriteLine("namespace Generico");
                writer.WriteLine("{");
                writer.WriteLine("\tclass Program");
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tstatic List<string> T = new List<string>();");
                writer.WriteLine("\t\tstatic Dictionary<string, int> Actions = new Dictionary<string, int>();");
                writer.WriteLine($"\t\tstatic int[,] matrizEstado = new int[{Logica.estadosTransportar.Count}, {Logica.Sets.Count}]");
                writer.WriteLine("\t\t{");
                var cont = 1;
                foreach (var estado in Logica.estados.Values)
                {
                    var aux = "{";
                    foreach (var key in estado)
                    {
                        var aux1 = "";
                        foreach (var miniEstado in key.Value)
                        {
                            aux1 += $"{miniEstado},";
                        }
                        aux1 = aux1.TrimEnd(',');
                        aux += Logica.estadosTransportar.IndexOf(aux1).ToString() + ",";
                    }
                    aux = aux.TrimEnd(',') + "}";
                    if (cont == Logica.estados.Count)
                    {
                        writer.WriteLine($"\t\t\t{aux}");
                    }
                    else
                    {
                        writer.WriteLine($"\t\t\t{aux},");
                    }
                    cont++;
                }
                writer.WriteLine("\t\t};");
                writer.WriteLine("\t\tstatic List<int> aprobacion = new List<int>();");

                writer.WriteLine("\t\tstatic void Main(string[] args)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tvar opc = 0;");
                writer.WriteLine("\t\t\tRellenarDatos();");
                writer.WriteLine("\t\t\twhile (opc != 3)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"Menu\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"1.Ingresar Archivo de.txt\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"2.Ingresar cadena\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"3.Salir\");");
                writer.WriteLine("\t\t\t\topc = Convert.ToInt32(Console.ReadLine());");

                writer.WriteLine("\t\t\t\tvar txt = string.Empty;");

                writer.WriteLine("\t\t\t\tswitch (opc)");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tcase 1:");

                writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(\"Arrastre el archivo a consola\");");
                writer.WriteLine("\t\t\t\t\t\ttxt = File.ReadAllText(Console.ReadLine().Trim('\"'));");
                writer.WriteLine("\t\t\t\t\t\tbreak;");

                writer.WriteLine("\t\t\t\t\tcase 2:");

                writer.WriteLine("\t\t\t\t\t\tConsole.WriteLine(\"Ingrese cadena a evaluar\");");
                writer.WriteLine("\t\t\t\t\t\ttxt = Console.ReadLine();");
                writer.WriteLine("\t\t\t\t\t\tbreak;");
                writer.WriteLine("\t\t\t\t}");


                writer.WriteLine("\t\t\t\tvar correct = true;");
                writer.WriteLine("\t\t\t\tvar aux = string.Empty;");

                writer.WriteLine("\t\t\t\tvar Token = string.Empty;");
                writer.WriteLine("\t\t\t\tvar estado = 0;");

                writer.WriteLine("\t\t\t\twhile (correct && txt.Length != 0)");
                writer.WriteLine("\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\tvar controlReservada = true;");

                writer.WriteLine("\t\t\t\t\tvar cont = 0;");

                
                
                writer.WriteLine("\t\t\t\t\tforeach (var action in Actions)");
                writer.WriteLine("\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\tif (txt.Length >= action.Key.Length)");
                writer.WriteLine("\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\tcont = 0;");
                writer.WriteLine("\t\t\t\t\t\t\twhile (cont < action.Key.Length && action.Key[cont] == Convert.ToChar(txt[cont].ToString().ToUpper()))");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\tcont++;");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\tif (cont == action.Key.Length)");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\tConsole.WriteLine(txt.Substring(0, cont) + \" = \" + action.Value.ToString());");
                writer.WriteLine("\t\t\t\t\t\t\t\ttxt = txt.Substring(cont).TrimStart();");
                writer.WriteLine("\t\t\t\t\t\t\t\tcontrolReservada = false;");
                writer.WriteLine("\t\t\t\t\t\t\t\tbreak;");

                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t}");

                writer.WriteLine("\t\t\t\t\tif (controlReservada)");
                writer.WriteLine("\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\tcont = 1;");

                writer.WriteLine("\t\t\t\t\t\tvar pos = EvaluarLetra(txt[0]);");
                writer.WriteLine("\t\t\t\t\t\tif (pos != -1)");
                writer.WriteLine("\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\tif (matrizEstado[estado, pos] == -1)");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\tif (!aprobacion.Contains(estado))");
                writer.WriteLine("\t\t\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\t\t\tcorrect = false;");

                writer.WriteLine("\t\t\t\t\t\t\t\t\tConsole.WriteLine(\"Cadena ingresada invalida\");");
                writer.WriteLine("\t\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\t\t\testado = 0;");
                writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(1).TrimStart();");
                writer.WriteLine("\t\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\t\testado = matrizEstado[estado, pos];");
                writer.WriteLine("\t\t\t\t\t\t\t\ttxt = txt.Substring(1);");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\tcorrect = false;");
                //MODIFICAR EN CODIGO AL ERROR OBTENIDO
                writer.WriteLine("\t\t\t\t\t\t\tConsole.WriteLine(\"ERROR CARACTER INVALIDO\");");
                writer.WriteLine("\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"Presione enter para continuar\");");
                writer.WriteLine("\t\t\t\tConsole.ReadKey();");
                writer.WriteLine("\t\t\t\tConsole.Clear();");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t}");

                #region TRANSPORTAR LOS DATOS
                writer.WriteLine("\t\tstatic void RellenarDatos()");
                writer.WriteLine("\t\t{");

                foreach (var set in Logica.Sets)
                {
                    writer.WriteLine($"\t\t\tT.Add(\"{set}\");");
                }
                foreach (var aprob in aprobacion)
                {
                    writer.WriteLine($"\t\t\taprobacion.Add({aprob});");
                }
                foreach (var act in Logica.actionsTransportar)
                {
                    writer.WriteLine($"\t\t\tActions.Add(\"{act.Key}\", {act.Value.ToString()});");
                }

                writer.WriteLine("\t\t}");
                #endregion

                writer.WriteLine("\t\tstatic int EvaluarLetra(int letra)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tvar cont = 0;");

                writer.WriteLine("\t\t\tforeach (var validacion in T)");
                writer.WriteLine("\t\t\t{");

                writer.WriteLine("\t\t\t\tvar info = validacion.Split('|');");
                writer.WriteLine("\t\t\t\tif (info[1] == \"\")");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tif (Convert.ToString(letra) == info[0])");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\treturn cont;");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t\telse");
                writer.WriteLine("\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\tvar conjunto = info[1].Split('+');");
                writer.WriteLine("\t\t\t\t\tforeach (var pares in conjunto)");
                writer.WriteLine("\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\tvar partes = pares.Split('.');");
                writer.WriteLine("\t\t\t\t\t\tif (partes.Length == 2)");
                writer.WriteLine("\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\tif (Convert.ToInt32(partes[0]) < Convert.ToInt32(partes[1]) && letra >= Convert.ToInt32(partes[0]) && letra <= Convert.ToInt32(partes[1]))");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\treturn cont;");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\telse if (Convert.ToInt32(partes[0]) > Convert.ToInt32(partes[1]) && (letra >= Convert.ToInt32(partes[0]) || letra <= Convert.ToInt32(partes[1])))");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\treturn cont;");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\telse");
                writer.WriteLine("\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\tif (letra == Convert.ToInt32(partes[0]))");
                writer.WriteLine("\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\treturn cont;");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");

                writer.WriteLine("\t\t\t\tcont++;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn -1;");
                writer.WriteLine("\t\t}");

                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }


            #region EJECUTAR SCANNER
            BuildExe();
            try
            {
                Process.Start("Escanner.exe");
            }
            catch (Exception)
            {
                BuildExe();
            }
            #endregion
        }
    }
}
