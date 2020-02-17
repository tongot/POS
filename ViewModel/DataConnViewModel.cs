using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POS
{
    public class DataConnViewModel:BaseViewModel
    {
        ConnectionStringSetter con = new ConnectionStringSetter();
        public string DataSource { get; set; }
        public string Password { get; set; }
        public string UserID { get; set; }
        public string InitialCatalog  { get; set; }
        public string Error { get; set; }

        public string Pooling { get; set; }
        public List<string> poolops { get; set; } = new List<string> { "false", "true" };

        public ICommand  btnTestConnection { get; set; }
        public ICommand btnSaveConnection { get; set; }
        public ICommand btnExit { get; set; }
        public DataConnViewModel()
        {
            btnTestConnection = new RelayCommand(testConnection);
            btnSaveConnection = new RelayCommand(saveConnection);
            btnExit = new RelayCommand(backToLogIn);
        }

        void testConnection()
        {
            Error = con.Connected();
        }
        void saveConnection(object obj)
        {
            var pass = obj as PasswordBox;
            if(string.IsNullOrWhiteSpace(DataSource)| string.IsNullOrWhiteSpace(pass.Password)|
                string.IsNullOrWhiteSpace(UserID)|string.IsNullOrWhiteSpace(InitialCatalog))
            {
                Error = "All fields are required";
                return;
            }
            string connection = $"User ID={UserID};Password={pass.Password};Pooling={Pooling};Data Source={DataSource};Initial Catalog={InitialCatalog}";
            Error =con.saveConnectionString(connection);
            if(Error=="Connected")
            {
                Error = "Connected, Restarting";
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
        void backToLogIn()
        {
            if(Error!="Connected")
            {
                Error = "Please Provide a valid connection";
                return;
            }
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.logInPage;
            
        }
    }
}
