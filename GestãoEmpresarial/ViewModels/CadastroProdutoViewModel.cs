using GestãoEmpresarial.Interface;
using GestãoEmpresarial.Models;
using GestãoEmpresarial.Repositorios;
using GestãoEmpresarial.Validations;
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

        private readonly RCodigosDAL _codigosDal;
        private readonly REstoqueDAL _restoqueDal;
        
        public CadastroProdutoViewModel(int? id, ProdutoValidar validar, IDAL<ProdutoModel> repositorio, 
            RCodigosDAL codigosDAL, RCategoriaDAL categoriaDAL, REstoqueDAL estoqueDAL) 
            : base(id, validar, repositorio)
        {
            _codigosDal = codigosDAL;
            _restoqueDal = estoqueDAL;

            CategoriasLista = categoriaDAL.List(null).ToDictionary(b => b.IdCategoria, a => a.Descricao);
            MarcasList = _codigosDal.GetListaMarcasFerramenta().ToDictionary(b => b.Id, a => a.Nome);
        }
        public override int InserirObjectoBD()
        {
            int idproduto = base.InserirObjectoBD();

            _restoqueDal.Insert(new EstoqueModel
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
            _restoqueDal.Update(new EstoqueModel
            {
                Quantidade = ObjectoEditar.EstoqueQuantidade,
                Localizacao = ObjectoEditar.EstoqueLocalizacao,
                IdEstoque = ObjectoEditar.IdEstoque,
                IdProduto = ObjectoEditar.IdProduto,
            });

        }
    }
}
