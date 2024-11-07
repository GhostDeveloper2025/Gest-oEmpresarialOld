using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Providers
{
    internal class ColaboradorVendedorProvider : ColaboradorProvider
    {
        protected override string TipoColaborador => "VENDEDOR";
    }
}
