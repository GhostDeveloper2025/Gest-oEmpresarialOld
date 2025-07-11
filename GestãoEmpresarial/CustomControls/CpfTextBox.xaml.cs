﻿using GestãoEmpresarial.Utils;
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
    /// Interação lógica para CpfTextBox.xam
    /// </summary>
    public partial class CpfTextBox : TextBox
    {
        public CpfTextBox()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (e.Key == Key.Back && textBox.CaretIndex > 0)
            {
                int caretIndex = textBox.CaretIndex;

                // Se o cursor estiver antes de um ponto ou traço, move o cursor para a posição anterior
                if (caretIndex == 12 || caretIndex == 8 || caretIndex == 4)
                {
                    caretIndex--;
                }

                if (caretIndex > 0)
                {
                    // Remove o caractere antes do cursor
                    textBox.Text = textBox.Text.Remove(caretIndex - 1, 1);

                    // Atualiza a posição do cursor
                    textBox.CaretIndex = caretIndex;

                    // Marca o evento como manipulado para evitar que outros manipuladores de eventos do teclado sejam chamados
                    e.Handled = true;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            Methods.FormatCPF(ref textBox);
        }
    }
}
