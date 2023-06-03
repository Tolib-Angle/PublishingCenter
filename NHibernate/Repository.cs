using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PublishingCenter_v2.NHibernate
{
    public class Repository
    {

        private readonly HttpClient Client = new HttpClient();

        private static Repository _instance = null;


        private Repository()
        {
            //var handler = new WinHttpHandler();
            //Client = new HttpClient(handler);
        }

        public static Repository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Repository();
                }

                return _instance;
            }
        }
        public IList<T> FindByCondition<T>(Query query)
        {
            var body = JsonSerializer.Serialize(query);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5000/api/list"),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };
            var result = Client.SendAsync(request).Result;

            string json = result.Content.ReadAsStringAsync().Result;

            List<T> list = JsonSerializer.Deserialize<List<T>>(json);

            return list;
        }
        public T GetByNumber<T>(int number)
        {
            var str = Client.GetAsync($"http://localhost:5000/api/get_by_number?table={typeof(T).Name}&number={number}").Result.Content.ReadAsStringAsync().Result;
            T result = JsonSerializer.Deserialize<T>(str);
            return result;
        }
        public int GetCount<T>() where T : class
        {
            string str = Client.GetAsync($"http://localhost:5000/api/get_count?table={typeof(T).Name}").Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine(str);
            int count = Convert.ToInt32(str);
            return count;
        }
        public void Create<T>(T entity) where T : Entity
        {
            entity.id = -1;
            Update(entity);
        }

        public void Update<T>(T entity) where T : Entity
        {
            string body = JsonSerializer.Serialize(entity);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://localhost:5000/api/upsert?table={typeof(T).Name}"),
                Content = new StringContent(body, Encoding.UTF8, "application/json"),
            };

            var response = Client.SendAsync(request).Result;
            string str = response.Content.ReadAsStringAsync().Result;
            if(str != "error")
                entity.id = Convert.ToInt32(str);
            Console.WriteLine(str);
        }
        public void Delete<T>(T entity) where T : Entity
        {
            _ = Client.PostAsync($"http://localhost:5000/api/delete?table={typeof(T).Name}&id={entity.id}", null).Result;
        }
        public T Find<T>(int id)
        {
            var json = Client.GetAsync($"http://localhost:5000/api/get?table={typeof(T).Name}&id={id}").Result;
            return JsonSerializer.Deserialize<T>(json.Content.ReadAsStringAsync().Result);
        }

        public void Refresh<T>(T entity) where T : Entity
        {
            entity = Find<T>(entity.id);
        }
    }
}
