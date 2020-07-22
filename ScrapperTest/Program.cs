using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Net;
using System.Diagnostics;
using Cloudmersive.APIClient.NET.NLP.Api;
using Cloudmersive.APIClient.NET.NLP.Client;
using Cloudmersive.APIClient.NET.NLP.Model;
using System.Net.Http;

namespace ScrapperTest
{
    class Program
    {
        static void Main()
        {
            //Source to be parsed
            var CNNSource = "https://www.cnn.com/2020/07/14/politics/trump-schools-reopening/index.html";

            //Source to be parsed
            var CNNSourceTwo = "https://www.cnn.com/2020/07/16/politics/supreme-court-florida-felons/index.html";

            //Source to be parsed
            var FoxSource = "https://www.foxnews.com/us/chicago-shooting-several-hospitalized";

            //Source to be parsed
            var FoxSourceTwo = "https://www.foxnews.com/world/chinese-hackers-charged-steal-usa-coronavirus-research";

            string result = null;

            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = client.GetAsync(CNNSource).Result)
            {
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync().Result;
                }
            }

             var parser = new HtmlParser();

             var document = parser.ParseDocument(result);

             string concat = null;

             if (document.Title.Contains("CNN"))
             {
                 var query = document.GetElementsByClassName("zn-body__paragraph");

                 foreach (var node in query)
                 {
                     // remove HTML
                     foreach (var element in node.QuerySelectorAll("cite, a, strong, em, h1, h2, h3, h4, h5, h6, i"))
                     {
                         if (element.HasTextNodes())
                         {
                             element.OuterHtml = element.TextContent;
                         }
                         else
                         {
                             element.Remove();
                         }
                     }

                     var decodedHtml = WebUtility.HtmlDecode(node.InnerHtml);

                     // do something with node.InnerHtml
                     if (query.Index(node) == 0)
                     {
                         concat = decodedHtml;
                     }
                     else
                     {
                         concat = concat + " " + decodedHtml;
                     }
                 }
             }
             else if (document.Title.Contains("Fox"))
             {
                 var query = document.GetElementsByTagName("p");

                 foreach (var node in query)
                 {
                     if (query.Index(node) == 0 || query.Index(node) == query.Length - 4 || query.Index(node) == query.Length - 3 || query.Index(node) == query.Length - 2 || query.Index(node) == query.Length - 1)
                     {
                         node.Remove();
                     }
                     else
                     {
                         // remove HTML
                         foreach (var element in node.QuerySelectorAll("cite, a, strong, em, h1, h2, h3, h4, h5, h6, i"))
                         {
                             if (element.HasTextNodes())
                             {
                                 element.OuterHtml = element.TextContent;
                             }
                             else
                             {
                                 element.Remove();
                             }
                         }

                         var decodedHtml = WebUtility.HtmlDecode(node.InnerHtml);

                         // do something with node.InnerHtml
                         if (query.Index(node) == 0)
                         {
                             concat = decodedHtml;
                         }
                         else
                         {
                             concat = concat + " " + decodedHtml;
                         }
                     }
                 }
             }
             concat = concat.Replace('"', ' ');

             Console.WriteLine("Output of query:");
             
             CloudmersiveSubjectivityAnalysis(concat);
             //CloudmersiveSentimentAnalysis(concat);
        }

        private static void CloudmersiveSubjectivityAnalysis(string criteria)
        {
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "YOUR_API_KEY"); // Get an API key at, https://account.cloudmersive.com/

            var apiInstance = new AnalyticsApi();
            var input = new SubjectivityAnalysisRequest(criteria); // SentimentAnalysisRequest | Input sentiment analysis request

            try
            {
                // Perform Sentiment Analysis and Classification on Text
                SubjectivityAnalysisResponse result = apiInstance.AnalyticsSubjectivity(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalyticsApi.AnalyticsSentiment: " + e.Message);
            }
        }

        private static void CloudmersiveSentimentAnalysis(string criteria)
        {
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "YOUR_API_KEY"); // Get an API key at, https://account.cloudmersive.com/

            var apiInstance = new AnalyticsApi();
            var input = new SentimentAnalysisRequest(criteria); // SentimentAnalysisRequest | Input sentiment analysis request

            try
            {
                // Perform Sentiment Analysis and Classification on Text
                SentimentAnalysisResponse result = apiInstance.AnalyticsSentiment(input);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling AnalyticsApi.AnalyticsSentiment: " + e.Message);
            }
        }
    }
}