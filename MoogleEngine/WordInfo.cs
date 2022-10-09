namespace MoogleEngine
{
    public class WordInfo
    {
        public string Word { get; private set; }
        public int Frequency { get; private set; }
        public List<int> Index { get; private set; }
        public double tf_idf_score { get; set; }

        public WordInfo(string word)
        {
            this.Word = word;
            this.Index = new List<int>();
            this.Frequency = 0;
            this.tf_idf_score = 0;
        }

        public void Add_index(int index)
        {
            this.Index.Add(index);
            this.Frequency++;
        }
    }
}