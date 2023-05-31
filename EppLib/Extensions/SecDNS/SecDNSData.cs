using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    public class SecDNSData
    {
        /// <summary>
        /// Base on https://datatracker.ietf.org/doc/html/rfc4034#section-5.1.1
        /// </summary>
        public ushort KeyTag { get; set; }
        public SecDNSAlgorithm Algorithm { get; set; }
        public SecDNSDigestType DigestType { get; set; }
        public string Digest { get; set; }

        public XmlNode ToXml(XmlDocument doc)
        {
            XmlElement dataNode = doc.CreateElement("secDNS:dsData", "urn:ietf:params:xml:ns:secDNS-1.1");

            XmlElement keyTagNode = doc.CreateElement("secDNS:keyTag", "urn:ietf:params:xml:ns:secDNS-1.1");
            keyTagNode.InnerText = KeyTag.ToString();
            dataNode.AppendChild(keyTagNode);

            XmlElement algNode = doc.CreateElement("secDNS:alg", "urn:ietf:params:xml:ns:secDNS-1.1");
            algNode.InnerText = ((int)Algorithm).ToString();
            dataNode.AppendChild(algNode);

            XmlElement digestTypeNode = doc.CreateElement("secDNS:digestType", "urn:ietf:params:xml:ns:secDNS-1.1");
            digestTypeNode.InnerText = ((int)DigestType).ToString();
            dataNode.AppendChild(digestTypeNode);

            XmlElement digestNode = doc.CreateElement("secDNS:digest", "urn:ietf:params:xml:ns:secDNS-1.1");
            digestNode.InnerText = Digest;
            dataNode.AppendChild(digestNode);

            return dataNode;
        }
    }
}
