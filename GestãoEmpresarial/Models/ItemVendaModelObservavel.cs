using GestãoEmpresarial.Models.Atributos;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class ItemVendaModelObservavel : ObservableObject
    {
        public static ItemVendaModel MapearItemVendaModel(ItemVendaModelObservavel model)
        {
            return new ItemVendaModel()
            {
                CustoTotal = model.CustoTotal,
                Desconto = model.Desconto,
                IdItensVenda = model.IdItensVenda,
                IdVenda = model.IdVenda,
                Produto = model.Produto,
                Quantidade = model.Quantidade,
                ValUnitario = model.ValUnitario
            };
        }

        public static ItemVendaModelObservavel MapearItemVendaModel(ItemVendaModel model)
        {
            return new ItemVendaModelObservavel()
            {
                Desconto = model.Desconto,
                IdItensVenda = model.IdItensVenda,
                IdVenda = model.IdVenda,
                Produto = model.Produto,
                Quantidade = model.Quantidade,
                ValUnitario = model.ValUnitario
            };
        }

        public int IdVenda { get; set; }
        public int IdItensVenda { get; set; }

        [ColumnHeader("Nome Produto")]
        public string NomeProduto
        {
            get { return _produto?.Nome; }
        }

        [ColumnHeader("Código")]
        public string CodigoProduto
        {
            get { return _produto?.CodProduto; }
        }

        private int _quantidade;

        [ColumnHeader("Quantidade")]
        public int Quantidade
        {
            get { return _quantidade; }
            set
            {
                _quantidade = value;
                RaisePropertyChanged(nameof(Quantidade));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        private decimal _valUnitario;

        [ColumnHeader("Valor unitário")]
        public decimal ValUnitario
        {
            get { return _valUnitario; }
            set
            {
                _valUnitario = value;
                RaisePropertyChanged(nameof(ValUnitario));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        public decimal _Desconto;

        [ColumnHeader("Desconto")]
        public decimal Desconto
        {
            get { return _Desconto; }
            set
            {
                _Desconto = value;
                RaisePropertyChanged(nameof(Desconto));
                RaisePropertyChanged(nameof(TotalItem));
            }
        }

        public decimal CustoTotal
        { get { return Quantidade * ValUnitario; } } // => ValTotal

        //public decimal DescontoValor
        //{ get { return Desconto <= 0 ? 0 : CustoTotal * Desconto / 100; } }

        //[ColumnHeader("Total Item")]
        //public decimal TotalItem { get { return CustoTotal - DescontoValor; } }
        //public decimal TotalItem
        //{ get { return Math.Round(CustoTotal - DescontoValor, 2); } }




        // Certifique-se que os cálculos de desconto e total do item usam o mesmo arredondamento:
        public decimal DescontoValor
        {
            get
            {
                return Math.Round((Desconto / 100) * (Quantidade * ValUnitario), 2, MidpointRounding.AwayFromZero);
            }
        }
        [ColumnHeader("Total Item")]
        public decimal TotalItem
        {
            get
            {
                return Math.Round((Quantidade * ValUnitario) - DescontoValor, 2, MidpointRounding.AwayFromZero);
            }
        }




        private ProdutoModel _produto;

        public ProdutoModel Produto
        {
            get { return _produto; }
            set
            {
                _produto = value;
                if (_produto != null)
                    ValUnitario = _produto.ValorVenda;
                RaisePropertyChanged(nameof(Produto));
            }
        }
    }
}