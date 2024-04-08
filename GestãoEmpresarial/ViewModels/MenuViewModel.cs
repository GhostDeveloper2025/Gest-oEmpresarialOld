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
                    Items = new ObservableCollection<TreeviewMenuItem>
                    {
                        new TreeviewMenuItem
                        {
                            Header = "Cliente",
                            Icon = PackIconKind.PersonAdd
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Colaborador",
                            Icon = PackIconKind.PersonChild
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Categoria",
                            Icon = PackIconKind.Tags
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Produto",
                             Icon = PackIconKind.BoxAdd
                        },
                        new TreeviewMenuItem
                        {
                            Header = "OS",
                            Icon= PackIconKind.HammerScrewdriver,
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Venda",
                            Icon = PackIconKind.BoxAdd,
                        },
                    }
                },
                new TreeviewMenu
                {
                    Header = "Pesquisar",
                    Icon = PackIconKind.FileSearch,
                    Items = new ObservableCollection<TreeviewMenuItem>
                    {
                        new TreeviewMenuItem
                        {
                            Header = "Cliente",
                             Icon = PackIconKind.PersonAdd
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Colaborador",
                            Icon = PackIconKind.PersonChild
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Categoria",
                            Icon = PackIconKind.Tags
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Produto",
                            Icon = PackIconKind.BoxAdd
                        },
                        new TreeviewMenuItem
                        {
                            Header = "OS",
                            Icon= PackIconKind.HammerScrewdriver,
                        },
                        new TreeviewMenuItem
                        {
                            Header = "Venda",
                            Icon = PackIconKind.BoxAdd,
                        },
                    }
                }
            };
        }
    }
}
