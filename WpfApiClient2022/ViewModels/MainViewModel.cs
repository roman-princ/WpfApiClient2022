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
using static WpfApiClient2022.Models.ActorsJson;
using static WpfApiClient2022.Models.MoviesJson;

namespace WpfApiClient2022.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("https://localhost:7123/api/");
        private HttpClient _client;

        private string _response;
        private IEnumerable<object> _resObj;
        private ObservableCollection<object> _result = new ObservableCollection<object>();
        

        public MainViewModel()
        {
            _client = new HttpClient();
            Response = "";
            _client.BaseAddress = ApiUri;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _client.Timeout = TimeSpan.FromSeconds(30);
            ReloadActorsCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("actors");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<ActorObject>>(Response);
                        Result = new ObservableCollection<object>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        Result.Clear();
                    }
                }
                );

            ReloadMoviesCommand = new RelayCommand(
                async () =>
                {                   
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("movies");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<MovieObject>>(Response);
                        Result = new ObservableCollection<object>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        Result.Clear();
                    }
                }
                );
        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public ObservableCollection<object> Result { get { return _result; } set { _result = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadActorsCommand { get; set; }
        public RelayCommand ReloadMoviesCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
