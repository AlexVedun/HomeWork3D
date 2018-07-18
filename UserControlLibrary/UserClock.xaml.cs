using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserClock : UserControl
    {
        private DateTime date, dateUtc;
        private TimeZoneInfo selectZone;

        public UserClock()
        {
            InitializeComponent();
            foreach (var item in TimeZoneInfo.GetSystemTimeZones())
            {
                timeZonesList.Items.Add(item.DisplayName);
            }
            timeZonesList.SelectedIndex = timeZonesList.Items.IndexOf(TimeZoneInfo.Local.DisplayName);
            timeZonesList.ScrollIntoView(timeZonesList.SelectedItem);
        }

        private void ChangeTimeZone(int _index)
        {
            selectZone = TimeZoneInfo.GetSystemTimeZones()[_index];
            dateUtc = DateTime.UtcNow;
            date = TimeZoneInfo.ConvertTimeFromUtc(dateUtc, selectZone);
        }

        public void Start()
        {
            double seconds = date.Second;
            double minutes = date.Minute;
            double hours = date.Hour;
            if (hours > 11.0)
            {
                hours = hours - 12.0;
            }
            double minutesPlusSeconds = minutes + seconds / 60.0;
            double hoursPlusMinutes = hours + minutes / 60.0;

            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = seconds * 6;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(0, 0, 60 - (int)seconds);
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Completed += OnSecondsCompleted;
            ((RotateTransform)((TransformGroup)secondArrow.RenderTransform).Children[1]).Angle = seconds * 6;    

            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = minutesPlusSeconds * 6;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(0, 60 - (int)minutes, 60 - (int)seconds);
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Completed += OnMinutesCompleted;
            ((RotateTransform)((TransformGroup)minuteArrow.RenderTransform).Children[1]).Angle = minutesPlusSeconds * 6;

            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = hoursPlusMinutes * 30;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(12 - (int)hours, 60 - (int)minutes, 60 - (int)seconds);
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Completed += OnHoursCompleted;
            ((RotateTransform)((TransformGroup)hoursArrow.RenderTransform).Children[1]).Angle = hoursPlusMinutes * 30;

            ((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Begin();
            ((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Begin();
            ((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Begin();
        }

        private void OnSecondsCompleted(object sender, EventArgs e)
        {

            //((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).RepeatBehavior = RepeatBehavior.Forever;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = 0;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(0, 1, 0);
            secondArrow.BeginStoryboard(((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard);
        }

        private void OnMinutesCompleted(object sender, EventArgs e)
        {
            //((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).RepeatBehavior = RepeatBehavior.Forever;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = 0;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(1, 0, 0);
            minuteArrow.BeginStoryboard(((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard);
        }

        private void OnHoursCompleted(object sender, EventArgs e)
        {
            //((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).RepeatBehavior = RepeatBehavior.Forever;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).From = 0;
            ((DoubleAnimation)((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Children[0]).Duration = new TimeSpan(12, 0, 0);
            hoursArrow.BeginStoryboard(((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard);
        }

        private void timeZonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((BeginStoryboard)((EventTrigger)secondArrow.Triggers[0]).Actions[0]).Storyboard.Stop();
            ((BeginStoryboard)((EventTrigger)minuteArrow.Triggers[0]).Actions[0]).Storyboard.Stop();
            ((BeginStoryboard)((EventTrigger)hoursArrow.Triggers[0]).Actions[0]).Storyboard.Stop();
            ChangeTimeZone(timeZonesList.SelectedIndex);
            Start();
        }
    }
}
