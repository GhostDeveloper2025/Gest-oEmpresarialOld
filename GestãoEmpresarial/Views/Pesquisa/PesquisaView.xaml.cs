using GestãoEmpresarial.Models.Atributos;
using GestãoEmpresarial.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestãoEmpresarial.Views.Pesquisa
{
    /// <summary>
    /// Interação lógica para PesquisaView.xam
    /// </summary>
    public partial class PesquisaView : UserControl
    {
        private readonly IPesquisaViewModel _viewModel;

        public PesquisaView(IPesquisaViewModel viewModel)
            : this(viewModel, new PesquisaBarraView())
        {
        }

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
    }
}
