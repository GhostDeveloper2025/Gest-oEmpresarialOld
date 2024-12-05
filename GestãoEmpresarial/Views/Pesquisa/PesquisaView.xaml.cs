using GestãoEmpresarial.Models.Atributos;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GestãoEmpresarial.Views.Pesquisa
{
    /// <summary>
    /// Interação lógica para PesquisaView.xam
    /// </summary>
    public partial class PesquisaView : UserControl
    {
        private readonly IPesquisaViewModel _viewModel;

        private DispatcherTimer _toggleTimer;

        //public PesquisaView(IPesquisaViewModel viewModel)
        //    : this(viewModel, new PesquisaBarraView())
        //{
        //}

        public PesquisaView(IPesquisaViewModel viewModel, UIElement barraPesquisa)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;
            if (barraPesquisa != null)
            {
                fContainer.Children.Add(barraPesquisa);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var func = DI.CadastrosViews[_viewModel.NomeEditarView];
            //Switcher.Switch(EditViewType, new[] { DataGridGlobal.SelectedItem });
            Switcher.Switch(func(_viewModel.Id));
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
