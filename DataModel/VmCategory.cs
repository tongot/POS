
using AppDatabase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace POS
{
    /// <summary>
    /// view model category for presenting categories
    /// </summary>
    public class VmCategory: BaseViewModel
    {
        private ICategory db;
        public VmCategory()
        {
            db = new CategoryApp();

        }
    
        public string Name { get; set; }
        public bool isSelected { get; set; }
        public int categoryId { get; set; }
        public string textError { get; set; }
        public string searchName { get; set; }
        /// <summary>
        /// set back the category view model to the database class
        /// </summary>
        /// <param name="b">view model category</param>
        /// <returns></returns>
        public Category setCategory(VmCategory c)
        {
            Category ct = new Category();
            ct.Name = c.Name;
            ct.CategoryId = c.categoryId;
            return ct;
        }
        /// <summary>
        /// set category from the database to the view model and retrive an observable collection
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<VmCategory> GetAllCategories()
        {
            ObservableCollection<VmCategory> vmb = new ObservableCollection<VmCategory>();

            foreach (var item in db.GetAllCategories(searchName))
            {
                VmCategory vm = new VmCategory();
                vm.Name = item.Name;
                vm.categoryId = item.CategoryId;
                vmb.Add(vm);
            }
            return vmb;
        }
       
        public bool addNewCategory(VmCategory category)
        {
            if(validated(category))
            {
db.addCategory(setCategory(category));
                return true;
            }
            return false;
            
        }
        public void deleteCategory(int id)
        {
            db.deleteCategory(id);
        }
        public void updateCategory(VmCategory c)
        {
            db.updateCategory(setCategory(c));
        }
        public bool validated(VmCategory category)
        {
            StringBuilder error = new StringBuilder();
            if (db.categoryExist(category.Name))
            {
                error.Append("This category name already exist\n");
            }
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                error.Append("Category name is required\n");
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
