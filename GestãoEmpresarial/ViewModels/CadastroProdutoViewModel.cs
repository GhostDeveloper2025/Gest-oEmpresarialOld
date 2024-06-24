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
    internal class CadastroProdutoViewModel : CadastroViewModel<ProdutoModel, EditarProdutoModel>
    {
        // Propriedades para obter listas de marcas e status da classe ValoresEstaticos,
        // para facilitar a ligação a ComboBoxes.
        public Dictionary<int, string> MarcasList { get; internal set; } // null
        public Dictionary<int, string> CategoriasLista { get; internal set; }

        private readonly RCodigosDAL codigosDal;
        private readonly REstoqueDAL restoqueDal;
        //
        public CadastroProdutoViewModel(int? id, IDAL<ProdutoModel> Repositorio) : base(id, Repositorio)
        {
            codigosDal = new RCodigosDAL(LoginViewModel.colaborador.IdFuncionario);
            var categoriaDal = new RCategoriaDAL(LoginViewModel.colaborador.IdFuncionario);
            restoqueDal = new REstoqueDAL(LoginViewModel.colaborador.IdFuncionario);

            CategoriasLista = categoriaDal.List(null).ToDictionary(b => b.IdCategoria, a => a.Descricao);
            MarcasList = codigosDal.GetListaMarcasFerramenta().ToDictionary(b => b.Id, a => a.Nome);
        }
        public override int InserirObjectoBD()
        {
            int idproduto = base.InserirObjectoBD();

            restoqueDal.Insert(new EstoqueModel
            {
                IdProduto = idproduto,
                Localizacao = ObjectoEditar.EstoqueLocalizacao,
                Quantidade = ObjectoEditar.EstoqueQuantidade,
            });
            return 0;
        }

        public override void AtualizarObjectoBD()
        {
            base.AtualizarObjectoBD();
            restoqueDal.Update(new EstoqueModel
            {
                Quantidade = ObjectoEditar.EstoqueQuantidade,
                Localizacao = ObjectoEditar.EstoqueLocalizacao,
                IdEstoque = ObjectoEditar.IdEstoque,
                IdProduto = ObjectoEditar.IdProduto,
            });

        }
    }
}
