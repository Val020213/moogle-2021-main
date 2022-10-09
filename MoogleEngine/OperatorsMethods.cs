using System;
using System.IO;
namespace MoogleEngine
{
    public class OperatorsMethods // Metodos relacionados con los operadores
    {
        //los operados mustbe y mustnotbe se comprueban en la similaridad de cos directamente
        public static int IndexOrder((int, string) A, (int, string) B) => A.Item1.CompareTo(B.Item1);
        public static double check_near(Document Doc, List<List<string>> NearsWords, Dictionary<string, WordInfo> Allwords)
        {
            double plus_score = 0;
            foreach (var inear in NearsWords)
            {
                plus_score += cicle_near(Doc, inear, Allwords);
            }
            return plus_score;
        }
        public static double cicle_near(Document Doc, List<string> NearList, Dictionary<string, WordInfo> Allwords)
        {
            List<(int, string)> Index_byWord = new List<(int, string)>();
            Dictionary<string, int> mask_index = new Dictionary<string, int>();
            //Construir
            foreach (string word in NearList)
            {
                if (!Allwords.ContainsKey(word)) return 0;
                if (!mask_index.ContainsKey(word)) mask_index.Add(word, 0);

                foreach (int index in Allwords[word].Index)
                {
                    Index_byWord.Add((index, word));
                }
            }

            Index_byWord.Sort(IndexOrder);

            int cont = 0;
            int lot_of_words = NearList.Count;

            List<(int, string)> temp = new List<(int, string)>();
            int li = 0;
            double distance = 100000;

            for (int i = 0; i < Index_byWord.Count; i++)
            {
                if (mask_index[Index_byWord[i].Item2] == 0) cont++;
                mask_index[Index_byWord[i].Item2]++;

                temp.Add(Index_byWord[i]);

                if (cont == lot_of_words)
                {
                    while (cont == lot_of_words)
                    {
                        mask_index[temp[li].Item2]--;
                        if (mask_index[temp[li].Item2] == 0)
                        {
                            cont--;
                            distance = Math.Min(distance, (temp[temp.Count - 1].Item1 - temp[li].Item1));
                        }
                        li++;
                    }
                }

            }

            double score = 1 / distance;
            return (distance == 0) ? 0 : score;
        }
    }
}