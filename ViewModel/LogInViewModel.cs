using Ninject;
using POS.Ioc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS
{
    /// <summary>
    /// log in view model
    /// controls the log in processes
    /// </summary>
    public class LogInViewModel : BaseViewModel
    {
        public ICommand log { get; set; }
        public LogInView user { get; set; } = new LogInView();
        public string password { get; set; }
        public LogInViewModel()
        {
            log = new RelayCommand(login);
        }
        /// <summary>
        /// log in to the system
        /// password comes from the log in form itself as an obj
        /// </summary>
        private void login(object obj)
        {
            user.user.error = "";
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;
            user.authenticateUser(user.user, password);

        }
      
    }
}
