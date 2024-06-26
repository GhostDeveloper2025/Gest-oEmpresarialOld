using GestãoEmpresarial.Validations;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestãoEmpresarial.Models
{
    /// <summary>
    /// Este objecto é o que faz bind na View.
    /// </summary>
    public class EditarVendaModel : EditarBaseModel<VendaModel>
    {
        public EditarVendaModel(VendaValidar validar) : this(null, validar)
        {
            DataVenda = DateTime.Now;
            //DataFinalizacao = DateTime.Now;
        }

        public EditarVendaModel(VendaModel vendaModel, VendaValidar validar) : base(vendaModel, validar)
        {
            ItemVendaAdicionar = new ItemVendaModel();
            if (ListItensVenda == null)
                ListItensVenda = new ObservableCollection<ItemVendaModel>();

            NumberOfRecords = ListItensVenda.Count;
            // Registre o evento CollectionChanged para atualizar o NumberOfRecords quando a coleção mudar
            ListItensVenda.CollectionChanged += (sender, e) => { NumberOfRecords = ListItensVenda.Count; };
        }

        public int IdVenda { get; set; }
        public int Situacao { get; set; }
        public int IdCodigoTipoPagamento { get; set; }
        public DateTime DataVenda { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        public ClienteModel Cliente { get; set; }

        public ItemVendaModel ItemVendaAdicionar { get; set; }

        public ObservableCollection<ItemVendaModel> ListItensVenda { get; set; }
        public void AdicionarNaLista()
        {
            ListItensVenda.Add(ItemVendaAdicionar);
            ItemVendaAdicionar = new ItemVendaModel();
            RaisePropertyChanged(nameof(ItemVendaAdicionar));
        }

        // Variável privada para armazenar o número de registros
        private int _numberOfRecords;

        public int NumberOfRecords
        {
            get { return _numberOfRecords; } // Obtém o número de registros
            set
            {
                _numberOfRecords = value; // Define o valor da variável privada
                RaisePropertyChanged(nameof(NumberOfRecords)); // Notifica a View sobre a mudança na propriedade NumberOfRecords
            }
        }

        public decimal _ValorFrete;

        public decimal ValorFrete
        {
            get { return _ValorFrete; }
            set
            {
                _ValorFrete = value;
                RaisePropertyChanged(nameof(ValorFrete));
                //RaisePropertyChanged(nameof(TotalVenda));
            }
        }

        //total valor sem desconto
        public decimal SubTotalProduto { get { return ListItensVenda.Sum(x => x.CustoTotal); } }

        //public decimal TotalVenda { get { return TotalProduto + ValorFrete; } }

        //public decimal TotalDescontoProduto { get { return Math.Round(ListItensVenda.Sum(x => x.DescontoValor), 2); } }

        //public decimal TotalProduto { get { return ListItensVenda.Sum(x => x.TotalItem); } } //total valor com desconto

        public override VendaModel DevolveObjectoBD()
        {
            return new VendaModel()
            {
                DataFinalizacao = DataFinalizacao,
                IdVenda = IdVenda,
                Situacao = Situacao,
                Cliente = Cliente,
                ValorFrete = ValorFrete,
                IdCodigoTipoPagamento = IdCodigoTipoPagamento,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(VendaModel obj)
        {
            ListItensVenda = new ObservableCollection<ItemVendaModel>(obj.ListItensVenda);
            IdVenda = obj.IdVenda;
            Situacao = obj.Situacao;
            DataFinalizacao = obj.DataFinalizacao;
            Cliente = obj.Cliente;
            ValorFrete = obj.ValorFrete;
            IdCodigoTipoPagamento = obj.IdCodigoTipoPagamento;
        }
    }
}