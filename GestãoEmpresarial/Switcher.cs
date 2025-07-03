using GestãoEmpresarial.Models;
using GestãoEmpresarial.Views;
using GestãoEmpresarial.Views.Layout;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GestãoEmpresarial
{
    /// <summary>
    /// É uma classe static (estatica) para que apenas exista 1 durante a vida da aplicação
    /// Ou seja, o valor colocado é sempre igual em qualquer lado.
    /// </summary>
    internal class Switcher 
    {
        public static MainWindow pageSwitcher;
        public static LayoutView layoutSwitcher;

        /// <summary>
        /// É um construtor (metodo) estático, para inicializar as variaveis necessárias.
        /// </summary>
        static Switcher()
        {
        }

        public static void Switch(NavigationMenu menu)
        {
            layoutSwitcher.Navigate(menu);
        }

        public static void Switch(UserControl view)
        {
            layoutSwitcher.Navigate(view);
            Focus(view);
        }

        public static void SwitchPagina(UserControl view)
        {
            pageSwitcher.Navigate(view);
            Focus(view);
        }

        // Ficou aceitavel
        public static void Imprimir(UserControl uc)
        {
            PrintDialog printDlg = new PrintDialog();
            if (printDlg.ShowDialog() == true)
            {
                // Medir o UserControl para seu tamanho desejado
                uc.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                uc.Arrange(new Rect(uc.DesiredSize));

                // Criar um UserControl para a impressão (sem usar clone aqui, apenas referência)
                UserControl printableUserControl = uc;

                // Determinar o tamanho da área imprimível
                Size printableSize = new Size(printDlg.PrintableAreaWidth, printDlg.PrintableAreaHeight);
                double scaleX = printableSize.Width / printableUserControl.ActualWidth;
                double scaleY = printableSize.Height / printableUserControl.ActualHeight;
                double scale = Math.Min(scaleX, scaleY);

                // Evitar uma escala muito pequena
                scale = Math.Max(scale, 0.8);  // Ajuste para um valor mínimo de 50% de escala

                printableUserControl.LayoutTransform = new ScaleTransform(scale, scale);

                // Medir e arranjar o UserControl escalado
                printableUserControl.Measure(printableSize);
                printableUserControl.Arrange(new Rect(printableSize));
                printDlg.PrintVisual(printableUserControl, "User Control Printing.");
            }
        }

        private static UserControl CloneUserControl(UserControl original)
        {
            // Create a copy of the UserControl using XamlWriter (for simplicity)
            string xaml = System.Windows.Markup.XamlWriter.Save(original);
            using (var reader = new System.IO.StringReader(xaml))
            {
                using (var xmlReader = System.Xml.XmlReader.Create(reader))
                {
                    return (UserControl)System.Windows.Markup.XamlReader.Load(xmlReader);
                }
            }
        }

        public static void Focus(UIElement element)
        {
            if (!element.Focus())
            {
                element.Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(delegate ()
                {
                    if (element.Focusable == true)
                    {
                        element.Focus();
                        Keyboard.Focus(element);
                    }
                }));
            }
        }
    }
}
