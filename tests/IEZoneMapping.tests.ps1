Set-StrictMode -Version 3.0

Describe "Scenario testing for IEZoneMapping module" {
    It "Should be able to add, get, and remove the pattern '<Pattern>' in the <ZoneType> zone" -TestCases @(
        @{ Pattern = "https://aws.amazon.com", "https://azure.microsoft.com"; ZoneType = "TrustedSite" },
        @{ Pattern = "http://example.foo.com"; ZoneType = "RestrictedSite" }
    ) {
        { Add-IEZoneMapping -Pattern $Pattern -ZoneType $ZoneType -ErrorAction Stop } | Should -Not -Throw

        $zoneMappings = Get-IEZoneMapping -ZoneType $ZoneType
        $zoneMappings | Should -BeOfType "IEZoneMapping.ZoneMapping"
        $zoneMappings.Count | Should -Be $Pattern.Count

        { $zoneMappings | Remove-IEZoneMapping -ErrorAction Stop } | Should -Not -Throw

        $zoneMappings = Get-IEZoneMapping -ZoneType $ZoneType
        $zoneMappings | Should -BeNullOrEmpty
    }
}
