

using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using static POS.VmEmployee;

namespace POS
{
    public class EmployeeViewModel:BaseViewModel
    {
        public VmEmployee emplo { get; set; }
        private readonly IDialogService dialogService;
        public ObservableCollection<VmEmployeeModel> EmploFilter { get; set; }
        #region commands
       
        public ICommand backHomeBtn { get; set; }
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
        #endregion
        public EmployeeViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            emplo = new VmEmployee();
            InitialSelect();

            saveBtn = new RelayCommand(saveEmployee);
            backHomeBtn = new RelayCommand(backHome);
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
            emplo.employee = emplo.employees.FirstOrDefault();
            if(emplo.employee!= null)
            {
             emplo.employee.isSelected = true;
            emplo.getRoles(emplo.employee.EmployeeId)    ;
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
            emplo.employee.isSelected = false;
            emplo.employee = emplo.employees.Where(x => x.EmployeeId == (int)id).FirstOrDefault();
            emplo.employee.isSelected = true;
            emplo.getRoles(emplo.employee.EmployeeId);

            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            emplo.Errors = string.Empty;
            if(emplo.employee!=null)
            {
                emplo.employee.isSelected = false;
            }           
            emplo.employee = new VmEmployeeModel();
            emplo.employee.Branch = new Branch();
            emplo.employee.branches = emplo.branches;
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            emplo.Search();
        }
     
            /// <summary>
            /// show edit panel
            /// </summary>
        private void Edit()
        {
            emplo.Errors = string.Empty;
            var selected = emplo.employees.Where(x => x.isSelected == true);
            emplo.getRoles(selected.First().EmployeeId);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {
            if (emplo.addEmployee())
            {
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
            if(result==true)
            {
                emplo.deleteEmplyee(emplo.employee.EmployeeId);
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
                if (emplo.IsValid())
                {
                    emplo.updateEmployee(emplo.employee);
                    emplo.addRoles(emplo.employee.EmployeeId);
                showEdit = false;
                showAdd = false;
                showDetails = true;
               }
        }

        private void backHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }

        void saveEmployee()
        {
            emplo.addEmployee();
        }
    }
}
