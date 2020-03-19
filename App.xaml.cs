
using POS.Ioc;
using System;
using System.Windows;
using System.Windows.Threading;

namespace POS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// for us to imidiatly load the IOc before anthing 
        /// </summary>
        /// <param name="e"></param>


        DispatcherTimer td = new DispatcherTimer(); 
        Splash splash = new Splash();
        protected override void OnStartup(StartupEventArgs e)
        {
           
            //base do
            base.OnStartup(e);

            //splash.Show();
            //td.Tick += new System.EventHandler(td_tick);
            //td.Interval = new TimeSpan(0, 0, 3);
            //td.Start();

            IocContainer.AppDomainSetup();
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            //show the main window

        }

        private void td_tick(object sender, EventArgs e)
        {
            
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
            td.Stop();
            splash.Close();
        }
    }
}
