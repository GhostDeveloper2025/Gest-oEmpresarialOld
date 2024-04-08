using FontAwesome.Sharp;
using GestãoEmpresarial.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class MenuViewModel
    {
        public ObservableCollection<TreeviewMenu> Items { get; set; }

        public MenuViewModel()
        {
            Items = new ObservableCollection<TreeviewMenu>()
            {
                new TreeviewMenu
                {
                    Header = "Cadastros",
                    Icon = PackIconKind.PlaylistPlus,
                    Items = new TreeviewMenuCollection
                    {
                        { "Cliente", PackIconKind.PersonAdd , null},
                        { "Colaborador", PackIconKind.PersonChild, null },
                        { "Categoria", PackIconKind.Tags, null },
                        { "Produto", PackIconKind.BoxAdd, null },
                        { "OS", PackIconKind.HammerScrewdriver, null },
                        { "Venda", PackIconKind.BoxAdd, null },
                    }
                },
                new TreeviewMenu
                {
                    Header = "Pesquisar",
                    Icon = PackIconKind.FileSearch,
                    Items = new TreeviewMenuCollection
                    {
                        { "Cliente", PackIconKind.PersonAdd , null},
                        { "Colaborador", PackIconKind.PersonChild, null },
                        { "Categoria", PackIconKind.Tags, null },
                        { "Produto", PackIconKind.BoxAdd, null },
                        { "OS", PackIconKind.HammerScrewdriver, null },
                        { "Venda", PackIconKind.BoxAdd, null },
                    }
                }
            };
        }
    }
}
