using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
using GestãoEmpresarial.Views.Cadastro;
using GestãoEmpresarial.Views.Pesquisa;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace GestãoEmpresarial.ViewModels
{
    public class MenuViewModel
    {
        public ObservableCollection<TreeviewMenu> Items { get; set; }

        private TRepositorio GetRepositorio<TRepositorio>()
            where TRepositorio : class
        {
            return Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.colaborador.IdFuncionario) as TRepositorio;
        }

        private CadastroView GetCadastroView<TViewModel, TRepositorio, TValidar, TView>(params object[] args)
            where TRepositorio : class
            where TViewModel : ICadastroViewModel
            where TView : UIElement
        {
            var repositorio = GetRepositorio<TRepositorio>();
            var validar = Activator.CreateInstance<TValidar>();
            var view = Activator.CreateInstance<TView>();
            List<object> listArgs = new List<object>()
            {
                null, validar, repositorio
            };
            foreach (var item in args)
            {
                listArgs.Add(item);
            }
            var viewModel = Activator.CreateInstance(typeof(TViewModel), listArgs.ToArray()) as ICadastroViewModel;
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
                        { "Produto", PackIconKind.BoxAdd,  () => GetCadastroView<CadastroProdutoViewModel, RProdutoDAL, ProdutoValidar, CadastroProdutoView>
                            (
                                GetRepositorio<RCodigosDAL>(),
                                GetRepositorio<RCategoriaDAL>(),
                                GetRepositorio<REstoqueDAL>()
                            )
                        },
                        { "OS", PackIconKind.HammerScrewdriver,  () => GetCadastroView<CadastroOrdemServicoViewModel, ROsDAL, OrdemServicoValidar, CadastroOrdemServicoView>
                            (
                                new ItemOrdemServicoValidar(),
                                GetRepositorio<RCodigosDAL>(),
                                GetRepositorio<RItensOSDAL>()
                            )
                        },
                        { "Venda", PackIconKind.BoxAdd,  () => GetCadastroView <CadastroVendaViewModel, RVendasDAL, VendaValidar, CadastroVendaView >(GetRepositorio<RCodigosDAL>(), GetRepositorio<RItensVendaDAL>(), new ItemVendaValidar()) },
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
                                            GetRepositorio<ROsDAL>(),
                                            GetRepositorio<RCodigosDAL>()
                                            )
                                        );
                            }
                        },
                        { "Venda", PackIconKind.BoxAdd,  () =>
                            {
                                return new PesquisaView(
                                    new PesquisaVendaViewModel(
                                        GetRepositorio<RVendasDAL>(),
                                        GetRepositorio<RCodigosDAL>()
                                        )
                                    );
                            }
                        },
                    }
                },
                new TreeviewMenu
                {
                    Header = "Relatórios",
                    Icon = PackIconKind.ReportBox,
                    Items = new TreeviewMenuCollection
                    {
                        { "Cliente", PackIconKind.PersonAdd ,  () => null },
                        { "Cliente", PackIconKind.PersonAdd ,  () => null },
                        { "Cliente", PackIconKind.PersonAdd ,  () => null },
                        { "Cliente", PackIconKind.PersonAdd ,  () => null },
                    }
                }
            };
        }
    }
}