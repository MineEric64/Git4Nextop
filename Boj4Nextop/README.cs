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
        public List<README> Children { get; set; } = new List<README>();

        public README()
        {

        }

        public README(string name)
        {
            Name = name;
        }
    }
}
