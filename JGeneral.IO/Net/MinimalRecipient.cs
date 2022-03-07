namespace JGeneral.IO.Net
{
    public class MinimalRecipient : IRecipient
    {
        public string Id { get; }

        public MinimalRecipient(string id)
        {
            Id = id;
        }
    }
}