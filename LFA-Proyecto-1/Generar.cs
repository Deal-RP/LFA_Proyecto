using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

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
        public static string path;
        public static List<string> T;

        public static void CrearPrograma()
        {
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
                writer.WriteLine("\t\tstatic List<Tuple<string, int>> Tokens = new List<Tuple<string, int>>();");
                writer.WriteLine("\t\tstatic string desplegar;");
                writer.WriteLine("\t\tstatic int num = 0;");

                writer.WriteLine("\t\tstatic void Main(string[] args)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tvar opc = 0;");
                writer.WriteLine("\t\t\tRellenarDatos();");
                writer.WriteLine("\t\t\twhile (opc != 3)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tvar empezar = true;");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"Menu\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"1.Ingresar Archivo de.txt\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"2.Ingresar cadena\");");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"3.Salir\");");
                writer.WriteLine("\t\t\t\tint.TryParse(Console.ReadLine(), out opc);");

                writer.WriteLine("\t\t\t\tvar txt = string.Empty;");
                writer.WriteLine("\t\t\t\tdesplegar = string.Empty;");

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

                writer.WriteLine("\t\t\t\t\tcase 3:");
                writer.WriteLine("\t\t\t\t\t\tempezar = false;");
                writer.WriteLine("\t\t\t\t\t\tbreak;");

                writer.WriteLine("\t\t\t\t\tdefault:");
                writer.WriteLine("\t\t\t\t\t\tempezar = false;");
                writer.WriteLine("\t\t\t\t\t\tbreak;");

                writer.WriteLine("\t\t\t\t}");


                writer.WriteLine("\t\t\t\tvar correct = true;");

                writer.WriteLine("\t\t\t\tvar Token = string.Empty;");
                writer.WriteLine("\t\t\t\tvar estado = 0;");

                writer.WriteLine("\t\t\t\tvar controlReservada = true;");

                writer.WriteLine("\t\t\t\twhile (correct && txt.Length != 0)");
                writer.WriteLine("\t\t\t\t{");


                writer.WriteLine("\t\t\t\t\tcontrolReservada = true;");
                writer.WriteLine("\t\t\t\t\tvar cont = 0;");

                writer.WriteLine("\t\t\t\t\ttxt = txt.TrimStart(new char[4] { '\\t', ' ', '\\r', '\\n' });");

                #region CARGAR ESTADOS
                var estados = new Dictionary<string, Dictionary<string, string>>();

                foreach (var estado in Logica.estados)
                {
                    var auxkey = string.Empty;

                    foreach (var key in estado.Key)
                    {
                        auxkey += $"{key},";
                    }
                    var miniDiccionario = new Dictionary<string, string>();
                    foreach (var value in estado.Value)
                    {
                        var aux = string.Empty;
                        foreach (var miniEstado in value.Value)
                        {
                            aux += $"{miniEstado},";
                        }
                        var auxDiccionario = value.Key[0] == '\'' ? ((int)value.Key[1]).ToString() : value.Key;
                        miniDiccionario.Add(auxDiccionario, aux.TrimEnd(','));
                    }
                    estados.Add(auxkey.TrimEnd(','), miniDiccionario);
                }

                var auxValidacion = string.Empty;
                var auxCorrecto = string.Empty;
                for (int i = 0; i < estados.Count; i++)
                {
                    if (!aprobacion.Contains(i))
                    {
                        auxValidacion += $" || estado == {i.ToString()}";
                    }
                    else
                    {
                        auxCorrecto += $" || estado == {i.ToString()}";
                    }
                }
                #endregion

                writer.WriteLine($"\t\t\t\t\tif (estado == 0 || {auxCorrecto.TrimStart(new char[2] { '|', ' ' })})");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tforeach (var action in Actions)");
                writer.WriteLine("\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\tif (txt.Length >= action.Key.Length)");
                writer.WriteLine("\t\t\t\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\t\t\t\tcont = 0;");
                writer.WriteLine("\t\t\t\t\t\t\t\twhile (cont < action.Key.Length && action.Key[cont] == Convert.ToChar(txt[cont].ToString().ToUpper()))");
                writer.WriteLine("\t\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\t\tcont++;");
                writer.WriteLine("\t\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\t\tif (cont == action.Key.Length)");
                writer.WriteLine("\t\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\t\tif (desplegar != \"\")");
                writer.WriteLine("\t\t\t\t\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\t\t\t\t\tConsole.WriteLine(desplegar + \" = \" + num);");
                writer.WriteLine("\t\t\t\t\t\t\t\t\t\tdesplegar = \"\";");
                writer.WriteLine("\t\t\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\t\t\tConsole.WriteLine(txt.Substring(0, cont) + \" = \" + action.Value.ToString());");
                writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(cont);");
                writer.WriteLine("\t\t\t\t\t\t\t\t\testado = 0;");
                writer.WriteLine("\t\t\t\t\t\t\t\t\tcontrolReservada = false;");
                writer.WriteLine("\t\t\t\t\t\t\t\t\tbreak;");

                writer.WriteLine("\t\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t\t\t}");

                writer.WriteLine("\t\t\t\t\t}");

                writer.WriteLine("\t\t\t\t\tif (controlReservada && txt.Length > 0)");
                writer.WriteLine("\t\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\t\tvar letra = (int)txt[0];");

                #region SWICTH
                var numEstado = 0;
                foreach (var estado in Logica.estadosTransportar)
                {
                    var partes = estado.Split(',').ToList();
                    if (partes.Contains(Logica.Follows.Count.ToString()))
                    {
                        aprobacion.Add(numEstado);
                    }
                    numEstado++;
                }

                int[,] matrizEstado = new int[Logica.estadosTransportar.Count, T.Count];

                var posy = 0;
                foreach (var estado in estados)
                {
                    var posx = 0;
                    foreach (var miniEstado in estado.Value)
                    {
                        matrizEstado[posy, posx] = Logica.estadosTransportar.IndexOf(miniEstado.Value);
                        posx++;
                    }
                    posy++;
                }
                var cont = 0;
                writer.WriteLine("\t\t\t\t\t\tswitch (estado)");
                writer.WriteLine("\t\t\t\t\t\t{");
                for (int i = 0; i < Logica.estadosTransportar.Count; i++)
                {
                    var posX = 0;
                    cont = 0;
                    writer.WriteLine($"\t\t\t\t\t\t\tcase {i}:");
                    if (i == 0)
                    {
                        writer.WriteLine("\t\t\t\t\t\t\t\tif (desplegar != \"\")");
                        writer.WriteLine("\t\t\t\t\t\t\t\t{");
                        writer.WriteLine("\t\t\t\t\t\t\t\t\tConsole.WriteLine(desplegar + \" = \" + num);");
                        writer.WriteLine("\t\t\t\t\t\t\t\t\tdesplegar = \"\";");
                        writer.WriteLine("\t\t\t\t\t\t\t\t}");
                    }
                    foreach (var validacion in T)
                    {
                        var info = validacion.Split('|');
                        if (estados[Logica.estadosTransportar[i]][info[0]] != "")
                        {
                            if (info[1] == "")
                            {
                                if (cont != 0)
                                {
                                    writer.WriteLine("\t\t\t\t\t\t\t\telse");
                                }
                                writer.WriteLine($"\t\t\t\t\t\t\t\tif (letra == {info[0]})");
                                writer.WriteLine("\t\t\t\t\t\t\t\t{");
                                writer.WriteLine($"\t\t\t\t\t\t\t\t\testado = {matrizEstado[i, posX]};");
                                writer.WriteLine("\t\t\t\t\t\t\t\t\tdesplegar += txt[0];");
                                writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(1);");
                                if (i == 0)
                                {
                                    writer.WriteLine($"\t\t\t\t\t\t\t\t\tVerificarToken(\"{info[0]}\");");
                                }
                                writer.WriteLine("\t\t\t\t\t\t\t\t}");
                                cont++;
                            }
                            else
                            {
                                var aux = string.Empty;
                                var conjunto = info[1].Split('+');
                                foreach (var pares in conjunto)
                                {
                                    var partes = pares.Split('.');
                                    if (partes.Length == 2)
                                    {
                                        if (Convert.ToInt32(partes[0]) < Convert.ToInt32(partes[1]))
                                        {
                                            if (cont != 0)
                                            {
                                                writer.WriteLine("\t\t\t\t\t\t\t\telse");
                                            }
                                            writer.WriteLine($"\t\t\t\t\t\t\t\tif (letra >= {Convert.ToInt32(partes[0]).ToString()} && letra <= {Convert.ToInt32(partes[1]).ToString()})");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t{");
                                            writer.WriteLine($"\t\t\t\t\t\t\t\t\testado = {matrizEstado[i, posX]};");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t\tdesplegar += txt[0];");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(1);");
                                            if (i == 0)
                                            {
                                                writer.WriteLine($"\t\t\t\t\t\t\t\t\tVerificarToken(\"{info[0]}\");");
                                            }
                                            writer.WriteLine("\t\t\t\t\t\t\t\t}");
                                            cont++;
                                        }
                                        else if (Convert.ToInt32(partes[0]) > Convert.ToInt32(partes[1]))
                                        {
                                            if (cont != 0)
                                            {
                                                writer.WriteLine("\t\t\t\t\t\t\t\telse");
                                            }
                                            writer.WriteLine($"\t\t\t\t\t\t\t\tif (letra >= {Convert.ToInt32(partes[0]).ToString()} || letra <= {Convert.ToInt32(partes[1]).ToString()})");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t{");
                                            writer.WriteLine($"\t\t\t\t\t\t\t\t\testado = {matrizEstado[i, posX]};");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t\tdesplegar += txt[0];");
                                            if (i == 0)
                                            {
                                                writer.WriteLine($"\t\t\t\t\t\t\t\t\tVerificarToken(\"{info[0]}\");");
                                            }
                                            writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(1);");
                                            writer.WriteLine("\t\t\t\t\t\t\t\t}");
                                            cont++;
                                        }
                                    }
                                    else
                                    {
                                        if (cont != 0)
                                        {
                                            writer.WriteLine("\t\t\t\t\t\t\t\telse");
                                        }
                                        writer.WriteLine($"\t\t\t\t\t\t\t\tif (letra == {Convert.ToInt32(partes[0]).ToString()})");
                                        writer.WriteLine("\t\t\t\t\t\t\t\t{");
                                        writer.WriteLine($"\t\t\t\t\t\t\t\t\testado = {matrizEstado[i, posX]};");
                                        writer.WriteLine("\t\t\t\t\t\t\t\t\tdesplegar += txt[0];");
                                        writer.WriteLine("\t\t\t\t\t\t\t\t\ttxt = txt.Substring(1);");
                                        if (i == 0)
                                        {
                                            writer.WriteLine($"\t\t\t\t\t\t\t\t\tVerificarToken(\"{info[0]}\");");
                                        }
                                        writer.WriteLine("\t\t\t\t\t\t\t\t}");
                                        cont++;
                                    }
                                }
                            }
                        }
                        posX++;
                    }

                    if (cont != 0)
                    {
                        writer.WriteLine("\t\t\t\t\t\t\t\telse");
                        writer.WriteLine("\t\t\t\t\t\t\t\t{");

                        if (aprobacion.Contains(i))
                        {
                            writer.WriteLine("\t\t\t\t\t\t\t\t\testado = 0;");
                            writer.WriteLine("\t\t\t\t\t\t\t\t\tcorrect = evaluar(letra);");
                        }
                        else
                        {
                            writer.WriteLine("\t\t\t\t\t\t\t\t\tConsole.WriteLine(\"Cadena invalida\");");
                            writer.WriteLine("\t\t\t\t\t\t\t\t\tcorrect = false;");
                        }
                        writer.WriteLine("\t\t\t\t\t\t\t\t}");
                    }
                    else
                    {
                        writer.WriteLine("\t\t\t\t\t\t\t\testado = 0;");
                        writer.WriteLine("\t\t\t\t\t\t\t\tcorrect = evaluar(letra);");
                    }
                    

                    writer.WriteLine("\t\t\t\t\t\t\t\tbreak;");
                }

                writer.WriteLine("\t\t\t\t\t\t}");
                #endregion

                writer.WriteLine("\t\t\t\t\t}");
                writer.WriteLine("\t\t\t\t}");
                

                writer.WriteLine($"\t\t\t\tif (controlReservada && empezar && correct && ({auxValidacion.TrimStart(new char[2] { '|' , ' '})}))");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tConsole.WriteLine(\"Cadena incorrecta\");");
                writer.WriteLine("\t\t\t\t\tcorrect = false;");
                writer.WriteLine("\t\t\t\t}");

                writer.WriteLine("\t\t\t\tif (empezar && correct && desplegar != \"\")");
                writer.WriteLine("\t\t\t\t{");
                writer.WriteLine("\t\t\t\t\tConsole.WriteLine(desplegar + \" = \" + num);");
                writer.WriteLine("\t\t\t\t}");


                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"Presione enter para continuar\");");
                writer.WriteLine("\t\t\t\tConsole.ReadKey();");
                writer.WriteLine("\t\t\t\tConsole.Clear();");
                writer.WriteLine("\t\t\t}");

                writer.WriteLine("\t\t}");

                #region TRANSPORTAR LOS DATOS
                writer.WriteLine("\t\tstatic void RellenarDatos()");
                writer.WriteLine("\t\t{");

                var auxList = new List<string>();

                foreach (var item in Logica.actionsTransportar)
                {
                    auxList.Add(item.Key);
                }

                auxList = auxList.OrderByDescending(s => s.Length).ToList();

                foreach (var item in auxList)
                {
                    writer.WriteLine($"\t\t\tActions.Add(\"{item}\", {Logica.actionsTransportar[item]});");
                }

                foreach (var item in Logica.tokensTransportar)
                {
                    writer.WriteLine($"\t\t\tTokens.Add(new Tuple<string, int>(\"{item.Item1}\", {item.Item2}));");
                }

                //foreach (var act in Logica.actionsTransportar)
                //{
                //    writer.WriteLine($"\t\t\tActions.Add(\"{act.Key}\", {act.Value.ToString()});");
                //}

                writer.WriteLine("\t\t}");
                #endregion

                #region CORRECTO
                writer.WriteLine("\t\tstatic bool evaluar(int letra)");
                writer.WriteLine("\t\t{");
                cont = 0;
                foreach (var validacion in T)
                {
                    var info = validacion.Split('|');
                    if (info[1] == "")
                    {
                        if (cont != 0)
                        {
                            writer.WriteLine("\t\t\telse");
                        }
                        writer.WriteLine($"\t\t\tif (letra == {info[0]})");
                        writer.WriteLine("\t\t\t{ }");
                        cont++;
                    }
                    else
                    {
                        var aux = string.Empty;
                        var conjunto = info[1].Split('+');
                        foreach (var pares in conjunto)
                        {
                            var partes = pares.Split('.');
                            if (partes.Length == 2)
                            {
                                if (Convert.ToInt32(partes[0]) < Convert.ToInt32(partes[1]))
                                {
                                    if (cont != 0)
                                    {
                                        writer.WriteLine("\t\t\telse");
                                    }
                                    writer.WriteLine($"\t\t\tif (letra >= {Convert.ToInt32(partes[0]).ToString()} && letra <= {Convert.ToInt32(partes[1]).ToString()})");
                                    writer.WriteLine("\t\t\t{ }");
                                    cont++;
                                }
                                else if (Convert.ToInt32(partes[0]) > Convert.ToInt32(partes[1]))
                                {
                                    if (cont != 0)
                                    {
                                        writer.WriteLine("\t\t\telse");
                                    }
                                    writer.WriteLine($"\t\t\tif (letra >= {Convert.ToInt32(partes[0]).ToString()} || letra <= {Convert.ToInt32(partes[1]).ToString()})");
                                    writer.WriteLine("\t\t\t{ }");
                                    cont++;
                                }
                            }
                            else
                            {
                                if (cont != 0)
                                {
                                    writer.WriteLine("\t\t\telse");
                                }
                                writer.WriteLine($"\t\t\tif (letra == {Convert.ToInt32(partes[0]).ToString()})");
                                writer.WriteLine("\t\t\t{ }");
                                cont++;
                            }
                        }
                    }
                }
                writer.WriteLine("\t\t\telse");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tConsole.WriteLine(\"Caracter invalido\");");
                writer.WriteLine("\t\t\t\treturn false;");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t\treturn true;");

                writer.WriteLine("\t\t}");
                #endregion
                #region MOSTRARTOKEN
                writer.WriteLine("\t\tstatic void VerificarToken(string t)");
                writer.WriteLine("\t\t{");
                writer.WriteLine("\t\t\tvar first = true;");
                writer.WriteLine("\t\t\tforeach (var item in Tokens)");
                writer.WriteLine("\t\t\t{");
                writer.WriteLine("\t\t\t\tif (first && item.Item1 == t)");
                writer.WriteLine("\t\t\t\t{");

                writer.WriteLine("\t\t\t\t\tnum = item.Item2;");

                writer.WriteLine("\t\t\t\t\tfirst = false;");
                writer.WriteLine("\t\t\t\t}");
                writer.WriteLine("\t\t\t}");
                writer.WriteLine("\t\t}");
                #endregion

                writer.WriteLine("\t}");
                writer.WriteLine("}");

                
            }

            Directory.CreateDirectory(Path.Combine(path, "Generico"));
            CopyFolder("Generico", Path.Combine(path, "Generico"));
            //#region EJECUTAR SCANNER
            //BuildExe();
            //try
            //{
            //    Process.Start($"{path}\\Generico\\Generico\\bin\\Debug\\netcoreapp3.1\\Generico.exe");
            //}
            //catch (Exception)
            //{
            //    BuildExe();
            //}
            //#endregion
        }

        static void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest, true);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
    }
}
