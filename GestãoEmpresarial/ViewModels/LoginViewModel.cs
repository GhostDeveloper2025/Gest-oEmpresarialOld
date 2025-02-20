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
        // Campo privado que armazena o estado de foco do TextBox.
        // Inicializa como true para que o TextBox receba foco assim que a View for carregada.
        private bool _isTextBoxFocused = true;

        // Propriedade pública que permite o binding com a View (XAML). Neste Caso Para adicionar um foco no TextBox
        // Essa propriedade é observada pela View, e qualquer mudança nela reflete no controle associado.
        public bool IsTextBoxFocused
        {
            get => _isTextBoxFocused;
            set
            {
                _isTextBoxFocused = value;
                RaisePropertyChanged(nameof(IsTextBoxFocused));
            }
        }

        private static readonly LoginViewModel _viewModel = new LoginViewModel();
        private bool _estaAutenticando;
        private double _progressoLogin;

        private LoginViewModel()
        {
#if DEBUG
            Usuario = "ROOT@GMAIL.COM";
            Senha = "0000";
#endif
            LoginCommand = new RelayCommand(ExecutarLoginAsync, PodeAutenticar);
        }

        public static LoginViewModel Instancia => _viewModel;

        public string Usuario { get; set; }
        public string Senha { get; set; }
        public string Erro { get; set; }

        public bool EstaAutenticando
        {
            get => _estaAutenticando;
            set
            {
                if (_estaAutenticando != value)
                {
                    _estaAutenticando = value;
                    RaisePropertyChanged(nameof(EstaAutenticando));
                }
            }
        }

        public double ProgressoLogin
        {
            get => _progressoLogin;
            set
            {
                if (_progressoLogin != value)
                {
                    _progressoLogin = value;
                    RaisePropertyChanged(nameof(ProgressoLogin));
                }
            }
        }

        public ICommand LoginCommand { get; }

        public bool PodeAutenticar() => !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Senha);

        public async void ExecutarLoginAsync()
        {
            EstaAutenticando = true;
            ProgressoLogin = 0;

            try
            {
                var repo = new RLoginDAL();
                colaborador = await Task.Run(() => repo.AutenticaoValida(Usuario, Senha));

                if (colaborador != null)
                {
                    Erro = null;
                    // Redirecionar para outra página
                    var func = DI.PaginasView[nameof(LayoutView)];
                    Switcher.SwitchPagina(func());
                }
                else
                {
                    Erro = "Email ou senha errados!";
                }
            }
            catch (Exception ex)
            {
                Erro = $"Erro durante o login: {ex.Message}";
            }
            finally
            {
                EstaAutenticando = false;
                ProgressoLogin = 1;
                RaisePropertyChanged(nameof(Erro));
            }
        }

        public ColaboradorModel colaborador { get; private set; }
    }
}
