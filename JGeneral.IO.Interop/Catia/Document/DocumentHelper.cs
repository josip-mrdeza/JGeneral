using System.Linq;
using INFITF;

namespace JGeneral.IO.Interop.Catia.Document
{
    public static class DocumentHelper
    {
        public static INFITF.Document[] GetAllOpenDocuments(this Application application)
        {
            var docs = application.Documents;
            var arr = new INFITF.Document[docs.Count];
            var enumerator = docs.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
            {
                arr[count] = enumerator.Current as INFITF.Document;
            }

            return arr.Where(x => x != null).ToArray();
        }

        public static CatiaDocument FromDocument(this INFITF.Document document)
        {
            return new CatiaDocument(document);
        }
    }
}