

using System;
using System.Collections.Generic;
using System.Windows;

namespace POS
{
    public interface IDialog
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        Window Owner { get; set; }
        void Close();
        bool? ShowDialog();
    }
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                          where TView : IDialog;
        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
    }
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestEventArgs> CloseRequest;
    }
    public class DialogCloseRequestEventArgs : EventArgs
    {
        public DialogCloseRequestEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }
        public bool? DialogResult { get; }
    }
    public class DialogService : IDialogService
    {

        private readonly Window owner;
        public DialogService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }
        public IDictionary<Type, Type> Mappings { get; }
        public void Register<TViewModel, TView>()
            where TViewModel : IDialogRequestClose
            where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new AggregateException($"Type {typeof(TViewModel)} is aready mapped to {typeof(TView)}");
            }
            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);
            EventHandler<DialogCloseRequestEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.CloseRequest -= handler;
                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            };
            viewModel.CloseRequest += handler;
            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();
        }
    }
}
