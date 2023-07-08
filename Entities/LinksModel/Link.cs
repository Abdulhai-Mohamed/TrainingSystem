using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinksModel
{
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public Link()
        {
            
        }
        public Link(string _href,string _rel, string _method)
        {
            Href = _href;
            Rel = _rel;
            Method = _method;

        }

    }
}
