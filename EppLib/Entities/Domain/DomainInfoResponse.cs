﻿// Copyright 2012 Code Maker Inc. (http://codemaker.net)
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
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using EppLib.Extensions.SecDNS;

namespace EppLib.Entities
{
    public class DomainInfoResponse : EppResponse
    {
        protected Domain _domain = new Domain();

        public virtual Domain Domain
        {
            get { return _domain; }
        }

        public IList<SecDNSData> SecDnsList { get; private set; }

        public DomainInfoResponse(string xml) : base(xml)
        {
        }

        public DomainInfoResponse(byte[] bytes) : base(bytes)
        {
        }

        protected override void ProcessDataNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            namespaces.AddNamespace("domain", "urn:ietf:params:xml:ns:domain-1.0");

            XmlNode children = doc.SelectSingleNode("//domain:infData", namespaces);

            if (children != null)
            {
                XmlNode nameNode = children.SelectSingleNode("domain:name", namespaces);

                if (nameNode != null)
                {
                    Domain.Name = nameNode.InnerText;
                }

                XmlNode roidNode = children.SelectSingleNode("domain:roid", namespaces);

                if (roidNode != null)
                {
                    Domain.Roid = roidNode.InnerText;
                }

                XmlNode registrantNode = children.SelectSingleNode("domain:registrant", namespaces);

                if (registrantNode != null)
                {
                    Domain.RegistrantId = registrantNode.InnerText;
                }

                XmlNodeList statusNodes = children.SelectNodes("domain:status", namespaces);

                if (statusNodes != null)
                {
                    foreach (XmlNode statusNode in statusNodes)
                    {
                        if (statusNode != null)
                        {
                            if (statusNode.Attributes != null)
                            {
                                string value = statusNode.Attributes["s"].Value;


                                Domain.Status.Add(new Status(statusNode.InnerText, value));
                            }
                        }
                    }
                }

                XmlNodeList contactNodes = children.SelectNodes("domain:contact", namespaces);

                if (contactNodes != null)
                {
                    foreach (XmlNode contactNode in contactNodes)
                    {
                        if (contactNode != null)
                        {
                            if (contactNode.Attributes != null)
                            {
                                string type = contactNode.Attributes["type"].Value;

                                Domain.Contacts.Add(new DomainContact(contactNode.InnerText, type));
                            }
                        }
                    }
                }

                XmlNodeList hostNodes = children.SelectNodes("domain:host", namespaces);

                if (hostNodes != null)
                {
                    foreach (XmlNode hostNode in hostNodes)
                    {
                        Domain.Hosts.Add(hostNode.InnerText);
                    }
                }

                XmlNode nsNode = children.SelectSingleNode("domain:ns", namespaces);

                if (nsNode != null)
                {
                    XmlNodeList hostObjNodes = nsNode.SelectNodes("domain:hostObj", namespaces);

                    if (hostObjNodes != null)
                    {
                        foreach (XmlNode hostObjNode in hostObjNodes)
                        {
                            if (hostObjNode != null)
                            {
                                Domain.NameServers.Add(hostObjNode.InnerText);
                            }
                        }
                    }

                    XmlNodeList hostAttrNodes = nsNode.SelectNodes("domain:hostAttr", namespaces);

                    if (hostAttrNodes != null)
                    {
                        foreach (XmlNode hostAttrNode in hostAttrNodes)
                        {
                            if (hostAttrNode != null)
                            {
                                XmlNodeList hostNames = hostAttrNode.SelectNodes("domain:hostName", namespaces);

                                if (hostNames != null)
                                {
                                    foreach (XmlNode hostName in hostNames)
                                    {
                                        Domain.NameServers.Add(hostName.InnerText);
                                    }
                                }
                            }
                        }
                    }
                }


                XmlNode clIdNode = children.SelectSingleNode("domain:clID", namespaces);

                if (clIdNode != null)
                {
                    Domain.ClId = clIdNode.InnerText;
                }

                XmlNode crIdNode = children.SelectSingleNode("domain:crID", namespaces);

                if (crIdNode != null)
                {
                    Domain.CrId = crIdNode.InnerText;
                }

                XmlNode crDateNode = children.SelectSingleNode("domain:crDate", namespaces);

                if (crDateNode != null)
                {
                    Domain.CrDate = crDateNode.InnerText;
                }

                XmlNode upIdNode = children.SelectSingleNode("domain:upID", namespaces);

                if (upIdNode != null)
                {
                    Domain.UpId = upIdNode.InnerText;
                }

                XmlNode upDateNode = children.SelectSingleNode("domain:upDate", namespaces);

                if (upDateNode != null)
                {
                    Domain.UpDate = upDateNode.InnerText;
                }

                XmlNode trDateNode = children.SelectSingleNode("domain:trDate", namespaces);

                if (trDateNode != null)
                {
                    Domain.TrDate = trDateNode.InnerText;
                }

                XmlNode exDateNode = children.SelectSingleNode("domain:exDate", namespaces);

                if (exDateNode != null)
                {
                    Domain.ExDate = exDateNode.InnerText;
                }

                //read authinfo .. maybe this should be part of all the objects  or not
                XmlNode passwordNode = children.SelectSingleNode("domain:authInfo/domain:pw", namespaces);

                if (passwordNode != null)
                {
                    Domain.Password = passwordNode.InnerText;
                }
            }
        }

        protected override void ProcessExtensionNode(XmlDocument doc, XmlNamespaceManager namespaces)
        {
            base.ProcessExtensionNode(doc, namespaces);
            const string prefix = "sec";

            namespaces.AddNamespace(prefix, "urn:ietf:params:xml:ns:secDNS-1.1");
            const string dsDataSelector = "/ns:epp/ns:response/ns:extension/sec:infData/sec:dsData";

            var dsDataNodes = doc.SelectNodes(dsDataSelector, namespaces)?.Cast<XmlElement>().ToList();
            if (dsDataNodes != null && dsDataNodes.Any())
            {
                SecDnsList = new List<SecDNSData>();
                foreach (XmlElement sourceDsDataNode in dsDataNodes)
                {
                    var keyTag = sourceDsDataNode.SelectSingleNode($"{prefix}:keyTag", namespaces);
                    var alg = sourceDsDataNode.SelectSingleNode($"{prefix}:alg", namespaces);
                    var digestType = sourceDsDataNode.SelectSingleNode($"{prefix}:digestType", namespaces);
                    var digest = sourceDsDataNode.SelectSingleNode($"{prefix}:digest", namespaces);
                    if (keyTag != null && 
                        alg != null && 
                        digestType != null && 
                        digest != null)
                    {
                        var secDnsData = new SecDNSData
                        {
                            KeyTag = ushort.Parse(keyTag.InnerText),
                            Algorithm = (SecDNSAlgorithm)Enum.Parse(typeof(SecDNSAlgorithm), alg.InnerText),
                            DigestType = (SecDNSDigestType)Enum.Parse(typeof(SecDNSDigestType), digestType.InnerText),
                            Digest = digest.InnerText
                        };
                        SecDnsList.Add(secDnsData);
                    }
                }
            }
        }

    }
}