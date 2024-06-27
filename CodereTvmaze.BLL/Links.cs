using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodereTvmaze.BLL
{
    public class Links
    {
        /// <summary>
        /// Class <c>Links</c> contains data included in MainInfo object.
        /// </summary>
        public Self? self { get; set; }
        public Episode? previousepisode { get; set; }
        public Episode? nextepisode { get; set; }
    }
}
