using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGFN.Boj4Nextop
{
    public class BojManager
    {
        public static List<Baekjoon> ReadMarkdown(string raw)
        {
            List<Baekjoon> bojList = new List<Baekjoon>();
            Baekjoon boj = new Baekjoon();
            string[] lines = raw.Split(new string[1] { "\r\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.StartsWith("#"))
                {
                    if (!boj.IsEmpty)
                    {
                        bojList.Add(boj);
                        boj = new Baekjoon();
                    }

                    string[] bigTitles = line.Split(' ');

                    if (bigTitles.Length >= 2)
                    {
                        boj.BigTitleCommand = bigTitles[0];
                        boj.BigTitle = string.Join(" ", bigTitles.Skip(1));
                    }
                    else
                    {
                        boj.AddExtra(line);
                    }
                }
                else if (line.StartsWith("|"))
                {
                    if (boj.Titles.Count == 0)
                    {
                        string[] titles = line.Split('|');
                        boj.AddTitles(titles);
                    }
                    else
                    {
                        if (line.StartsWith("|:---:|")) continue;

                        string[] content = line.Split('|');
                        boj.AddContent(content);
                    }
                }
                else
                {
                    boj.AddExtra(line);
                }
            }
            bojList.Add(boj);

            return bojList;
        }
    }
}
