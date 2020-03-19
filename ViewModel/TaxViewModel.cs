
using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS
{
    public class TaxViewModel : BaseViewModel
    {
        private readonly IDialogService dialogService;
        public VmTax VmTax { get; set; } = new VmTax();
        public ObservableCollection<VmTax> taxes { get; set; } = new ObservableCollection<VmTax>();
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




        public TaxViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            taxes = VmTax.GetAllTaxes();
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
            taxes = VmTax.GetAllTaxes();
            VmTax = taxes.FirstOrDefault();
            if (VmTax != null)
            {
                VmTax.isSelected = true;
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
            VmTax.isSelected = false;
            VmTax = taxes.Where(x => x.TaxId == (int)id).FirstOrDefault();
            VmTax.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (VmTax != null)
            {
                VmTax.isSelected = false;
            }
            VmTax = new VmTax();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            taxes.Clear();
            taxes = VmTax.GetAllTaxes();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            VmTax.textError = string.Empty;
            var selected = taxes.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {

            if (VmTax.addNewTax(VmTax))
            {
                taxes.Clear();
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
                VmTax.deleteTax(VmTax.TaxId);
                taxes.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (VmTax.validated(VmTax))
            {
                VmTax.upadateTax(VmTax);
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
