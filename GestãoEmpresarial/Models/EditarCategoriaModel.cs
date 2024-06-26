using GestãoEmpresarial.Validations;

namespace GestãoEmpresarial.Models
{
    public class EditarCategoriaModel : EditarBaseModel<CategoriaModel>
    {
        public EditarCategoriaModel(CategoriaValidar validar) : this(null, validar)
        {
        }

        public EditarCategoriaModel(CategoriaModel obj, CategoriaValidar validar) : base(obj, validar)
        {
        }

        public override CategoriaModel DevolveObjectoBD()
        {
            return new CategoriaModel()
            {
                IdCategoria = IdCategoria,
                Nome = Nome,
                Descricao = Descricao,
            };
        }

        protected override void SetPropriedadesDoObjectoBD(CategoriaModel obj)
        {
            IdCategoria = obj.IdCategoria;
            Nome = obj.Nome;
            Descricao = obj.Descricao;
        }

        public int IdCategoria { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    }
}