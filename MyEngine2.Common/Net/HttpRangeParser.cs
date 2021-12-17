namespace MyEngine2.Common.Net
{
    public class HttpRangeParser
    {
        public LinkedList<HttpRange> HttpRanges { get; set; }

        public HttpRangeParser(string rawString)
        {
            HttpRanges = new();
            var rs = rawString[6..].Split(',');
            int index = 0;
            foreach (string i in rs)
            {
                if (i[0].Equals('-'))
                {
                    int end = int.Parse(i[(index + 1)..]);
                    HttpRanges.AddLast(new HttpRange(-1, end));
                }
                else
                {
                    var pair = i.Split('-');
                    if (pair[1].Equals(""))
                    {
                        int start = int.Parse(pair[0]);
                        HttpRanges.AddLast(new HttpRange(start, -1));
                    }
                    else
                    {
                        int start = int.Parse(pair[0]);
                        int end = int.Parse(pair[1]);
                        HttpRanges.AddLast(new HttpRange(start, end));
                    }
                }
            }
        }
    }
}