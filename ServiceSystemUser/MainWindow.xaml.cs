using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceSystemUser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            int Branch = GetBranch();
            DisplayWelcomeMsg(Branch);
        }

        static int BranchID;
        public int GetBranch()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/branch.txt";
            string val = "";
            if (File.Exists(path))
            {
                val = File.ReadAllText(path);
            }
            if (val != null || val != "")
            {
                string[] temp = val.Split('=');
                BranchID = int.Parse(temp[1]);
            }
            return BranchID;
        }
        public void DisplayWelcomeMsg(int BranchID)
        {
            OutResponse<List<ServicesDTO>> res = new OutResponse<List<ServicesDTO>>();
            List<Service> serviceList = new List<Service>();
            string BranchName = "";
            using (HttpClient client = new HttpClient())
            {
                res = client.GetFromJsonAsync<OutResponse<List<ServicesDTO>>>($"https://localhost:44391/api/Service/{BranchID}").Result;

                if (res.ResData.Count == 0)
                {
                    WelcomeMsg.Content = "No Service";
                }
                else
                {
                    foreach (var item in res.ResData)
                    {
                        Service services = new Service
                        {
                            BranchID = BranchID,
                            ServiceID = item.ServiceID,
                            ServiceName = item.ServiceName
                        };
                        serviceList.Add(services);
                        BranchName = item.BranchName;
                    }
                    WelcomeMsg.Content = $"Welcome To {BranchName}";
                    UpdatingServiceStack(serviceList);
                }
            }
        }

        private void UpdatingServiceStack(List<Service> serviceList)
        {
            foreach (var item in serviceList)
            {
                AddServiceButton(item.ServiceName, item.ServiceID);
            }
        }
        public void AddServiceButton(string ServiceName, int ServiceID)
        {
            Button button = new Button();
            button.Name = $"ServiceName{ServiceID}";
            button.Height = 50;
            button.Width = 340;
            button.Content = ServiceName;
            button.FontSize = 20;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.FontWeight = FontWeights.Bold;
            button.Background = new SolidColorBrush(Colors.BlanchedAlmond);
            button.BorderBrush = new SolidColorBrush(Colors.BurlyWood);
            button.BorderThickness = new Thickness(2);
            button.Margin = new Thickness(5);
            button.Click += (ss, ee) => {
                ServiceWindow service = new ServiceWindow();
                service.Show();
                this.Close();
                service.DisplayQuestions(ServiceID,BranchID);
            };

            DynamicServices.Children.Add(button);
            DynamicServices.Height += 50;
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
