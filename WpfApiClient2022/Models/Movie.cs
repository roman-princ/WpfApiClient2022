using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApiClient2022.Models
{
    internal class Movie
    {
        public Guid MovieId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
