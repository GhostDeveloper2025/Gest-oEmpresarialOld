using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Pesquisa;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestãoEmpresarial.ViewModels
{
    public class MenuViewModel
    {
        public ObservableCollection<TreeviewMenu> Items { get; set; }

        private CadastroView GetCadastroView<TViewModel, TRepositorio, TValidar, TView>()
            where TViewModel : ICadastroViewModel
            where TView : UIElement
        {
            var repositorio = Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.colaborador.IdFuncionario);
            var validar = Activator.CreateInstance<TValidar>();
            var view = Activator.CreateInstance<TView>();
            var viewModel = Activator.CreateInstance(typeof(TViewModel), null, validar, repositorio) as ICadastroViewModel;
            return new CadastroView(viewModel, view);
        }

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
                        { "Cliente", PackIconKind.PersonAdd , () => GetCadastroView<CadastroClienteViewModel, RClienteDAL, ClienteValidar, CadastroClienteView>() },
                        { "Colaborador", PackIconKind.PersonChild,  () => GetCadastroView<CadastroColaboradorViewModel, RColaboradorDAL, ColaboradorValidar, CadastroColaboradorView>() },
                        { "Categoria", PackIconKind.Tags,  () => GetCadastroView<CadastroCategoriaViewModel, RCategoriaDAL, CategoriaValidar, CadastroCategoriaView>() },
                        { "Produto", PackIconKind.BoxAdd,  () => GetCadastroView<CadastroProdutoViewModel, RProdutoDAL, ProdutoValidar, CadastroProdutoView >()},
                        { "OS", PackIconKind.HammerScrewdriver,  () => GetCadastroView<CadastroOrdemServicoViewModel, ROsDAL, OrdemServicoValidar, CadastroOrdemServicoView >()},
                        { "Venda", PackIconKind.BoxAdd,  () => GetCadastroView <CadastroVendaViewModel, RVendasDAL, VendaValidar, CadastroVendaView >() },
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