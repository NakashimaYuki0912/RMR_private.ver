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
function Read-Text([string]$path) {
    return [System.IO.File]::ReadAllText((Join-Path $script:RepoRoot $path), [System.Text.Encoding]::UTF8)
}

function Assert-Contains([string]$name, [string]$text, [string]$needle) {
    if ($text -notlike "*$needle*") {
        throw "FAIL: $name missing [$needle]"
    }
    Write-Host "PASS: $name"
}

function Assert-Xml([string]$path) {
    [xml]$xml = Read-Text $path
    Write-Host "PASS: XML parses $path"
    return $xml
}

$mysteryBase = Read-Text 'abcdcode_LOGLIKE_MOD\MysteryBase.cs'
$rmrEvents = Read-Text 'RMR_MysteryEvents.cs'

Assert-Contains 'RMR initial event script class exists' $rmrEvents 'class MysteryModel_RMR_ChStartNew : MysteryBase'
Assert-Contains 'RMR initial event opens realization panel' $rmrEvents 'case 6:'
Assert-Contains 'RMR initial event closes one-shot realization gate for normal relics' $rmrEvents 'SetInitialRelicEntryAvailable(false)'
Assert-Contains 'MysteryBase has safe frame root resolver' $mysteryBase 'private Transform GetMysteryFrameRoot()'
Assert-Contains 'MysteryBase has safe artwork resolver' $mysteryBase 'private Sprite GetArtworkOrFallback(string key)'
Assert-Contains 'MysteryBase creates mystery images through guarded helper' $mysteryBase 'private Image CreateMysteryImage'
Assert-Contains 'MysteryBase has safe choice desc resolver' $mysteryBase 'private string GetChoiceDescSafe(MysteryChoiceInfo choice)'
Assert-Contains 'MysteryBase guards missing xmlinfo or frame root' $mysteryBase 'if (this.xmlinfo == null || this.FrameObj == null)'
Assert-Contains 'MysteryBase guards missing frame id' $mysteryBase 'if (nextFrame == null)'
Assert-Contains 'MysteryBase handles null choice list' $mysteryBase 'this.curFrame.choices ?? new List<MysteryChoiceInfo>()'
Assert-Contains 'MysteryBase no longer dereferences localized choice chain directly' $mysteryBase 'localizedChoice != null && !string.IsNullOrEmpty(localizedChoice.Desc)'

$eventXml = Assert-Xml 'SpecialStaticInfo\MysteryXmlInfos\RMR_chstart.xml'
$cnXml = Assert-Xml 'Localize\cn\MysteryEvents\RMR_chstart.xml'
$enXml = Assert-Xml 'Localize\en\MysteryEvents\RMR_chstart.xml'
$krXml = Assert-Xml 'Localize\kr\MysteryEvents\RMR_chstart.xml'

$eventChoices = @($eventXml.SelectNodes("//Mystery[@ID='-100']/Frame[@ID='0']/Choice") | ForEach-Object { $_.ID })
if ($eventChoices -notcontains '6') {
    throw 'FAIL: initial event XML is missing realization choice 6'
}
Write-Host 'PASS: initial event XML includes realization choice 6'
foreach ($loc in @($cnXml, $enXml, $krXml)) {
    $locChoices = @($loc.SelectNodes("//Mystery[@ID='-100']/Frame[@ID='0']/Choice") | ForEach-Object { $_.ID })
    foreach ($id in $eventChoices) {
        if ($locChoices -notcontains $id) {
            throw "FAIL: localization is missing initial event choice $id"
        }
    }
}
Write-Host 'PASS: initial event localization choices match event XML'

