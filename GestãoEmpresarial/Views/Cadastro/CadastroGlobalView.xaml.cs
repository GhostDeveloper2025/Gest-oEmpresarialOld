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

namespace GestãoEmpresarial.Views.Cadastro
{
    /// <summary>
    /// Interação lógica para CadastroGlobalView.xam
    /// </summary>
    public partial class CadastroGlobalView : UserControl
    {
        public CadastroGlobalView(ICadastroViewModel viewModel, UIElement view)
        {
            InitializeComponent();
            DataContext = viewModel;
            fContainer.Children.Clear();
            fContainer.Children.Add(view);
        }

        //private void Imprimir_Click(object sender, RoutedEventArgs e)
        //{
        //    if (DataContext is CadastroVendaViewModel vendaViewModel)
        //    {
        //        // Impressão para Venda
        //        int? id = vendaViewModel.Id;
        //        var func = DI.CadastrosViews[nameof(RelatorioReciboVendaViewModel)];
        //        if (id.HasValue)
        //            Switcher.Imprimir(func(id));
        //    }
        //    else if (DataContext is CadastroOrdemServicoViewModel ordemServicoViewModel)
        //    {
        //        // Impressão para Ordem de Serviço
        //        int? id = ordemServicoViewModel.Id;
        //        var func = DI.CadastrosViews[nameof(RelatorioReciboOrdemServicoViewModel)];
        //        if (id.HasValue)
        //            Switcher.Imprimir(func(id));
        //    }
        //    else
        //    {
        //        // Exibe uma mensagem caso o DataContext não seja reconhecido
        //        MessageBox.Show(
        //            "O tipo de contexto atual não suporta a funcionalidade de impressão.",
        //            "Erro de Impressão",
        //            MessageBoxButton.OK,
        //            MessageBoxImage.Warning);
        //    }

        //}
    }
}
