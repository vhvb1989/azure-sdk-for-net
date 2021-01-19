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
        @{ regex = "windows.*"; expectedFirst = "windows2019_netcoreapp21"; length = 3 }
        @{ regex = "windows2019_netcoreapp21_withfoo"; expectedFirst = "windows2019_netcoreapp21"; length = 1 }
    ) {
        $matrix = GenerateMatrix $config "all"
        $filtered = FilterMatrixDisplayName $matrix $regex
        $filtered.Length | Should -Be $length
        $filtered[0].Name | Should -Be $expectedFirst
    }
}
