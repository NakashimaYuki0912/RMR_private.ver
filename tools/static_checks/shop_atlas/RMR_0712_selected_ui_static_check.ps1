$ErrorActionPreference = "Stop"

$repo = (Resolve-Path (Join-Path $PSScriptRoot "..\..\..")).Path
$hubPath = Join-Path $repo "RMR_StartHubPanel.cs"
$helpPath = Join-Path $repo "RMR_HelpHandbookPanel.cs"
$atlasPath = Join-Path $repo "abcdcode_LOGLIKE_MOD\LogAtlasPanel.cs"
$uiPaths = @("cn", "en", "kr") | ForEach-Object { Join-Path $repo ("Localize\" + $_ + "\UIs.txt") }

$utf8 = [System.Text.UTF8Encoding]::new($false, $true)
foreach ($path in @($hubPath, $helpPath, $atlasPath) + $uiPaths) {
    [void]$utf8.GetString([System.IO.File]::ReadAllBytes($path))
}
foreach ($path in $uiPaths) {
    [xml]$uiXml = Get-Content -LiteralPath $path -Raw -Encoding UTF8
    $duplicateIds = $uiXml.localize.text | Group-Object id | Where-Object Count -gt 1
    if ($duplicateIds) {
        throw ("Duplicate localization ids in " + $path + ": " + (($duplicateIds.Name) -join ", "))
    }
    foreach ($key in @("ui_RMR_Help_Body_Overview", "ui_RMR_Help_Body_Route", "ui_RMR_Help_Body_Rewards", "ui_RMR_Help_Body_Shop", "ui_RMR_Help_Body_Atlas", "ui_RMR_Help_Body_Realization", "ui_RMR_Atlas_Title", "ui_RMR_Atlas_Progress", "ui_RMR_Atlas_Unlocked")) {
        if (-not ($uiXml.localize.text | Where-Object id -eq $key)) {
            throw ("Missing localization key " + $key + " in " + $path)
        }
    }
}

$hub = Get-Content -LiteralPath $hubPath -Raw -Encoding UTF8
$help = Get-Content -LiteralPath $helpPath -Raw -Encoding UTF8
$atlas = Get-Content -LiteralPath $atlasPath -Raw -Encoding UTF8

$checks = @(
    @{ Name = "hub invitation backdrop"; Pass = $hub.Contains('InvitationBackdrop') },
    @{ Name = "hub left identity sigil"; Pass = $hub.Contains('AddInvitationSigil') },
    @{ Name = "hub right action index"; Pass = $hub.Contains('RECEPTION INDEX') },
    @{ Name = "help selected player copy"; Pass = $help.Contains('BuildPlayerPages()') },
    @{ Name = "help illustrated nav"; Pass = $help.Contains('NavIcon') -and $help.Contains('Banner') },
    @{ Name = "help continuous scroll"; Pass = $help.Contains('ScrollRect') -and $help.Contains('ApplyBodyText') },
    @{ Name = "help has no body pager"; Pass = -not $help.Contains('MakePageBtn(') -and -not $help.Contains('_pageLabel') -and -not $help.Contains('SplitBodyPages') },
    @{ Name = "atlas archive wall grid"; Pass = $atlas.Contains('CenterCols = 4') -and $atlas.Contains('CenterRows = 5') },
    @{ Name = "atlas paging below wall"; Pass = $atlas.Contains('const float pagingY = -315f') },
    @{ Name = "atlas flat categories"; Pass = $atlas.Contains('currentCategory == AtlasCategory.AbnormalityPage') -and $atlas.Contains('currentCategory == AtlasCategory.EgoPage') },
    @{ Name = "atlas hides progress header"; Pass = $atlas.Contains('sectionHeader.gameObject.SetActive(!flatCategory)') },
    @{ Name = "atlas ignores progress filter for flat categories"; Pass = $atlas.Contains('flatCategory || currentSection == AtlasSection.All || x.Section == currentSection') },
    @{ Name = "atlas resets flat category progress"; Pass = $atlas.Contains('currentSection = AtlasSection.All') },
    @{ Name = "sharp TMP path"; Pass = $hub.Contains('ApplyTmpFontPreservingSharpMaterial') -and $help.Contains('ApplyTmpFontPreservingSharpMaterial') -and $atlas.Contains('ApplyTmpFontPreservingSharpMaterial') },
    @{ Name = "no synthetic bold in selected panels"; Pass = $hub -notmatch 'fontStyle\s*=\s*FontStyles\.Bold' -and $help -notmatch 'fontStyle\s*=\s*FontStyles\.Bold' -and $atlas -notmatch 'fontStyle\s*=\s*FontStyles\.Bold' }
)

$failed = @()
foreach ($check in $checks) {
    if ($check.Pass) {
        Write-Output ("PASS " + $check.Name)
    }
    else {
        Write-Output ("FAIL " + $check.Name)
        $failed += $check.Name
    }
}

if ($failed.Count -gt 0) {
    throw ("Selected UI static check failed: " + ($failed -join ", "))
}

Write-Output "PASS UTF-8 strict decode and cn/en/kr UI XML parse"
