using System;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.DomainInfo
{
	public class NominetDomainInfo : DomainBase<NominetDomainInfoResponse>
	{
		private readonly string domainName;

		public string Hosts { get; set; }

		public NominetDomainInfo(string domainName)
		{
			this.domainName = domainName;
		}

		protected override XmlElement BuildCommandElement(XmlDocument doc, XmlElement commandRootElement)
		{
			XmlElement domainInfo = BuildCommandElement(doc, "info", commandRootElement);

			XmlElement domainNameElement = AddXmlElement(doc, domainInfo, "domain:name", domainName, namespaceUri);

			if (!string.IsNullOrEmpty(Hosts))
			{
				domainNameElement.SetAttribute("hosts", Hosts);
			}

			return domainInfo;
		}

		public override NominetDomainInfoResponse FromBytes(byte[] bytes)
		{
			return new NominetDomainInfoResponse(bytes);
		}
	}
}