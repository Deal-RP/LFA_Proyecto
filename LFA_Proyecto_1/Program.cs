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
        //Mantiene el conteo de lineas dentro de cada evaluacion
        static int contLinea = 1;

        #region METODOS DE APROBACION
        //Toma cada set recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static bool AprobarSets(string linea, string ER)
        {
            var arbol = CreacionArbol(ER);
            var aux = string.Empty;
            var SetsCompletado = false;

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
                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                    return false;
                }
            }

            if (aux.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN IDENTIFICADOR");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA '=' Y DEFINICION");
                return false;
            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");
                return false;
            }

            var first = true;
            while (linea.Length != 0)
            {
                if (!first)
                {
                    if (linea.Length > 1 && linea[0] == '+')
                    {
                        linea = linea.Substring(1);
                    }
                    else
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERA UN +");
                        return false;
                    }

                    if (linea.Length == 0)
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERA UNA DEFINICION A CONTINUACION DEL +");
                        return false;
                    }
                }
                if (linea.Length > 7 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'' && linea[3] == '.' && linea[4] == '.' && linea[5] == '\'' && PerteneceASCII(linea[6]) && linea[7] == '\'')
                {
                    linea = linea.Substring(8);
                    first = false;
                }
                else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                {
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
                        Console.WriteLine($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                        return false;
                    }
                    else
                    {
                        linea = linea.Substring(pos);
                    }
                    if (linea.Length != 0 && linea[0] == ')')
                    {
                        linea = linea.Substring(1);
                    }
                    else
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                        return false;
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
                            Console.WriteLine($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                            return false;
                        }
                        else
                        {
                            linea = linea.Substring(pos);
                        }
                        if (linea.Length != 0 && linea[0] == ')')
                        {
                            linea = linea.Substring(1);
                        }
                        else
                        {
                            Console.WriteLine($"ERROR LINEA: {contLinea} CHR IMCOMPLETO");
                            return false;
                        }
                    }
                    first = false;
                }
                else
                {
                    Console.WriteLine($"ERROR LINEA: {contLinea} DEFINICION ERRONEA");
                    return false;
                }
            }
            return true;
        }

        //Toma cada token recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static bool AprobarTokens(string linea, string ER)
        {
            var arbol = CreacionArbol(ER);
            var aux = string.Empty;
            var TokensCompletados = false;

            var pos = 0;

            if (linea.Length > 4 && linea.Substring(0, 5) == "TOKEN")
            {
                linea = linea.Substring(5);
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERA LA PALABRA TOKEN");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");
                return false;
            }

            if (linea[0] == '\t' || linea[0] == ' ')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} FALTA ESPACIO ENTRE TOKEN Y NUMERO");
                return false;
            }

            pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");
                return false;
            }
            else
            {
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");
                return false;
            }

            var first = true;
            while (linea.Length != 0)
            {
                if (linea.Length > 0 && linea[0] == '*' && !first)
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '+' && !first)
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else if (linea.Length > 0 && linea[0] == '?' && !first)
                {
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
                        Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA RESERVADAS()");
                        return false;
                    }
                    if (linea.Length > 0 && linea[0] == '}')
                    {
                        linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    }
                    else
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA {"}"}");
                        return false;
                    }
                }
                else if (linea.Length > 0 && linea[0] == '|' && !first)
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });

                    if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                    {
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
                            else
                            {
                                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");
                        return false;
                    }
                }
                else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                {
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
                        else
                        {
                            Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                            return false;
                        }
                    }
                }
                else if (linea.Length > 0 && linea[0] == '(')
                {
                    var posicion = 0;
                    var cerrarParentesis = false;
                    var firstParentesis = true;
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                    while (posicion < linea.Length && !cerrarParentesis)
                    {
                        if (linea.Length > 0 && linea[0] == ')')
                        {
                            if (firstParentesis)
                            {
                                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA CONTENIDO DENTRO DEL PARENTESIS");
                                return false;
                            }
                            else
                            {
                                cerrarParentesis = true;
                                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                            }
                        }
                        else if (linea.Length > 0 && linea[0] == '*' && !firstParentesis)
                        {
                            linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                        }
                        else if (linea.Length > 0 && linea[0] == '+' && !firstParentesis)
                        {
                            linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                        }
                        else if (linea.Length > 0 && linea[0] == '?' && !firstParentesis)
                        {
                            linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                        }
                        else if (linea.Length > 0 && linea[0] == '|' && !firstParentesis)
                        {
                            linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });

                            if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                            {
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
                                    else if (linea[0] == '\t' || linea[0] == ' ' || linea[0] == '(' || linea[0] == ')' || linea[0] == '\'' || linea[0] == '{')
                                    {
                                        TokensCompletados = true;
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        pos++;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");
                                return false;
                            }
                        }
                        else if (linea.Length > 2 && linea[0] == '\'' && PerteneceASCII(linea[1]) && linea[2] == '\'')
                        {
                            linea = linea.Substring(3).Trim(new char[2] { '\t', ' ' });
                            firstParentesis = false;
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
                                else if (linea[0] == '\t' || linea[0] == ' ' || linea[0] == ')' || linea[0] == '\'' || linea[0] == '{')
                                {
                                    TokensCompletados = true;
                                    linea = linea.Trim(new char[2] { '\t', ' ' });
                                    firstParentesis = false;
                                    pos++;
                                }
                                else
                                {
                                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                                    return false;
                                }
                            }
                        }
                    }
                    if (!cerrarParentesis)
                    {
                        Console.WriteLine($"ERROR LINEA: {contLinea} FALTA CERRAR PARENTESIS");
                        return false;
                    }
                    first = false;
                }
                else
                {
                    Console.WriteLine($"ERROR LINEA: {contLinea} TOKEN INCOMPLETO");
                    return false;
                }
            }

            return true;
        }

        //Toma cada action recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static bool AprobarActions(string linea, string ER)
        {
            var arbol = CreacionArbol(ER);
            var pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");
                return false;
            }
            else
            {
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} FALTA SIGNO '=' Y DEFINICION");
                return false;
            }

            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");
                return false;
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
                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA LETRA MAYUSCULA");
                    return false;
                }
                else
                {
                    linea = linea.Substring(pos);
                }
                if (linea.Length > 0 && linea[0] == '\'')
                {
                    linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
                }
                else
                {
                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA '");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA '");
                return false;
            }

            return true;
        }

        //Toma cada errors recibido del txt en donde regresa false al momento de detectar un error o true si todo esta correcto
        static bool AprobarErrors(string linea, string ER)
        {
            var arbol = CreacionArbol(ER);
            if (linea[0] == '=')
            {
                linea = linea.Substring(1).Trim(new char[2] { '\t', ' ' });
            }
            else
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN '='");
                return false;
            }

            if (linea.Length == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UNA DEFINICION");
                return false;
            }

            var pos = 0;
            while (pos < linea.Length && PerteneceDigito(linea[pos]))
            {
                pos++;
            }
            if (pos == 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN NUMERO");
                return false;
            }
            else
            {
                linea = linea.Substring(pos).Trim(new char[2] { '\t', ' ' });
            }
            if (linea.Length != 0)
            {
                Console.WriteLine($"ERROR LINEA: {contLinea} CARACTER INGRESADO NO ES DIGITO");
                return false;
            }

            return true;
        }

        #endregion

        #region MANEJO DE ARBOL DE EXPRESIONES
        //Informacion para diferenciar entre terminales y no terminales
        static List<string> Terminales = new List<string>() {"a", "b", "c", "d", "e", "f", "\\#" };

        //Metodo para crear el arbol de expresion
        public static Nodo CreacionArbol(string linea)
        {
            var S = new Stack<Nodo>();
            var T = new Stack<string>();
            //var Op = new List<char>() { '+', '*', '.', '?' };
            var Jerarquia = new Dictionary<string, int>();
            Jerarquia.Add("*", 1);
            Jerarquia.Add("+", 1);
            Jerarquia.Add("?", 1);
            Jerarquia.Add(".", 2);
            Jerarquia.Add("|", 3);

            /*
             * a = [A...Z]
             * b = " "
             * c = [A...Z] y [0...9]
             * d = [0..9]
             * e = \n
             * f = \t
             */

            var St = new List<string>() { "S", "E", "T", "S", "C", "H", "R", "a", "b", "c", "d", "e", "f", "=", "'", "\\.", "\\(", "\\)", "\\+", "\\#" };

            var pos = 0;
            while (pos < linea.Length)
            {
                var Token = linea[pos].ToString();
                if (Token == "\\")
                {
                    pos++;
                    Token += linea[pos].ToString();
                }

                if (St.Contains(Token.ToString()))
                {
                    S.Push(new Nodo() { simbolo = Token });
                }
                else if (Token == "(")
                {
                    T.Push(Token);
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
                    T.Pop();
                }
                else if (Jerarquia.ContainsKey(Token))
                {
                    //Revisar que es unario
                    //Token == '*' || Token == '+' || Token == '?'
                    if (Jerarquia[Token] == 1)
                    {
                        var nodo = new Nodo { simbolo = Token };
                        nodo.HijoIzquierdo = S.Pop();
                        S.Push(nodo);
                    }
                    else if (T.Count != 0 && T.Peek() != "(" && Jerarquia[Token] <= Jerarquia[T.Peek()])
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
                    //Token == '|' || Token == '.'
                    if (Jerarquia[Token] == 2 || Jerarquia[Token] == 3)
                    {
                        T.Push(Token);
                    }
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
        #endregion

        #region VALIDACIONES DE CARACTERES
        //Metodo verifica que este entre el rango de caracteres permitidos
        static bool PerteneceASCII (char linea)
        {
            if (linea > 31 && linea < 256)
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

        static void Main(string[] args)
        {
            //Booleana central que maneja cuando parar el analisis del txt
            var Correcto = true;
            try
            {
                var path = Console.ReadLine().Trim('"');

                using (var sr = new StreamReader(path))
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
                    while (Correcto && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
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
                                Correcto = false;
                                Console.WriteLine($"ERROR EN LINEA: {contLinea} NO EMPIEZA CON SETS O TOKENS");
                            }
                            contLinea++;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                            Correcto = false;
                        }
                    }
                    continuar = true;
                    #endregion

                    #region SETS
                    while (Correcto && ContieneSets && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "TOKENS")
                            {
                                ContieneTokens = true;
                                continuar = false;
                            }
                            else if (linea == "") { }
                            else if (!AprobarSets(linea, "a.b.c.(d.c)*.\\#"))
                            {
                                Correcto = false;
                            }
                            else
                            {
                                conteo++;
                            }
                            contLinea++;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                            Correcto = false;
                        }
                    }

                    continuar = true;

                    if (conteo == 0 && ContieneSets && Correcto)
                    {
                        Console.WriteLine($"ERROR EN LINEA: {contLinea} DEBE DE IR UN SET");
                        Correcto = false;
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
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "ACTIONS")
                            {
                                ContieneActions = true;
                                continuar = false;
                            }
                            else if (linea == "") { }
                            else if (!AprobarTokens(linea, "a.b.c+.d.e*.\\#"))
                            {
                                Correcto = false;
                            }
                            else if (AprobarTokens(linea, "a.b.c+.d.e*.\\#"))
                            {
                                conteo++;
                            }
                            contLinea++;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                            Correcto = false;
                        }
                    }

                    continuar = true;
                    if (conteo == 0 && ContieneTokens && Correcto)
                    {
                        Console.WriteLine($"ERROR EN LINEA: {contLinea} DEBE DE IR UN TOKEN");
                        Correcto = false;
                    }
                    else
                    {
                        conteo = 0;
                    }

                    #endregion

                    #region ACTIONS
                    var contieneReservadas = false;
                    while (Correcto && ContieneActions && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea == "RESERVADAS()")
                            {
                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "{")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea != "")
                                        {
                                            Console.WriteLine($"ERROR EN LINEA: {contLinea} SE ESPERABA {"{"}");
                                            Correcto = false;
                                        }
                                        contLinea++;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                                        Correcto = false;
                                    }
                                }
                                continuar = true;

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "}")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea == "") { }
                                        else if (!AprobarActions(linea, "a.b.c+.d.\\#"))
                                        {
                                            Correcto = false;
                                        }
                                        else if (AprobarActions(linea, "a.b.c+.d.\\#"))
                                        {
                                            conteo++;
                                        }
                                        contLinea++;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                                        Correcto = false;
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
                                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN IDENTIFICADOR");
                                    Correcto = false;
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
                                    Console.WriteLine($"ERROR LINEA: {contLinea} SE ESPERABA UN ()");
                                    Correcto = false;
                                }

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "{")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea != "")
                                        {
                                            Console.WriteLine($"ERROR EN LINEA: {contLinea} SE ESPERABA {"{"}");
                                            Correcto = false;
                                        }
                                        contLinea++;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                                        Correcto = false;
                                    }
                                }
                                continuar = true;

                                while (Correcto && continuar)
                                {
                                    if ((linea = sr.ReadLine()) != null)
                                    {
                                        linea = linea.Trim(new char[2] { '\t', ' ' });
                                        if (linea == "}")
                                        {
                                            continuar = false;
                                        }
                                        else if (linea == "") { }
                                        else if (!AprobarActions(linea, "a.b.c+.d.\\#"))
                                        {
                                            Correcto = false;
                                        }
                                        else if (AprobarActions(linea, "a.b.c+.d.\\#"))
                                        {
                                            conteo++;
                                        }
                                        contLinea++;
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                                        Correcto = false;
                                    }
                                }
                                continuar = true;

                            }
                            else if (linea != "")
                            {
                                Console.WriteLine($"ERROR EN LINEA: {contLinea} SE ESPERABA SE ESPERABA FUNCIONES");
                                Correcto = false;
                            }
                            contLinea++;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR EN LINEA: {contLinea} ARCHIVO INCOMPLETO");
                            Correcto = false;
                        }

                    }

                    continuar = true;

                    if (Correcto && !contieneReservadas)
                    {
                        Console.WriteLine($"ERROR EN LINEA: {contLinea} DEBE DE IR RESERVADAS()");
                        Correcto = false;
                    }

                    if (conteo == 0 && ContieneActions && Correcto)
                    {
                        Console.WriteLine($"ERROR EN LINEA: {contLinea} DEBE DE IR UN ACTION");
                        Correcto = false;
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
                        var aprueba = AprobarErrors(linea, "a.b.c+.\\#");
                        if (aprueba)
                        {
                            conteo++;
                        }
                        else
                        {
                            Correcto = false;
                            Console.WriteLine($"ERROR EN LINEA: {contLinea} SE ESPERABA ERRORS");
                        }
                    }

                    while (Correcto && continuar)
                    {
                        if ((linea = sr.ReadLine()) != null)
                        {
                            linea = linea.Trim(new char[2] { '\t', ' ' });
                            if (linea.Length > 4 && linea.Substring(0, 5) == "ERROR")
                            {
                                linea = linea.Substring(5).Trim(new char[2] { '\t', ' ' });
                                var aprueba = AprobarErrors(linea, "a.b.c+.\\#");
                                if (aprueba)
                                {
                                    conteo++;
                                }
                                else
                                {
                                    Correcto = false;
                                }
                            }
                            else if (linea != "")
                            {
                                Console.WriteLine($"ERROR EN LINEA: {contLinea} SE ESPERABA ERRORS");
                                Correcto = false;
                            }
                            contLinea++;
                        }
                        else
                        {
                            continuar = false;
                        }
                    }

                    if (conteo == 0 && Correcto)
                    {
                        Console.WriteLine($"ERROR EN LINEA: {contLinea} DEBE DE IR UN ERROR");
                        Correcto = false;
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            //Si todo se ha cumplido regresa el mensaje de formato correcto
            if (Correcto)
            {
                Console.WriteLine($"FORMATO CORRECTO");
            } 

            Console.ReadKey();
        }
    }
}
