using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models.Atributos
{
    public class DisplayFormatDataBRAttribute : DisplayFormatAttribute
    {
        public DisplayFormatDataBRAttribute() { DataFormatString = "dd/MM/yyyy"; }
    }
}
