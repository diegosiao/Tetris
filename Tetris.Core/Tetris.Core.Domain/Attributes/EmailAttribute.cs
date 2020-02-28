using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris.Core.Domain.Attributes
{
    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute(string errorMessage = "Informe um e-mail válido") : base(errorMessage)
        {

        }

        public override bool IsValid(object value)
        {


            return true;
        }
    }
}
