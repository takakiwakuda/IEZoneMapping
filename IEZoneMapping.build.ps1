<#
.SYNOPSIS
    Build script for IEZoneMapping module.
#>
[CmdletBinding()]
param (
    [Parameter()]
    [ValidateSet("Debug", "Release")]
    [string]
    $Configuration = (property Configuration Release),

    [Parameter()]
    [ValidateSet("net462", "net7.0")]
    [string]
    $Framework
)

if ($Framework.Length -eq 0) {
    $Framework = if ($PSEdition -eq "Core") { "net7.0" } else { "net462" }
}

$PSCmdlet.WriteVerbose("Configuration : $Configuration")
$PSCmdlet.WriteVerbose("Framework     : $Framework")

<#
.SYNOPSIS
    Build IEZoneMapping assembly.
#>
task BuildIEZoneMapping @{
    Inputs  = {
        Get-ChildItem -Path src/IEZoneMapping/*.cs, src/IEZoneMapping/IEZoneMapping.csproj
    }
    Outputs = "src/IEZoneMapping/bin/$Configuration/$Framework/publish/IEZoneMapping.dll"
    Jobs    = {
        exec { dotnet publish -c $Configuration -f $Framework src/IEZoneMapping }
    }
}

<#
.SYNOPSIS
    Lay out IEZoneMapping module.
#>
task LayoutModule BuildIEZoneMapping, {
    $version = (Import-PowerShellDataFile -LiteralPath src/IEZoneMapping/IEZoneMapping.psd1).ModuleVersion
    $destination = "$PSScriptRoot/out/$Configuration/$Framework/IEZoneMapping/$version"
    $source = "$PSScriptRoot/src/IEZoneMapping/bin/$Configuration/$Framework"

    if (Test-Path -LiteralPath $destination -PathType Container) {
        Remove-Item -LiteralPath $destination -Recurse
    }
    $null = New-Item -Path $destination -ItemType Directory -Force

    Copy-Item -LiteralPath $source/IEZoneMapping.dll -Destination $destination
    Copy-Item -LiteralPath $source/IEZoneMapping.psd1 -Destination $destination
    Copy-Item -LiteralPath $source/IEZoneMapping.types.ps1xml -Destination $destination
    Copy-Item -LiteralPath $source/IEZoneMapping.format.ps1xml -Destination $destination
}

<#
.SYNOPSIS
    Run IEZoneMapping module tests.
#>
task RunModuleTest LayoutModule, {
    $command = @"
    & {
        Import-Module -Name '$PSScriptRoot/out/$Configuration/$Framework/IEZoneMapping';
        Invoke-Pester -Path '$PSScriptRoot/tests' -Output Detailed
    }
"@

    switch ($Framework) {
        "net7.0" {
            exec { pwsh -nop -c $command }
        }
        default {
            exec { powershell -noprofile -command $command }
        }
    }
}

<#
.SYNOPSIS
    Run default tasks.
#>
task . RunModuleTest
