using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGFN.Boj4Nextop
{
    public class Baekjoon
    {
        private List<string> _titles = new List<string>();
        private List<List<string>> _contents = new List<List<string>>();
        private List<string> _extra = new List<string>();

        public ReadType Read { get; private set; } = ReadType.Unknown;
        public string BigTitle { get; set; } = string.Empty;
        public string BigTitleCommand { get; set; } = "#";

        public IReadOnlyList<string> Titles => _titles;
        public IReadOnlyList<IReadOnlyList<string>> Contents => _contents;
        public IReadOnlyList<string> Extra => _extra;

        public bool IsEmpty => Read == ReadType.Unknown && _titles.Count == 0 && _contents.Count == 0 && _extra.Count == 0;

        public Baekjoon()
        {

        }

        public void AddTitles(ICollection<string> titles)
        {
            foreach (var title in titles)
            {
                if (string.IsNullOrWhiteSpace(title)) continue;
                _titles.Add(title);
            }
        }

        public void AddContent(ICollection<string> content)
        {
            List<string> contentList = new List<string>();

            foreach (var item in content)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                contentList.Add(item);
            }
            _contents.Add(contentList);
        }

        public void AddExtra(string extra)
        {
            _extra.Add(extra);
        }
    }
}
