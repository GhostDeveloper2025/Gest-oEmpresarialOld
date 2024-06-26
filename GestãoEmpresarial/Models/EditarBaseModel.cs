using FluentValidation;
using MicroMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestãoEmpresarial.Models
{
    public abstract class EditarBaseModel<ObjectoBD> : ObservableObject, IDataErrorInfo
    {
        private readonly AbstractValidator<ObjectoBD> validador;

        public string Error
        {
            get
            {
                if (validador != null)
                {
                    var objBD = DevolveObjectoBD();
                    var results = validador.Validate(objBD);
                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }
                }
                return string.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var objBD = DevolveObjectoBD();
                var result = validador.Validate(objBD);
                var firstOrDefault = result.Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                if (firstOrDefault != null)
                    return firstOrDefault.ErrorMessage;
                else
                    return string.Empty;
            }
        }

        //public int Id { get; set; }
        //public DateTime DataCadastro { get; private set; }

        public EditarBaseModel(ObjectoBD obj, AbstractValidator<ObjectoBD> validador)
        {
            this.validador = validador;
            if (obj != null)
                SetPropriedadesDoObjectoBD(obj);
        }

        protected abstract void SetPropriedadesDoObjectoBD(ObjectoBD obj);
        public abstract ObjectoBD DevolveObjectoBD();
    }
}
