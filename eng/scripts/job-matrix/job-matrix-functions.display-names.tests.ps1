Import-Module ./job-matrix-functions.psm1 -Force
Import-Module Pester

BeforeAll {
    $matrixConfig = @"
{
    "displayNames": {
        "--enableFoo": "withfoo"
    },
    "matrix": {
        "operatingSystem": [
          "windows-2019",
          "ubuntu-18.04",
          "macOS-10.15"
        ],
        "framework": [
          "net461",
          "netcoreapp2.1"
        ],
        "additionalArguments": [
            "",
            "--enableFoo"
        ]
    },
    "include": [
        {
            "operatingSystem": "windows-2019",
            "framework": ["net461", "netcoreapp2.1", "net50"],
            "additionalArguments": "--enableWindowsFoo"
        }
    ],
    "exclude": [
        {
            "operatingSystem": "windows-2019",
            "framework": "net461"
        },
        {
            "operatingSystem": "macOS-10.15",
            "framework": "netcoreapp2.1"
        },
        {
            "operatingSystem": ["macOS-10.15", "ubuntu-18.04"],
            "additionalArguments": "--enableFoo"
        }
    ]
}
"@
    $config = GetMatrixConfigFromJson $matrixConfig
}

Describe "Matrix-Lookup" -Tag "filter" {
    It "Should filter by matrix display name" -TestCases @(
        @{ regex = "windows.*"; expectedFirst = "windows2019_netcoreapp21"; length = 5 }
        @{ regex = "windows2019_netcoreapp21_withfoo"; expectedFirst = "windows2019_netcoreapp21_withfoo"; length = 1 }
        @{ regex = "doesnotexist"; expectedFirst = $null; length = 0 }
        @{ regex = ".*ubuntu.*"; expectedFirst = "ubuntu1804_net461"; length = 2 }
    ) {
        $matrix = GenerateMatrix $config "all"
        [array]$filtered = FilterMatrixDisplayName $matrix $regex
        $filtered.Length | Should -Be $length
        if ($null -ne $filtered) {
            $filtered[0].Name | Should -Be $expectedFirst
        } else {
            $expectedFirst | Should -BeNullOrEmpty
        }
    }
}
