using System;
using System.IO;

namespace MoogleEngine
{
    public class TF_IDF
    {
        Dictionary<Document, Dictionary<string, WordInfo>> TFxIDF;
        Dictionary<string, int> IDF;

        public Dictionary<Document, Dictionary<string, WordInfo>> get_TFxIDF { get { return TFxIDF; } }
        public Dictionary<string, int> get_IDF { get { return IDF; } }

        public Dictionary<string, WordInfo> get_DocInfo(Document Doc)
        {
            return this.TFxIDF[Doc];
        }

        public TF_IDF(Document query_doc)//adaptador para la query
        {
            this.TFxIDF = new Dictionary<Document, Dictionary<string, WordInfo>>();
            this.TFxIDF.Add(query_doc, new Dictionary<string, WordInfo>());
            this.IDF = new Dictionary<string, int>();//para que sea reutilizables las funciones

            this.TFxIDF[query_doc] = tf_4doc(query_doc);
            Calcule_tf_idf(this.TFxIDF[query_doc], 1, (double)query_doc.get_Length);
        }

        public TF_IDF(Text_Files Documents)
        {
            this.TFxIDF = new Dictionary<Document, Dictionary<string, WordInfo>>();
            this.IDF = new Dictionary<string, int>();

            //Obtener el tf y el idf de cada palabra
            foreach (FileInfo text_file in Documents.get_AllFiles)
            {
                Document doc = new Document(text_file);
                this.TFxIDF[doc] = tf_4doc(doc);
            }

            //calculo del tf_idf de cada palabra en el documento
            double doc_quantity = Documents.get_Length;

            foreach (Document Doc in this.TFxIDF.Keys)
            {
                Calcule_tf_idf(this.TFxIDF[Doc], doc_quantity, Doc.get_Length);
            }
        }

        Dictionary<string, WordInfo> tf_4doc(Document Doc)//tf idf para un documento
        {
            Dictionary<string, WordInfo> temp = new Dictionary<string, WordInfo>();

            int index = 0;

            foreach (string word in Doc.get_Text)
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

        void Calcule_tf_idf(Dictionary<string, WordInfo> temp, double doc_quantity, double words_quantity)
        {
            foreach (string words in temp.Keys)
            {
                double cte_ifd = (doc_quantity == 1) ? 1 : (Math.Log10(doc_quantity / IDF[words]));//para el caso que solo sea un doc

                double score = (temp[words].get_frequency / words_quantity) * cte_ifd;
                temp[words].set_tf_idf_score(score);
 
            }
        }

        public static void print_TF_IDF(TF_IDF temp)
        {
            foreach (var i in temp.get_TFxIDF.Keys)
            {
                foreach (var j in temp.get_TFxIDF[i].Keys)
                {
                    Console.WriteLine($"{j} {temp.get_TFxIDF[i][j].get_tf_idf_score}");
                }
            }
        }
    }
}