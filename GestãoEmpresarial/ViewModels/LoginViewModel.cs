using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Services;
using GestãoEmpresarial.Views.Layout;
using MicroMvvm;
using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private static readonly LoginViewModel _viewModel = new LoginViewModel();
        public static LoginViewModel Instancia => _viewModel;

        // O tipo do serviço para o novo nome / Salva Usuario e Senha
        private readonly ConfiguracaoService _configuracaoService;

        private bool _lembrarUsuario;
        private bool _estaAutenticando;
        private double _progressoLogin;
        private string _usuario;
        private string _senha;
        private string _erro;
        //Campo privado que armazena o estado de foco do TextBox.
        //Inicializa como true para que o TextBox receba foco assim que a View for carregada.
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

        public LoginViewModel()
        {

            // ALTERADO: Instancia o novo serviço
            _configuracaoService = new ConfiguracaoService();
            LoginCommand = new RelayCommand(async () => await ExecutarLoginAsync(), PodeAutenticar);

            CarregarCredenciais();
        }

        public bool LembrarUsuario
        {
            get => _lembrarUsuario;
            set
            {
                if (_lembrarUsuario != value)
                {
                    _lembrarUsuario = value;
                    RaisePropertyChanged(nameof(LembrarUsuario));
                }
            }
        }

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

        public string Usuario
        {
            get => _usuario;
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    RaisePropertyChanged(nameof(Usuario));

                    // Sempre que o usuário for alterado, limpar a senha
                    Senha = string.Empty;
                }
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                if (_senha != value)
                {
                    _senha = value;
                    RaisePropertyChanged(nameof(Senha));
                }
            }
        }

        public string Erro
        {
            get => _erro;
            set
            {
                if (_erro != value)
                {
                    _erro = value;
                    RaisePropertyChanged(nameof(Erro));
                }
            }
        }

        public ICommand LoginCommand { get; }

        public bool PodeAutenticar() => !string.IsNullOrWhiteSpace(Usuario) && !string.IsNullOrWhiteSpace(Senha);

        public async Task ExecutarLoginAsync()
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

                    if (LembrarUsuario)
                        _configuracaoService.SalvarCredenciais(Usuario, Senha);
                    else
                        _configuracaoService.LimparCredenciais();

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
            }
        }

        public ColaboradorModel colaborador { get; private set; }

        private void CarregarCredenciais()
        {
            var credenciais = _configuracaoService.CarregarCredenciais();
            if (credenciais.HasValue)
            {
                Usuario = credenciais.Value.usuario;
                Senha = credenciais.Value.senha;
                LembrarUsuario = true;
            }
        }
    }
}
