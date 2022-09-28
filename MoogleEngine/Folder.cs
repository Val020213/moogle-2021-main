namespace MoogleEngine
{
    public class Folder
    {
        public string Direction {get; private set;}
        public System.IO.DirectoryInfo Folder_DirectoryInfo {get; private set;}

        public Folder(bool current)//Dirrecion usual de la carpeta content
        {
            this.Direction = System.IO.Path.Join("..", "Content");
            this.Folder_DirectoryInfo = new System.IO.DirectoryInfo(this.Direction);
            check();
        }
        public Folder(string Path)//para cambiar la direccion de la carpeta content
        {
            this.Direction = Path;
            this.Folder_DirectoryInfo = new System.IO.DirectoryInfo(this.Direction);
            check();
        }

        private void check()//check para no recibir entradas null
        {
            if (!this.Folder_DirectoryInfo.Exists)
                throw new System.Exception($"LA CARPETA CONTENT NO SE ENCUENTRA EN LA DIRECCION {this.Direction}");
        }
    }
}
