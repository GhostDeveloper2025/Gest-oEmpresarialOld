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
    /// Interação lógica para BotaoImprimir.xam
    /// </summary>
    //public partial class BotaoImprimir : Button
    //{
    //    public BotaoImprimir()
    //    {
    //        InitializeComponent();
    //    }

    //    private void Imprimir_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (DataContext is CadastroVendaViewModel vendaViewModel)
    //        {
    //            // Impressão para Venda
    //            int? id = vendaViewModel.Id;
    //            var func = DI.CadastrosViews[nameof(RelatorioReciboVendaViewModel)];
    //            if (id.HasValue)
    //                Switcher.Imprimir(func(id));
    //        }
    //        else if (DataContext is CadastroOrdemServicoViewModel ordemServicoViewModel)
    //        {
    //            // Impressão para Ordem de Serviço
    //            int? id = ordemServicoViewModel.Id;
    //            var func = DI.CadastrosViews[nameof(RelatorioReciboOrdemServicoViewModel)];
    //            if (id.HasValue)
    //                Switcher.Imprimir(func(id));
    //        }
    //        else
    //        {
    //            // Exibe uma mensagem caso o DataContext não seja reconhecido
    //            MessageBox.Show(
    //                "O tipo de contexto atual não suporta a funcionalidade de impressão.",
    //                "Erro de Impressão",
    //                MessageBoxButton.OK,
    //                MessageBoxImage.Warning);
    //        }
    //        //Fecha a janela que contém o botão
    //        Window parentWindow = Window.GetWindow(this);
    //        parentWindow?.Close();
    //    }
    //}

    public partial class BotaoImprimir : Button
    {
        public static readonly DependencyProperty FecharJanelaPropriedade =
          DependencyProperty.Register("FecharJanela", typeof(bool), typeof(BotaoImprimir), new UIPropertyMetadata(false));

        public bool FecharJanela
        {
            get
            {
                return (bool)GetValue(FecharJanelaPropriedade);
            }
            set
            {
                SetValue(FecharJanelaPropriedade, value);
            }
        }

        public BotaoImprimir()
        {
            InitializeComponent();
        }

        // Funcionou
        public void Imprimir(string chave, int? id)
        {
            var func = DI.CadastrosViews[chave];
            if (id.HasValue)
            {
                UserControl uc = func(id);
                PrintDialog printDlg = new PrintDialog();

                if (printDlg.ShowDialog() == true)
                {
                    // Força o UserControl a renderizar completamente antes da impressão
                    uc.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    uc.Arrange(new Rect(uc.DesiredSize));
                    uc.UpdateLayout();

                    // Calcula a escala para ajustar à área de impressão
                    Size printableSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                    double scaleX = printableSize.Width / uc.ActualWidth;
                    double scaleY = printableSize.Height / uc.ActualHeight;
                    double scale = Math.Min(scaleX, scaleY);
                    scale = Math.Max(scale, 0.8); // Garante que a escala não fique muito pequena

                    uc.LayoutTransform = new ScaleTransform(scale, scale);

                    // Renderiza a interface gráfica antes de imprimir
                    uc.Dispatcher.Invoke(() =>
                    {
                        uc.Measure(printableSize);
                        uc.Arrange(new Rect(printableSize));
                        uc.UpdateLayout();
                    }, System.Windows.Threading.DispatcherPriority.Render);

                    // Realiza a impressão
                    printDlg.PrintVisual(uc, "Imprimindo User Control");
                }
            }
        }
        private void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CadastroVendaViewModel vendaViewModel)
            {
                // Impressão para Venda
                int? id = vendaViewModel.Id;
                Imprimir(nameof(RelatorioReciboVendaViewModel), id);

            }
            else if (DataContext is CadastroOrdemServicoViewModel ordemServicoViewModel)
            {
                // Impressão para Ordem de Serviço
                int? id = ordemServicoViewModel.Id;
                Imprimir(nameof(RelatorioReciboOrdemServicoViewModel), id);
            }
            else
            {
                // Exibe uma mensagem caso o DataContext não seja reconhecido
                MessageBox.Show(
                    "O tipo de contexto atual não suporta a funcionalidade de impressão.",
                    "Erro de Impressão",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            if (FecharJanela)
            {
                //Fecha a janela que contém o botão
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Close();
            }
        }
    }
}
