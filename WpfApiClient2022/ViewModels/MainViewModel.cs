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
        private Uri ApiUri = new Uri("http://localhost:5000/api/");
        private HttpClient _client;

        private string _response;
        private ObservableCollection<object> _actormovies;
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

            ActorMoviesCommand = new ParametrizedRelayCommand<Guid>(
                async (ActorId) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.GetAsync("actors/" + ActorId + "/movies");
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        Response = "No movies";
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        Response = await response.Content.ReadAsStringAsync();
                        _resObj = JsonConvert.DeserializeObject<IEnumerable<MovieObject>>(Response);
                        ActorMovies = new ObservableCollection<object>(_resObj);
                    }
                    else
                    {
                        Response = "OOPS";
                        ActorMovies.Clear();
                    }
                    MoviesWindow m_window = new MoviesWindow();
                    m_window.Show();

                }
                );
            DeleteActorCommand = new ParametrizedRelayCommand<Guid>
                (
                async (ActorId) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.DeleteAsync("actors/" + ActorId);
                    if (response.IsSuccessStatusCode)
                    {
                        ReloadActorsCommand.Execute(null);
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
        public ObservableCollection<object> ActorMovies { get { return _actormovies; } set { _actormovies = value; NotifyPropertyChanged(); } } 

        public RelayCommand ReloadActorsCommand { get; set; }
        public RelayCommand ReloadMoviesCommand { get; set; }
        public ParametrizedRelayCommand<Guid> ActorMoviesCommand { get; set; }
        public ParametrizedRelayCommand<Guid> DeleteActorCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
