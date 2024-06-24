using GestãoEmpresarial.Utils;
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

namespace GestãoEmpresarial.CustomControls
{
    /// <summary>
    /// Interação lógica para CnpjTextBox.xam
    /// </summary>
    public partial class CnpjTextBox : TextBox
    {
        public CnpjTextBox()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.Key == Key.Back && textBox.CaretIndex > 0)
            {
                int caretIndex = textBox.CaretIndex;

                // Se o cursor estiver antes de um ponto, barra ou traço, move o cursor para a posição anterior
                if (caretIndex == 17 || caretIndex == 13 || caretIndex == 10 || caretIndex == 6 || caretIndex == 2)
                {
                    caretIndex--;
                }

                if (caretIndex > 0)
                {
                    // Se o caractere antes do cursor for uma barra, move o cursor uma posição adiante
                    if (textBox.Text[caretIndex - 1] == '/')
                    {
                        caretIndex--;
                    }

                    // Remove o caractere antes do cursor
                    int removeIndex = caretIndex - 2;
                    if (removeIndex >= 0 && removeIndex < textBox.Text.Length)
                    {
                        textBox.Text = textBox.Text.Remove(removeIndex, 2);
                        // Atualiza a posição do cursor
                        textBox.CaretIndex = caretIndex;
                        // Marca o evento como manipulado para evitar que outros manipuladores de eventos do teclado sejam chamados
                        e.Handled = true;
                    }
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Methods.FormatCNPJ(ref textBox);
        }
    }
}
