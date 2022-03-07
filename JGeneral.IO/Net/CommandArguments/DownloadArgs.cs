namespace JGeneral.IO.Net.CommandArguments
{
    public struct DownloadArgs : IArgs
    {
        public string URL { get; set; }

        public DownloadArgs(string url)
        {
            URL = url;
        }
        
    }
}