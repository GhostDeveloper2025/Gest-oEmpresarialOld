using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    public class RelatorioReciboVendaViewModel : ObservableObject
    {
        public RelatorioReciboVendaViewModel(RRelatorioReciboDAL relatorioReciboDAL, RClienteDAL rClienteDAL, int id)
        {
            Relatorio = relatorioReciboDAL.ObterReciboVenda(id);
            Cliente = rClienteDAL.GetById(Relatorio.VendaModel.IdCliente);
        }

        public ClienteModel Cliente { get; set; }

        public RelatorioReciboVendaModel Relatorio { get; set; }

        public ObservableCollection<ItemVendaModelObservavel> ListItemsObservaveis
        {
            get
            {
                var listDeObservaveis = Relatorio.VendaModel.ListItensVenda.Select(a => ItemVendaModelObservavel.MapearItemVendaModel(a));
                return new ObservableCollection<ItemVendaModelObservavel>(listDeObservaveis);
            }
        }

        private decimal TotalProduto { get { return ListItemsObservaveis.Sum(x => x.TotalItem); } } //total valor com desconto

        public decimal SubTotalProduto { get { return Relatorio.VendaModel.ListItensVenda.Sum(x => x.CustoTotal); } }

        public decimal TotalDescontoProduto { get { return Math.Round(Relatorio.VendaModel.ListItensVenda.Sum(x => x.Desconto), 2); } }

        public decimal TotalVenda { get { return TotalProduto + Relatorio.VendaModel.ValorFrete; } }
        public string TotalProdutos { get { return "Total Produto: " + TotalProduto.ToString("C"); } }
        public string SubTotalProdutos { get { return "Subtotal  Produto: " + SubTotalProduto.ToString("C"); } }
        public string TotalVendas { get { return "Total TotalVenda: " + TotalVenda.ToString("C"); } }
        public string TotalDescontoProdutos { get { return "Total Desconto Produto: " + (TotalDescontoProduto / 100).ToString("P2"); } }

        public string ValorFretes
        {
            get { return "Frete: " + Relatorio.VendaModel.ValorFrete.ToString("C"); }
        }
        public string NomeSituacao
        {
            get
            {
                if (Relatorio.VendaModel.Situacao)
                    return "FINALIZADO";
                else
                    return "VENDA SALVA";
            }
        }
    }
}
