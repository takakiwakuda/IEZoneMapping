using System.Management.Automation;

namespace IEZoneMapping;

/// <summary>
/// The Get-IEZoneMapping cmdlet gets Internet Explorer zone mappings.
/// </summary>
[Cmdlet(VerbsCommon.Get, "IEZoneMapping")]
[OutputType(typeof(ZoneMapping))]
public sealed class GetIEZoneMappingCommand : PSCmdlet
{
    /// <summary>
    /// Specifies the type of zone to which websites are mapped to.
    /// </summary>
    [Parameter]
    public ZoneType ZoneType
    {
        get => _zoneType;
        set => _zoneType = value;
    }

    private ZoneType _zoneType = ZoneType.TrustedSite;

    /// <summary>
    /// ProcessRecord override.
    /// </summary>
    protected override void ProcessRecord()
    {
        using InternetSecurityManager internetSecurityManager = new();
        WriteObject(internetSecurityManager.GetZoneMappings(_zoneType), true);
    }
}
