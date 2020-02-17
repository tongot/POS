
using AppDatabase;
using Ninject;
using POS.Ioc;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace POS
{
    public class categoryViewModel:BaseViewModel
    {
        private readonly IDialogService dialogService;
        public VmCategory VmCategory { get; set; } = new VmCategory();
        public ObservableCollection<VmCategory> categories { get; set; } = new ObservableCollection<VmCategory>();
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




        public categoryViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            categories = VmCategory.GetAllCategories();
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
            categories = VmCategory.GetAllCategories();
            VmCategory = categories.FirstOrDefault();
            if (VmCategory != null)
            {
                VmCategory.isSelected = true;
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
            VmCategory.isSelected = false;
            VmCategory = categories.Where(x => x.categoryId == (int)id).FirstOrDefault();
            VmCategory.isSelected = true;
            showEdit = false;
            showDetails = true;
            showAdd = false;
        }
        /// <summary>
        /// show add new panel
        /// </summary>
        private void addNew()
        {
            if (VmCategory != null)
            {
                VmCategory.isSelected = false;
            }
            VmCategory = new VmCategory();
            showEdit = false;
            showDetails = false;
            showAdd = true;
        }
        private void Search()
        {
            categories.Clear();
            categories = VmCategory.GetAllCategories();
        }

        /// <summary>
        /// show edit panel
        /// </summary>
        private void Edit()
        {
            VmCategory.textError = string.Empty;
            var selected = categories.Where(x => x.isSelected == true);
            showAdd = false;
            showDetails = false;
            showEdit = true;
        }
        private void SaveNew()
        {

            if (VmCategory.addNewCategory(VmCategory))
            {
                categories.Clear();
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
                VmCategory.deleteCategory(VmCategory.categoryId);
                categories.Clear();
                InitialSelect();
            }
            else
            {
                return;
            }

        }
        public void SaveEdit()
        {
            if (VmCategory.validated(VmCategory))
            {
                VmCategory.updateCategory(VmCategory);
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
