using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models
{
    public class Walk
    {
        public DateTime Date { get; set; }
        public Owner Client { get; set; }
        public int Duration { get; set; }
    }
}
