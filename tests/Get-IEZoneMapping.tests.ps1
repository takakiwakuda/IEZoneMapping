Set-StrictMode -Version 3.0

Describe "Get-IEZoneMapping tests" {
    It "Pass with the zone type '<ZoneType>'" -TestCases @(
        @{ ZoneType = "MyComputer" },
        @{ ZoneType = "LocalIntranet" },
        @{ ZoneType = "TrustedSite" },
        @{ ZoneType = "Internet" },
        @{ ZoneType = "RestrictedSite" }
    ) {
        { Get-IEZoneMapping -ZoneType $ZoneType } | Should -Not -Throw
    }
}
