Set-StrictMode -Version 3.0

Describe "Add-IEZoneMapping tests" {
    It "Throws an exception if the pattern '<Pattern>' is invalid" -TestCases @(
        @{ Pattern = "test" },
        @{ Pattern = "http://test" },
        @{ Pattern = "https://??" }
    ) {
        $errorRecord = { Add-IEZoneMapping -Pattern $Pattern -ErrorAction Stop } | Should -Throw -PassThru

        $errorRecord.Exception | Should -BeOfType "System.FormatException"
        $errorRecord.FullyQualifiedErrorId | Should -Be "InvalidWebsite,IEZoneMapping.AddIEZoneMappingCommand"
    }

    It "Throws an exception if the pattern '<Pattern>' is already mapped" -TestCases @(
        @{ Pattern = "https://www.google.com" }
    ) {
        Add-IEZoneMapping -Pattern $Pattern -ErrorAction Ignore

        try {
            $errorRecord = { Add-IEZoneMapping -Pattern $Pattern -ErrorAction Stop } | Should -Throw -PassThru

            $errorRecord.Exception | Should -BeOfType "System.IO.IOException"
            $errorRecord.FullyQualifiedErrorId | Should -Be "WebsiteAlreadyMapped,IEZoneMapping.AddIEZoneMappingCommand"
        } finally {
            Remove-IEZoneMapping -Pattern $Pattern -ErrorAction Ignore
        }
    }
}
