namespace MoogleEngine;
public class SearchItem
{
    public SearchItem((Document, double) obj, string best_snippet)
    {
        this.Title = obj.Item1.Name;
        this.Snippet = best_snippet;
        this.Score = obj.Item2;
        string tester = obj.Item1.Doc_FileInfo.FullName;
        string resp = "file:///";
        for(int i = 1 ; i < tester.Length; i++)resp+=tester[i];
        this.Direction = resp;

    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public double Score { get; private set; }

    public string Direction { get; private set; }
}