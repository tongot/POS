

using System;
using System.Windows.Input;

namespace POS
{
    public class DialogViewModel:IDialogRequestClose
    {
        public string Message { get; } 
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public DialogViewModel(string Message)
        {
            this.Message = Message;
            OkCommand = new RelayCommand(ok);
            CancelCommand = new RelayCommand(cancel);
        }
        public event EventHandler<DialogCloseRequestEventArgs> CloseRequest;
        void ok()
        {
            CloseRequest?.Invoke(this, new DialogCloseRequestEventArgs(true));
        }
        void cancel()
        {
            CloseRequest?.Invoke(this, new DialogCloseRequestEventArgs(false));
        }
    }
}
