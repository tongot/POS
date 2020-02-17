using System;

namespace POS
{
    public class VmSearchSaleFilter:BaseViewModel
    {
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string productCode { get; set; }
        public string refNumber { get; set; }
        public string  branchName { get; set; }
        public string  productName { get; set; }
        public string customerId { get; set; }

        public void clear()
        {
            startDate = null;
            endDate = null;
            productCode = null;
            refNumber = null;
            branchName = null;
            productName = null;
            customerId = null;
        }
    }
}
