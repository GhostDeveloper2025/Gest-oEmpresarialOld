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
        // Propriedades para obter listas de marcas e categorias
        public Dictionary<int, string> MarcasList { get; internal set; }
        public Dictionary<int, string> CategoriasLista { get; internal set; }

        private readonly RCodigosDAL _codigosDal;
        private readonly REstoqueDAL _restoqueDal;

        // Campo privado que armazena o estado de foco do TextBox.
        // Inicializa como true para que o TextBox receba foco assim que a View for carregada.
        private bool _isTextBoxFocused = true;

        // Propriedade pública que permite o binding com a View (XAML). Neste Caso Para adicionar um foco no TextBox
        // Essa propriedade é observada pela View, e qualquer mudança nela reflete no controle associado.
        public bool IsTextBoxFocused
        {
            get => _isTextBoxFocused;
            set
            {
                _isTextBoxFocused = value;
                RaisePropertyChanged(nameof(IsTextBoxFocused));
            }
        }

        public CadastroProdutoViewModel(int? id, ProdutoValidar validar, IDAL<ProdutoModel> repositorio,
            RCodigosDAL codigosDAL, RCategoriaDAL categoriaDAL, REstoqueDAL estoqueDAL)
            : base(id, validar, repositorio)
        {
            _codigosDal = codigosDAL;
            _restoqueDal = estoqueDAL;

            MarcasList = _codigosDal.GetListaMarcasFerramentaAsync().Result.ToDictionary(b => b.Id, a => a.Nome);
            // Carregar listas de categorias e marcas
            CategoriasLista = categoriaDAL.List(null).ToDictionary(b => b.IdCategoria, a => a.Nome);
        }

        public override async Task<int> InserirObjectoBDAsync()
        {
            // Inserir o produto e obter o ID gerado
            int idproduto = await base.InserirObjectoBDAsync();

            // Inserir o estoque correspondente ao produto
            await _restoqueDal.InsertAsync(new EstoqueModel
            {
                IdProduto = idproduto,
                Localizacao = ObjectoEditar.EstoqueLocalizacao,
                Quantidade = ObjectoEditar.EstoqueQuantidade,
            });

            return idproduto;
        }

        public override async Task AtualizarObjectoBDAsync()
        {
            // Atualizar o produto
            await base.AtualizarObjectoBDAsync();

            // Atualizar o estoque correspondente ao produto
            await _restoqueDal.UpdateAsync(new EstoqueModel
            {
                Quantidade = ObjectoEditar.EstoqueQuantidade,
                Localizacao = ObjectoEditar.EstoqueLocalizacao,
                IdEstoque = ObjectoEditar.IdEstoque,
                IdProduto = ObjectoEditar.IdProduto,
            });
        }
    }
}

