namespace MoogleEngine;
using System;
using System.Diagnostics;
using System.IO;
using MoogleEngine;

public static class Moogle
{
    public static Dictionary<Document, Dictionary<string, WordInfo>> Base = new Dictionary<Document, Dictionary<string, WordInfo>>();
    public static Dictionary<string, int> Words_of_Base = new Dictionary<string, int>();
    public static string Time;
    public static void Preprocess()//Procesamiento del texto, calculo del tf*idf
    {
        Folder Content = new Folder("/home/osvaldo/CC111/Proyecto/Contentsss/ContentMedium");
        //path of Moogle Content System.IO.Path.Join("..", "Content");
        Text_Files Content_text_files = new Text_Files(Content);
        TF_IDF Base_Stats = new TF_IDF(Content_text_files);

        Words_of_Base = Base_Stats.IDF;
        Base = Base_Stats.TFxIDF;
    }
    public static SearchResult Query(string query)
    {
        var time = new Stopwatch();//cronometro
        time.Start();

        //Procesar la query
        Query input = new Query(query);
        Document partial = new Document("query", input.Words);
        TF_IDF query_stats = new TF_IDF(partial);

        //Modelo de Espacio Vectorial (producto punto)
        Vector query_vector = new Vector(query_stats.TFxIDF[partial]);//vector query
        List<(Document, double)> Result = new List<(Document, double)>();//lista de resultados

        foreach (Document Doc in Base.Keys)//recorrido por todos los documentos para buscar la similitud
        {
            Vector Doc_vector = new Vector(Base[Doc]);
            double score = Vector.Cos_Similarity(Doc_vector, query_vector, input);//calculo del cos
            score += OperatorsMethods.check_near(Doc, input.Near_operator, Base[Doc]);//operador cerania

            if (score != 0) Result.Add((Doc, score));//guardar los que contengan semejanza con la query
        }

        Result.Sort(Vector.Compare_Vectors);//organizar decrecientes, segun el valor del Cos

        int lot_of_items = Math.Min(50, Result.Count);//cantidad de resultados a mostrar
        int snippet_length = 100;//cantidad de palabras del snippet
        SearchItem[] items = new SearchItem[lot_of_items];

        for (int index = 0; index < lot_of_items; index++)//construccion del snippet
        {
            string best_snippet = StringMethods.Search_BestSnippet(Result[index].Item1, query_vector, snippet_length, input);
            items[index] = new SearchItem(Result[index], best_snippet);
        }

        //make suggestion
        string suggestion = StringMethods.Get_best_suggestions(input.Words, Words_of_Base);
        if (suggestion == input.User_query) suggestion = "";

        time.Stop();
        TimeSpan time_temp = time.Elapsed;//resultado del cronometro
        Time = time_temp.ToString(@"m\:ss\.fff");

        return new SearchResult(items, suggestion);
    }
}