namespace MoogleEngine
{
    public class Document
    {
        System.IO.FileInfo Doc_FileInfo;
        string Name;
        string[] Text;
        int Length;

        public System.IO.FileInfo get_Doc_FileInfo { get { return Doc_FileInfo; } }
        public string get_Name { get { return Name; } }
        public string[] get_Text { get { return Text; } }
        public int get_Length { get { return Length; } }

        public Document(string name, string[] text)//aqui el campo Docfileinfo es null, (solo se usa en psudo docs)
        {
            this.Name = name;
            this.Text = text;
            this.Length = this.Text.Length;
        }

        public Document(System.IO.FileInfo Current_file)
        {
            this.Doc_FileInfo = Current_file;
            this.Name = Current_file.Name;
            this.Text = StringMethods.Normalize_Text(File.ReadAllText(Current_file.FullName));
            this.Length = this.Text.Length;
        }
    }
}