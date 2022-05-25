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
using System.Windows;
using WpfApiClient2022.Models;
using static WpfApiClient2022.Models.ActorsJson;
using static WpfApiClient2022.Models.MoviesJson;

namespace WpfApiClient2022.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private Uri ApiUri = new Uri("http://localhost:5000/api/");
        private HttpClient _client;
        private int _actorage;
        private string _response;
        private string _currentactor;
        private string _currentmovie;
        public string ActorFirstName { get; set; }

        public string ActorLastName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string CurrentActor { get { return _currentactor; } set { _currentactor = value; NotifyPropertyChanged(); } }
        public string CurrentMovie { get { return _currentmovie; } set { _currentmovie = value; NotifyPropertyChanged(); } }


        private ObservableCollection<MovieObject> _actormovies;
        private ObservableCollection<ActorObject> _movieactors;

        private IEnumerable<ActorObject> _actorObj;
        private IEnumerable<MovieObject> _movieObj;

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
            _actormovies = new ObservableCollection<MovieObject>();

            ReloadCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await _client.GetAsync("movies");
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            _movieObj = JsonConvert.DeserializeObject<IEnumerable<MovieObject>>(Response);
                            Movies = new ObservableCollection<MovieObject>(_movieObj);
                        }
                        else
                        {
                            Response = "OOPS";
                            Movies.Clear();
                        }

                        response = await _client.GetAsync("actors");
                        if (response.IsSuccessStatusCode)
                        {
                            Response = await response.Content.ReadAsStringAsync();
                            _actorObj = JsonConvert.DeserializeObject<IEnumerable<ActorObject>>(Response);
                            Actors = new ObservableCollection<ActorObject>(_actorObj);
                        }
                        else
                        {
                            Response = "OOPS";
                            Actors.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                        
                    }

                    
                }
                );

            ActorMoviesCommand = new ParametrizedRelayCommand<ActorObject>(
                async (Actor) =>
                {
                    CurrentActor = "";
                    ActorMovies.Clear();
                    HttpResponseMessage response = new HttpResponseMessage();
                    try {
                        response = await _client.GetAsync("actors/" + Actor.actorId + "/movies");
                        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            CurrentActor = $"{Actor.firstName} {Actor.lastName}";
                            Response = "No movies";
                        }
                        else if (response.IsSuccessStatusCode)
                        {
                            CurrentActor = $"{Actor.firstName} {Actor.lastName}";
                            Response = await response.Content.ReadAsStringAsync();
                            _movieObj = JsonConvert.DeserializeObject<IEnumerable<MovieObject>>(Response);
                            ActorMovies = new ObservableCollection<MovieObject>(_movieObj);

                        }
                        else
                        {
                            Response = "OOPS";
                            ActorMovies.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                    }

                }
                );
            MovieActorsCommand = new ParametrizedRelayCommand<MovieObject>(
                async (Movie) =>
                {
                    if (MovieActors != null)
                    {
                        MovieActors.Clear();
                    }
                    CurrentMovie = "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await _client.GetAsync("movies/" + Movie.movieId + "/actors");
                        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            CurrentMovie = Movie.title;
                            Response = "No actors";
                        }
                        else if (response.IsSuccessStatusCode)
                        {
                            CurrentMovie = Movie.title;
                            Response = await response.Content.ReadAsStringAsync();
                            _actorObj = JsonConvert.DeserializeObject<IEnumerable<ActorObject>>(Response);
                            MovieActors = new ObservableCollection<ActorObject>(_actorObj);

                        }
                        else
                        {
                            Response = "OOPS";
                            MovieActors.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                    }




                }
                );
            DeleteActorCommand = new ParametrizedRelayCommand<Guid>
                (
                async (ActorId) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await _client.DeleteAsync("actors/" + ActorId);
                        if (response.IsSuccessStatusCode)
                        {

                            ReloadCommand.Execute(null);
                        }
                        else
                        {
                            Response = "OOPS";

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                    }

                }
                );
            DeleteMovieCommand = new ParametrizedRelayCommand<Guid>
                (
                async (MovieId) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.DeleteAsync("movies/" + MovieId);
                    if (response.IsSuccessStatusCode)
                    {
                        ReloadCommand.Execute(null);
                    }
                    else
                    {
                        Response = "Something went wrong";
                    }
                }
                );
            DeleteMovieFromActor = new ParametrizedRelayCommand<Guid>
                (
                async (MovieId) =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.DeleteAsync("actors/" + CurrentActor + "/movie/" + MovieId);
                    if (response.IsSuccessStatusCode)
                    {
                        ReloadCommand.Execute(null);

                    }
                    else
                    {
                        Response = "Something went wrong";
                    }
                }
                );
            //create new actor command from user input
            CreateActorCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await _client.PostAsync("actors", new StringContent(JsonConvert.SerializeObject(new ActorObject { actorId = Guid.NewGuid(), firstName = ActorFirstName, lastName = ActorLastName, age = ActorAge }), Encoding.UTF8, "application/json"));
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Actor created, click the reload button");
                            ReloadCommand.Execute(null);
                        }
                        else
                        {
                            MessageBox.Show("something went wrong: " + response.StatusCode);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                    }

                }
                );
            CreateMovieCommand = new RelayCommand(
                async () =>
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    try
                    {
                        response = await _client.PostAsync("movies", new StringContent(JsonConvert.SerializeObject(new MovieObject { movieId = Guid.NewGuid(), title = Title, description = Description }), Encoding.UTF8, "application/json"));
                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Movie created, click the reload button");
                            ReloadCommand.Execute(null);
                        }
                        else
                        {
                            MessageBox.Show("something went wrong: " + response.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("API is not avalible at the moment, try again later.");
                    }


                }
                );
            //create edit actor command from user input
            EditActorCommand = new ParametrizedRelayCommand<ActorObject>(
                async (actor) =>
                {


                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.PutAsync("actors/" + actor.actorId, new StringContent(JsonConvert.SerializeObject(actor), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("actor edited");
                        ReloadCommand.Execute(null);

                    }
                    else
                    {
                        MessageBox.Show("something went wrong: " + response.StatusCode);
                    }
                }
                );
            //create edit movie command from user input
            EditMovieCommand = new ParametrizedRelayCommand<MovieObject>(
                async (movie) =>
                {

                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await _client.PutAsync("movies/" + movie.movieId, new StringContent(JsonConvert.SerializeObject(movie), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("movie edited");
                        ReloadCommand.Execute(null);
                    }
                    else
                    {
                        MessageBox.Show("something went wrong: " + response.StatusCode);
                    }
                }
                );



        }

        public string Response { get { return _response; } set { _response = value; NotifyPropertyChanged(); } }
        public int ActorAge { get { return _actorage; } set { _actorage = value; NotifyPropertyChanged(); } }
        public ObservableCollection<ActorObject> Actors { get { return _actors; } set { _actors = value; NotifyPropertyChanged(); } }
        public ObservableCollection<MovieObject> Movies { get { return _movies; } set { _movies = value; NotifyPropertyChanged(); } }
        public ObservableCollection<MovieObject> ActorMovies { get { return _actormovies; } set { _actormovies = value; NotifyPropertyChanged(); } }
        public ObservableCollection<ActorObject> MovieActors { get { return _movieactors; } set { _movieactors = value; NotifyPropertyChanged(); } }

        public RelayCommand ReloadCommand { get; set; }
        public RelayCommand CreateActorCommand { get; set; }
        public RelayCommand CreateMovieCommand { get; set; }
        public ParametrizedRelayCommand<ActorObject> EditActorCommand { get; set; }
        public ParametrizedRelayCommand<MovieObject> EditMovieCommand { get; set; }

        public ParametrizedRelayCommand<ActorObject> GetActorCommand { get; set; }
        public ParametrizedRelayCommand<ActorObject> ActorMoviesCommand { get; set; }
        public ParametrizedRelayCommand<MovieObject> MovieActorsCommand { get; set; }
        public ParametrizedRelayCommand<Guid> DeleteActorCommand { get; set; }
        public ParametrizedRelayCommand<Guid> DeleteMovieCommand { get; set; }
        public ParametrizedRelayCommand<Guid> DeleteMovieFromActor { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
