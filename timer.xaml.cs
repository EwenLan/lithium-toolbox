using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace lithium_toolbox
{
    /// <summary>
    /// Interaction logic for timer.xaml
    /// </summary>
    public partial class Timer : Window
    {
        private Window father;
        private System.Timers.Timer timerCounter = new System.Timers.Timer();
        DispatcherTimer displayRefreshTimer = new DispatcherTimer();
        DateTime timesUpTime = DateTime.Now;
        public Timer(Window father)
        {
            InitializeComponent();
            this.father = father;
            timerCounter.AutoReset = false;
            displayRefreshTimer.Interval = TimeSpan.FromMilliseconds(100);
            timerCounter.Elapsed += new System.Timers.ElapsedEventHandler(OnTimerTimesUp);
            displayRefreshTimer.Tick += new EventHandler(displayRefreshHandler);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timerCounter.Dispose();
            displayRefreshTimer.Stop();
            father.Show();
        }

        private void M1Button_Click(object sender, RoutedEventArgs e)
        {
            StartTimerInMinutes(1);
        }
        private void OnTimerTimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            string content = "Time's Up";
            string caption = "Time's Up";
            displayRefreshTimer.Stop();
            MessageBox.Show(content, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void StartTimerInMinutes(int minutes)
        {
            var totalSeconds = minutes * 60;
            timerCounter.Interval = totalSeconds * 1000;
            timerCounter.Start();
            timesUpTime = DateTime.Now.AddSeconds(totalSeconds);
            displayRefreshTimer.Start();
        }
        private void displayRefreshHandler(object sender, EventArgs e) {

            var now = DateTime.Now;
            var restTime = timesUpTime - now;
            var seconds = restTime.Seconds;
            timeDisplay.Content = seconds.ToString();
        }

        private void M3Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
