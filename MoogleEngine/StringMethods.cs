using System;
using System.IO;
namespace MoogleEngine
{
    public class StringMethods //Metodos de normalizar, edit distance, snippet, sugerencias
    {
        public static (string[], int[]) Normalize_Text(string Text)
        {
            List<string> Words = new List<string>();
            List<int> Words_char_index = new List<int>();
            Text = Text.ToLower();

            for (int i = 0; i < Text.Length; i++)
            {
                if (char.IsLetterOrDigit(Text[i]))
                {
                    (string, int) temp = get_nextWord(Text, i);
                    i = temp.Item2;

                    if (temp.Item1 != "")
                    {
                        Words.Add(temp.Item1);
                        Words_char_index.Add(temp.Item2 - 1);
                    }
                }
            }
            return (Words.ToArray(), Words_char_index.ToArray());
        }
        public static string get_previousWord(string s, int pos)
        {
            string temp = "";
            while (pos >= 0 && s[pos] != ' ' && s[pos] != '~')
            {
                if (char.IsLetterOrDigit(s[pos])) temp += s[pos].ToString();
                pos--;
            }
            return StringMethods.Reverse(temp);
        }
        public static (string, int) get_nextWord(string s, int pos)
        {
            string temp = "";
            while (pos < s.Length && s[pos] != ' ' && s[pos] != '\n')
            {
                if (char.IsLetterOrDigit(s[pos])) temp += s[pos].ToString();
                pos++;
            }
            return (temp, pos);
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
        public static string Search_BestSnippet(Document Doc, Vector Query, int snippet_length, Query queryInfo)
        {
            //subarray de snippet maximo
            string[] Text = Doc.Text;
            if (snippet_length > Text.Length) return File.ReadAllText(Doc.Doc_FileInfo.FullName);//caso base

            int[] Words_index = Doc.Text_words_index;

            int li = 0;
            (int, int) limits = (0, 0);
            double best_score = 0;
            double score = 0;

            Dictionary<string, int> WordxFrequency = new Dictionary<string, int>();//limitador de palabras iguales en el score
            Dictionary<string, double> Importants_words = queryInfo.Important_operator;

            //rellenar el limitador
            foreach (string word in Query.Components.Keys) if (!WordxFrequency.ContainsKey(word)) WordxFrequency.Add(word, 0);

            //buscar el mejor snippet
            for (int ls = 0; ls < Text.Length; ls++)
            {
                if (Query.Components.ContainsKey(Text[ls]))
                {
                    WordxFrequency[Text[ls]]++;
                    double limiter = WordxFrequency[Text[ls]];
                    score += 1 / limiter;
                    if (Importants_words.ContainsKey(Text[ls])) score += Importants_words[Text[ls]];
                }

                if (ls - li >= snippet_length)
                {
                    if (score > best_score)
                    {
                        limits = (li, ls);
                        best_score = score;
                    }

                    if (Query.Components.ContainsKey(Text[li]))
                    {
                        double limiter = WordxFrequency[Text[li]];
                        WordxFrequency[Text[li]]--;
                        score -= 1 / limiter;
                        if (Importants_words.ContainsKey(Text[li])) score -= Importants_words[Text[li]];
                    }
                    li++;
                }
            }

            int start = Words_index[limits.Item1] - Text[limits.Item1].Length + 1;
            int lenght = Words_index[limits.Item2] - start;

            return (File.ReadAllText(Doc.Doc_FileInfo.FullName).Substring(start, lenght));
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
    }
}