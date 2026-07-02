$ErrorActionPreference = 'Stop'

$script:StaticCheckScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$script:RepoRoot = $script:StaticCheckScriptDir
while ($script:RepoRoot -and -not (Test-Path (Join-Path $script:RepoRoot 'RogueLike Mod Reborn.csproj'))) {
    $script:RepoRoot = Split-Path -Parent $script:RepoRoot
}
if (-not $script:RepoRoot) {
    throw 'Could not locate repository root for static check.'
}
Set-Location $script:RepoRoot
$root = $script:RepoRoot
function ReadText($rel) { Get-Content -LiteralPath (Join-Path $root $rel) -Raw -Encoding UTF8 }
function AssertContains($name, $text, $needle) { if ($text -notlike "*$needle*") { throw "$name missing: $needle" } }
function AssertNotContains($name, $text, $needle) { if ($text -like "*$needle*") { throw "$name should not contain: $needle" } }

$unlock = ReadText 'RMR_AbnormalityUnlocks.cs'

AssertContains 'downward tier helper' $unlock 'IsRewardTierAvailableForChapter'
AssertContains 'reward candidates use downward tier helper' $unlock 'IsRewardTierAvailableForChapter(GetTierForScript(info.script), grade)'
AssertContains 'shop candidates use downward tier helper' $unlock 'IsRewardTierAvailableForChapter(GetTierForScript(info.script), grade)'
AssertNotContains 'reward candidates must not require exact current tier' $unlock '.Where(info => GetTierForScript(info.script) == tier)'
AssertContains 'normal drop chance helper' $unlock 'ShouldEnqueueNormalAbnormalityReward'
AssertContains 'urban legend reduced normal drop constant' $unlock 'UrbanLegendNormalAbnormalityRewardChance'
AssertContains 'urban plague reduced normal drop constant' $unlock 'UrbanPlagueNormalAbnormalityRewardChance'
AssertContains 'normal battle uses reduced chance gate' $unlock 'ShouldEnqueueNormalAbnormalityReward(LogLikeMod.curchaptergrade)'
AssertContains 'elite battles still enqueue reward' $unlock 'StageType.Elite'

Write-Host 'RMR downward abnormality pool static check passed.'

