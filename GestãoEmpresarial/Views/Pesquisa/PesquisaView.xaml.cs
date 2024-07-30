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

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var prop = ((PropertyDescriptor)e.PropertyDescriptor);
            if (prop.Attributes.OfType<DisplayNameAttribute>().Any() == false)
                e.Cancel = true;

            e.Column.Header = prop.DisplayName;

            //e.Column.Header = ((PropertyDescriptor)e.PropertyDescriptor).DisplayName;
            if (e.PropertyType == typeof(System.DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
            else if (e.PropertyType == typeof(System.DateTime?))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy HH:mm:ss";
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var func = DI.Views[_viewModel.NomeEditarView];
            //Switcher.Switch(EditViewType, new[] { DataGridGlobal.SelectedItem });
            Switcher.Switch(func(_viewModel.Id));
        }
    }
}
