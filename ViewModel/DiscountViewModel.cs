
using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS
{
    public class DiscountViewModel:BaseViewModel
    {
        private readonly IDialogService dialogService;
        public VmDiscount VmDiscount { get; set; } = new VmDiscount();
        public ObservableCollection<VmDiscount> discounts { get; set; } = new ObservableCollection<VmDiscount>();
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




        public DiscountViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            discounts = VmDiscount.GetAllDiscount();
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
            discounts = VmDiscount.GetAllDiscount();
            VmDiscount = discounts.FirstOrDefault();
            if (VmDiscount != null)
            {
                VmDiscount.isSelected = true;
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
            VmDiscount.isSelected = false;
            VmDiscount = discounts.Where(x => x.DiscountId == (int)id).FirstOrDefault();
            VmDiscount.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (VmDiscount != null)
            {
                VmDiscount.isSelected = false;
            }
            VmDiscount = new VmDiscount();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            discounts.Clear();
            discounts = VmDiscount.GetAllDiscount();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            VmDiscount.textError = string.Empty;
            var selected = discounts.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {

            if (VmDiscount.addNewDiscount(VmDiscount))
            {
                discounts.Clear();
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
                VmDiscount.deleteDiscount(VmDiscount.DiscountId);
                discounts.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (VmDiscount.validated(VmDiscount))
            {
                VmDiscount.upadateDisount(VmDiscount);
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
