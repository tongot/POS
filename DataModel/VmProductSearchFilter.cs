

using AppDatabase;
using System.Collections.ObjectModel;

namespace POS
{
    /// <summary>
    /// filter class view model to supply filtering details
    /// </summary>
    public class VmProductSearchFilter : BaseViewModel
    {
        /// <summary>
        /// filters view model to filter with product list
        /// </summary>
        public VmProductSearchFilter()
        {

        }

        public string searchString { get; set; }
        public double? minPrice { get; set; } = 0;
        public double? maxPrice { get; set; } = 0;
        public int? minQuantity { get; set; } = 0;
        public int? maxQuantity { get; set; } = 0;
        public double? minTotalValue { get; set; } = 0;
        public double? maxTotalValue { get; set; } = 0;

        public string ErrorNumberTostring { get;set; }
        public string PriceError { get; set; }
        public string QuantityError { get; set; } 
        public string TotalValueError { get; set; }

        /// <summary>
        /// set the values of the filter to match the ones in database library
        /// </summary>
        /// <param name="f">the values of model to set</param>
        /// <returns></returns>
        public ProductSearchFilter setFilter(VmProductSearchFilter f)
        {
        
                ProductSearchFilter pf = new ProductSearchFilter();
                pf.searchString = f.searchString;
                pf.minPrice = f.minPrice.Value;
                pf.maxPrice = f.maxPrice.Value;
                pf.minQuantity = f.minQuantity.Value;
                pf.maxQuantity = f.maxQuantity.Value;
                pf.minTotalValue = f.minTotalValue.Value;
                pf.maxTotalValue = f.maxTotalValue.Value;
                return pf;

        }
        /// <summary>
        /// validating the filters before sending
        /// </summary>
        /// <param name="f">filter values</param>
        /// <returns>returns true if filter is error free and the opposite is true</returns>
        public bool validateFilterValue(VmProductSearchFilter f)
        {
            int i;double d;
            //check if all interger filters are numbers 
            if (!double.TryParse(f.maxPrice.ToString(), out d)
               | !double.TryParse(f.minPrice.ToString(), out d)
               | !double.TryParse(f.maxTotalValue.ToString(), out d)
               | !double.TryParse(f.minTotalValue.ToString(), out d)
               | !int.TryParse(f.minQuantity.ToString(), out i)
               | !int.TryParse(f.maxQuantity.ToString(), out i))
            {
                ErrorNumberTostring = "all fields must be numbers";
                return false;
            }
                if (f.minPrice >f.maxPrice &f.maxPrice>0)
            {
                PriceError = "Max price must be greater than the min price";
                maxPrice = 0;
                minPrice = 0;
                return false;
            }
            if (f.minQuantity > f.maxQuantity& f.maxQuantity > 0)
            {
                QuantityError = "Max Qnty must be greater than the min Qnty";
                maxQuantity = 0;
                minQuantity = 0;
                return false;

            }
            if (f.minTotalValue > f.minTotalValue& f.minTotalValue>0)
            {
                TotalValueError = "Max Tvalue must be greater than the min Tvalue";
                maxTotalValue = 0;
                minTotalValue = 0;
                return false;
            }
            return true;
        }

    }
}
