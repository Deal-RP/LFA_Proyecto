using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace LFA_Proyecto_1
{
    class Logica
    {
        //Mantiene el conteo de lineas dentro de cada evaluacion
        static int contLinea;
        public static string ER;
        public static List<string> Sets;
        public static Dictionary<string, string> auxSets;
        public static List<string> estadosTransportar;
        public static Dictionary<string, int> actionsTransportar;
        public static List<Tuple<string, int>> tokensTransportar;

        #region METODOS DE APROBACION
        //Toma cada set recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static string AprobarSets(string linea)
        {
            var aux = string.Empty;
            var SetsCompletado = false;
            var set = string.Empty;

            var pos = 0;

            while (!SetsCompletado && linea.Length != 0)
            {
                if (PerteneceLetra(linea[0]))
                {
                    aux += linea[0];
                    linea = linea.Substring(1);
                }
                else if (linea[0] == '=' || linea[0] == '\t' || linea[0] == ' ')
                {
                    SetsCompletado = true;
                    linea = linea.Trim(new char[2] { '\t', ' ' });
                    pos++;
                }
                else
                {
                    return $"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA";
                }
            }

            if (Sets.Contains(aux))
            {
                return $"ERROR LINEA: {contLinea} SE REPITIO EL SETS";
            }
            else
            {
                Sets.Add(aux);
            }

            if (aux.Length == 0)
            {
                return $"ERROR LINEA: {contLinea} SE ESPERABA UN IDENTIFICADOR";
            }

            if (linea.Length == 0)
            {
                return $"ERROR LINEA: {contLinea} SE ESPERABA '=' Y DEFINICION";
            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                return $"ERROR LINEA: {contLinea} SE ESPERABA UN '='";
            }

            if (linea.Length == 0)
            {
                return $"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION";
            }


            var first = true;
            while (linea.Length != 0)
            {
                if (!first)
                {
                    if (linea.Length > 1 && linea.Trim(new char[2] { '\t', ' ' })[0] == '+')
                    {
                        linea = linea.Trim(new char[2] { '\t', ' ' });
                        set += linea.Substring(0, 1);
                        linea = linea.Substring(1);
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} SE ESPERA UN +");
                    }

                    if (linea.Length == 0)
                    {
                        return ($"ERROR LINEA: {contLinea} SE ESPERA UNA DEFINICION A CONTINUACION DEL +");
                    }
                }
                if (linea.Length > 7 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'' && linea[3] == '.' && linea[4] == '.' && linea[5] == '\'' && PerteneceASCII(linea[6]) && linea[7] == '\'')
                {
                    set += $"{Convert.ToString((int)linea[1])}.{Convert.ToString((int)linea[6])}";
                    linea = linea.Substring(8);
                    first = false;
                }
                else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                {
                    set += Convert.ToString((int)linea[1]);
                    linea = linea.Substring(3);
                    first = false;
                }
                else if (linea.Length > 3 && linea[0] == 'C' && linea[1] == 'H' && linea[2] == 'R' && linea[3] == '(')
                {
                    linea = linea.Substring(4);
                    pos = 0;
                    while (pos < linea.Length && PerteneceDigito(linea[pos]))
                    {
                        pos++;
                    }
                    if (pos == 0)
                    {
                        return ($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                    }
                    else
                    {
                        set += linea.Substring(0, pos);
                        linea = linea.Substring(pos);
                    }
                    if (linea.Length != 0 && linea[0] == ')')
                    {
                        linea = linea.Substring(1);
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");

                    }

                    if (linea.Length > 5 && linea[0] == '.' && linea[1] == '.' && linea[2] == 'C' && linea[3] == 'H' && linea[4] == 'R' && linea[5] == '(')
                    {
                        linea = linea.Substring(6);
                        pos = 0;
                        while (pos < linea.Length && PerteneceDigito(linea[pos]))
                        {
                            pos++;
                        }
                        if (pos == 0)
                        {
                            return ($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                        }
                        else
                        {
                            set += $".{linea.Substring(0, pos)}";
                            linea = linea.Substring(pos);
                        }
                        if (linea.Length != 0 && linea[0] == ')')
                        {
                            linea = linea.Substring(1);
                        }
                        else
                        {
                            return ($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                        }
                    }
                    first = false;
                }
                else
                {
                    return ($"ERROR LINEA: {contLinea} DEFINICION ERRONEA");

                }
            }
            auxSets.Add(aux, set);
            return "Correcto";
        }

        //Toma cada token recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static string AprobarTokens(string linea)
        {
            var aux = string.Empty;
            var inicioToken = true;
            var num = 0;
            var TokensCompletados = false;

            var pos = 0;

            if (linea.Length > 4 && linea.Substring(0, 5) == "TOKEN")
            {
                linea = linea.Substring(5);
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERA LA PALABRA TOKEN");
            }

            if (linea.Length == 0)
            {
                return ($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");
            }

            if (linea[0] == '\t' || linea[0] == ' ')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} FALTA ESPACIO ENTRE TOKEN Y NUMERO");
            }

            pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");
            }
            else
            {
                num = Convert.ToInt32(linea.Substring(0, pos));
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");
            }

            if (linea.Length == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");
            }




            var first = true;
            while (linea.Length != 0)
            {
                if (linea.Length > 0 && linea[0] == '*' && !first)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '+' && !first)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '?' && !first)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '{' && !first)
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    if (linea.Length > 11 && linea.Substring(0, 12) == "RESERVADAS()")
                    {
                        linea = linea.Substring(12).Trim(new char[2] { '\t', ' ' });
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} SE ESPERABA RESERVADAS()");
                    }
                    if (linea.Length > 0 && linea[0] == '}')
                    {
                        linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} SE ESPERABA {"}"}");
                    }
                }
                else if (linea.Length > 0 && linea[0] == '|' && !first)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    inicioToken = true;

                    if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                    {
                        if (inicioToken)
                        {
                            tokensTransportar.Add(new Tuple<string, int>(Convert.ToString((int)linea[1]), num));
                            inicioToken = false;
                        }

                        ER += $"{linea.Substring(0, 3)}.";
                        linea = linea.Substring(3).Trim(new char[2] { '\t', ' ' });
                    }
                    else if (linea.Length > 1 && PerteneceLetra(linea[0]))
                    {
                        TokensCompletados = false;
                        aux = string.Empty;
                        pos = 0;
                        while (!TokensCompletados && linea.Length != 0)
                        {
                            if (PerteneceLetra(linea[0]))
                            {
                                aux += linea[0];
                                linea = linea.Substring(1);
                            }
                            else if (linea[0] == '\t' || linea[0] == ' ')
                            {
                                TokensCompletados = true;
                                linea = linea.Trim(new char[2] { '\t', ' ' });
                                pos++;
                            }
                            else if (linea[0] == '(' || linea[0] == ')' || linea[0] == '*' || linea[0] == '+' || linea[0] == '?' || linea[0] == '\'' || linea[0] == '|')
                            {
                                TokensCompletados = true;
                                linea = linea.Trim(new char[2] { '\t', ' ' });
                            }
                            else
                            {
                                return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                            }
                        }
                        if (Sets.Contains(aux))
                        {
                            if (inicioToken)
                            {
                                tokensTransportar.Add(new Tuple<string, int>(aux, num));
                                inicioToken = false;
                            }
                            ER += $"{aux}.";
                        }
                        else
                        {
                            return ($"ERROR LINEA: {contLinea} LA PALABRA NO SE ENCUENTRA EN SETS");
                        }
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");

                    }
                }
                else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                {
                    if (inicioToken)
                    {
                        tokensTransportar.Add(new Tuple<string, int>(Convert.ToString((int)linea[1]), num));
                        inicioToken = false;
                    }
                    ER += $"{linea.Substring(0, 3)}.";
                    linea = linea.Substring(3).Trim(new char[2] { '\t', ' ' });
                    first = false;
                }
                else if (linea.Length > 1 && PerteneceLetra(linea[0]))
                {
                    TokensCompletados = false;
                    aux = string.Empty;
                    pos = 0;
                    while (!TokensCompletados && linea.Length != 0)
                    {
                        if (PerteneceLetra(linea[0]))
                        {
                            aux += linea[0];
                            linea = linea.Substring(1);
                        }
                        else if (linea[0] == '\t' || linea[0] == ' ' || linea[0] == '(' || linea[0] == '\'' || linea[0] == '{')
                        {
                            TokensCompletados = true;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            first = false;
                            pos++;
                        }
                        else if (linea[0] == '(' || linea[0] == ')' || linea[0] == '*' || linea[0] == '+' || linea[0] == '?' || linea[0] == '\'' || linea[0] == '|')
                        {
                            TokensCompletados = true;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                        }
                        else
                        {
                            return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");

                        }
                    }
                    if (Sets.Contains(aux))
                    {
                        if (inicioToken)
                        {
                            tokensTransportar.Add(new Tuple<string, int>(aux, num));
                            inicioToken = false;
                        }
                        ER += $"{aux}.";
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} LA PALABRA NO SE ENCUENTRA EN SETS");

                    }
                }
                else if (linea.Length > 0 && linea[0] == '(')
                {
                    var auxParentesis = AprobarParentesis(ref linea);
                    if (auxParentesis != "")
                    {
                        return auxParentesis;
                    }
                }
                else
                {
                    return ($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");

                }
                first = false;
            }

            return "Correcto";
        }

        static string AprobarParentesis(ref string linea)
        {
            var posicion = 0;
            var TokensCompletados = false;
            var cerrarParentesis = false;
            var firstParentesis = true;
            ER += $"{linea[0]}";
            linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            while (posicion < linea.Length && !cerrarParentesis)
            {
                if (linea.Length > 0 && linea[0] == '(')
                {
                    var auxParentesis = AprobarParentesis(ref linea);
                    if (auxParentesis != "")
                    {
                        return auxParentesis;
                    }
                }
                else if (linea.Length > 0 && linea[0] == ')')
                {
                    if (firstParentesis)
                    {
                        return ($"ERROR LINEA: {contLinea} SE ESPERABA CONTENIDO DENTRO DEL PARENTESIS");
                    }
                    else
                    {
                        cerrarParentesis = true;
                        ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                        linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    }
                }
                else if (linea.Length > 0 && linea[0] == '*' && !firstParentesis)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '+' && !firstParentesis)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '?' && !firstParentesis)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}.";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '|' && !firstParentesis)
                {
                    ER = $"{ER.TrimEnd('.')}{linea[0]}";
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });

                    if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                    {
                        ER += $"{linea.Substring(0, 3)}.";
                        linea = linea.Substring(3).Trim(new char[2] { '\t', ' ' });
                    }
                    else if (linea.Length > 1 && PerteneceLetra(linea[0]))
                    {
                        TokensCompletados = false;
                        var aux = string.Empty;
                        while (!TokensCompletados && linea.Length != 0)
                        {
                            if (PerteneceLetra(linea[0]))
                            {
                                aux += linea[0];
                                linea = linea.Substring(1);
                            }
                            else if (linea[0] == '\t' || linea[0] == ' ' || linea[0] == '(' || linea[0] == ')' || linea[0] == '\'' || linea[0] == '{')
                            {
                                TokensCompletados = true;
                                linea = linea.Trim(new char[2] { '\t', ' ' });
                            }
                            else if (linea[0] == '(' || linea[0] == ')' || linea[0] == '*' || linea[0] == '+' || linea[0] == '?' || linea[0] == '\'' || linea[0] == '|')
                            {
                                TokensCompletados = true;
                                linea = linea.Trim(new char[2] { '\t', ' ' });
                            }
                            else
                            {
                                return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                            }
                        }
                        if (Sets.Contains(aux))
                        {
                            ER += $"{aux}.";
                        }
                        else
                        {
                            return ($"ERROR LINEA: {contLinea} LA PALABRA NO SE ENCUENTRA EN SETS");

                        }
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");

                    }
                }
                else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                {
                    ER += $"{linea.Substring(0, 3)}.";
                    linea = linea.Substring(3).Trim(new char[2] { '\t', ' ' });
                    firstParentesis = false;
                }
                else if (linea.Length > 1 && PerteneceLetra(linea[0]))
                {
                    TokensCompletados = false;
                    var aux = string.Empty;
                    while (!TokensCompletados && linea.Length != 0)
                    {
                        if (PerteneceLetra(linea[0]))
                        {
                            aux += linea[0];
                            linea = linea.Substring(1);
                        }
                        else if (linea[0] == '\t' || linea[0] == ' ' || linea[0] == ')' || linea[0] == '\'' || linea[0] == '{')
                        {
                            TokensCompletados = true;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            firstParentesis = false;
                        }
                        else if (linea[0] == '(' || linea[0] == ')' || linea[0] == '*' || linea[0] == '+' || linea[0] == '?' || linea[0] == '\'' || linea[0] == '|')
                        {
                            TokensCompletados = true;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            firstParentesis = false;
                        }
                        else
                        {
                            return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                        }
                    }
                    if (Sets.Contains(aux))
                    {
                        ER += $"{aux}.";
                    }
                    else
                    {
                        return ($"ERROR LINEA: {contLinea} LA PALABRA NO SE ENCUENTRA EN SETS");

                    }
                }
            }
            if (!cerrarParentesis)
            {
                return ($"ERROR LINEA: {contLinea} FALTA CERRAR PARENTESIS");
            }
            return "";
        }

        //Toma cada action recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static string AprobarActions(string linea)
        {
            var aux = string.Empty;
            var pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");

            }
            else
            {
                aux = linea.Substring(0, pos);
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }

            if (linea.Length == 0)
            {
                return ($"ERROR LINEA: {contLinea} FALTA SIGNO '=' Y DEFINICION");

            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");

            }

            if (linea.Length == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");

            }

            if (linea.Length > 0 && linea[0] == '\'')
            {
                linea = linea.Substring(1);

                pos = 0;
                while (pos < linea.Length && PerteneceLetra(linea[pos]))
                {
                    pos++;
                }
                if (pos == 0)
                {
                    return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");

                }
                else
                {
                    if (!actionsTransportar.ContainsKey(linea.Substring(0, pos)))
                    {
                        actionsTransportar.Add(linea.Substring(0, pos), Convert.ToInt32(aux));
                    }
                    linea = linea.Substring(pos);
                }
                if (linea.Length > 0 && linea[0] == '\'')
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else
                {
                    return ($"ERROR LINEA: {contLinea} SE ESPERABA '");

                }
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA '");

            }

            return "Correcto";
        }

        //Toma cada errors recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static string AprobarErrors(string linea)
        {
            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");

            }

            if (linea.Length == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");

            }

            var pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                return ($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");

            }
            else
            {
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }
            if (linea.Length != 0)
            {
                return ($"ERROR LINEA: {contLinea} CARACTER INGRESADO NO ES DIGITO");

            }

            return "Correcto";
        }

        #endregion

        #region MANEJO DE ARBOL DE EXPRESIONES
        //Informacion para diferenciar entre terminales y no terminales
        public static List<string> Terminales = new List<string>();

        //Metodo para crear el arbol de expresion
        public static Nodo CreacionArbol()
        {
            var S = new Stack<Nodo>();
            var T = new Stack<string>();
            var Jerarquia = new Dictionary<string, int>();
            Jerarquia.Add("*", 1);
            Jerarquia.Add("+", 1);
            Jerarquia.Add("?", 1);
            Jerarquia.Add("|", 2);
            Jerarquia.Add(".", 3);

            var Token = string.Empty;

            var pos = 0;
            while (pos < ER.Length)
            {
                if (PerteneceLetra(ER[pos]))
                {
                    Token += ER[pos];
                    if (Sets.Contains(Token) && !Terminales.Contains(Token))
                    {
                        Terminales.Add(Token);
                    }
                }
                else if (ER[pos] == '\'')
                {
                    Token = $"{ER[pos]}{ER[pos + 1]}{ER[pos + 2]}";
                    if (!Terminales.Contains(Token))
                    {
                        Terminales.Add(Token);
                    }
                    pos += 2;
                }
                else
                {
                    Token = ER[pos].ToString();
                }

                if (Terminales.Contains(Token) || Token == "#")
                {
                    S.Push(new Nodo() { simbolo = Token });
                    Token = string.Empty;
                }
                else if (Token == "(")
                {
                    T.Push(Token);
                    Token = string.Empty;
                }
                else if (Token == ")")
                {
                    while (T.Count > 0 && T.Peek() != "(")
                    {
                        if (T.Count == 0 && S.Count < 2)
                        {
                            //ERROR NO TENGO MI ER BUENA
                        }
                        var temp = new Nodo();
                        temp.simbolo = T.Pop();
                        temp.HijoDerecho = S.Pop();
                        temp.HijoIzquierdo = S.Pop();
                        S.Push(temp);
                    }
                    Token = string.Empty;
                    T.Pop();
                }
                else if (Jerarquia.ContainsKey(Token))
                {
                    if (Jerarquia[Token] == 1)
                    {
                        var nodo = new Nodo { simbolo = Token };
                        nodo.HijoIzquierdo = S.Pop();
                        S.Push(nodo);
                    }
                    else if (T.Count != 0)
                    {
                        while (T.Peek() != "(" && Jerarquia[Token] <= Jerarquia[T.Peek()])
                        {
                            if (S.Count < 2)
                            {
                                //ERROR
                            }
                            var temp = new Nodo();
                            temp.simbolo = T.Pop();
                            temp.HijoDerecho = S.Pop();
                            temp.HijoIzquierdo = S.Pop();
                            S.Push(temp);
                        }
                    }
                    if (Jerarquia[Token] == 2 || Jerarquia[Token] == 3)
                    {
                        T.Push(Token);
                    }
                    Token = string.Empty;
                }
                pos++;
            }
            while (T.Count > 0)
            {
                if (T.Peek() == "(" && S.Count < 2)
                {
                    //ERROR
                }
                var temp = new Nodo();
                temp.simbolo = T.Pop();
                temp.HijoDerecho = S.Pop();
                temp.HijoIzquierdo = S.Pop();
                S.Push(temp);
            }
            return S.Pop();
        }

        public static int altura;

        public static int MostrarArbol(Nodo raiz, Graphics grafo, Font fuente, Brush relleno, Brush rellenoFuente, Pen lapiz, Brush encuentro)
        {
            altura = 0;
            int x = 400;
            int y = 0;

            raiz.PosNodo(ref x, y);
            raiz.MostrarRamas(grafo, lapiz);
            raiz.MostrarNodo(grafo, fuente, relleno, rellenoFuente, lapiz, encuentro);
            return x;
        }

        public static void Colorear(Nodo raiz, Graphics grafo, Font fuente, Brush relleno, Brush rellenoFuente, Pen lapiz, Brush encuentro)
        {
            if (raiz != null)
            {
                Colorear(raiz.HijoIzquierdo, grafo, fuente, relleno, rellenoFuente, lapiz, encuentro);
                raiz.colorear(grafo, fuente, relleno, rellenoFuente, lapiz);
                Colorear(raiz.HijoDerecho, grafo, fuente, relleno, rellenoFuente, lapiz, encuentro);
            }

        }

        public static int cont;
        public static Dictionary<int, List<int>> Follows;
        public static List<string> listTerminales;

        public static void TablaFLN(Nodo Actual, DataGridView Tabla)
        {
            if (Actual != null)
            {
                TablaFLN(Actual.HijoIzquierdo, Tabla);
                TablaFLN(Actual.HijoDerecho, Tabla);
                Tabla.Rows.Add(Actual.FuncionesFLN());
            }
        }

        public static void TablaF(DataGridView Tabla)
        {
            var sortList = Follows.Keys.ToList();
            sortList.Sort();
            var dic = new Dictionary<int, List<int>>();

            foreach (var key in sortList)
            {
                dic.Add(key, Follows[key]);
            }

            Follows = dic;
            Follows.Add(cont - 1, new List<int>());

            foreach (var item in Follows)
            {
                var aux = string.Empty;

                foreach (var itemList in Follows[item.Key])
                {
                    aux += $"{itemList.ToString()},";
                }
                Tabla.Rows.Add(item.Key, aux.TrimEnd(','));
            }
        }

        public static Dictionary<List<int>, Dictionary<string, List<int>>> estados;

        public static void TablaEstados(Nodo Raiz, DataGridView Tabla)
        {
            estados = new Dictionary<List<int>, Dictionary<string, List<int>>>();
            var estadoActual = Raiz.First;
            var ordenEstados = new Queue<string>();
            var completado = false;
            Generar.aprobacion = new List<int>();

            while (!completado)
            {
                var dic = new Dictionary<string, List<int>>();

                for (int i = 0; i < Terminales.Count(); i++)
                {
                    var trans = new List<int>();
                    foreach (var estado in estadoActual)
                    {
                        if (listTerminales[estado - 1] == Terminales[i])
                        {
                            foreach (var follow in Follows[estado])
                            {
                                if (!trans.Contains(follow))
                                {
                                    trans.Add(follow);
                                }
                            }
                        }
                    }
                    trans.Sort();
                    dic.Add(Terminales[i], trans);
                }

                estados.Add(estadoActual, dic);

                foreach (var item in dic.Values)
                {
                    var auxDic = string.Empty;
                    foreach (var key in item)
                    {
                        auxDic += $"{key},";
                    }
                    auxDic = auxDic.TrimEnd(',');

                    var misEstados = new List<string>();

                    foreach (var estado in estados)
                    {
                        var auxEstado = string.Empty;

                        foreach (var key in estado.Key)
                        {
                            auxEstado += $"{key},";
                        }
                        misEstados.Add(auxEstado.TrimEnd(','));
                    }

                    if (auxDic != "" && !misEstados.Contains(auxDic) && !ordenEstados.Contains(auxDic))
                    {
                        ordenEstados.Enqueue(auxDic);
                    }
                }

                if (ordenEstados.Count() != 0)
                {
                    var aux = ordenEstados.Dequeue().Split(',');
                    var nuevo = new List<int>();
                    foreach (var item in aux)
                    {
                        nuevo.Add(Convert.ToInt32(item));
                    }
                    estadoActual = nuevo;
                }
                else
                {
                    completado = true;
                }
            }

            var first = false;
            var cont = 0;

            foreach (var estado in estados)
            {
                var fila = new List<string>();
                var aux = string.Empty;

                foreach (var item in estado.Key)
                {
                    if (!Generar.aprobacion.Contains(cont) && item == listTerminales.Count)
                    {
                        Generar.aprobacion.Add(cont);
                    }
                    aux += $"{item},";
                }
                fila.Add(aux.TrimEnd(','));
                estadosTransportar.Add(aux.TrimEnd(','));

                foreach (var item in estado.Value)
                {
                    if (!first)
                    {
                        Tabla.Columns.Add(item.Key.ToLower(), item.Key);
                    }

                    var aux1 = string.Empty;
                    foreach (var miniEstado in item.Value)
                    {
                        aux1 += $"{miniEstado},";
                    }
                    fila.Add(aux1.TrimEnd(','));
                }

                first = true;
                Tabla.Rows.Add(fila.ToArray());
                cont++;
            }
        }

        public static void InicializarSets()
        {
            Generar.T = new List<string>();
            listTerminales.Remove("#");
            foreach (var n in listTerminales)
            {
                if (n[0] == '\'' && !Generar.T.Contains($"{(int)n[1]}|"))
                {
                    Generar.T.Add($"{Convert.ToString((int)n[1])}|");
                }
                else if (Logica.auxSets.ContainsKey(n))
                {
                    Generar.T.Add($"{n}|{Logica.auxSets[n]}");
                    auxSets.Remove(n);
                }
            }
        }
        #endregion

        #region VALIDACIONES DE CARACTERES
        //Metodo verifica que este entre el rango de caracteres permitidos
        static bool PerteneceASCII(char linea)
        {
            if (linea > 0 && linea < 256)
            {
                return true;
            }
            return false;
        }

        //Metodo verifica que sea un digito
        static bool PerteneceDigito(char linea)
        {
            if (linea > 47 && linea < 58)
            {
                return true;
            }
            return false;
        }

        //Metodo verifica que este sea una letra mayuscula
        static bool PerteneceLetra(char linea)
        {
            if (linea > 64 && linea < 91)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Lectura
        public static string Lectura(Stream archivo)
        {
            //Booleana central que maneja cuando parar el analisis del txt
            contLinea = 0;
            ER = string.Empty;
            Terminales = new List<string>();
            Sets = new List<string>();
            auxSets = new Dictionary<string, string>();
            estadosTransportar = new List<string>();
            actionsTransportar = new Dictionary<string, int>();
            tokensTransportar = new List<Tuple<string, int>>();

            var Correcto = true;
            try
            {
                using (var sr = new StreamReader(archivo))
                {
                    #region VARIABLES
                    //Variables de manejo de errores
                    var linea = string.Empty;
                    var continuar = true;
                    var ContieneSets = false;
                    var ContieneTokens = false;
                    var ContieneActions = false;
                    var ContieneErrors = false;

                    var conteo = 0;
                    #endregion

                    #region INICIO
                    while (continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            contLinea++;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "SETS")
                            {
                                ContieneSets = true;
                                continuar = false;
                            }
                            else if (linea == "TOKENS")
                            {
                                ContieneTokens = true;
                                continuar = false;
                            }
                            else if (linea != "")
                            {
                                return $"ERROR EN LINEA: {contLinea} NO EMPIEZA CON SETS O TOKENS";
                            }
                        }
                        else
                        {
                            return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                        }
                    }
                    continuar = true;
                    #endregion

                    #region SETS
                    while (Correcto && ContieneSets && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            contLinea++;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "TOKENS")
                            {
                                ContieneTokens = true;
                                continuar = false;
                            }
                            else if (linea == "") { }
                            else
                            {
                                var aux = AprobarSets(linea);
                                if (aux != "Correcto")
                                {
                                    Correcto = false;
                                    return aux;
                                }
                                else
                                {
                                    conteo++;
                                }
                            }
                        }
                        else
                        {
                            Correcto = false;
                            return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                        }
                    }

                    continuar = true;

                    if (conteo == 0 && ContieneSets && Correcto)
                    {
                        Correcto = false;
                        return $"ERROR EN LINEA: {contLinea} DEBE DE IR UN SET";
                    }
                    else
                    {
                        conteo = 0;
                    }
                    #endregion

                    #region TOKENS
                    while (Correcto && ContieneTokens && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            contLinea++;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "ACTIONS")
                            {
                                ContieneActions = true;
                                continuar = false;
                            }
                            else if (linea == "") { }
                            else if (AprobarTokens(linea) != "Correcto")
                            {
                                Correcto = false;
                                return AprobarTokens(linea);
                            }
                            else
                            {
                                ER = $"{ER.TrimEnd('.')}|";
                                conteo++;
                            }
                        }
                        else
                        {
                            Correcto = false;
                            return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                        }
                    }

                    continuar = true;
                    if (conteo == 0 && ContieneTokens && Correcto)
                    {
                        Correcto = false;
                        return $"ERROR EN LINEA: {contLinea} DEBE DE IR UN TOKEN";
                    }
                    else
                    {
                        ER = $"({ER.TrimEnd('|')}).#";
                        conteo = 0;
                    }

                    #endregion

                    #region ACTIONS
                    var contieneReservadas = false;
                    while (Correcto && ContieneActions && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            contLinea++;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "RESERVADAS()")
                            {
                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        contLinea++;
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "{")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea != "")
                                        {
                                            Correcto = false;
                                            return $"ERROR EN LINEA: {contLinea} SE ESPERABA {"{"}";
                                        }
                                    }
                                    else
                                    {
                                        Correcto = false;
                                        return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                                    }
                                }
                                continuar = true;

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        contLinea++;
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "}")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea == "") { }
                                        else if (AprobarActions(linea) != "Correcto")
                                        {
                                            Correcto = false;
                                            return AprobarActions(linea);
                                        }
                                        else if (AprobarActions(linea) == "Correcto")
                                        {
                                            conteo++;
                                        }
                                    }
                                    else
                                    {
                                        Correcto = false;
                                        return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                                    }
                                }
                                continuar = true;
                                contieneReservadas = true;
                            }
                            else if (linea.Length > 4 && linea.Substring(0, 5) == "ERROR")
                            {
                                continuar = false;
                                ContieneErrors = true;
                            }
                            else if (linea.Length > 0 && PerteneceLetra(linea[0]))
                            {
                                var pos = 0;
                                while (pos < linea.Length && PerteneceLetra(linea[pos]))
                                {
                                    pos++;
                                }
                                if (pos == 0)
                                {
                                    Correcto = false;
                                    return $"ERROR LINEA: {contLinea} SE ESPERABA UN IDENTIFICADOR";
                                }
                                else
                                {
                                    linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
                                }

                                if (linea.Length > 1 && linea[0] == '(' && linea[1] == ')')
                                {
                                    linea = linea.Substring(2).Trim(new char[2] { '\t', ' ' });
                                }
                                else
                                {
                                    Correcto = false;
                                    return $"ERROR LINEA: {contLinea} SE ESPERABA UN ()";
                                }

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        contLinea++;
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "{")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea != "")
                                        {
                                            Correcto = false;
                                            return $"ERROR EN LINEA: {contLinea} SE ESPERABA {"{"}";
                                        }
                                    }
                                    else
                                    {
                                        Correcto = false;
                                        return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                                    }
                                }
                                continuar = true;

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        contLinea++;
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "}")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea == "") { }
                                        else if (AprobarActions(linea) != "Correcto")
                                        {
                                            Correcto = false;
                                            return AprobarActions(linea);
                                        }
                                        else if (AprobarActions(linea) == "Correcto")
                                        {
                                            conteo++;
                                        }
                                    }
                                    else
                                    {
                                        Correcto = false;
                                        return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                                    }
                                }
                                continuar = true;

                            }
                            else if (linea != "")
                            {
                                Correcto = false;
                                return $"ERROR EN LINEA: {contLinea} SE ESPERABA SE ESPERABA FUNCIONES";
                            }
                        }
                        else
                        {
                            Correcto = false;
                            return $"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO";
                        }

                    }

                    continuar = true;

                    if (Correcto && !contieneReservadas)
                    {
                        Correcto = false;
                        return $"ERROR EN LINEA: {contLinea} DEBE DE IR RESERVADAS()";
                    }

                    if (conteo == 0 && ContieneActions && Correcto)
                    {
                        Correcto = false;
                        return $"ERROR EN LINEA: {contLinea} DEBE DE IR UN ACTION";
                    }
                    else
                    {
                        conteo = 0;
                    }
                    #endregion

                    #region ERRORS
                    if (linea != null && linea.Length > 4 && linea.Substring(0, 5) == "ERROR" && Correcto && ContieneErrors)
                    {
                        linea = linea.Substring(5).Trim(new char[2] { '\t', ' ' });
                        var aprueba = AprobarErrors(linea);
                        if (aprueba == "Correcto")
                        {
                            conteo++;
                        }
                        else
                        {
                            Correcto = false;
                            return aprueba;
                        }
                    }

                    while (Correcto && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            contLinea++;
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea.Length > 4 && linea.Substring(0, 5) == "ERROR")
                            {
                                linea = linea.Substring(5).Trim(new char[2] { '\t', ' ' });
                                var aprueba = AprobarErrors(linea);
                                if (aprueba == "Correcto")
                                {
                                    conteo++;
                                }
                                else
                                {
                                    Correcto = false;
                                    return aprueba;
                                }
                            }
                            else if (linea != "")
                            {
                                Correcto = false;
                                return $"ERROR EN LINEA: {contLinea} SE ESPERABA ERRORS";
                            }
                        }
                        else
                        {
                            continuar = false;
                        }
                    }

                    if (conteo == 0 && Correcto)
                    {
                        Correcto = false;
                        return $"ERROR EN LINEA: {contLinea} DEBE DE IR UN ERROR";
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

            return "FORMATO CORRECTO";
        }
        #endregion
    }
}
