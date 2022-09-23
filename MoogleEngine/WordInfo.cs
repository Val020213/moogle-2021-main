namespace MoogleEngine
{
    public class WordInfo
    {
        string Word;
        int Frequency;
        List<int> Index;
        double tf_idf_score;

        public int get_frequency { get { return Frequency; } }
        public List<int> get_Index { get { return Index; } }
        public double get_tf_idf_score { get { return tf_idf_score; } }

        public void set_tf_idf_score(double assign)
        {
            this.tf_idf_score = assign;
        }

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