
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace POS
{
    /// <summary>
    /// Interaction logic for homeDashboard.xaml
    /// </summary>
    public partial class homeDashboard : UserControl
    {
        public homeDashboard()
        {
            InitializeComponent();
            DataContext = new homeDashViewModel();

           
       
        }
       
        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}

