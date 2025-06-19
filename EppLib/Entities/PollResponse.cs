// Copyright 2012 Code Maker Inc. (http://codemaker.net)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Globalization;
using System.Xml;

namespace EppLib.Entities
{
    public class PollResponse : EppResponse
    {
        public string MsgId;
        public string DomainName;

		public PollResponse(string xml) : base(xml) { }
        public PollResponse(byte[] bytes) : base(bytes) { }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            XmlNode messageNode = doc.SelectSingleNode("/ns:epp/ns:response/ns:msgQ", namespaces);

            if (messageNode != null)
            {
                if (messageNode.Attributes != null)
                {
                    Id = messageNode.Attributes["id"].Value;
                    Count = Convert.ToInt32(messageNode.Attributes["count"].Value,CultureInfo.InvariantCulture);
                }

                XmlNode qDateNode = messageNode.SelectSingleNode("ns:qDate", namespaces);

                if(qDateNode!=null)
                {
                    QDate = qDateNode.InnerText;
                }

                XmlNode msgNode = messageNode.SelectSingleNode("ns:msg", namespaces);

                if (msgNode != null)
                {
                    Body = msgNode.InnerText;

					if (msgNode.Attributes != null && msgNode.Attributes["lang"] != null)
                    {
						Language = msgNode.Attributes["lang"].Value;
                    }
                }
                
            }

            XmlNode resData = doc.SelectSingleNode("/ns:epp/ns:response/ns:resData", namespaces);

            namespaces.AddNamespace("poll","urn:ietf:params:xml:ns:poll-1.0");

            if (resData != null)
            {
                ResultData = resData.InnerXml;
                XmlNode msgIDNode = resData.SelectSingleNode("poll:msgID", namespaces);

                if (msgIDNode != null)
                {
                    MsgId = msgIDNode.InnerText;
                }

                // Old code for extracting the domain name
                XmlNode domainNameNode = resData.SelectSingleNode("poll:domainName", namespaces);

                if (domainNameNode != null)
                {
                    DomainName = domainNameNode.InnerText;
                }

                XmlNamespaceManager domNamespaces = new XmlNamespaceManager(doc.NameTable);
                domNamespaces.AddNamespace("iis", "urn:se:iis:xml:epp:iis-1.2");
                domNamespaces.AddNamespace("dom", "urn:ietf:params:xml:ns:domain-1.0");

                // try to extract the domain name if it's an update notify messge
                if (DomainName == null || DomainName.Length == 0)
                {
                    XmlNode domNameNode = resData.SelectSingleNode("iis:updateNotify/dom:infData/dom:name", domNamespaces);
                    if (domNameNode != null)
                    {
                        DomainName = domNameNode.InnerText;
                    }
                }

                // try to extract the domain name if it's a domain delete message
                if (DomainName == null || DomainName.Length == 0)
                {
                    XmlNode delNameNode = resData.SelectSingleNode("iis:deleteNotify/dom:delete/dom:name", domNamespaces);
                    if (delNameNode != null)
                    {
                        DomainName = delNameNode.InnerText;
                    }
                }

                // try to extract the domain name if it's a domain transfer message
                if (DomainName == null || DomainName.Length == 0)
                {
                    XmlNode transNameNode = resData.SelectSingleNode("iis:transferNotify/dom:trnData/dom:name", domNamespaces);
                    if (transNameNode != null)
                    {
                        DomainName = transNameNode.InnerText;
                    }
                }
            }
        }

        public string Id { get; set; }
        public int Count { get; set; }
        public string QDate { get; set; }
        public string Body { get; set; }
        public string Language { get; set; }
		public string ResultData { get; set; }
    }
}
