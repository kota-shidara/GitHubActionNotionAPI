using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

class Notion
{
    static async Task Main(string[] args)
    {
        string url = "https://api.notion.com/v1/pages";
        string NOTION_KEY = Environment.GetEnvironmentVariable("NOTION_KEY");
        string NOTION_DATABASE_ID = Environment.GetEnvironmentVariable("NOTION_DATABASE_ID");

        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {NOTION_KEY}");

        var json_data = new
        {
            parent = new
            {
                database_id = NOTION_DATABASE_ID
            },
            properties = new
            {
                title = new
                {
                    title = new[]
                    {
                        new
                        {
                            text = new
                            {
                                content = $"データ追加{DateTime.Now}"
                            }
                        }
                    }
                }
            }
        };

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(json_data);

        HttpContent httpContent = new StringContent(json);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        HttpResponseMessage response = await httpClient.PostAsync(url, httpContent);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
    }
}
