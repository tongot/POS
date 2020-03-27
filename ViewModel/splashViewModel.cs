using AppDatabase;
using Ninject;
using POS.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace POS
{
    public class splashViewModel:BaseViewModel
    {
        DispatcherTimer tm = new DispatcherTimer();
        

        public string Connect { get; set; } = "Loading...";
        public splashViewModel()
        {
                
        }

        void start()
        {
            tm.Tick += new System.EventHandler(td_tick);
            tm.Interval = new TimeSpan(0, 0,3);
            tm.Start();

        }
        private void td_tick(object sender, EventArgs e)
        {
            Connect = "Testing connection...";
            ConnectionStringSetter con = new ConnectionStringSetter();
            if(con.Connected()!= "Connected")
            {
                IocContainer.Kenel.Get<AppViewModel>().CurrentPage = ApplicationPage.connections;
                tm.Stop();
            }
            tm.Stop();
        }
    }
}
