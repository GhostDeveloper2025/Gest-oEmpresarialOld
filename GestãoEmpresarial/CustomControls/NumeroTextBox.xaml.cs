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
    /// Interação lógica para NumeroTextBox.xam
    /// </summary>
    public partial class NumeroTextBox : TextBox
    {
        public NumeroTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty dependencyProperty =
            DependencyProperty.Register("EModea", typeof(bool), typeof(NumeroTextBox),
                new PropertyMetadata(false));

        public bool EModea
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }
        // Verifica se o conteúdo da caixa de texto é vazio ou nulo e, se for, define-o como "0".
        // Em seguida, move o cursor para o final do texto na caixa de texto.
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "0";
            }
            // Define a posição do cursor para o final do texto
            textBox.SelectionStart = textBox.Text.Length;
        }
    }
}
