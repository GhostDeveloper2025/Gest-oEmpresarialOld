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

        private static TRepositorio GetRepositorio<TRepositorio>()
        where TRepositorio : class
        {
            return Activator.CreateInstance(typeof(TRepositorio), LoginViewModel.colaborador.IdFuncionario) as TRepositorio;
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
                        { "Cliente", PackIconKind.PersonAdd, DI.GetView(nameof(CadastroClienteViewModel)) },
                        { "Colaborador", PackIconKind.PersonChild, DI.GetView(nameof(CadastroColaboradorViewModel)) },
                        { "Categoria", PackIconKind.Tags, DI.GetView(nameof(CadastroCategoriaViewModel)) },
                        { "Produto", PackIconKind.BoxAdd, DI.GetView(nameof(CadastroProdutoViewModel)) },
                        { "OS", PackIconKind.HammerScrewdriver, DI.GetView(nameof(CadastroOrdemServicoViewModel)) },
                        { "Venda", PackIconKind.BoxAdd, DI.GetView(nameof(CadastroVendaViewModel)) },
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
                        { "Comissão", PackIconKind.PersonAdd ,  () => null },
                        { "Venda de Produtos", PackIconKind.PersonAdd ,  () => null },
                        { "Ordem Serviço", PackIconKind.PersonAdd ,  () => null },
                    }
                }
            };
        }
    }
}