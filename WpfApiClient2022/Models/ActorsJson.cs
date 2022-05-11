using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApiClient2022.Models
{
    internal class ActorsJson
    {
        public class ActorObject
        {
            public Guid actorId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public int age { get; set; }
            public ICollection<Actor> Data { get; set; }
        }
        
    }
}
