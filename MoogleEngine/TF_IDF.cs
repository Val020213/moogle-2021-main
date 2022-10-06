using System;
using System.IO;

namespace MoogleEngine
{
    public class TF_IDF
    {
        public Dictionary<Document, Dictionary<string, WordInfo>> TFxIDF { get; private set; }
        public Dictionary<string, int> IDF { get; private set; }

        public TF_IDF(Document query_doc)//adaptador para la query
        {
            this.TFxIDF = new Dictionary<Document, Dictionary<string, WordInfo>>();
            this.TFxIDF.Add(query_doc, new Dictionary<string, WordInfo>());
            this.IDF = new Dictionary<string, int>();//para que sea reutilizables las funciones

            this.TFxIDF[query_doc] = TF_forDocument(query_doc);
            Calcule_TF_IDF_value(this.TFxIDF[query_doc], 1, (double)query_doc.Length);
        }

        public TF_IDF(Text_Files Documents)
        {
            this.TFxIDF = new Dictionary<Document, Dictionary<string, WordInfo>>();
            this.IDF = new Dictionary<string, int>();

            //Obtener el tf y el idf de cada palabra
            foreach (FileInfo text_file in Documents.AllFiles)
            {
                Document doc = new Document(text_file);
                this.TFxIDF[doc] = TF_forDocument(doc);
            }

            //calculo del tf_idf de cada palabra en el documento
            double doc_quantity = Documents.Length;

            foreach (Document Doc in this.TFxIDF.Keys)
            {
                Calcule_TF_IDF_value(this.TFxIDF[Doc], doc_quantity, Doc.Length);
            }
        }

        Dictionary<string, WordInfo> TF_forDocument(Document Doc)//tf idf para un documento
        {
            Dictionary<string, WordInfo> temp = new Dictionary<string, WordInfo>();

            int index = 0;

            foreach (string word in Doc.Text)
            {
                if (!temp.ContainsKey(word))
                {
                    temp.Add(word, new WordInfo(word));

                    if (!IDF.ContainsKey(word)) IDF.Add(word, 0);
                    IDF[word]++;
                }
                temp[word].Add_index(index);//aumenta la frecuencia de la palabra;
                index++;
            }
            return temp;
        }
        
        void Calcule_TF_IDF_value(Dictionary<string, WordInfo> temp, double doc_quantity, double words_quantity)
        {
            foreach (string words in temp.Keys)
            {
                double cte_ifd = (doc_quantity == 1) ? 1 : (Math.Log10(doc_quantity / IDF[words]));//para el caso que solo sea un doc

                double score = (temp[words].Frequency / words_quantity) * cte_ifd;
                temp[words].tf_idf_score = score;

            }
        }

        public static void print_TF_IDF(TF_IDF temp)
        {
            foreach (var i in temp.TFxIDF.Keys)
            {
                foreach (var j in temp.TFxIDF[i].Keys)
                {
                    Console.WriteLine($"{j} {temp.TFxIDF[i][j].tf_idf_score}");
                }
            }
        }
    }
}