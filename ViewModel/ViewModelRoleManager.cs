using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS
{
    public class ViewModelRoleManager:BaseViewModel
    {
        #region privete
        private readonly IDialogService dialogService;
        #endregion
        public VmRoleManger roleManager { get; set; } = new VmRoleManger();
        public ICommand btnBackHome { get; set; }

        public ICommand saveBtn { get; set; }
        public ICommand btnEdit { get; set; }
        public ICommand btnDelete { get; set; }
        public ICommand btnSearch { get; set; }
        public ICommand btnAddNew { get; set; }
        public ICommand btnSaveEdit { get; set; }
        public ICommand btnSaveNew { get; set; }
        public ICommand btnDetails { get; set; }
        public bool showDetails { get; set; } = true;
        public bool showEdit { get; set; } = false;
        public bool showAdd { get; set; } = false;


        public ViewModelRoleManager(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            InitialSelect();
            //saveBtn = new RelayCommand(saveEmployee);
            btnBackHome = new RelayCommand(backHome);
            btnEdit = new RelayCommand(Edit);
            btnSearch = new RelayCommand(Search);
            btnSaveEdit = new RelayCommand(SaveEdit);
            btnAddNew = new RelayCommand(addNew);
            btnSaveNew = new RelayCommand(SaveNew);
            btnDetails = new RelayCommand(details);
            btnDelete = new RelayCommand(delete);
        }
        
        void InitialSelect()
        {
            roleManager.getAllRoles();
            roleManager.Role= roleManager.Roles.FirstOrDefault();
            roleManager.getPermissions(roleManager.Role.RoleId);
            if (roleManager.Role != null)
            {
                roleManager.Role.isSelected = true;
            }
            else
            {
                showAdd = false;
                showEdit = false;
                showDetails = false;
            }

        }
        /// <summary>
        /// show details panel 
        /// </summary>
        /// <param name="id"></param>
        private void details(object id)
        {
            roleManager.permissions.Clear();
            roleManager.Role.isSelected = false;
            roleManager.Role = roleManager.Roles.Where(x => x.RoleId == (int)id).FirstOrDefault();
            roleManager.getPermissions(roleManager.Role.RoleId);
            roleManager.Role.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (roleManager.Role != null)
            {
                roleManager.Role.isSelected = false;
            }
            roleManager.Role = new vmRoles();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            roleManager.Roles.Clear();
            roleManager.getAllRoles();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            roleManager.error = string.Empty;
            var selected = roleManager.Roles.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {

            if (roleManager.addRoles())
            {
                roleManager.Roles.Clear();
                InitialSelect();

                showEdit = false;
                showDetails = true;
                showAdd = false;
            }


        }
        public void delete()
        {
            var viewModel = new DialogViewModel("Are sure you want to delete this record");
            bool? result = dialogService.ShowDialog(viewModel);
            if (result == true)
            {
                roleManager.deleteRole();
                roleManager.Roles.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (roleManager.validateE())
            {
                roleManager.setPermissions();
                showEdit = false;
                showAdd = false;
                showDetails = true;
            }
        }

        private void backHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
        //void backHome()
        //{
        //    IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        //}
        //void addNewRole()
        //{
        //    roleManager.addRoles();
        //    roleManager.getAllRoles();
        //}
        //void showPermissions(object obj)
        //{
        //    if(obj!=null)
        //    {
        //        roleManager.permissions.Clear();
        //        roleManager.getPermissions((int)obj);
        //    }
        //}
        //void setPermissions()
        //{
        //    roleManager.setPermissions();
        //}

    }
}
