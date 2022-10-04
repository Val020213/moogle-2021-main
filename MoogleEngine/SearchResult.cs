namespace MoogleEngine;

public class SearchResult
{
    private SearchItem[] items;
    public string Time { get; private set; }
    public string Suggestion { get; private set; }

    public SearchResult(SearchItem[] items, string suggestion = "", string time)
    {
        if (items == null)
        {
            throw new ArgumentNullException("items");
        }

        this.items = items;
        this.Time = time;
        this.Suggestion = suggestion;
    }

    public SearchResult() : this(new SearchItem[0])
    {

    }
    public void Reset_Query_Suggestion()
    {
        this.Suggestion = "";
    }
    public IEnumerable<SearchItem> Items()
    {
        return this.items;
    }

    public int Count { get { return this.items.Length; } }
}