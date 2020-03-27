using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Collections.Generic;
using System.Windows.Input;

namespace POS
{
    /// <summary>
    /// main window view model
    /// </summary>
    public class MainViewModel: BaseViewModel
    {
        IRole dbr;IPermissions dbp; IEmployee dbe;
        ConnectionStringSetter con = new ConnectionStringSetter();

        public ICommand  btnSignOut { get; set; }
        public ICommand btnChangePass { get; set; }
        public ICommand  btnOpenConnection { get; set; }


        public MainViewModel()
        {    
            dbr = new RoleApp();
            dbp = new PermissionApp();
            dbe = new EmployeeApp();
            TestConnection();
        
           // btnSignOut = new RelayCommand(SignOut);
            btnChangePass = new RelayCommand(ChangePass);
            btnOpenConnection = new RelayCommand(OpenConnection);

        }
    //    void SignOut()
    //    {
    //        dbe.logOutUser(IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username);

    //        IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username = string.Empty;
    //        IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId = null;
    //        IocContainer.Kenel.Get<AppViewModel>().NotLogged = true;
    //        IocContainer.Kenel.Get<AppViewModel>().Logged = false;
    //        IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.logInPage;
    //        IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id = null;

    //         User.branch_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id;
    //        User.username = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
    //        User.user_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId;
    //        User.branch_name = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_name;

    //}
        void ChangePass()
        {
            
        }
        void TestConnection()
        {
            if (con.Connected() != "Connected")
            {
                IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.connections;
                return;
            }
            setRoleManager();
        }
        void OpenConnection()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.connections;
        }
        /// <summary>
        /// setting the roles of the users when starting app
        ///
        /// </summary>
        void setRoleManager()
        {
            
            listRoleManagerViewModel rm = new listRoleManagerViewModel();
            rm.roles = new List<roleManagerVievModel>();
            var roles = dbr.roles(null);
            if(dbr.roles(null).Count>0)
            {
                foreach (var item in roles)
                {
                    roleManagerVievModel role = new roleManagerVievModel();
                    role.Role = item.Name;
                    IEnumerable<permissionToRoles> pm = dbp.getPermissionsForRole(item.RoleId);
                    if (pm != null)
                    {role.permissions = new List<string>();
                        foreach (var permission in pm)
                        {
                            
                            role.permissions.Add(dbp.GetPermissions(permission.PermissionsId).name);
                        }
                    }
                   
                    rm.roles.Add(role);
                }

            }
            IocContainer.Kenel.Get<AppViewModel>().RolesPermissions = rm;
        }
   
    }
}
