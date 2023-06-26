using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ServiceSystemUser
{
    /// <summary>
    /// Interaction logic for ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        public ServiceWindow()
        {
            InitializeComponent();
        }
        static int SID;
        static int BID;
        static int count = 0;
        internal void DisplayQuestions(int serviceID, int branchID)
        {
            SID = serviceID;
            BID = branchID;
            OutResponse<List<Question>> res = new OutResponse<List<Question>>();
            List<Question> question = new List<Question>();
            using (HttpClient client = new HttpClient())
            {
                res = client.GetFromJsonAsync<OutResponse<List<Question>>>($"https://localhost:44391/api/Question?serviceID={serviceID}").Result;
                
                foreach (var item in res.ResData)
                {
                    StackPanel QuestionPanel = new StackPanel();
                    Label QuestionLabel = new Label();
                    TextBox AnswerText = new TextBox();
                    
                    QuestionPanel = AddQuestionStackPanel(item.QuestionID);
                    QuestionLabel = AddQuestionLabel(item.QuestionString, item.QuestionID);
                    AnswerText = AddAnswerTextBox(item.QuestionID);

                    QuestionPanel.Children.Add(QuestionLabel);
                    QuestionPanel.Children.Add(AnswerText);

                    DynamicQuestions.Children.Add(QuestionPanel);
                    DynamicQuestions.Height += 30;
                    count++;
                }
            }
        }

        private TextBox AddAnswerTextBox(int questionID)
        {
            TextBox textBox = new TextBox();
            textBox.Height = 40;
            textBox.Width = 300;
            textBox.Name = $"Answer_{questionID}";
            textBox.BorderThickness = new Thickness(2);
            textBox.FontSize = 25;
            textBox.BorderBrush = new SolidColorBrush(Colors.Blue);
            textBox.TextWrapping = TextWrapping.Wrap;
            textBox.AcceptsReturn = true;
            return textBox;
        }

        private Label AddQuestionLabel(string questionString, int questionID)
        {
            Label QuestionLabel = new Label();
            QuestionLabel.Content = questionString;
            QuestionLabel.Name = $"Question{questionID}";
            QuestionLabel.Height = 40;
            QuestionLabel.Width = 400;
            QuestionLabel.HorizontalAlignment = HorizontalAlignment.Left;
            QuestionLabel.Margin = new Thickness(0, 0, 10, 0);
            QuestionLabel.FontSize = 25;
            return QuestionLabel;
        }

        private StackPanel AddQuestionStackPanel(int questionID)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Name = $"QuestionPanel{questionID}";
            stackPanel.Margin = new Thickness(0, 20, 0, 20);
            return stackPanel;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            List<Answer> answers = new List<Answer>();
            Answer answer = new Answer();
            //foreach (var item in DynamicQuestions.Children)
            //{
                
            //    answer.Answers = item.ToString().Trim();
            //    answers.Add(answer);
            //}
            for (int i = 0; i < count; i++)
            {
                StackPanel stack = (StackPanel)DynamicQuestions.Children[i];
                TextBox text = (TextBox)stack.Children[1];
                answer.Answers = text.Text.Trim();
                answer.AnswerID = i+1;
                int.TryParse(text.Name.Split('_')[1], out int result);
                answer.QuestionID = result;
                answers.Add(answer);
            }
            OutResponse<List<Token>> T = new OutResponse<List<Token>>();

            using (HttpClient client = new HttpClient())
            {
                T = client.PostAsJsonAsync($"https://localhost:44391/api/Answer?serviceID={SID}&branchID={BID}", answers).Result.Content.ReadFromJsonAsync<OutResponse<List<Token>>>().Result;
            }
            Token token = new Token();
            foreach (var item in T.ResData)
            {
                token.TokenID = item.TokenID;
                token.CounterID = item.CounterID;
            }

            TokenDisplayWindow TokenDisplay = new TokenDisplayWindow();
            TokenDisplay.Show();
            TokenDisplay.UpdateFields(token.TokenID, token.CounterID);
            this.Close();
        }        
    }
}
