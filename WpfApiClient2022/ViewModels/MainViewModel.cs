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
        private IEnumerable<ActorObject> _resObj;
        private IEnumerable<MovieObject> _resObj2;
        private ObservableCollection<ActorObject> _actors = new ObservableCollection<ActorObject>();
        private ObservableCollection<MovieObject> _movies = new ObservableCollection<MovieObject>();

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
                    Movies.Clear();
                    Response = "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("actors");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<ActorObject>>(Response);
                        Actors = new ObservableCollection<ActorObject>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        Actors.Clear();
                    }
                }
                );

            ReloadMoviesCommand = new RelayCommand(
                async () =>
                {
                    Actors.Clear();
                    Response = "";                    
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("movies");
                    if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj2 = JsonConvert.DeserializeObject<IEnumerable<MovieObject>>(Response);                    
                        Movies = new ObservableCollection<MovieObject>(_resObj2);
                    }
                    else
                    {
                        Response = "OOPS";
                        Movies.Clear();
                    }
                }
                );



        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public ObservableCollection<ActorObject> Actors { get { return _actors; } set { _actors = value; NotifyPropertyChanged(); } }
        public ObservableCollection<MovieObject> Movies { get { return _movies; } set { _movies = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadActorsCommand { get; set; }
        public RelayCommand ReloadMoviesCommand { get; set; }
        public ParametrizedRelayCommand<Guid> ActorByIdCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
