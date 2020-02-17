using AppDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS
{
    public class VmEmployee : BaseViewModel
    {
        IEmployee db; IBranch dbb; IRole dbr; IRolesToEmployee dbre;
        public ObservableCollection<VmEmployeeModel> employees { set; get; } = new ObservableCollection<VmEmployeeModel>();
        StringBuilder errors = new StringBuilder();
        public ObservableCollection<RoleViewModel> roles { get; set; } = new ObservableCollection<RoleViewModel>();
        public VmEmployeeModel employee { get; set; }
        public string securityError { get; set; }
        public string Errors { get; set; }
        public string searchName { get; set; }
        public VmEmployee()
        {
            db = new EmployeeApp();
            dbb = new BranchApp();
            dbr = new RoleApp();
            dbre = new RolesToEmployeeApp();
            employee = new VmEmployeeModel();
            employee.Branch = new Branch();
            setEmployees();
        }
        public void Search()
        {
            employees.Clear();
            setEmployees();
        }
        public IEnumerable<Branch> branches
        { get{ return dbb.getAllBranches(null);}  
        }
        /// <summary>
        /// get all employees to show on grid
        /// </summary>
        public void setEmployees()
        {
            foreach (var item in db.getAllEmployees(searchName))
            {
                employees.Add(setVmEmployee(item));
            }
        }
        /// <summary>
        /// method to save employee to the database
        /// </summary>
        public bool addEmployee()
        {

            //check if the form is valid
            if (IsValid())
            {
                if (employee.username.Length < 4)
                {
                    securityError = "Username must be at least 4 charactors long";
                    return false;
                }
                if (employee.Password.Length < 6)
                {
                    securityError = "Password must be at least 6 charactors long";
                    return false;
                }
                //check if the user name already exist
                if (db.userNameExist(employee.username))
                {
                    securityError = "This username is in use";
                    return false;
                }
                //set the firsttime log in to true
                employee.firsttimePassword = true;
                //send to database
                db.addEmployee(setEmployee(employee));
                refresh();
                return true;
            }
            return false;

        }
        /// <summary>
        /// set the view moodel employee with data from the database
        /// </summary>
        /// <param name="emplo">employee model from the database</param>
        /// <returns></returns>
        VmEmployeeModel setVmEmployee(Employee emplo)
        {
            VmEmployeeModel vmEmplo = new VmEmployeeModel();
            vmEmplo.Name = emplo.Name;
            vmEmplo.NationalIdNumber = emplo.NationalIdNumber;
            vmEmplo.Password = emplo.Password;
            vmEmplo.PhoneNumber = emplo.PhoneNumber;
            vmEmplo.JoiningDate = emplo.JoiningDate;
            vmEmplo.Surname = emplo.Surname;
            vmEmplo.BranchName = emplo.Branch;
            vmEmplo.Address = emplo.Address;
            vmEmplo.EmailAddress = emplo.EmailAddress;
            vmEmplo.EmployeeId = emplo.EmployeeId;
            vmEmplo.firsttimePassword = emplo.firsttimePassword;
            vmEmplo.username = emplo.username;
            vmEmplo.FullName = emplo.FullName;
            vmEmplo.branches= branches;
            vmEmplo.Branch = vmEmplo.branches.Where(x => x.Name == emplo.Branch).FirstOrDefault();
            if (emplo.LastLogInTime!=null)
            {
vmEmplo.lastLogIn = emplo.LastLogInTime.Value;
            }
            
            return vmEmplo;
        }
        void refresh()
        {
            employees.Clear();
            setEmployees();
        }
        /// <summary>
        /// delete employee
        /// </summary>
        /// <param name="id"></param>
        public void deleteEmplyee(int id)
        {
            db.deleteEmployee(id);
            refresh();
        }
        /// <summary>
        /// update employee on database
        /// </summary>
        /// <param name="emplo">view model employee type</param>
        public void updateEmployee(VmEmployeeModel emplo)
        {
            db.updateEmployee(setEmployee(emplo));
        }
        /// <summary>
        /// set back employeeview model to match the database setup employee
        /// </summary>
        /// <param name="vmEmplo"> the view model edited from the view</param>
        /// <returns></returns>
        Employee setEmployee(VmEmployeeModel vmEmplo)
        {
            Employee emplo = new Employee();

            emplo.Name = vmEmplo.Name;
            emplo.NationalIdNumber = vmEmplo.NationalIdNumber;
            emplo.Password = vmEmplo.Password;
            emplo.PhoneNumber = vmEmplo.PhoneNumber;
            emplo.JoiningDate = vmEmplo.JoiningDate;
            emplo.Surname = vmEmplo.Surname;
            emplo.Branch = vmEmplo.Branch.Name;
            emplo.Address = vmEmplo.Address;
            emplo.EmailAddress = vmEmplo.EmailAddress;
            emplo.EmployeeId = vmEmplo.EmployeeId;
            emplo.username = vmEmplo.username;
            emplo.firsttimePassword = vmEmplo.firsttimePassword;

            return emplo;
        }
      
        public void getRoles(int id)
        {
            roles.Clear();
            foreach (var item in dbr.roles(null))
            {
                RoleViewModel role = new RoleViewModel();
                foreach (var item2 in dbr.getUserRoles(id))
                {
                    if (item2.Name == item.Name)
                    {
                        role.isChecked = true;
                    }
                }
                role.role = item.Name;
                role.roleId = item.RoleId;
                role.EmployeeId = id;
                roles.Add(role);
            }
        }
        public void addRoles(int id)
        {

            dbre.deleteByEmployee(id);
            List<RoleToEmployee> roles = new List<RoleToEmployee>();
            foreach (var item in this.roles)
            {
                if (item.isChecked == true)
                {
                    RoleToEmployee r = new RoleToEmployee();
                    r.EmployeeId = id;
                    r.RoleId = item.roleId;
                    roles.Add(r);
                }

            }
            dbre.addRoleToEmployee(roles);
        }
        /// <summary>
        /// validation for employee b4 posting
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            errors.Clear();
            Errors = string.Empty;
            if (string.IsNullOrWhiteSpace(employee.Name))
            {
                errors.Append("Name is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.Surname))
            {
                errors.Append("Surname is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.PhoneNumber))
            {
                errors.Append("Phone Number is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.Address))
            {
                errors.Append("Address is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.NationalIdNumber))
            {
                errors.Append("National Id Number is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.Branch.Name))
            {
                errors.Append("Branch is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.Password))
            {
                errors.Append("Password is Required!\n");
            }
            if (string.IsNullOrWhiteSpace(employee.username))
            {
                errors.Append("Username is Required!\n");
            }
            if (errors.Length > 0)
            {
                Errors = errors.ToString();
                return false;
            }
            return true;
        }
        public class VmEmployeeModel : BaseViewModel
        {
            public int EmployeeId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string EmployeeNumber { get; set; }
            public string Password { get; set; }
            public string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string NationalIdNumber { get; set; }
            public Branch Branch { get; set; }
            public string BranchName { get; set; }
            public bool firsttimePassword { get; set; }
            public bool isSelected { get; set; }
            public DateTime JoiningDate { get; set; }
            public string username { get; set; }
            public string FullName { get; set; }
            public DateTime? lastLogIn { get; set; }

            public IEnumerable<Branch> branches { get; set; }


        }
        public class RoleViewModel:BaseViewModel
        {
            public bool isChecked { get; set; }
            public string role { get; set; }
            public int EmployeeId { get; set; }
            public int roleId { get; set; }
        }

    }
}
