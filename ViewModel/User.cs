using Ninject;
using POS.Ioc;


namespace POS
{
    public static class User
    {
        public static int branch_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_id;
        public static string username = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.username;
        public static int user_id = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.EmployeeId.Value;
        public static string branch_name = IocContainer.Kenel.Get<AppViewModel>().CurrentUser.branch_name;
    }
}
