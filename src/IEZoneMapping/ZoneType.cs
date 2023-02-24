namespace IEZoneMapping;

/// <summary>
/// Specifies a zone to which a pattern is mapped.
/// </summary>
public enum ZoneType
{
    /// <summary>
    /// My Computer zone.
    /// </summary>
    MyComputer = 0,
    /// <summary>
    /// Intranet zone.
    /// </summary>
    LocalIntranet = 1,
    /// <summary>
    /// Trusted Sites zone.
    /// </summary>
    TrustedSite = 2,
    /// <summary>
    /// Internet zone.
    /// </summary>
    Internet = 3,
    /// <summary>
    /// Restricted Sites zone.
    /// </summary>
    RestrictedSite = 4,
    /// <summary>
    /// Invalid zone.
    /// </summary>
    Invalid = -1,
}
