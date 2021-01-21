Import-Module ./job-matrix-functions.psm1 -Force
Import-Module Pester

BeforeAll {
    $matrixConfig = @"
{
    "matrix": {
        "operatingSystem": [ "windows-2019", "ubuntu-18.04", "macOS-10.15" ],
        "framework": [ "net461", "netcoreapp2.1" ],
        "additionalArguments": [ "", "mode=test" ]
    }
}
"@
    $config = GetMatrixConfigFromJson $matrixConfig
}

Describe "Matrix Filter" -Tag "filter" {
    It "Should filter by matrix display name" -TestCases @(
        @{ regex = "windows.*"; expectedFirst = "windows2019_net461"; length = 4 }
        @{ regex = "windows2019_netcoreapp21_modetest"; expectedFirst = "windows2019_netcoreapp21_modetest"; length = 1 }
        @{ regex = ".*ubuntu.*"; expectedFirst = "ubuntu1804_net461"; length = 4 }
    ) {
        [array]$matrix = GenerateMatrix $config "all" $regex
        $matrix.Length | Should -Be $length
        $matrix[0].Name | Should -Be $expectedFirst
    }

    It "Should handle no display name filter matches" {
        $matrix = GenerateMatrix $config "all"
        [array]$filtered = FilterMatrixDisplayName $matrix "doesnotexist"
        $filtered | Should -BeNullOrEmpty
    }

    It "Should filter by matrix key/value" -TestCases @(
        @{ filterString = "operatingSystem=windows.*"; expectedFirst = "windows2019_net461"; length = 4 }
        @{ filterString = "operatingSystem=windows-2019"; expectedFirst = "windows2019_net461"; length = 4 }
        @{ filterString = "framework=.*"; expectedFirst = "windows2019_net461"; length = 12 }
        @{ filterString = "additionalArguments=mode=test"; expectedFirst = "windows2019_net461_modetest"; length = 6 }
        @{ filterString = "additionalArguments=^$"; expectedFirst = "windows2019_net461"; length = 6 }
    ) {
        [array]$matrix = GenerateMatrix $config "all" -filters @($filterString)
        $matrix.Length | Should -Be $length
        $matrix[0].Name | Should -Be $expectedFirst
    }

    It "Should handle multiple matrix key/value filters " {
        [array]$matrix = GenerateMatrix $config "all" -filters "operatingSystem=windows.*","framework=.*","additionalArguments=mode=test"
        $matrix.Length | Should -Be 2
        $matrix[0].Name | Should -Be "windows2019_net461_modetest"
    }

    It "Should handle no matrix key/value filter matches" {
        [array]$matrix = GenerateMatrix $config "all" -filters @("doesnot=exist")
        $matrix | Should -BeNullOrEmpty
    }

    It "Should handle invalid matrix key/value filter syntax" {
        { GenerateMatrix $config "all" -filters @("invalid") } | Should -Throw
    }

    It "Should filter by key exclude" {
        [array]$matrix = GenerateMatrix $config "all" -filters @("!operatingSystem")
        $matrix.Length | Should -Be 0
    }
}
