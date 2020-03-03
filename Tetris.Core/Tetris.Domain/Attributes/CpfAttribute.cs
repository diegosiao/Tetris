using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tetris
{ 
    /// <summary>
    /// A pt-BR thing...
    /// </summary>
    public class CpfAttribute : ValidationAttribute
    {
        private bool _accetptsMask;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="acceptsMask"></param>
        public CpfAttribute(string errorMessage = "Informe um CPF válido", bool acceptsMask = false) : base(errorMessage)
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
                ErrorMessage = "O CPF deve ser representado por uma string";
                return false;
            }

            var cpf = (string)value;

            if (string.IsNullOrEmpty(cpf))
                return true;

            if (cpf.Length != 14 && (_accetptsMask && cpf.Length != 18))
                return false;

            if (!Regex.Match(cpf, @"([0-9]{11})|([0-9]{3}.[0-9]{3}.[0-9]{3}-[0-9]{2})").Success)
                return false;

            return IsValidCpf(cpf);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static bool IsValidCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
