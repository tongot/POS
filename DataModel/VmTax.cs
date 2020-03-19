
using AppDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace POS
{
    /// <summary>
    /// view model Discount for presenting discount
    /// </summary>
    public class VmTax: BaseViewModel
    {
        private ITax db;
        public VmTax()
        {
            db = new TaxApp();

        }
    
        public string Name { get; set; }
        public decimal percentage { get; set; }
        public bool isSelected { get; set; }
        public int TaxId { get; set; }
        public string textError { get; set; }
        public string searchName { get; set; }
        public TypeOfTaxV TypeOfTax { get; set; }
        /// <summary>
        /// set back the discount view model to the database class
        /// </summary>
        /// <param name="b">view model discouunt</param>
        /// <returns></returns>
        public Tax  setTax(VmTax d)
        {
            Tax tx = new Tax();
            tx.TaxId = d.TaxId;
            tx.Name = d.Name;
            tx.Percentage = d.percentage;
            tx.TypeOfTex = (TypeOfTax)d.TypeOfTax;
            return tx;
        }
        /// <summary>
        /// set discount from the database to the view model and retrive an observable collection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmTax> GetAllTaxes()
        {
            ObservableCollection<VmTax> vmb = new ObservableCollection<VmTax>();

            foreach (var item in db.GetTaxes(searchName))
            {
                VmTax vm = new VmTax();
                vm.Name = item.Name;
                vm.TaxId = item.TaxId;
                vm.percentage = item.Percentage;
                vm.TypeOfTax = (TypeOfTaxV)item.TypeOfTex;
                vmb.Add(vm);
            }
            return vmb;
        }
       
        public bool addNewTax(VmTax vmTax)
        {
            if(validated(vmTax))
            {
               db.AddTax(setTax(vmTax));
                return true;
            }
            return false;
            
        }
        public void deleteTax(int id)
        {
            db.DeleteTax(id);
        }
        public void upadateTax(VmTax d)
        {
            db.UpadateTax(setTax(d));
        }
        public bool validated(VmTax tax)
        {
            StringBuilder error = new StringBuilder();
            textError = string.Empty;

            if (string.IsNullOrWhiteSpace(tax.Name))
            {
                error.Append("discount name is required\n");
            }
            if(tax.percentage<0|tax.percentage>100)
            {
                error.Append("discount percentage out of range\n");
            }
            textError = error.ToString();
            if (string.IsNullOrEmpty(textError))
            {
                return true;
            }
            return false;
        }
        
    }
    public enum TypeOfTaxV
        {
            global=0,
            special=1
        }
}
