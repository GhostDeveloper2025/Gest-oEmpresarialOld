using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GestãoEmpresarial.Views.Relatorios
{
    /// <summary>
    /// Interação lógica para RelatorioComissao.xam
    /// </summary>
    public partial class RelatorioComissao : UserControl
    {
        private DispatcherTimer _toggleTimer;
        public RelatorioComissao(RelatorioComissaoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            // Configura o timer para desmarcar o botão após 3 segundos
            _toggleTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(3) // Define o tempo em segundos
            };
            _toggleTimer.Tick += (s, args) =>
            {
                TgbBuscar.IsChecked = false; // Volta ao estado inicial
                _toggleTimer.Stop(); // Para o timer
            };
            _toggleTimer.Start(); // Inicia o timer
        }
    }
}
