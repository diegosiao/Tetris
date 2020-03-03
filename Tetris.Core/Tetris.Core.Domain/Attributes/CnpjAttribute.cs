using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tetris.Core.Domain.Attributes
{
    /// <summary>
    /// A pt-BR thing...
    /// </summary>
    public class CnpjAttribute : ValidationAttribute
    {
        private bool _accetptsMask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="acceptsMask"></param>
        public CnpjAttribute(string errorMessage = "Informe um CNPJ válido", bool acceptsMask = false) : base(errorMessage)
        {
            _accetptsMask = acceptsMask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value != null && !(value is string))
            {
                ErrorMessage = "O CNPJ deve ser representado por uma string";
                return false;
            }

            var cnpj = (string)value;

            if (string.IsNullOrEmpty(cnpj))
                return true;

            if (cnpj.Length != 14 && (_accetptsMask && cnpj.Length != 18))
                return false;

            if (!Regex.Match(cnpj, @"([0-9]{14})|([0-9]{2}.[0-9]{3}.[0-9]{3}\/[0-9]{4}-[0-9]{2})").Success)
                return false;
            
            return IsValidCnpj(cnpj);
        }

        private static bool IsValidCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
