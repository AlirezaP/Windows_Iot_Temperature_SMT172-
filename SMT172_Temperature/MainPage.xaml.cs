using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SMT172_Temperature
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        int outputPin = 2;
        SMT172 smt;
        private void Grid_Loading(FrameworkElement sender, object args)
        {

            listView.ShowsScrollingPlaceholders = true;

            smt = new SMT172(outputPin);

            var timer2 = new DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer2.Tick += Timer2_Tick; ;

            timer2.Start();
        }

        private async void Timer2_Tick(object sender, object e)
        {
            var t = await smt.ReadTemperature();
            txtTemperature.Text = $"{t.ToString("00")} °C";
            txtDateTime.Text = DateTime.Now.ToString("hh:mm:ss");
            var res = $"T:{t.ToString()} at {DateTime.Now.ToString("hh/mm/ss")}";

            listView.Items.Add($"T:{res.ToString()} at {DateTime.Now.ToString("hh/mm/ss")}");

            if (listView.Items.Count() > 1)
            {
                listView.SelectedIndex = listView.Items.Count() - 1;
                listView.ScrollIntoView(listView.Items[listView.Items.Count() - 1]);
            }

            if (listView.Items.Count >= 100)
            {
                listView.Items.Clear();
            }
        }

    }
}
