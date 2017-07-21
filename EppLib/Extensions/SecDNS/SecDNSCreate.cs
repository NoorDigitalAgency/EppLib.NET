using EppLib.Entities;
using System.Collections.Generic;
using System.Xml;

namespace EppLib.Extensions.SecDNS
{
    public class SecDNSCreate : EppExtension
    {
        public int? MaxSigLife { get; set; }
        public IList<SecDNSData> DsData { get; } = new List<SecDNSData>();

        protected override string Namespace { get; set; }

        public override XmlNode ToXml(XmlDocument doc)
        {
            XmlElement root = doc.CreateElement("secDNS:create", "urn:ietf:params:xml:ns:secDNS-1.1");
            root.SetAttribute("xmlns:secDNS", "urn:ietf:params:xml:ns:secDNS-1.1");

            XmlAttribute xsd = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
            xsd.Value = "urn:ietf:params:xml:ns:secDNS-1.1 secDNS-1.1.xsd";
            root.Attributes.Append(xsd);

            if (MaxSigLife.HasValue)
            {
                XmlElement maxSigLifeNode = doc.CreateElement("secDNS:maxSigLife", "urn:ietf:params:xml:ns:secDNS-1.1");
                maxSigLifeNode.InnerText = MaxSigLife.Value.ToString();
                root.AppendChild(maxSigLifeNode);
            }

            foreach (SecDNSData data in DsData)
            {
                root.AppendChild(data.ToXml(doc));
            }

            return root;
        }
    }
}
