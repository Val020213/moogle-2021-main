namespace MoogleEngine
{
    public class Query
    {
        string User_query;
        string[] Words;
        int Length;
        List<List<string>> Near_operator;
        Dictionary<string, bool> Mustbe_operator;
        Dictionary<string, bool> Mustnotbe_operator;
        Dictionary<string, double> Important_operator;

        public string get_User_query { get { return User_query; } }
        public string[] get_Words { get { return Words; } }
        public int get_Length { get { return Length; } }

        public List<List<string>> get_Near_operator { get { return Near_operator; } }
        public Dictionary<string, bool> get_Mustbe_operator { get { return Mustbe_operator; } }
        public Dictionary<string, bool> get_Mustnotbe_operator { get { return Mustnotbe_operator; } }
        public Dictionary<string, double> get_Important_operator { get { return Important_operator; } }

        public bool Is_important() => (this.Important_operator.Count == 0);
        public bool Is_near() => (this.Near_operator.Count == 0);

        public Query(string search)
        {
            search = search.ToLower();

            this.User_query = search;
            this.Words = StringMethods.Normalize_Text(search);
            this.Length = this.Words.Length;

            //operadores
            this.Near_operator = new List<List<string>>();
            this.Mustbe_operator = new Dictionary<string, bool>();
            this.Mustnotbe_operator = new Dictionary<string, bool>();
            this.Important_operator = new Dictionary<string, double>();
            
            check_operators(search);
        }

        string get_previousWord(string s, int pos)
        {
            string temp = "";
            while (pos >= 0 && s[pos] != ' ' && s[pos] != '~')
            {
                temp += s[pos].ToString();
                pos--;
            }

            return StringMethods.Remove_rest(StringMethods.Reverse(temp));
        }
        (string, int) get_nextWord(string s, int pos)
        {
            string temp = "";
            while (pos < s.Length && s[pos] != ' ')
            {
                temp += s[pos].ToString();
                pos++;
            }
            return (StringMethods.Remove_rest(temp), pos);
        }
        (List<string>, int) Nearby_ring(string s, int pos)
        {
            List<string> words = new List<string>();
            words.Add(get_previousWord(s, pos - 1));
            pos++;

            while (pos < s.Length && s[pos] != ' ')
            {
                if (s[pos] == '~') words.Add(get_previousWord(s, pos - 1));
                pos++;
            }
            words.Add(get_previousWord(s, pos - 1));
            return (words, pos);
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

                    (string, int) temp = get_nextWord(query, i);
                    Important_operator.Add(temp.Item1, score);
                    i = temp.Item2;
                    continue;
                }

                else if (query[i] == '!')
                {
                    (string, int) temp = get_nextWord(query, i + 1);
                    Mustnotbe_operator.Add(temp.Item1, true);
                    i = temp.Item2;
                    continue;
                }

                else if (query[i] == '^')
                {
                    (string, int) temp = get_nextWord(query, i + 1);
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