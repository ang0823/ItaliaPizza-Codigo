using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ClienteItaliaPizza.Validacion
{
    public class Validador
    {
        public bool validarLetrasSinAcentosYNumeros(string texto)
        {
            string formato = "[a-zA-Z0-9._]";
            if (Regex.IsMatch(texto, formato))
            {
                if (Regex.Replace(texto, formato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool validarLetrasConAcentosYNumeros(string texto)
        {
            string formato = "[a-zA-ZäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9._]";
            if (Regex.IsMatch(texto, formato))
            {
                if (Regex.Replace(texto, formato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool validarSoloLetrasConAcentos(string texto)
        {
            // string formato = "[a-zA-Z]";
            string formato = @"[ A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ]+";
            if (Regex.IsMatch(texto, formato))
            {
                if (Regex.Replace(texto, formato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool validarSoloNumeros(string entrada)
        {
            string formato = "[0-9]";
            if (Regex.IsMatch(entrada, formato))
            {
                if (Regex.Replace(entrada, formato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
