Set-StrictMode -Version 3.0

Describe "Remove-IEZoneMapping tests" {
    It "Throws an exception if the pattern '<Pattern>' is invalid" -TestCases @(
        @{ Pattern = "https://??" }
    ) {
        $errorRecord = { Remove-IEZoneMapping -Pattern $Pattern -ErrorAction Stop } | Should -Throw -PassThru

        $errorRecord.Exception | Should -BeOfType "System.FormatException"
        $errorRecord.FullyQualifiedErrorId | Should -Be "InvalidWebsite,IEZoneMapping.RemoveIEZoneMappingCommand"
    }

    It "Should not throw an exception even if the pattern is not stored in a zone" {
        { Remove-IEZoneMapping -Pattern "http://does.not.exsit" -ErrorAction Stop } | Should -Not -Throw
    }
}
