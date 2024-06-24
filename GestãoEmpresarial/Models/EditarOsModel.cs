using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GestãoEmpresarial.Models
{
    public class EditarOsModel : EditarBaseModel<OrdemServicoModel>
    {
        public EditarOsModel() : this(null)
        {
            DataEntrada = DateTime.Now;
        }

        public EditarOsModel(OrdemServicoModel osModel) : base(osModel)
        {
            ItemOsAdicionar = new ItemOrdemServicoModel();

            if (ListItensOs == null)
                ListItensOs = new ObservableCollection<ItemOrdemServicoModel>();

            NumberOfRecords = ListItensOs.Count;
            // Registre o evento CollectionChanged para atualizar o NumberOfRecords quando a coleção mudar
            ListItensOs.CollectionChanged += (sender, e) => { NumberOfRecords = ListItensOs.Count; };
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

        public decimal _TotalMaoObra;

        public decimal TotalMaoObra
        {
            get { return _TotalMaoObra; }
            set
            {
                _TotalMaoObra = value;
                RaisePropertyChanged(nameof(TotalMaoObra));
                //RaisePropertyChanged(nameof(TotalOS));
            }
        }

        public decimal SubTotalProduto //total valor sem desconto
        {
            get
            {
                return ListItensOs.Sum(x => x.CustoTotal); //Func<ItensOsModel (parametro entrada), decimal (valor de saida)>
                //return ListItensOs.Sum(SomaDoTotalDosProdutos);
                //return ListItensOs.Sum(a => SomaDoTotalDosProdutos(a));
            }
        }

        //public decimal TotalOS { get { return TotalProduto + TotalMaoObra; } }

        //public decimal TotalDescontoProduto { get { return Math.Round(ListItensOs.Sum(x => x.DescontoValor), 2); } }

        //public decimal TotalProduto { get { return ListItensOs.Sum(x => x.TotalItem); } } //total valor com desconto

        public ItemOrdemServicoModel ItemOsAdicionar { get; set; }
        public ObservableCollection<ItemOrdemServicoModel> ListItensOs { get; set; }

        public void AdicionarNaLista()
        {
            ListItensOs.Add(ItemOsAdicionar);
            ItemOsAdicionar = new ItemOrdemServicoModel();
            RaisePropertyChanged(nameof(ItemOsAdicionar));
        }

        public override OrdemServicoModel DevolveObjectoBD()
        {
            return new OrdemServicoModel
            {
                //TotalOS = TotalOS,
                TotalMaoObra = TotalMaoObra,
                //TotalDescontoProduto = TotalDescontoProduto,
                //TotalProduto = TotalProduto,
                SubTotalProduto = SubTotalProduto,
                IdOs = IdOs,
                Status = Status,
                Finalizado = Finalizado,
                Box = Box,
                Marca = Marca,
                Modelo = Modelo,
                Ferramenta = Ferramenta,
                DataEntrada = DataEntrada,
                DataFinalizacao = DataFinalizacao,
                Responsavel = Responsavel,
                Tecnico = Tecnico,
                Obs = Obs,
                Garantia = Garantia,
                Cliente = Cliente,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(OrdemServicoModel obj)
        {
            ListItensOs = new ObservableCollection<ItemOrdemServicoModel>(obj.ListItensOs);
            TotalMaoObra = obj.TotalMaoObra;
            Status = obj.Status;
            IdOs = obj.IdOs;
            Finalizado = obj.Finalizado;
            Box = obj.Box;
            Marca = obj.Marca;
            Modelo = obj.Modelo;
            Ferramenta = obj.Ferramenta;
            DataEntrada = obj.DataEntrada;
            DataFinalizacao = obj.DataFinalizacao;
            Responsavel = obj.Responsavel;
            Tecnico = obj.Tecnico;
            Obs = obj.Obs;
            Garantia = obj.Garantia;
            Cliente = obj.Cliente;
        }

        public ColaboradorModel Responsavel { get; set; }
        public ColaboradorModel Tecnico { get; set; }
        public ClienteModel Cliente { get; set; }
        public int IdOs { get; set; }
        public bool Finalizado { get; set; }
        public int Status { get; set; }
        public string Box { get; set; }
        public int Marca { get; set; }
        public string Modelo { get; set; }
        public string Ferramenta { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string Obs { get; set; }
        public bool Garantia { get; set; }
    }
}