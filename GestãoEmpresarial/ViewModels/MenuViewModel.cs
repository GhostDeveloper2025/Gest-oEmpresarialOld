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
        public ObservableCollection<NavigationMenu> Items { get; set; }

        public string MenuCabecalho { get { return $"Bem Vindo {LoginViewModel.Instancia.colaborador.Nome}! "; } }

        public MenuViewModel()
        {
            Items = new ObservableCollection<NavigationMenu>();

            var cadastros = new NavigationMenu
            {
                Header = "Cadastros",
                Icon = PackIconKind.CreateNewFolderOutline,
                Items = new NavigationMenuCollection
            {
                { "Cliente", PackIconKind.AccountMultiplePlusOutline, DI.GetCadastroView(nameof(CadastroClienteViewModel)) },
                { "Colaborador", PackIconKind.PersonChild, DI.GetCadastroView(nameof(CadastroColaboradorViewModel)) },
                { "Categoria", PackIconKind.PersonAddOutline, DI.GetCadastroView(nameof(CadastroCategoriaViewModel)) },
                { "Produto", PackIconKind.PackageVariant, DI.GetCadastroView(nameof(CadastroProdutoViewModel)) },
                { "OS", PackIconKind.HammerScrewdriver, DI.GetCadastroGlobalView(nameof(CadastroOrdemServicoViewModel)) },
                { "Venda", PackIconKind.CartArrowRight, DI.GetCadastroGlobalView(nameof(CadastroVendaViewModel)) },
            }
            };
           
            var pesquisas = new NavigationMenu
            {
                Header = "Pesquisar",
                Icon = PackIconKind.FileSearch,
                Items = new NavigationMenuCollection
            {
                { "Cliente", PackIconKind.AccountSearch, DI.PesquisaViews[nameof(PesquisaClienteViewModel)] },
                //{ "Colaborador", PackIconKind.AccountSearch, DI.PesquisaViews[nameof(PesquisaColaboradorViewModel)] },
                { "Categoria", PackIconKind.TagSearch, DI.PesquisaViews[nameof(PesquisaCategoriaViewModel)] },
                { "Produto", PackIconKind.Package, DI.PesquisaViews[nameof(PesquisaProdutoViewModel)] },
                { "OS", PackIconKind.HammerWrench, DI.PesquisaViews[nameof(PesquisaOrdemServicoViewModel)] },
                { "Venda", PackIconKind.Cart, DI.PesquisaViews[nameof(PesquisaVendaViewModel)] },
            }
            };
            if (LoginViewModel.Instancia.colaborador.Gerente)
            {
                pesquisas.Items.Add("Colaborador", PackIconKind.AccountSearch, DI.PesquisaViews[nameof(PesquisaColaboradorViewModel)]);
            }

            Items.Add(cadastros);
            Items.Add(pesquisas);

            var menuRelatorio = new NavigationMenu
            {
                Header = "Relatórios",
                Icon = PackIconKind.ReportBox,
                Items = new NavigationMenuCollection
            {
                { "Historico Vendas", PackIconKind.PersonAdd, DI.RelatoriosViews[nameof(RelatorioHistoricoVendasViewModel)] },
                { "Venda de Produtos", PackIconKind.PersonAdd, DI.RelatoriosViews[nameof(RelatorioProdutoMaisVendidoViewModel)] },
            }
            };

            if (LoginViewModel.Instancia.colaborador.Gerente)
            {
                menuRelatorio.Items.Add("Comissão", PackIconKind.PersonAdd, DI.RelatoriosViews[nameof(RelatorioComissaoViewModel)]);
                menuRelatorio.Items.Add("Ordem Serviço", PackIconKind.PersonAdd, DI.RelatoriosViews[nameof(RelatorioHistoricoOsViewModel)]);
            }

            Items.Add(menuRelatorio);
        }
    }
}