using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    /// <summary>
    /// Vai servir de base (pai) para as classes viewmodel especificas do cadastro.
    /// ViewModel é colocado no DataContext da View.
    /// </summary>
    public class CadastroViewModel
    {
        public CadastroViewModel(int? id)
        {
            if (id.HasValue)
            {  
                //vamos ao bd
            }
            else
            {
                //criamos um novo model
            }
        }
    }
}
