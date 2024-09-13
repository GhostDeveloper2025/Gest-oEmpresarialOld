using GestãoEmpresarial.Models.Atributos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public class CategoriaModel
    {
        //[DisplayName("Nome")]
        //[Display(Name = "string1", ResourceType = typeof(Resource1))]
        //[Display(Description = "Id")]
        public int IdCategoria { get; set; }

        [DisplayName("Nome Categoria")]
        public string Nome { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }
    }
}
