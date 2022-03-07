using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JGeneral.IO.Net
{
    public class BasicRemoteDataSender : IRemoteClient
    {
        public string Id { get; }
        public Task<IRemoteCommand[]> CheckQueue() => throw new System.NotImplementedException();

        public void Send(IRecipient recipient, Command queryCommand, byte[] data)
        {
            var url = new Uri($"https://atriplexx.loophole.site/sender/{recipient.Id}");
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.GetRequestStream().Write(data, 0, data.Length);
            request.GetResponse().Close();
        }
    }
}