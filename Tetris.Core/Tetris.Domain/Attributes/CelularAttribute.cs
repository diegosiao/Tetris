using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Tetris
{
    /// <summary>
    /// A pt-BR thing...
    /// </summary>
    public class CelularAttribute : ValidationAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        public CelularAttribute(string errorMessage = "Informe um número de celular com DDD") : base(errorMessage)
        {

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
                ErrorMessage = "O celular deve ser representado por uma string";
                return false;
            }

            string celular = (string)value;

            return celular.Length == 11 && Regex.Match(celular, @"([0-9]{11})").Success;
        }
    }
}
