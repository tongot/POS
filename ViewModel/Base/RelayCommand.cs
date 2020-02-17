
using System;
using System.Windows.Input;

namespace POS
{
    public class RelayCommand : ICommand
    {
        private Action<object> _Action;
        private Action _action;
        private ICommand btnBackHome;

        public RelayCommand( Action<object> action)
        {
            _Action = action;
        }
        public RelayCommand(Action action)
        {
            _action = action;
        }

        public RelayCommand(ICommand btnBackHome)
        {
            this.btnBackHome = btnBackHome;
        }

        public event EventHandler CanExecuteChanged = (sender, e) =>{};

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //Loading.Load();
            if (parameter != null)
            {
                _Action(parameter);

            }
            else
            {
                _action();
            }
           // Loading.Load();
        }
    }
}
