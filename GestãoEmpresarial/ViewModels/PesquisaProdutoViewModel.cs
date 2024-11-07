using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.ViewModels
{
    internal class PesquisaProdutoViewModel : PesquisaViewModel<ProdutoModel, DataGridProdutoModel, RProdutoDAL>
    {
        public PesquisaProdutoViewModel(RProdutoDAL Repositorio, RCodigosDAL repositorioCodigos) : base(Repositorio)
        {
            MarcasList = repositorioCodigos.GetListaMarcasFerramentaAsync().Result.ToDictionary(b => b.Id, a => a.Nome);
        }

        public override int Id => ObjectoSelecionado.IdProduto;
        public override string NomeEditarView => nameof(CadastroProdutoViewModel);

        public Dictionary<int, string> MarcasList { get; internal set; }

        public string PesquisaNome { get; set; }
        public string PesquisaCodigo { get; set; }
        public string PesquisaLocalizacao { get; set; }
        public string PesquisaMarca { get; set; }

        public override DataGridProdutoModel GetDataGridModel(ProdutoModel item)
        {
            return new DataGridProdutoModel(item);
        }

        public override List<ProdutoModel> GetLista()
        {
            var lista = Repositorio.ListAsync(PesquisaNome, PesquisaCodigo, PesquisaLocalizacao, PesquisaMarca).Result;

            PesquisaNome = null;
            RaisePropertyChanged(nameof(PesquisaNome));
            PesquisaCodigo = null;
            RaisePropertyChanged(nameof(PesquisaCodigo));
            PesquisaLocalizacao = null;
            RaisePropertyChanged(nameof(PesquisaLocalizacao));
            PesquisaMarca = null;
            RaisePropertyChanged(nameof(PesquisaMarca));

            return lista;
        }

        public override bool PodeExecutarPesquisar(object parameter)
        {
            return string.IsNullOrWhiteSpace(PesquisaNome) == false
                || string.IsNullOrWhiteSpace(PesquisaCodigo) == false
                || string.IsNullOrWhiteSpace(PesquisaLocalizacao) == false
                || string.IsNullOrWhiteSpace(PesquisaMarca) == false;
        }
    }
}
