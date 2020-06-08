using System.Xml;
using System.Xml.Linq;

namespace TransactionStore.Api.Extensions
{
    public static class XElementExtensions
    {
        public static int GetLineNumber(this XElement element)
        {
            return ((IXmlLineInfo) element).LineNumber;
        }
    }
}