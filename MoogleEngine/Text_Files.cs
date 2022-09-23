namespace MoogleEngine
{
    public class Text_Files
    {
        System.IO.FileInfo[] AllFiles;
        int Length;

        public System.IO.FileInfo[] get_AllFiles { get { return AllFiles; } }

        public int get_Length { get { return Length; } }

        public Text_Files(Folder Current_Folder)
        {
            this.AllFiles = Current_Folder.get_Folder_DirectoryInfo.GetFiles("*txt");
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