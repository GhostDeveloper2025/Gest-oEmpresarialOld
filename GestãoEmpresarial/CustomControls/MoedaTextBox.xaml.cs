using GestãoEmpresarial.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GestãoEmpresarial.CustomControls
{
    /// <summary>
    /// Interação lógica para MoedaTextBox.xam
    /// </summary>
    public partial class MoedaTextBox : TextBox
    {
        public MoedaTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty dependencyProperty =
           DependencyProperty.Register("Emoeda", typeof(bool), typeof(NumeroTextBox),
               new PropertyMetadata(false));

        public bool Emoeda
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            textBox.SelectionStart = 0;

            if (Emoeda)
            {
                //Methods.Moeda(ref textBox);

                Methods.Moeda(this);
            }
        }
    }
}
