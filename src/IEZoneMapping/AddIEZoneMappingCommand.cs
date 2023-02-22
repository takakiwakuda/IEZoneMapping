using System;
using System.IO;
using System.Management.Automation;

namespace IEZoneMapping;

/// <summary>
/// The Add-IEZoneMapping cmdlet adds a mapping to an Internet Explorer zone.
/// </summary>
[Cmdlet(VerbsCommon.Add, "IEZoneMapping", SupportsShouldProcess = true)]
public sealed class AddIEZoneMappingCommand : PSCmdlet
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
                if (ShouldProcess(pattern, $"Add to {_zoneType} zone"))
                {
                    internetSecurityManager.AddZoneMapping(pattern, _zoneType);
                }
            }
            catch (FormatException ex)
            {
                ErrorRecord errorRecord = new(ex, "InvalidWebsite", ErrorCategory.InvalidArgument, pattern);
                WriteError(errorRecord);
            }
            catch (IOException ex)
            {
                ErrorRecord errorRecord = new(ex, "WebsiteAlreadyMapped", ErrorCategory.WriteError, pattern);
                WriteError(errorRecord);
            }
        }
    }
}
