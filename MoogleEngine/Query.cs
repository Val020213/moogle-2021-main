namespace MoogleEngine
{
    public class Query
    {
        public string User_query { get; private set; }
        public string[] Words { get; private set; }
        public int Length { get; private set; }
        public List<List<string>> Near_operator { get; private set; }
        public Dictionary<string, bool> Mustbe_operator { get; private set; }
        public Dictionary<string, bool> Mustnotbe_operator { get; private set; }
        public Dictionary<string, double> Important_operator { get; private set; }
        public Query(string search)
        {
            search = search.ToLower();

            this.User_query = search;
            this.Words = StringMethods.Normalize_Text(search).Item1;
            this.Length = this.Words.Length;

            //operadores
            this.Near_operator = new List<List<string>>();
            this.Mustbe_operator = new Dictionary<string, bool>();
            this.Mustnotbe_operator = new Dictionary<string, bool>();
            this.Important_operator = new Dictionary<string, double>();

            check_operators(search);
        }

       
        (List<string>, int) Nearby_ring(string s, int pos)
        {
            List<string> words = new List<string>();

            (bool,int) flag = (true,pos);

            while (pos < s.Length && flag.Item1)
            {
                if (s[pos] == '~')
                {
                    words.Add(StringMethods.Previous_Word(s, StringMethods.Search_Index_PreviousWord(s, pos)));
                    pos = StringMethods.Search_Index_NextWord(s, pos);
                }
                
                pos++;

                if (pos< s.Length && s[pos] == ' ')
                {
                    flag = StringMethods.Is_concatenation(s,pos);
                    pos = flag.Item2;
                } 
            }

            words.Add(StringMethods.Previous_Word(s, StringMethods.Search_Index_PreviousWord(s, pos)));
            return (words, pos - 1);
        }
        
        void check_operators(string query)
        {
            for (int i = 0; i < query.Length; i++)
            {
                if (query[i] == '*')
                {
                    double score = 0;
                    while (query[i] == '*')
                    {
                        score += 0.25;
                        i++;
                    }

                    (string, int) temp = StringMethods.Next_Word(query, i);
                    Important_operator.Add(temp.Item1, score);
                    i = temp.Item2;
                    continue;
                }

                else if (query[i] == '!')
                {
                    (string, int) temp = StringMethods.Next_Word(query, i + 1);
                    Mustnotbe_operator.Add(temp.Item1, true);
                    i = temp.Item2;
                    continue;
                }

                else if (query[i] == '^')
                {
                    (string, int) temp = StringMethods.Next_Word(query, i + 1);
                    Mustbe_operator.Add(temp.Item1, true);
                    i = temp.Item2;
                    continue;
                }

                else if (query[i] == '~')
                {
                    (List<string>, int) temp = Nearby_ring(query, i);
                    Near_operator.Add(temp.Item1);
                    i = temp.Item2;
                    continue;
                }
            }
        }

    }
}