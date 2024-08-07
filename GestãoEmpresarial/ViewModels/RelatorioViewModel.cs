using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public abstract class RelatorioViewModel<RelatorioModel>
    {
        public abstract RelatorioModel GetRelatorioModel();
    }
}
