using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models.Atributos
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ColumnHeaderAttribute : Attribute
    {
        public string TextoCabecalho { get; }

        public ColumnHeaderAttribute(string headerText)
        {
            TextoCabecalho = headerText;
        }
    }
}
