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
function AssertContains($name, $text, $needle) { if (-not $text.Contains($needle)) { throw "$name missing: $needle" } }

$logLike = ReadText 'abcdcode_LOGLIKE_MOD\LogLikeMod.cs'
$patches = ReadText 'abcdcode_Refactored\LogLikePatches.cs'

AssertContains 'extension text dictionary cleared before localized reload' $logLike 'TextDataModel.textDic.Clear()'
AssertContains 'extension text model marked loaded to prevent stale vanilla lazy-load' $logLike 'TextDataModel._isLoaded = true'
AssertContains 'game language separated from mod localization fallback' $logLike 'ResolveModLocalizeLanguage(language)'
AssertContains 'traditional chinese falls back to simplified mod localize' $logLike 'requested == "trcn"'
AssertContains 'japanese can fall back without changing game language' $logLike 'modLocalizeLanguage'
AssertContains 'satellite effect text uses mod localize fallback language' $logLike 'LoadSatelliteBattleTexts(localizeLanguage)'
AssertContains 'satellite dialog uses mod localize fallback language' $logLike 'LoadSatelliteBattleDialog(localizeLanguage)'
AssertContains 'mystery localization uses mod localize fallback language' $logLike 'RogueMysteryXmlList.Instance.Init(localizeLanguage)'
AssertContains 'japanese font probe checks kana not only kanji' $logLike 'return "\u65e5\u3042\u30a2\u6f22";'
AssertContains 'simplified chinese font probe checks common simplified glyphs' $logLike 'return "\u56fe\u6c49\u8bed\u6d4b\u8bd5";'
AssertContains 'traditional chinese font probe checks common traditional glyphs' $logLike 'return "\u5716\u6f22\u8a9e\u6e2c\u8a66";'
AssertContains 'korean font probe remains explicit' $logLike 'return "\ud55c\uae00\ub3c4";'
AssertContains 'simplified chinese UI prefers the native CJK TMP font' $logLike 'cnFont_notoSansCJKsc'
AssertContains 'preferred localized font blocks option dropdown font overwrite' $logLike 'ResolvePreferredLocalizedTmpFont(language)'
AssertContains 'preferred localized font selection is logged for Player.log verification' $logLike 'Using preferred TMP font'
AssertContains 'vanilla TextDataModel override is gated before reading mod text' $patches 'ShouldOverrideVanillaTextWithRmrText(id)'
AssertContains 'generic mod text only overrides while RMR is active' $patches 'LogLikeMod.CheckStage()'
AssertContains 'main menu RMR text remains available outside RMR stage' $patches 'id.StartsWith("ui_RMR_", StringComparison.Ordinal)'
AssertContains 'vanilla TextDataModel only overridden by non-empty mod dictionary values' $patches 'if (!(text != string.Empty))'
AssertContains 'RMR TMP font sanitizer rewrites existing UI text' $patches 'text.text = RewardingModel.SanitizeDisplayText(text.text)'
AssertContains 'battle card UI refreshes RMR TMP font after SetCard' $patches 'BattleDiceCardUI_SetCard_RmrFont'
AssertContains 'battle card UI SetCard patch uses the runtime two-argument signature' $patches 'typeof(BattleDiceCardModel), typeof(BattleDiceCardUI.Option[])'
AssertContains 'origin card slot refreshes RMR TMP font after SetData' $patches 'UIOriginCardSlot_SetData_RmrFont'
AssertContains 'battle dice behaviour description refreshes RMR TMP font after SetBehaviourInfo' $patches 'BattleDiceCard_BehaviourDescUI_SetBehaviourInfo_RmrFont'
AssertContains 'detail card dice description refreshes RMR TMP font after SetBehaviourInfo' $patches 'UIDetailCardDescSlot_SetBehaviourInfo_RmrFont'
AssertContains 'dice behaviour description patch uses the runtime four-argument signature' $patches 'typeof(DiceBehaviour), typeof(LorId), typeof(List<DiceBehaviour>), typeof(bool)'
AssertContains 'inventory card slot refreshes RMR TMP font after state update' $patches 'LogLikeRoutines.ApplyRmrTmpFont(self.gameObject)'
AssertContains 'passive succession popup refreshes RMR TMP font after open' $patches 'LogLikeRoutines.ApplyRmrTmpFont(UIPassiveSuccessionPopup.Instance?.gameObject)'

foreach ($file in @('Localize\cn\UIs.txt', 'Localize\en\UIs.txt', 'Localize\kr\UIs.txt')) {
    [xml](Get-Content -Raw -Encoding UTF8 (Join-Path $root $file)) | Out-Null
}

Write-Host 'RMR language sync static check passed.'

