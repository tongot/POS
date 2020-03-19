using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace POS
{
   
    public class LogInView:BaseViewModel
    {
        IEmployee db;
        public VmUserLogIn user { get; set; } = new VmUserLogIn();
        public LogInView()
        {
            db = new EmployeeApp();
        }

        public void authenticateUser(VmUserLogIn credentials,string password)
        {
            var userCredential = db.AuthnticateUser(password, user.username);
            if (userCredential.stateError != null)
            {
                user.error = userCredential.stateError;
                return;
            }
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username = userCredential.username;
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.roles = userCredential.roles;
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId = userCredential.EmployeeId;
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_name = userCredential.branch_name;
            IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id = userCredential.branch_id;

            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
            IocContainer.Kenel.Get<AppViewModel>().NotLogged = false;
            IocContainer.Kenel.Get<AppViewModel>().Logged = true;




        }
        public bool authenticateAdmin(string username, string password)
        {
            var userCredential = db.AuthnticateUser(password, username);
            if (userCredential.stateError != null)
            {
                user.error = userCredential.stateError;
                return false;
            }
            if(userCredential.roles.Contains("Admin"))
            {
                return true;
            }
            return false;
        }

    }
    public class VmUserLogIn : BaseViewModel
    {
        public string username { get; set; }
        public int? EmployeeId { get; set; }
        public string error { get; set; }
        public string branch_name { get; set; }
        public int branch_id { get; set; }
        public List<string> roles { get; set; }
    }
}
