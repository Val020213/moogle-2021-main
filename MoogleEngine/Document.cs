namespace MoogleEngine
{
    public class Document
    {
        public System.IO.FileInfo Doc_FileInfo { get; private set; }
        public string Name { get; private set; }
        public string[] Text { get; private set; }
        public int[] Text_words_index { get; private set; }
        public int Length { get; private set; }

        public Document(string name, string[] text)
        //aqui el campo Docfileinfo es null, (solo se usa en psudo docs)
        {
            this.Name = name;
            this.Text = text;
            this.Length = this.Text.Length;
        }
        public Document(System.IO.FileInfo Current_file)
        {
            this.Doc_FileInfo = Current_file;
            this.Name = Current_file.Name;
            var temp = StringMethods.Normalize_Text(File.ReadAllText(Current_file.FullName));
            this.Text = temp.Item1;
            this.Text_words_index = temp.Item2;
            this.Length = this.Text.Length;
        }
    }
}