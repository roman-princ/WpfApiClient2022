using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApiClient2022.Models;
using static WpfApiClient2022.Models.TestJson;

namespace WpfApiClient2022.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("https://localhost:7123/api/");
        private HttpClient _client;

        private string _response;
        private IEnumerable<Rootobject> _resObj;
        private ObservableCollection<Rootobject> _actors = new ObservableCollection<Rootobject>();

        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            ReloadCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("actors");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response);
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<Rootobject>>(Response);
                        //_resObj = System.Text.Json.JsonSerializer.Deserialize<ResponseIdeas>(Response, new System.Text.Json.JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                        //Ideas = new ObservableCollection<Datum>(_resObj.Select(x => x.Data).AsEnumerable());
                        Actors = new ObservableCollection<Rootobject>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        Actors.Clear();
                    }
                }
                );
        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public ObservableCollection<Rootobject> Actors { get { return _actors; } set { _actors = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
