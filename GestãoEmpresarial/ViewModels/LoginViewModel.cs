using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Views.Layout;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    public sealed class LoginViewModel : ObservableObject
    {
        private static readonly LoginViewModel _viewModel = new LoginViewModel();

        private LoginViewModel()
        {
#if DEBUG
            Usuario = "ROOT@GMAIL.COM";
            Senha = "0000";
#endif
            LoginCommand = new RelayCommand(Autenticar, PodeAutenticar);
        }

        //static LoginViewModel() { } // construtor estatico 

        public static LoginViewModel Instancia
        {
            get
            {
                return _viewModel;
            }
        }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public string Erro { get; set; }

        public ICommand LoginCommand { get; set; }

        public bool PodeAutenticar()
        {
            return !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Senha);
        }

        public void Autenticar()
        {
            var repo = new RLoginDAL();
            colaborador = repo.AutenticaoValida(Usuario, Senha);
            if (colaborador != null)
            {
                Erro = null;
                // vai a bd e ve se existe o colaborador
                // colaborador = resultado da bd
                var func = DI.PaginasView[nameof(LayoutView)];
                Switcher.SwitchPagina(func());
            }
            else
            {
                Erro = "Email ou senha erradas!";
                RaisePropertyChanged(nameof(Erro));
            }
        }

        // este objecto (que é uma propriedade), é sempre único em toda a aplicação
        public ColaboradorModel colaborador { get; private set; }
    }
}
