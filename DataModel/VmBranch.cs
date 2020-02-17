
using AppDatabase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace POS
{
    public class VmBranch : BaseViewModel
    {
        private IBranch db;
        private IEmployee emploDb;
        public VmBranch()
        {
            db = new BranchApp();
            emploDb = new EmployeeApp();
            Employee = new Employee();
        }
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Manager { get; set; }
        public bool isSelected { get; set; }
        public string textError { get; set; }
        public string searchName { get; set; }
        public Employee Employee { get; set; }

        /// <summary>
        /// set back the branch view model to the database class
        /// </summary>
        /// <param name="b">view model branch</param>
        /// <returns></returns>
        public Branch setBranch(VmBranch b)
        {
            Branch br = new Branch();
            br.BranchId = b.BranchId;
            br.Name = b.Name;
            br.Region = b.Region;
            br.Manager = b.Manager;
            return br;
        }
        /// <summary>
        /// set category from the database to the view model and retrive an observable collection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmBranch> GetBranchAllBranches()
        {
            ObservableCollection<VmBranch> vmb = new ObservableCollection<VmBranch>();

            foreach (var item in db.getAllBranches(searchName))
            {
                VmBranch vm = new VmBranch();
                vm.BranchId = item.BranchId;
                vm.Name = item.Name;
                vm.Region = item.Region;
                vm.Manager = item.Manager;
                vm.Name = item.Name;

                vmb.Add(vm);
            }
            return vmb;
        }
        public List<Employee> Employees
        {
            get { return emploDb.getAllEmployees(null); }
        }
        public bool addNewBranch(VmBranch branch)
        {
            if (validated(branch)) { 
            db.addBranch(setBranch(branch));
                return true;
            }
            return false;
            
        }
        public void deleteBranch(int id)
        {
            db.deleteBranch(id);
        }
        public void updateBranch(VmBranch b)
        {
            db.UpdateBranch(setBranch(b));
        }
        public bool validated(VmBranch branch)
        {
            textError = string.Empty;
            StringBuilder error = new StringBuilder();
            if(db.branchExist(branch.Name))
            {
                error.Append("This branche name already exist\n");
            }
            if (string.IsNullOrWhiteSpace(branch.Name))
            {
                error.Append("Branch name is required\n");
            }
            if (string.IsNullOrWhiteSpace(branch.Region))
            {
                error.Append("Branch region is required\n");
            }
            if (string.IsNullOrWhiteSpace(branch.Manager))
            {
                error.Append("Branch Manager is required");
            }
            textError = error.ToString();
            if (string.IsNullOrEmpty(textError))
            {
                return true;
            }
            return false;
        }
    }
}
