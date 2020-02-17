
using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        #region privete
        private readonly IDialogService dialogService;
        #endregion

        public VmCustomer VmCustomer { get; set; } = new VmCustomer();
        public ObservableCollection<VmCustomer> customers { get; set; } = new ObservableCollection<VmCustomer>();

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





        public CustomerViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            customers = VmCustomer.GetAllCustomers();
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
            customers = VmCustomer.GetAllCustomers();
            VmCustomer = customers.FirstOrDefault();
            if (VmCustomer != null)
            {
                VmCustomer.isSelected = true;
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
            VmCustomer.isSelected = false;
            VmCustomer = customers.Where(x => x.CustomerId == (int)id).FirstOrDefault();
            VmCustomer.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (VmCustomer != null)
            {
                VmCustomer.isSelected = false;
            }
            VmCustomer = new VmCustomer();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            customers.Clear();
            customers = VmCustomer.GetAllCustomers();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            VmCustomer.textError = string.Empty;
            var selected = customers.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {

            if (VmCustomer.addNewCustomer(VmCustomer))
            {
                customers.Clear();
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
                VmCustomer.deleteCustomer(VmCustomer.CustomerId);
                customers.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (VmCustomer.validated(VmCustomer))
            {
                VmCustomer.updateCustomer(VmCustomer);
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
