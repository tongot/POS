using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;


namespace POS
{
    /// <summary>
    /// Interaction logic for master_dashboard.xaml
    /// </summary>
    public partial class master_dashboard : UserControl
    {
       
        public master_dashboard()
        {
            InitializeComponent();
            DataContext = new MasterChartViewModel();
        }
    }
}
