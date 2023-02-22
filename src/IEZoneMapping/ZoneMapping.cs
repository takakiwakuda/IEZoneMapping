using System;

namespace IEZoneMapping;

/// <summary>
/// Represnts information for a pattern mapped to an Internet Explorer zone.
/// </summary>
public sealed class ZoneMapping
{
    /// <summary>
    /// Gets the pattern mapped to the zone.
    /// </summary>
    public string Pattern => _pattern;

    /// <summary>
    /// Gets the zone type.
    /// </summary>
    public ZoneType ZoneType => _zoneType;

    private readonly string _pattern;
    private readonly ZoneType _zoneType;

    /// <summary>
    /// Initializes a new instance of the <see cref="ZoneMapping"/> class.
    /// </summary>
    /// <param name="pattern"></param>
    /// <param name="zoneType"></param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="pattern"/> is <see langword="null"/>.
    /// </exception>
    internal ZoneMapping(string pattern, ZoneType zoneType)
    {
        if (pattern is null)
        {
            throw new ArgumentNullException(nameof(pattern));
        }

        _pattern = pattern;
        _zoneType = zoneType;
    }

    /// <summary>
    /// Returns a string that represents the pattern of this zone mapping.
    /// </summary>
    /// <returns>
    /// A string that represents the pattern of this zone mapping.
    /// </returns>
    public override string ToString() => _pattern;
}
