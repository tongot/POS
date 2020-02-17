

using AppDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace POS
{
    public class VmCustomer:BaseViewModel
    {
        private ICustomer db;

        public VmCustomer()
        {
            db = new CustomerApp();
        }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Age { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateJoined { get; set; }
        public string nationalId { get; set; }
        public string NextOfKinContact { get; set; }
        //public string MyProperty { get; set; }
        public string NextOfKin { get; set; }
        public Customer customer { get; set; }
        public string textError { get; set; }
        public bool isSelected { get; set; }
        public string searchName { get; set; }
        public string FullName { get; set; }
        public ICollection<Sale> sales { get; set; }

        /// <summary>
        /// set back the branch view model to the database class
        /// </summary>
        /// <param name="b">view model branch</param>
        /// <returns></returns>
        public Customer setCustomer(VmCustomer c)
        {
            Customer customer = new Customer();
            customer.CustomerId = c.CustomerId;
            customer.Name = c.Name;
            customer.Surname = c.Surname;
            customer.Age = c.Age;
            customer.Address = c.Address;
            customer.PhoneNumber = c.PhoneNumber;
            customer.DateJoined = c.DateJoined;
            customer.nationalId = c.nationalId;
            customer.NextOfKin = c.NextOfKin;
            customer.NextOfKinContact = c.NextOfKinContact;
            return customer;
        }
        /// <summary>
        /// set customer from the database to the view model and retrive an observable collection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmCustomer> GetAllCustomers()
        {

            ObservableCollection<VmCustomer> vmb = new ObservableCollection<VmCustomer>();

            foreach (var c in db.getAllCustomers(searchName))
            {
                VmCustomer customer = new VmCustomer();
                customer.CustomerId = c.CustomerId;
                customer.Name = c.Name;
                customer.Surname = c.Surname;
                customer.Age = c.Age.Value;
                customer.Address = c.Address;
                customer.PhoneNumber = c.PhoneNumber;
                customer.DateJoined = c.DateJoined;
                customer.nationalId = c.nationalId;
                customer.NextOfKin = c.NextOfKin;
                customer.NextOfKinContact = c.NextOfKinContact;
                customer.FullName = c.FullName;
                vmb.Add(customer);
            }
            return vmb;
        }

        public bool addNewCustomer(VmCustomer customer)
        {
            if(validated(customer))
            {
                db.addCustomer(setCustomer(customer));
                return true;
            }
            return false;
        }
        public void deleteCustomer(int id)
        {
            db.deleteCustomer(id);
        }
        public void updateCustomer(VmCustomer customer)
        {
            db.updateCustomer(setCustomer(customer));
        }
        public bool validated(VmCustomer customer)
        {
            StringBuilder error = new StringBuilder();

            if (string.IsNullOrWhiteSpace(customer.Name))
            {
                error.Append(" Name is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.Surname))
            {
                error.Append("Surname is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.NextOfKin))
            {
                error.Append("Next of kin is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.NextOfKinContact))
            {
                error.Append("Next of kin contact is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.nationalId))
            {
                error.Append("National id is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                error.Append("Phone Number is required\n");
            }
            if (string.IsNullOrWhiteSpace(customer.Address))
            {
                error.Append("Address is required\n");
            }
            try
            {
              if (customer.Age<0||customer.Age==null)
                        {
                            error.Append("Age is required\n");
                        }
            }catch
            {
                error.Append("Age is required and age is a number\n");
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
