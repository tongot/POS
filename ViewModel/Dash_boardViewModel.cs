using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;

using System.Windows.Input;

namespace POS
{
    public class Dash_boardViewModel:BaseViewModel
    {
        public ICommand backHomeBtn { get; set; }

        
        public Dash_boardViewModel()
        {
            backHomeBtn = new RelayCommand(backHome);
        }

        private void backHome()
        {
            IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.menuPage;
        }
    }
}
