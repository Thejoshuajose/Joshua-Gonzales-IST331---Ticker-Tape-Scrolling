using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Joshua_Gonzales___IST331___Ticker_Tape_Scrolling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This code is meant to act as a stock ticker for a predeterined list of stocks with prices.
    /// </summary>
    public partial class MainWindow : Window
    {
        List<String> stockList = new List<String>();
        List<object> stockTickerList = new List<object>();
        DispatcherTimer testTimer = new DispatcherTimer();
        private int currentPosition = 0;
        private string stockInfo = "";


        public MainWindow()
        {
            InitializeComponent();
            InitializeStocks();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            testTimer.Tick += new EventHandler(TestTimer_Tick);
            testTimer.Interval = TimeSpan.FromMilliseconds(sldTickerSpeed.Value);
        }
        private void InitializeStocks()
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\joshu\\Desktop\\VisStudioCode\\Joshua Gonzales ~ IST331 - Ticker Tape Scrolling\\StockList.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    stockList.Add(line);
                    line = sr.ReadLine();
                }
                lstStocksToChoose.ItemsSource = (stockList);
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing final Stock.");
            }
            always_Running();


        }
        private void always_Running()
        {
            if (lstStocksChosen.Items.IsEmpty)
            {
                btnRemove.IsEnabled = false;
                btnStart.IsEnabled = false;
            }
            else
            {
                btnStart.IsEnabled = true;
            }
            if (lstStocksToChoose.Items.IsEmpty)
            {
                btnAdd.IsEnabled = false;
            }
            lblTickerSpeed.Content = (1200 - sldTickerSpeed.Value).ToString();
        }
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = true;
            btnStart.IsEnabled = false;
            btnAdd.IsEnabled = false;
            btnRemove.IsEnabled = false;
            btnStop.Visibility = Visibility.Visible;
            btnStart.Visibility = Visibility.Hidden;
            always_Running();
            stockTickerBody();
            testTimer.Start();
            stockInfo = "";


        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
            btnAdd.IsEnabled = true;
            btnRemove.IsEnabled = true;
            btnStop.Visibility = Visibility.Hidden;
            btnStart.Visibility = Visibility.Visible;
            always_Running();
            testTimer.Stop();
            stockInfo = "";
            stockTickerList.Clear();



        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            InitializeStocks();
            always_Running();

            btnAdd.Visibility = Visibility.Hidden;
            btnRemove.Visibility = Visibility.Hidden;
            btnStop.Visibility = Visibility.Hidden;
            btnStart.Visibility = Visibility.Visible;
            lstStocksChosen.Items.Clear();
            currentPosition = 0;
            txtScrolling.Clear();
            testTimer.Stop();
            stockTickerList.Clear();
            btnStart.IsEnabled = false;
            stockInfo = "";
            stockTickerList.Clear();



        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            stockInfo = "";

            if (!lstStocksChosen.Items.Contains(lstStocksToChoose.SelectedItem))
            {
                lstStocksChosen.Items.Add(lstStocksToChoose.SelectedItem);
            }
            else if (lstStocksChosen.Items.Contains(lstStocksToChoose.SelectedItem))
            {
                MessageBox.Show("Item has already been added...");
            }



        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstStocksChosen.SelectedItem != null)
            {
                lstStocksChosen.Items.Remove(lstStocksChosen.SelectedItem);
                stockTickerList.Remove(lstStocksChosen.SelectedItem);
                txtScrolling.Clear();
                stockTickerBody();

            }
            always_Running();
            stockInfo = "";
            stockTickerList.Clear();



        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you Sure?", "Do you want to Exit?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void sldTickerSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (WinTickerTape.IsInitialized)
            {
                always_Running();
                testTimer.Interval = TimeSpan.FromMilliseconds(sldTickerSpeed.Value);
            }
        }

        private void lstStocksToChoose_GotFocus(object sender, RoutedEventArgs e)
        {
            btnAdd.Visibility = Visibility.Visible;
            btnRemove.IsEnabled = false;
            btnAdd.IsEnabled = true;
            btnRemove.Visibility = Visibility.Hidden;
            always_Running();


        }

        private void lstStocksChosen_GotFocus(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = false;
            btnAdd.Visibility = Visibility.Hidden;
            btnRemove.IsEnabled = true;
            btnRemove.Visibility = Visibility.Visible;
            always_Running();

        }

        private void stockTickerBody()
        {
            for (int i = 0; i < lstStocksChosen.Items.Count; i++)
            {
                stockTickerList.Add(" | " + lstStocksChosen.Items.GetItemAt(i) + " | ");

            }
        }

        private void TestTimer_Tick(object sender, EventArgs e)
        {
            always_Running();
            if (stockTickerList.Count <= 2)
            {
                txtScrolling.Text = string.Join("", stockTickerList); // Display all stock items without scrolling
                return;
            }
            StringBuilder builder = new StringBuilder();
            
                for (int i = 0; i < stockTickerList.Count; i++)
                {
                    string stockItem = stockTickerList[i].ToString();
                    if (currentPosition >= stockItem.Length)
                        currentPosition = 0;

                    // Append one character at a time
                    for (int j = 0; j < stockItem.Length; j++)
                    {
                        char currentChar = stockItem[(currentPosition + j) % stockItem.Length];
                        builder.Append(currentChar);
                    }


                }

                currentPosition++;

                txtScrolling.Text = builder.ToString();
            }
        
        /*private void TestTimer_Tick(object sender, EventArgs e)
        {
            always_Running();


            for (int i = 0; i < stockTickerList.Count; i++)

            {
                stockInfo += " | " + stockTickerList[i] + " | ";
            }
            if(stockTickerList.Count > 2)
            { 

                currentPosition++;
                if (currentPosition >= stockInfo.Length)
                    currentPosition = 0;

                stockInfo = stockInfo.Substring(currentPosition) + stockInfo.Substring(0, currentPosition); 
            }
            txtScrolling.Text = stockInfo;

        }*/
    }
}