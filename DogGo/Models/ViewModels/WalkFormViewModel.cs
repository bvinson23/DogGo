using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Models.ViewModels
{
    public class WalkFormViewModel
    {
        public Walks Walk { get; set; }
        public List<Dog> Dogs { get; set; }
    }
}
