using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        CountDownTimer countDownTimer = new CountDownTimer();
        DisplayController displayController = new DisplayController();

        public Timer(Window father)
        {
            InitializeComponent();
            this.father = father;
            // 显示刷新周期100毫秒
            displayRefreshTimer.Interval = TimeSpan.FromMilliseconds(100);
            displayRefreshTimer.Tick += new EventHandler(displayRefreshHandler);
            displayRefreshTimer.Start();
            displayController.AddDigit(D0Label);
            displayController.AddDigit(D1Label);
            displayController.AddDigit(D2Label);
            displayController.AddDigit(D3Label);
            displayController.AddDigit(D4Label);
            displayController.AddDigit(D5Label);
            displayController.AddDigit(D6Label);
            displayController.AddDigit(D7Label);
            displayController.AddDigit(D8Label);
            displayController.AddDigit(D9Label);
            displayController.Show("");

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timerCounter.Dispose();
            displayRefreshTimer.Stop();
            father.Show();
        }

        private void M1Button_Click(object sender, RoutedEventArgs e)
        {
            var minutesInput = 0;
            if (!int.TryParse(MinuteInput.Text, out minutesInput))
            {
                minutesInput = 1;
            }
            this.countDownTimer.SetTimer(minutesInput);
        }

        private void displayRefreshHandler(object sender, EventArgs e)
        {

            var display = this.countDownTimer.Display();
            this.displayController.Show(display);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.countDownTimer.ClearTimer();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            M1Button.Opacity = 1.0;
            ClearButton.Opacity = 1.0;
            ExitButton.Opacity = 1.0;
            MinuteInput.Opacity = 1.0;
        }

        private void M1Button_MouseLeave(object sender, MouseEventArgs e)
        {
            M1Button.Opacity = 0.0;
            ClearButton.Opacity = 0.0;
            ExitButton.Opacity = 0.0;
            MinuteInput.Opacity = 0.0;
        }

        private void MinuteInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (M1Button == null)
            {
                return;
            }
            var minutesInput = 0;
            // 字符串解析失败或解析成功且为1分钟时，显示单数形式
            if ((int.TryParse(MinuteInput.Text, out minutesInput) && minutesInput == 1) || !int.TryParse(MinuteInput.Text, out minutesInput))
            {
                M1Button.Content = "Minute Start";
                return;
            }
            // 显示复数形式
            M1Button.Content = "Minutes Start";
        }
    }

    public class CountDownTimer
    {
        enum State
        {
            Init,
            Running,
            Stopped,
        }

        DateTime timesUpTime;
        State previousState;

        public CountDownTimer()
        {
            timesUpTime = DateTime.MinValue;
            previousState = State.Init;
        }

        private State GetCurrentState()
        {
            var now = DateTime.Now;
            if (timesUpTime > now)
            {
                return State.Running;
            }
            TimeSpan stoppedDelay = TimeSpan.FromSeconds(12);
            var pasedTime = now - timesUpTime;
            if (pasedTime < stoppedDelay)
            {
                return State.Stopped;
            }
            return State.Init;
        }

        private string GetDisplayTimeStr()
        {
            var leftTime = timesUpTime - DateTime.Now;
            var totalSeconds = leftTime.TotalSeconds;
            var minutes = (int)Math.Floor(totalSeconds / 60);
            var seconds = (int)Math.Floor(totalSeconds - minutes * 60);
            if (minutes > 0)
            {
                return String.Format("{0,4}", minutes) + ":" + String.Format("{0,2}", seconds);
            }
            if (seconds > 1)
            {
                return string.Format("{0,2}", seconds) + " Seconds";
            }
            return " 1 Second";
        }
        public string Display()
        {
            switch (this.GetCurrentState())
            {
                case State.Running:
                    previousState = State.Running;
                    return GetDisplayTimeStr();
                case State.Stopped:
                    if (previousState == State.Running)
                    {
                        PlayTimesUpNotification();
                    }
                    this.previousState = State.Stopped;
                    
                    return "Time's Up!";
                default:
                    return "";
            }
        }

        public void SetTimer(int minutes)
        {
            timesUpTime = DateTime.Now + TimeSpan.FromMinutes(minutes);
        }


        public void ClearTimer()
        {
            timesUpTime = DateTime.MinValue;
        }

        private void PlayTimesUpNotification()
        {
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer();
            sp.SoundLocation = @"./Alarm01.wav";
            sp.Play();
        }
    }

    public class DisplayController
    {
        List<Label> digitsDisplay = new List<Label>();

        public DisplayController()
        {

        }

        public void AddDigit(Label label)
        {
            this.digitsDisplay.Add(label);
        }

        public void Show(string content)
        {
            var filledLen = digitsDisplay.Count <= content.Length ? digitsDisplay.Count : content.Length;
            for (int i = 0; i < filledLen; i++)
            {
                this.digitsDisplay[i].Content = content[i];
            }
            for (int i = filledLen; i < digitsDisplay.Count; i++)
            {
                this.digitsDisplay[i].Content = "";
            }
        }
    }
}
