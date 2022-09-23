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
            string[] Text = Doc.get_Text;
            int li = 0;
            (int, int) limits = (0, 0);
            int best_score = 0;
            int score = 0;
            snippet_length = (snippet_length > Text.Length) ? Text.Length : snippet_length;

            for (int ls = 0; ls < Text.Length; ls++)
            {
                if (Query.get_Components.ContainsKey(Text[ls])) { score++; }

                if (ls - li >= snippet_length)
                {
                    if (score > best_score)
                    {
                        limits = (li, ls);
                        best_score = score;
                    }

                    if (Query.get_Components.ContainsKey(Text[li])) score--;
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
            List<string> Get_best_suggestions = new List<string>();
            int cont = 0;

            foreach (string word in query_words)
            {

                if (!Allwords.ContainsKey(word))
                {
                    string temp = Search_suggestions(word, Allwords);
                    if (temp != word) cont++;
                    Get_best_suggestions.Add(temp);
                }

                else Get_best_suggestions.Add(word);
            }
            return (cont == 0) ? "" : string.Join(' ', Get_best_suggestions);
        }

        //snippet en modo prueba
        /*
        public static string SnippetBeta(Document Doc, Vector VQuery, int snippet_length)
        {
            snippet_length *= 4;//aproximacion a caracteres
            string Original_Text = File.ReadAllText(Doc.get_Doc_FileInfo.FullName)
            //continuar
            objetivo hacer un psudo-vector con 50 palabras del doc y
            aplicar la similitud de coseno con la query, y quedarme con el mejor
            Razones para hacer este:
            Utilizaria el operador de importancia que no esta en el snippet implementado
            Tendria en cuenta la importancia de la palabra en el cacho del texto
        }
        */
    }
}