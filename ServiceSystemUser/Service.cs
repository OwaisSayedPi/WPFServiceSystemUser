using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSystemUser
{
    public class Service
    {
        public int BranchID { get; set; }
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int CounterID { get; set; }
        public List<Question> Question { get; set; }
        List<Answer> Answers = new List<Answer>();

        public OutResponse<List<Question>> question = new OutResponse<List<Question>>();

        public OutResponse<string> response = new OutResponse<string>();
        public void DisplayQuestions()
        {
            using (HttpClient client = new HttpClient())
            {
                question = client.GetFromJsonAsync<OutResponse<List<Question>>>($"https://localhost:44391/api/Question?serviceID={ServiceID}").Result;
                Answer answer;
                foreach (var item in question.ResData)
                {
                    answer = new Answer();
                    Console.WriteLine(item.QuestionString);
                    answer.Answers = Console.ReadLine();
                    answer.QuestionID = item.QuestionID;
                    answer.AnswerID = item.QuestionID;
                    Answers.Add(answer);
                }
                OutResponse<List<Token>> T = client.PostAsJsonAsync($"https://localhost:44391/api/Answer?serviceID={ServiceID}&branchID={BranchID}", Answers).Result.Content.ReadFromJsonAsync<OutResponse<List<Token>>>().Result;
                Token token = new Token();
                foreach (var item in T.ResData)
                {
                    token.TokenID = item.TokenID;
                    token.CounterID = item.CounterID;
                }
                Console.WriteLine($"Your Token :  {token.TokenID}\nPlease Proceed to Counter: {token.CounterID}");
                Console.ReadLine();
            }
        }
    }
}
