using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GroceryList.ViewModel.Commands
{
    class RelayCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if(_canExecute == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
