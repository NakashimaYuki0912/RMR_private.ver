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
function AssertContains($name, $text, $needle) {
    if ($text -notlike "*$needle*") { throw "$name missing: $needle" }
}

$unlock = ReadText 'RMR_AbnormalityUnlocks.cs'
$realization = ReadText 'RMR_RealizationManager.cs'
$mystery = ReadText 'RMR_MysteryEvents.cs'
$core = ReadText 'RMR_Core.cs'

$floorScripts = @{
    Malkuth = @('snowwhite')
    Yesod = @('SingingMachine1', 'Butterfly3', 'freischutz3')
    Hod = @('blackswan')
    Netzach = @('orchestra')
    Tiphereth = @('clownofnihil')
    Gebura = @('nothing')
    Chesed = @('wizard')
    Binah = @('bossbird')
    Hokma = @('onebadmanygood', 'plaguedoctor', 'whitenight')
    Keter = @('quietKid')
}
foreach ($floor in $floorScripts.Keys) {
    AssertContains "reward script floor $floor" $unlock "SephirahType.$floor"
    foreach ($script in $floorScripts[$floor]) {
        AssertContains "reward script $floor/$script" $unlock "`"$script`""
    }
}

$egoIdsByFloor = @{
    Malkuth = 910001..910005
    Yesod = 910011..910015
    Hod = 910016..910020
    Netzach = 910021..910025
    Tiphereth = 910026..910030
    Gebura = 910031..910035
    Chesed = 910036..910040
    Binah = 910041..910045
    Hokma = 910046..910050
    Keter = 910086..910090
}
foreach ($floor in $egoIdsByFloor.Keys) {
    foreach ($id in $egoIdsByFloor[$floor]) {
        AssertContains "EGO $floor/$id" $unlock "new LorId($id)"
    }
}

AssertContains 'realization script prefix matching' $unlock 'script.StartsWith(configuredScript, StringComparison.OrdinalIgnoreCase)'
AssertContains 'initial entry flag declaration' $realization 'InitialRelicEntryAvailable'
AssertContains 'initial entry setter' $realization 'SetInitialRelicEntryAvailable'
AssertContains 'initial entry set before first relic mystery' $core 'RMRRealizationManager.SetInitialRelicEntryAvailable(true)'
AssertContains 'initial choice gate' $mystery 'RMRRealizationManager.InitialRelicEntryAvailable'
AssertContains 'normal relic choice consumes entry' $mystery 'RMRRealizationManager.SetInitialRelicEntryAvailable(false)'

$chStartClass = [regex]::Match($mystery, 'public class MysteryModel_RMR_ChStartNew[\s\S]*?public override void OnEnterChoice')
if (-not $chStartClass.Success) { throw 'ChStart class block not found' }
AssertContains 'ChStart class consumes entry' $chStartClass.Value 'RMRRealizationManager.SetInitialRelicEntryAvailable(false)'
$abnoBaseClass = [regex]::Match($mystery, 'public abstract class MysteryModel_RMR_AbnoRewardBase[\s\S]*?public class MysteryModel_RMR_AbnoBlackForest')
if (-not $abnoBaseClass.Success) { throw 'Abno reward base block not found' }
if ($abnoBaseClass.Value -like '*SetInitialRelicEntryAvailable(false)*') { throw 'Abno reward base must not consume realization entry' }
Write-Host 'RMR all-floor realization static check passed.'

