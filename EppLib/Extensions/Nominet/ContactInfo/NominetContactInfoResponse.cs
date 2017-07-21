using System;
using System.Collections.Generic;
using System.Xml;
using EppLib.Entities;

namespace EppLib.Extensions.Nominet.ContactInfo
{
	public class NominetContactInfoResponse : ContactInfoResponse
	{
		private NominetContactExtension _nomContact = new NominetContactExtension();
		public virtual new NominetContactExtension Contact
		{
			get { return _nomContact; }
		}

        private NominetDataQualityExtension _nomDataQuality;
        public virtual NominetDataQualityExtension DataQuality{ get { return _nomDataQuality; }}

		public NominetContactInfoResponse(byte[] bytes) : base(bytes) { }

		protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
		{
			base.ProcessDataNode(doc, namespaces);

			_nomContact.Id = _contact.Id;
			_nomContact.Voice = _contact.Voice;
			_nomContact.Fax = _contact.Fax;
			_nomContact.Email = _contact.Email;
			_nomContact.PostalInfo = _contact.PostalInfo;
			_nomContact.Roid = _contact.Roid;
			_nomContact.Status = _contact.Status;
		    _nomContact.StatusList = _contact.StatusList;
			_nomContact.ClId = _contact.ClId;
			_nomContact.CrId = _contact.CrId;
			_nomContact.CrDate = _contact.CrDate;
			_nomContact.UpDate = _contact.UpDate;
			_nomContact.UpId = _contact.UpId;
            _nomContact.DiscloseFlag = _contact.DiscloseFlag;
            _nomContact.DiscloseMask = _contact.DiscloseMask;
		}

		protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
		{
			namespaces.AddNamespace("contact", "urn:ietf:params:xml:ns:contact-1.0");
			namespaces.AddNamespace("contact-nom-ext", "http://www.nominet.org.uk/epp/xml/contact-nom-ext-1.0");
            namespaces.AddNamespace("nom-data-quality", "http://www.nominet.org.uk/epp/xml/nom-data-quality-1.1");

			XmlNode children = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/contact-nom-ext:infData", namespaces);
            XmlNode quality = doc.SelectSingleNode("/ns:epp/ns:response/ns:extension/nom-data-quality:infData", namespaces);

			if (children != null)
			{
				XmlNode tradeNameNode = children.SelectSingleNode("contact-nom-ext:trad-name", namespaces);

				if (tradeNameNode != null)
				{
					_nomContact.TradeName = tradeNameNode.InnerText;
				}

				XmlNode coNoNode = children.SelectSingleNode("contact-nom-ext:co-no", namespaces);

				if (coNoNode != null)
				{
					_nomContact.CompanyNumber = coNoNode.InnerText;
				}

				XmlNode typeNode = children.SelectSingleNode("contact-nom-ext:type", namespaces);

				if (typeNode != null)
				{
					_nomContact.Type = (CoType)Enum.Parse(typeof(CoType), typeNode.InnerText);
				}

				XmlNode optOutNode = children.SelectSingleNode("contact-nom-ext:opt-out", namespaces);
				if (optOutNode != null)
				{
					_nomContact.OptOut = (YesNoFlag)Enum.Parse(typeof(YesNoFlag), optOutNode.InnerText);
				}
            }
            if (quality != null)
            {
                _nomDataQuality = new NominetDataQualityExtension();
                XmlNode status = quality.SelectSingleNode("nom-data-quality:status", namespaces);
                if (status != null)
                {
                    _nomDataQuality.Status = status.InnerText;
                }
                XmlNode reason = quality.SelectSingleNode("nom-data-quality:reason", namespaces);
                if (reason != null)
                {
                    _nomDataQuality.Reason = reason.InnerText;
                }
                XmlNode dateCommenced = quality.SelectSingleNode("nom-data-quality:dateCommenced", namespaces);
                if (dateCommenced != null)
                {
                    DateTime date;
                    if (DateTime.TryParse(dateCommenced.InnerText, out date))
                    {
                        _nomDataQuality.DateCommenced = date;
                    }
                }
                XmlNode dateToSuspend = quality.SelectSingleNode("nom-data-quality:dateToSuspend", namespaces);
                if (dateToSuspend != null)
                {
                    DateTime date;
                    if (DateTime.TryParse(dateToSuspend.InnerText, out date))
                    {
                        _nomDataQuality.DateToSuspend = date;
                    }
                }
                XmlNode lockApplied = quality.SelectSingleNode("nom-data-quality:lockApplied", namespaces);
                if (lockApplied != null)
                {
                    bool lockValue;
                    if (!bool.TryParse(lockApplied.InnerText, out lockValue))
                    {
                        if (lockApplied.InnerText == "1" || lockApplied.InnerText.ToLower() == "y")
                        {
                            lockValue = true;
                        }
                        else
                        {
                            lockValue = false;
                        }
                        _nomDataQuality.LockApplied = lockValue;
                    }
                }
                XmlNode domainListNodes = quality.SelectSingleNode("nom-data-quality:domainListData", namespaces);
                if (domainListNodes != null)
                {
                    _nomDataQuality.DomainList = new List<string>();
                    XmlNodeList domainNodes = domainListNodes.SelectNodes("nom-data-quality:domainName", namespaces);
                    if (domainNodes != null)
                    {
                        foreach (XmlNode domain in domainNodes)
                        {
                            _nomDataQuality.DomainList.Add(domain.InnerText);
                        }
                    }
                }
            }

		}
	}
}