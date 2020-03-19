using System;
using System.Collections.Generic;

using System.Windows.Input;

namespace POS
{
    public class Dash_boardViewModel:BaseViewModel
    {
        public ICommand test_btn { get; set; }
        public string test_string { get; set; } = "";
        
        public Dash_boardViewModel()
        {
            test_btn = new RelayCommand(test);
        }

        private void test()
        {
            test_string = "wala";
        }
    }
}
