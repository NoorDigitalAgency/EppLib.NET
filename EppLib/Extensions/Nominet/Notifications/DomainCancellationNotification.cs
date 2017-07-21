using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.Notifications
{
    public class DomainCancellationNotification : PollResponse
    {
        public string DomainName { get; set; }
        public string Originator { get; set; }
        public DomainCancellationNotification(string xml) : base(xml){ }
        public DomainCancellationNotification(byte[] bytes) : base(bytes){ }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            base.ProcessDataNode(doc, namespaces);

            namespaces.AddNamespace("n", "http://www.nominet.org.uk/epp/xml/std-notifications-1.2");
            XmlNode domainCancelledNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData/n:cancData", namespaces);

            if (domainCancelledNode != null)
            {
                XmlNode domainNameNode = domainCancelledNode.SelectSingleNode("n:domainName", namespaces);
                if (domainNameNode != null)
                {
                    DomainName = domainNameNode.InnerText;
                }

                XmlNode originatorNode = domainCancelledNode.SelectSingleNode("n:orig", namespaces);
                if (originatorNode != null)
                {
                    Originator = originatorNode.InnerText;
                }
            }
        }
    }
}
