using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestãoEmpresarial.Themes
{
    public static class ThemesController
    {
        // Esta propriedade é a chave. Ela nos diz qual tema está ativo.
        // Tornar o setter privado para controle interno
        public static ThemeTypes CurrentTheme { get; private set; }

        public enum ThemeTypes
        {
            Light, Dark
        }

        private static void ChangeTheme(Uri themeUri)
        {
            try
            {
                // Cria o novo dicionário de tema
                ResourceDictionary newThemeDictionary = new ResourceDictionary() { Source = themeUri };

                // Encontra o índice do dicionário de tema atual (Light ou Dark)
                int currentThemeIndex = -1;
                for (int i = 0; i < Application.Current.Resources.MergedDictionaries.Count; i++)
                {
                    var dict = Application.Current.Resources.MergedDictionaries[i];
                    // Verifica se a Source não é nula e se o caminho termina com o nome de um dos seus arquivos de tema
                    if (dict.Source != null && (dict.Source.OriginalString.EndsWith("LightTheme.xaml") || dict.Source.OriginalString.EndsWith("DarkTheme.xaml")))
                    {
                        currentThemeIndex = i;
                        break;
                    }
                }

                if (currentThemeIndex != -1)
                {
                    // Remove o dicionário antigo na posição encontrada
                    Application.Current.Resources.MergedDictionaries.RemoveAt(currentThemeIndex);
                    // Insere o novo dicionário na mesma posição para tentar manter a ordem dos recursos
                    Application.Current.Resources.MergedDictionaries.Insert(currentThemeIndex, newThemeDictionary);
                }
                else
                {
                    // Se nenhum tema foi encontrado (pode acontecer na primeira carga se não estiver no App.xaml ou se houver erro)
                    // Adiciona o novo tema. Pode ser necessário ajustar a posição de inserção dependendo de como seus recursos são organizados.
                    Application.Current.Resources.MergedDictionaries.Add(newThemeDictionary);
                    // É recomendável logar esta situação para depuração
                    Console.WriteLine("Aviso: Dicionário de tema personalizado anterior não encontrado. Adicionando novo tema ao final da coleção.");
                }
            }
            catch (Exception ex)
            {
                // Logar ou tratar a exceção adequadamente
                Console.WriteLine($"Erro ao trocar o tema: {ex.Message}");
                // Considere adicionar uma notificação ao usuário ou usar um tema padrão seguro.
            }
        }

        public static void SetTheme(ThemeTypes theme)
        {
            string themeName;
            switch (theme)
            {
                case ThemeTypes.Dark:
                    themeName = "DarkTheme";
                    break;
                case ThemeTypes.Light:
                    themeName = "LightTheme";
                    break;
                default: // Caso padrão ou erro
                    // Define um tema padrão para evitar que a aplicação fique sem tema
                    themeName = "LightTheme";
                    Console.WriteLine("Aviso: Tipo de tema desconhecido. Aplicando LightTheme como padrão.");
                    break;
            }

            CurrentTheme = theme; // Atualiza o estado do tema atual

            try
            {
                // Constrói a Uri relativa à pasta Themes
                // Garanta que a pasta 'Themes' esteja na raiz do seu projeto e marcada como 'Resource' ou 'Content' (dependendo da sua configuração)
                // Se a pasta estiver em outro lugar, ajuste o caminho aqui.
                Uri themeUri = new Uri($"Themes/{themeName}.xaml", UriKind.Relative);
                ChangeTheme(themeUri);
            }
            catch (Exception ex)
            {
                // Logar ou tratar a exceção que pode ocorrer na criação da Uri ou em ChangeTheme
                Console.WriteLine($"Erro ao definir o tema {themeName}: {ex.Message}");
                // Considere notificar o usuário ou reverter para um tema padrão
            }
        }

    }
}
