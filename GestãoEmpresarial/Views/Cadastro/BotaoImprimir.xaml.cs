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
    // Este tem o método async para trabalhar em uma trhad separada

    public partial class BotaoImprimir : Button // testando ainda
    {
        public static readonly DependencyProperty FecharJanelaPropriedade =
            DependencyProperty.Register("FecharJanela", typeof(bool), typeof(BotaoImprimir), new UIPropertyMetadata(false));

        public bool FecharJanela
        {
            get => (bool)GetValue(FecharJanelaPropriedade);
            set => SetValue(FecharJanelaPropriedade, value);
        }

        public BotaoImprimir()
        {
            InitializeComponent();
        }

        // Executa a impressão de forma assíncrona
        public async Task ImprimirAsync(string chave, int? id)
        {
            if (!id.HasValue) return;

            var func = DI.CadastrosViews[chave];
            UserControl uc = func(id);

            // Executa a impressão em uma thread separada
            await Task.Run(() =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PrintDialog printDlg = new PrintDialog();
                        if (printDlg.ShowDialog() == true)
                        {
                            uc.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                            uc.Arrange(new Rect(uc.DesiredSize));
                            uc.UpdateLayout();

                            Size printableSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                            double scaleX = printableSize.Width / uc.ActualWidth;
                            double scaleY = printableSize.Height / uc.ActualHeight;
                            double scale = Math.Min(scaleX, scaleY);
                            scale = Math.Max(scale, 0.8); // Garante que a escala não fique muito pequena

                            uc.LayoutTransform = new ScaleTransform(scale, scale);

                            // Força o render da interface antes da impressão
                            uc.Dispatcher.Invoke(() =>
                            {
                                uc.Measure(printableSize);
                                uc.Arrange(new Rect(printableSize));
                                uc.UpdateLayout();
                            }, System.Windows.Threading.DispatcherPriority.Render);

                            // Executa a impressão
                            printDlg.PrintVisual(uc, "Imprimindo User Control");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(
                            "Erro ao imprimir Impressora Desconectada Ou Inoperante: " + ex.Message,
                            "Erro de Impressão",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    });
                }
            });
        }

        private async void Imprimir_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is CadastroVendaViewModel vendaViewModel)
            {
                await ImprimirAsync(nameof(RelatorioReciboVendaViewModel), vendaViewModel.Id);
            }
            else if (DataContext is CadastroOrdemServicoViewModel ordemServicoViewModel)
            {
                await ImprimirAsync(nameof(RelatorioReciboOrdemServicoViewModel), ordemServicoViewModel.Id);
            }
            else
            {
                MessageBox.Show(
                    "O tipo de contexto atual não suporta a funcionalidade de impressão.",
                    "Erro de Impressão",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            if (FecharJanela)
            {
                Window parentWindow = Window.GetWindow(this);
                parentWindow?.Close();
            }
        }
    }


    // Este antigo funciona so que nao trata erros de impressora
    //public partial class BotaoImprimir : Button
    //{
    //    public static readonly DependencyProperty FecharJanelaPropriedade =
    //      DependencyProperty.Register("FecharJanela", typeof(bool), typeof(BotaoImprimir), new UIPropertyMetadata(false));

    //    public bool FecharJanela
    //    {
    //        get
    //        {
    //            return (bool)GetValue(FecharJanelaPropriedade);
    //        }
    //        set
    //        {
    //            SetValue(FecharJanelaPropriedade, value);
    //        }
    //    }

    //    public BotaoImprimir()
    //    {
    //        InitializeComponent();
    //    }

    //    // Funcionou
    //    public void Imprimir(string chave, int? id)
    //    {
    //        var func = DI.CadastrosViews[chave];
    //        if (id.HasValue)
    //        {
    //            UserControl uc = func(id);
    //            PrintDialog printDlg = new PrintDialog();

    //            if (printDlg.ShowDialog() == true)
    //            {
    //                // Força o UserControl a renderizar completamente antes da impressão
    //                uc.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
    //                uc.Arrange(new Rect(uc.DesiredSize));
    //                uc.UpdateLayout();

    //                // Calcula a escala para ajustar à área de impressão
    //                Size printableSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
    //                double scaleX = printableSize.Width / uc.ActualWidth;
    //                double scaleY = printableSize.Height / uc.ActualHeight;
    //                double scale = Math.Min(scaleX, scaleY);
    //                scale = Math.Max(scale, 0.8); // Garante que a escala não fique muito pequena

    //                uc.LayoutTransform = new ScaleTransform(scale, scale);

    //                // Renderiza a interface gráfica antes de imprimir
    //                uc.Dispatcher.Invoke(() =>
    //                {
    //                    uc.Measure(printableSize);
    //                    uc.Arrange(new Rect(printableSize));
    //                    uc.UpdateLayout();
    //                }, System.Windows.Threading.DispatcherPriority.Render);

    //                // Realiza a impressão
    //                printDlg.PrintVisual(uc, "Imprimindo User Control");
    //            }
    //        }
    //    }
    //    private void Imprimir_Click(object sender, RoutedEventArgs e)
    //    {
    //        if (DataContext is CadastroVendaViewModel vendaViewModel)
    //        {
    //            // Impressão para Venda
    //            int? id = vendaViewModel.Id;
    //            Imprimir(nameof(RelatorioReciboVendaViewModel), id);

    //        }
    //        else if (DataContext is CadastroOrdemServicoViewModel ordemServicoViewModel)
    //        {
    //            // Impressão para Ordem de Serviço
    //            int? id = ordemServicoViewModel.Id;
    //            Imprimir(nameof(RelatorioReciboOrdemServicoViewModel), id);
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

    //        if (FecharJanela)
    //        {
    //            //Fecha a janela que contém o botão
    //            Window parentWindow = Window.GetWindow(this);
    //            parentWindow?.Close();
    //        }
    //    }
    //}
}
