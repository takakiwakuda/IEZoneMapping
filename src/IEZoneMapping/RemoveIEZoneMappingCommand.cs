using System;
using System.Management.Automation;

namespace IEZoneMapping;

/// <summary>
/// The Remove-IEZoneMapping cmdlet removes a mapping from an Internet Explorer zone.
/// </summary>
[Cmdlet(VerbsCommon.Remove, "IEZoneMapping", SupportsShouldProcess = true)]
public sealed class RemoveIEZoneMappingCommand : PSCmdlet
{
    [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("Website")]
    public string[] Pattern
    {
        get => _patterns;
        set => _patterns = value;
    }

    /// <summary>
    /// Specifies the type of zone to which websites are mapped to.
    /// </summary>
    [Parameter(ValueFromPipelineByPropertyName = true)]
    public ZoneType ZoneType
    {
        get => _zoneType;
        set => _zoneType = value;
    }

    private string[] _patterns = Array.Empty<string>();
    private ZoneType _zoneType = ZoneType.TrustedSite;

    /// <summary>
    /// ProcessRecord override.
    /// </summary>
    protected override void ProcessRecord()
    {
        using InternetSecurityManager internetSecurityManager = new();

        foreach (string pattern in _patterns)
        {
            try
            {
                if (ShouldProcess(pattern, $"Remove from {_zoneType} zone"))
                {
                    internetSecurityManager.RemoveZoneMapping(pattern, _zoneType);
                }
            }
            catch (FormatException ex)
            {
                ErrorRecord errorRecord = new(ex, "InvalidWebsite", ErrorCategory.InvalidArgument, pattern);
                WriteError(errorRecord);
            }
        }
    }
}
