namespace JGeneral.IO.Net.V2.Services.Helpers
{
    public struct ContextInfo
    {
        public int Code;
        public int Received;
        public int Sent;

        internal ContextInfo(int code, int received, int sent)
        {
            Code = code;
            Received = received;
            Sent = sent;
        }
    }
}