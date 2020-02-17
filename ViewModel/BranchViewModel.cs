
using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS.ViewModel
{
    public class BranchViewModel : BaseViewModel
    {
        #region privete
        private readonly IDialogService dialogService;
        #endregion

        public VmBranch VmBranch { get; set; } = new VmBranch();
        public ObservableCollection<VmBranch> branches { get; set; } = new ObservableCollection<VmBranch>();

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


        public BranchViewModel(IDialogService dialogService)
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
            branches = VmBranch.GetBranchAllBranches();
            VmBranch = branches.FirstOrDefault();
            if (VmBranch != null)
            {
                VmBranch.isSelected = true;
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
            VmBranch.isSelected = false;
            VmBranch = branches.Where(x => x.BranchId == (int)id).FirstOrDefault();
            VmBranch.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (VmBranch != null)
            {
                VmBranch.isSelected = false;
            }
            VmBranch = new VmBranch();
            VmBranch.Employee = new Employee();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            branches.Clear();
            branches = VmBranch.GetBranchAllBranches();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            VmBranch.textError = string.Empty;
            var selected = branches.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {
            if (VmBranch.Employee!=null)
            {
                VmBranch.Manager = VmBranch.Employee.FullName;
            }
            if (VmBranch.addNewBranch(VmBranch))
            {   
                branches.Clear();
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
                VmBranch.deleteBranch(VmBranch.BranchId);
                branches.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (VmBranch.validated(VmBranch))
            {
                VmBranch.updateBranch(VmBranch);
                showEdit = false;
                showAdd = false;
                showDetails = true;
            }
        }

        private void backHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }

    }
}
