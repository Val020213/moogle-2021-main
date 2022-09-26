using System;
using System.IO;
namespace MoogleEngine
{
    public class Vector
    {
        public Dictionary<string, WordInfo> Components { get; private set; }

        public Vector(Dictionary<string, WordInfo> components)
        {
            this.Components = components;
        }

        public static int Compare_Vectors((Document, double) A, (Document, double) B) => B.Item2.CompareTo(A.Item2);
        public static double Cos_Similarity(Vector Doc, Vector VQuery, Query queryInfo)
        {//Cos con el calculo del producto punto, y verificar las condiciones

            Dictionary<string, bool> Must_be = queryInfo.Mustbe_operator;
            Dictionary<string, bool> Must_not = queryInfo.Mustnotbe_operator;
            Dictionary<string, double> Important = queryInfo.Important_operator;

            double Dot_Product = 0;

            double partial_doc_mod = 0;
            double partial_query_mod = 0;
            double score_doc = 0;
            double score_query = 0;
            double Score = 0;

            int cont_must_be = 0;

            foreach (string Component in VQuery.Components.Keys)
            {
                score_query = VQuery.Components[Component].tf_idf_score;

                if (Doc.Components.ContainsKey(Component))//componentes comunes a la query
                {
                    if (Important.ContainsKey(Component)) Score += Important[Component];
                    if (Must_not.ContainsKey(Component)) return 0;//requisitos
                    if (Must_be.ContainsKey(Component)) cont_must_be++;

                    score_doc = Doc.Components[Component].tf_idf_score;
                    Dot_Product += (score_doc * score_query);
                    partial_doc_mod += score_doc * score_doc;
                }

                partial_query_mod += score_query * score_query;
            }

            double mod = Math.Sqrt(partial_doc_mod * partial_query_mod);

            Score = Dot_Product / mod;

            return (mod == 0 || cont_must_be != Must_be.Count) ? 0 : (Score);
        }
    }
}