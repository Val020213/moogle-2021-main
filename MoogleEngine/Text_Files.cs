namespace MoogleEngine
{
    public class Text_Files
    {
        public System.IO.FileInfo[] AllFiles {get; private set;}
        public int Length {get; private set;}

        public Text_Files(Folder Current_Folder)
        {
            this.AllFiles = Current_Folder.Folder_DirectoryInfo.GetFiles("*txt");
            this.Length = AllFiles.Length;
            check();
        }
        private void check()//check para verificar al menos una entrada de documento
        {
            if (this.Length == 0)
                throw new System.Exception("LA CARPETA NO CONTIENE ARCHIVOS DE TEXTO");
        }
    }
}