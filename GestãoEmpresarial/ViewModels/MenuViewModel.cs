using FontAwesome.Sharp;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Views;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Cache;
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
                        { "Cliente", PackIconKind.PersonAdd , typeof(CadastroClienteView), new[] { new CadastroClienteViewModel(null) } },
                        { "Colaborador", PackIconKind.PersonChild, typeof(CadastroColaboradorView), new[] { new CadastroColaboradorViewModel(null) } },
                        { "Categoria", PackIconKind.Tags, typeof(CadastroCategoriaView), new[] { new CadastroCategoriaViewModel(null) } },
                        { "Produto", PackIconKind.BoxAdd, typeof(CadastroProdutoView) , new[] { new CadastroProdutoViewModel(null) }},
                        { "OS", PackIconKind.HammerScrewdriver, typeof(CadastroOrdemServicoView) , new[] { new CadastroOrdemServicoViewModel(null) }},
                        { "Venda", PackIconKind.BoxAdd, typeof(CadastroVendaView), new[] { new CadastroVendaViewModel (null) } },
                    }
                },
                new TreeviewMenu
                {
                    Header = "Pesquisar",
                    Icon = PackIconKind.FileSearch,
                    Items = new TreeviewMenuCollection
                    {
                        { "Cliente", PackIconKind.PersonAdd , typeof(PesquisaView), new[] { new PesquisaClienteViewModel(new RClienteDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                        { "Colaborador", PackIconKind.PersonChild, typeof(PesquisaView), new[] { new PesquisaColaboradorViewModel(new RColaboradorDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                        { "Categoria", PackIconKind.Tags, typeof(PesquisaView), new[] { new PesquisaCategoriaViewModel(new RCategoriaDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                        { "Produto", PackIconKind.BoxAdd, typeof(PesquisaView) , new[] { new PesquisaProdutoViewModel(new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                        { "OS", PackIconKind.HammerScrewdriver, typeof(PesquisaView), new[] { new PesquisaOrdemServicoViewModel(new ROsDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                        { "Venda", PackIconKind.BoxAdd, typeof(PesquisaView), new[] { new PesquisaVendaViewModel(new RVendasDAL(LoginViewModel.colaborador.IdFuncionario)) } },
                    }
                }
            };
        }
    }
}
