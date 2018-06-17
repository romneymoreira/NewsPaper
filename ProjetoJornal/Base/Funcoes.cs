using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ProjetoJornal.Base
{
    public class Funcoes
    {
        public string RemoveTagsHTML(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public string RemoverMascaras(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return str
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "")
                .Replace("_", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace("/", "");
        }

        public string TrataTelefone(string numero)
        {
            //Recupera somente os digitos
            numero = Regex.Replace(numero, @"[^\d]", "");

            //Se ddd começa com zero, remove
            if (numero.StartsWith("0"))
                numero = numero.Remove(0, 1);

            //Valida se é telefone valido
            if (numero.Length < 10 || numero.Length > 11)
                throw new Exception("Telefone deve conter no mínimo 10 e no máximo 11 digitos");

            return numero;
        }
        public string TelefoneFormatado(string telefone)
        {
            if (telefone != null)
            {
                if (telefone.Length == 10)
                    return string.Format("{0:(##) ####-####}", telefone);
                if (telefone.Length == 11)
                    return string.Format("{0:(##) ####-#####}", telefone);
            }
            return string.Empty;
        }

        public string RetornarSubString(int tamanho, string texto)
        {
            if (texto.Length > tamanho)
                return texto.Substring(0, tamanho) + " ...";

            return texto;
        }
    }
}