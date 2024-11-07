using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

//namespace MicroMvvm
//{
//    public class RelayCommandWithParameter : ICommand
//    {
//        private Action<object> _execute;
//        private Func<object, bool> _canExecute;

//        public RelayCommandWithParameter(Action<object> execute, Func<object, bool> canExecute = null)
//        {
//            _execute = execute;
//            _canExecute = canExecute;
//        }

//        public event EventHandler CanExecuteChanged
//        {
//            add { CommandManager.RequerySuggested += value; }
//            remove { CommandManager.RequerySuggested -= value; }
//        }

//        public bool CanExecute(object parameter)
//        {
//            return _canExecute == null || _canExecute(parameter);
//        }

//        public void Execute(object parameter)
//        {
//            _execute(parameter);
//        }
//    }
//}
//

//Chate GPT quem fez este e o de cima foi o Professor
public class RelayCommandWithParameterAsync : ICommand
{
    private Func<object, Task> _execute;
    private Func<object, bool> _canExecute;

    public RelayCommandWithParameterAsync(Func<object, Task> execute, Func<object, bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    public async void Execute(object parameter)
    {
        await _execute(parameter);
    }
}


