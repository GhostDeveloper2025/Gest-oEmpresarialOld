using GestãoEmpresarial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class LoginViewModel
    {
        // este objecto (que é uma propriedade), é sempre único em toda a aplicação
        public static ColaboradorModel colaborador { get; private set; }

        private void Login(string usuario, string senha)
        {
            // vai a bd e ve se existe o colaborador
            // colaborador = resultado da bd
        }

#if DEBUG

        static LoginViewModel()
        {
            colaborador = new ColaboradorModel()
            {
                IdFuncionario = 1,
                Nome = "Antonio",
                Cargo = "chefe"
            };
        }

#endif
    }
}
