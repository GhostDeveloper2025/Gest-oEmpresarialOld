using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Pesquisa;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

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
                        { "Cliente", PackIconKind.PersonAdd , () => { return new CadastroClienteView(new CadastroClienteViewModel(null, new RClienteDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                        { "Colaborador", PackIconKind.PersonChild,  () => { return new CadastroColaboradorView(new CadastroColaboradorViewModel(null, new RColaboradorDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                        { "Categoria", PackIconKind.Tags,  () => { return new CadastroCategoriaView(new CadastroCategoriaViewModel(null, new RCategoriaDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                        { "Produto", PackIconKind.BoxAdd,  () => { return new CadastroProdutoView(new CadastroProdutoViewModel(null, new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario))); }},
                        { "OS", PackIconKind.HammerScrewdriver,  () => { return new CadastroOrdemServicoView(new CadastroOrdemServicoViewModel(null, new ROsDAL(LoginViewModel.colaborador.IdFuncionario))); }},
                        { "Venda", PackIconKind.BoxAdd,  () => { return new CadastroVendaView(new CadastroVendaViewModel (null, new RVendasDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                    }
                },
                new TreeviewMenu
                {
                    Header = "Pesquisar",
                    Icon = PackIconKind.FileSearch,
                    Items = new TreeviewMenuCollection
                    {
                        { "Cliente", PackIconKind.PersonAdd ,  () => { return new PesquisaView(new PesquisaClienteViewModel(new RClienteDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                        { "Colaborador", PackIconKind.PersonChild,  () => { return new PesquisaView( new PesquisaColaboradorViewModel(new RColaboradorDAL(LoginViewModel.colaborador.IdFuncionario)) );} },
                        { "Categoria", PackIconKind.Tags,  () => { return new PesquisaView(new PesquisaCategoriaViewModel(new RCategoriaDAL(LoginViewModel.colaborador.IdFuncionario)) );} },
                        { "Produto", PackIconKind.BoxAdd,  () => { return new PesquisaView(new PesquisaProdutoViewModel(new RProdutoDAL(LoginViewModel.colaborador.IdFuncionario))); } },
                        { "OS", PackIconKind.HammerScrewdriver,  () => 
                            { 
                                    return new PesquisaView(
                                        new PesquisaOrdemServicoViewModel(
                                            new ROsDAL(LoginViewModel.colaborador.IdFuncionario), 
                                            new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario))
                                        ); 
                            } 
                        },
                        { "Venda", PackIconKind.BoxAdd,  () => 
                            {
                                return new PesquisaView(
                                    new PesquisaVendaViewModel(
                                        new RVendasDAL(LoginViewModel.colaborador.IdFuncionario),
                                            new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario))); 
                            } 
                        },
                    }
                }
            };
        }
    }
}