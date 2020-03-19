
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
    public class VmDiscount: BaseViewModel
    {
        private IDiscount db;
        public VmDiscount()
        {
            db = new AppDiscount();

        }
    
        public string Name { get; set; }
        public decimal percentage { get; set; }
        public bool isSelected { get; set; }
        public int DiscountId { get; set; }
        public string textError { get; set; }
        public string searchName { get; set; }
        public TypeOfDiscountV TypeOfDiscount { get; set; }
        /// <summary>
        /// set back the discount view model to the database class
        /// </summary>
        /// <param name="b">view model discouunt</param>
        /// <returns></returns>
        public Discount  setDiscount(VmDiscount d)
        {
            Discount ds = new Discount();
            ds.DiscountId = d.DiscountId;
            ds.Name = d.Name;
            ds.percentage = d.percentage;
            ds.TypeOfDiscount = (TypeOfDiscount)d.TypeOfDiscount;
            return ds;
        }
        /// <summary>
        /// set discount from the database to the view model and retrive an observable collection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmDiscount> GetAllDiscount()
        {
            ObservableCollection<VmDiscount> vmb = new ObservableCollection<VmDiscount>();

            foreach (var item in db.GetDiscounts(searchName))
            {
                VmDiscount vm = new VmDiscount();
                vm.Name = item.Name;
                vm.DiscountId = item.DiscountId;
                vm.percentage = item.percentage;
                vm.TypeOfDiscount = (TypeOfDiscountV)item.TypeOfDiscount;
                vmb.Add(vm);
            }
            return vmb;
        }
       
        public bool addNewDiscount(VmDiscount vmDiscount)
        {
            if(validated(vmDiscount))
            {
               db.AddDiscount(setDiscount(vmDiscount));
                return true;
            }
            return false;
            
        }
        public void deleteDiscount(int id)
        {
            db.DeleteDiscount(id);
        }
        public void upadateDisount(VmDiscount d)
        {
            db.UpadateDiscount(setDiscount(d));
        }
        public bool validated(VmDiscount discount)
        {
            StringBuilder error = new StringBuilder();
            textError = string.Empty;

            if (string.IsNullOrWhiteSpace(discount.Name))
            {
                error.Append("discount name is required\n");
            }
            if(discount.percentage<0|discount.percentage>100)
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
    public enum TypeOfDiscountV
        {
            global=0,
            special=1
        }
}
