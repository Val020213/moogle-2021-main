using System;
using System.IO;
namespace MoogleEngine
{
    public class StringMethods //Metodos de normalizar, edit distance, snippet, sugerencias
    {
        public static string[] Normalize_Text(string Text)
        {
            Text = Text.ToLower();

            char[] specials_chars = {
            '\n',' ',',' , '.' , '/' , '?' , '>' , '<' , ';' , ':' , '}' , ']',
            '[' , '{' , '-' , '_' , '=' , '+' , ')' , '(' , '&',
            '%' , '$' , '#' , '@' , '!' , '^' , '`' , (char)92 , (char)39, '~', '*'
            };

            string[] Text_proccesed = Text.Split(specials_chars, System.StringSplitOptions.RemoveEmptyEntries);

            Text_proccesed = Normalize_words(Text_proccesed); //normalizar profundo

            return Text_proccesed;
        }
        private static string[] Normalize_words(string[] words)
        {
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = Remove_rest(words[i]);
            }
            return words;
        }

        public static string Remove_rest(string word)
        {
            string correct = "";
            for (int i = 0; i < word.Length; i++)
            {
                if (char.IsLetterOrDigit(word[i])) correct += word[i].ToString();
            }
            return correct;
        }

        public static string get_previousWord(string s, int pos)
        {
            string temp = "";
            while (pos >= 0 && s[pos] != ' ' && s[pos] != '~')
            {
                temp += s[pos].ToString();
                pos--;
            }
            return StringMethods.Remove_rest(StringMethods.Reverse(temp));
        }

        public static (string, int) get_nextWord(string s, int pos)
        {
            string temp = "";
            while (pos < s.Length && s[pos] != ' ')
            {
                temp += s[pos].ToString();
                pos++;
            }
            return (StringMethods.Remove_rest(temp), pos);
        }
        public static string Reverse(string s)
        {
            string temp = "";
            for (int i = s.Length - 1; i >= 0; i--)
            {
                temp += s[i].ToString();
            }
            return temp;
        }

        public static int editDistance(string word_A, string word_B)
        {

            int m = word_A.Length;
            int n = word_B.Length;

            if (word_A == null || word_B == null) return 100;//caso base

            int[,] DP = new int[m + 2, n + 2];

            for (int i = 0; i <= m; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    if (i == 0) DP[i, j] = j;

                    else if (j == 0) DP[i, j] = i;

                    else if (word_A[i - 1] == word_B[j - 1]) DP[i, j] = DP[i - 1, j - 1];

                    else
                    {
                        DP[i, j] = 1 + Math.Min(DP[i - 1, j], Math.Min(DP[i, j - 1], DP[i - 1, j - 1]));
                    }
                }
            }
            return DP[m, n];
        }

        public static string Search_BestSnippet(Document Doc, Vector Query, int snippet_length)
        {//subarray de snippet maximo
            string[] Text = Doc.Text;
            int li = 0;
            (int, int) limits = (0, 0);
            int best_score = 0;
            int score = 0;
            snippet_length = (snippet_length > Text.Length) ? Text.Length : snippet_length;

            for (int ls = 0; ls < Text.Length; ls++)
            {
                if (Query.Components.ContainsKey(Text[ls])) { score++; }

                if (ls - li >= snippet_length)
                {
                    if (score > best_score)
                    {
                        limits = (li, ls);
                        best_score = score;
                    }

                    if (Query.Components.ContainsKey(Text[li])) score--;
                    li++;
                }
            }

            return makesnip(limits.Item1, limits.Item2, Text);
        }

        static string makesnip(int li, int ls, string[] Text)
        {
            string snipp = "";
            for (int i = li; i <= ls; i++)
            {
                snipp += Text[i] + " ";
            }
            return snipp;
        }

        public static string Search_suggestions(string miss_word, Dictionary<string, int> Allwords)
        {
            int min_diference = (miss_word.Length / 2 < 4) ? miss_word.Length + 1 : 4;
            string best_word = "";

            foreach (string word in Allwords.Keys)
            {
                int diference = editDistance(miss_word, word);
                if (diference < min_diference)
                {
                    min_diference = diference;
                    best_word = word;
                }
            }
            return (best_word == "") ? miss_word : best_word;
        }
        public static string Get_best_suggestions(string[] query_words, Dictionary<string, int> Allwords)
        {
            List<string> best_suggestions = new List<string>();
            int cont = 0;

            foreach (string word in query_words)
            {

                if (!Allwords.ContainsKey(word))
                {
                    string temp = Search_suggestions(word, Allwords);
                    if (temp != word) cont++;
                    best_suggestions.Add(temp);
                }

                else best_suggestions.Add(word);
            }
            return (cont == 0) ? "" : string.Join(' ', best_suggestions);
        }

        //snippet en modo prueba
        public static void Update_Components(Dictionary<string, WordInfo> tf_idf, int lot_of_words)
        {
            foreach (string word in tf_idf.Keys)
            {
                tf_idf[word].tf_idf_score = tf_idf[word].Frequency / lot_of_words;
            }
            return;
        }
        public static string SnippetBeta(Document Doc, Vector VQuery, Query input, int snippet_length)
        {
            snippet_length *= 4;//aproximacion a caracteres
            string Text = File.ReadAllText(Doc.Doc_FileInfo.FullName);

            snippet_length = Math.Min(snippet_length, Text.Length);

            string score = 0;
            string best_score = -1;

            int li = 0;
            Queue<string> Cola = new Queue<string>();//guardar las palabras escogidas
            Dictionary<string, WordInfo> Snip_vector = new Dictionary<string, double>();

            int palabras_pos = 0;
            int palabras_neg = 0;

            //base
            for (int i = 0; i < snippet_length; i++)
            {
                bool Is_taken = false;

                if (char.IsLetterOrDigit(Text[i]))
                {
                    string word = get_nextWord(Text, i);
                    palabras_pos++;

                    if (VQuery.Components.ContainsKey(word))
                    {
                        Cola.Enqueue(word);
                        if (!Snip_vector.ContainsKey(word)) Snip_vector.Add(word, new WordInfo(word));
                        Snip_vector[word].Add_index(0);
                        Is_taken = true;
                    }

                    if (i - li >= snippet_length && Is_taken)
                    {
                        string palabra_a_quitar = Cola.Peek();//garantizamos que la palabra esta en el dicc

                        Snip_vector[palabra_a_quitar]--;
                        palabras_neg--;

                        Cola.Dequeue();

                        score = Vector.Cos_Similarity(new Vector(Snip_vector), VQuery, input);

                        if (score > best_score)
                    }
                }
            }


        }
    }
}