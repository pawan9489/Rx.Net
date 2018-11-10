using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static Random r = new Random();
        private IEnumerable<int> randoms(int n)
        {
            if (n < 0)
                throw new Exception("Please provide +ve Values.");
            while (n > 0)
            {
                n--;
                yield return r.Next(1, 1000);
            }
        }

        IDisposable _subscription;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text.Clear();
            generate.IsEnabled = false;
            stop.IsEnabled = true;
            //var numbers = (from number in Enumerable.Range(
            //                int.Parse(start.Text), int.Parse(count.Text))
            //               select slow(number)).ToObservable()
            //               .SubscribeOn(ThreadPoolScheduler.Instance)
            //               .ObserveOnDispatcher()
            //               .Finally(() =>
            //               {
            //                   generate.IsEnabled = true;
            //                   stop.IsEnabled = false;
            //               });

            var numbers = (from n in randoms(10) select n)
                            .ToObservable()
                            .SubscribeOn(ThreadPoolScheduler.Instance)
                            .ObserveOnDispatcher()
                            .Buffer(3)
                            .Finally(() =>
                            {
                                generate.IsEnabled = true;
                                stop.IsEnabled = false;
                            });
            _subscription = numbers.Subscribe(
                n =>
                {
                    StringBuilder str = new StringBuilder();
                    foreach (var i in n)
                        str.Append(string.Format("{0} ", i));
                    text.AppendText(string.Format("{0}\n", str));
                },
                err => text.AppendText("Oops Error Occured\n"),
                () => text.AppendText("Done\n"));
        }

        private int slow(int number)
        {
            Thread.Sleep(1000);
            return number;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _subscription.Dispose();
        }
    }
}
