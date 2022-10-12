using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGFN.Boj4Nextop
{
    public class BojManager
    {
        public static README ReadMarkdown(string raw)
        {
            README readme = new README();
            Baekjoon boj = new Baekjoon();
            string[] lines = raw.Split(new string[1] { "\r\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.StartsWith("#")) //Big Title
                {
                    if (!boj.IsEmpty)
                    {
                        readme.Contents.Add(boj);
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
                else if (line.StartsWith("|")) //Title & Contents
                {
                    if (boj.Titles.Count == 0) //Title
                    {
                        string[] titles = line.Split('|');
                        boj.AddTitles(titles);
                    }
                    else //Contents
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
            readme.Contents.Add(boj);

            return readme;
        }
    }
}
