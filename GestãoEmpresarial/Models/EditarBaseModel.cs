using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public abstract class EditarBaseModel<ObjectoBD> : ObservableObject
    {
        //public int Id { get; set; }
        //public DateTime DataCadastro { get; private set; }

        public EditarBaseModel(ObjectoBD obj)
        {
            if (obj != null)
                SetPropriedadesDoObjectoBD(obj);
        }

        protected abstract void SetPropriedadesDoObjectoBD(ObjectoBD obj);
        public abstract ObjectoBD DevolveObjectoBD();
    }
}
