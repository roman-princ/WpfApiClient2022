using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApiClient2022.Models
{
    internal class MoviesJson
    {
        public class MovieObject
        {
            public Guid movieId { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            
        } 
    }
}
