namespace MoogleEngine;
public class SearchItem
{
    public SearchItem((Document, double) obj, string best_snippet)
    {
        this.Title = obj.Item1.get_Name;
        this.Snippet = best_snippet;
        this.Score = obj.Item2;
    }
    public SearchItem(string title, string snippet, double score)
    {
        this.Title = title;
        this.Snippet = snippet;
        this.Score = score;
    }

    public string Title { get; private set; }

    public string Snippet { get; private set; }

    public double Score { get; private set; }
}