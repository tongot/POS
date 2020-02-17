

namespace POS
{
    /// <summary>
    /// view model for the application that can be accessed globally
    /// </summary>
    public class AppViewModel:BaseViewModel
    {
        /// <summary>
        /// page navigater that can be changed to any page
        /// </summary>
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.logInPage;
        public VmUserLogIn CurrentUser { get; set; } = new VmUserLogIn();
        public bool NotLogged { get; set; } = true;
        public bool Logged { get; set; } = false;
        public listRoleManagerViewModel RolesPermissions { get; set; } = new listRoleManagerViewModel();


    }
}
