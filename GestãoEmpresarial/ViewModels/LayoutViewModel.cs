using GestãoEmpresarial.Services;
using GestãoEmpresarial.Themes;
using MaterialDesignThemes.Wpf;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestãoEmpresarial.ViewModels
{
    public class LayoutViewModel : INotifyPropertyChanged
    {
        // Instância do serviço de configuração
        private readonly ConfiguracaoService _configuracaoService;
        private bool _isDarkTheme;
        private string _header;
        private PackIconKind _icon;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ToggleThemeCommand { get; private set; }

        private UserControl _currentView;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public bool IsDarkTheme
        {
            get { return _isDarkTheme; }
            set
            {
                if (_isDarkTheme != value)
                {
                    _isDarkTheme = value;
                    OnPropertyChanged();
                    ApplyTheme(); // O método ApplyTheme agora também salvará a escolha
                }
            }
        }
        public string Header
        {
            get { return _header; }
            set
            {
                if (_header != value)
                {
                    _header = value;
                    OnPropertyChanged();
                }
            }
        }

        public PackIconKind Icon
        {
            get { return _icon; }
            set
            {
                if (_icon != value)
                {
                    _icon = value;
                    OnPropertyChanged();
                }
            }
        }

        public LayoutViewModel()
        {
            // 1. Ainda precisamos do serviço para SALVAR o tema depois
            _configuracaoService = new ConfiguracaoService();

            // 2. O comando para o botão continua igual
            ToggleThemeCommand = new RelayCommand<object>(ExecuteToggleTheme);

            // 3. AQUI ESTÁ A ÚNICA LÓGICA NECESSÁRIA AGORA:
            //    Ele simplesmente define o estado do botão (true/false)
            //    baseado na informação que o ThemesController já possui.
            _isDarkTheme = ThemesController.CurrentTheme == ThemesController.ThemeTypes.Dark;
        }

        private void ExecuteToggleTheme(object parameter)
        {
            IsDarkTheme = !IsDarkTheme;
        }

        private void ApplyTheme()
        {
            ThemesController.SetTheme(IsDarkTheme ? ThemesController.ThemeTypes.Dark : ThemesController.ThemeTypes.Light);
            _configuracaoService.SalvarTema(IsDarkTheme);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
