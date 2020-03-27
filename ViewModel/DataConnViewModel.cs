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
using static AppDatabase.ConnectionStringSetter;

namespace POS
{
    public class DataConnViewModel:BaseViewModel
    {
        ConnectionStringSetter con = new ConnectionStringSetter();
        stringProperties str;

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
             str= new stringProperties(con.getCurrentConnection());
            setCurrentConnValues();

            btnTestConnection = new RelayCommand(testConnection);
            btnSaveConnection = new RelayCommand(saveConnection);
            btnExit = new RelayCommand(backToLogIn);
            testConnection();
            
        }
        void setCurrentConnValues()
        {
            DataSource = str.data_source;
            UserID = str.user_id;
            InitialCatalog = str.catalog;
            Pooling = str.pooling;
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
            string connection = $"User ID={UserID};Password={pass.Password};Pooling={Pooling};Data Source={DataSource};Initial Catalog={InitialCatalog}; Trusted_Connection=True; MultipleActiveResultSets = True";
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
            if (Error != "Connected")
            {
                Error = "Please Enter a valid connection or close the application";
                return;
            }
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.logInPage;
            
        }
    }
}
