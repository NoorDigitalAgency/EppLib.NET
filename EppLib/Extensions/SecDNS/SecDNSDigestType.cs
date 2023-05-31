namespace EppLib.Extensions.SecDNS
{
    /// <summary>
    /// List of DNSSEC Digest Type
    /// Base on :
    /// https://datatracker.ietf.org/doc/html/rfc4034#appendix-A.2
    /// https://www.iana.org/assignments/ds-rr-types/ds-rr-types.xhtml
    /// </summary>

    public enum SecDNSDigestType
    {
        SHA_1 = 1,
        SHA_256 = 2,
        GOST_R_34_11_94 = 3,
        SHA_384 = 4
    }
}