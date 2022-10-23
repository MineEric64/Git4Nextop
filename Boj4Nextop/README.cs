using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGFN.Boj4Nextop
{
    public class README
    {
        public string Name { get; set; } = string.Empty;
        public List<Baekjoon> Contents { get; set; } = new List<Baekjoon>();
        public README Child { get; set; } = null;

        public bool IsEmpty => string.IsNullOrEmpty(Name) && Contents.Count == 0;

        public README()
        {

        }

        public README(string name)
        {
            Name = name;
        }
    }
}
