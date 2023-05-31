namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// List of DNSSEC Algorithms
    /// based on :
    /// https://datatracker.ietf.org/doc/html/rfc4034#appendix-A  and 
    /// https://www.iana.org/assignments/dns-sec-alg-numbers/dns-sec-alg-numbers.xhtml
    /// </summary>
    public enum SecDNSAlgorithm
    {
        RSAMD5 = 1,
        DH = 2,
        DSA = 3,
        ECC = 4,
        RSASHA1 = 5,
        DSA_NSEC3_SHA1 = 6,
        RSASHA1_NSEC3_SHA1 = 7,
        RSASHA256 = 8,
        RSASHA512 = 10,
        ECC_GOST = 12,
        ECDSAP256SHA256 = 13,
        ECDSAP384SHA384 = 14,
        ED25519 = 15,
        ED448 = 16,
        INDIRECT = 252,
        PRIVATEDNS = 253,
        PRIVATEOID = 254
    }
}