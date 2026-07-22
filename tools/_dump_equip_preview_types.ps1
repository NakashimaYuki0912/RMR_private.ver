# Dump vanilla types related to equip-page hover preview
Add-Type -Path 'D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\dependencies\Mono.Cecil.dll'
$m = [Mono.Cecil.ModuleDefinition]::ReadModule('D:\VS_program\ruina-roguelike-reborn-main\ruina-roguelike-reborn-main\dependencies\Assembly-CSharp.dll')

foreach ($name in @('UISettingEquipPageInvenPanel','UIEquipPagePreviewPanel')) {
    $t = $m.Types | Where-Object { $_.Name -eq $name } | Select-Object -First 1
    if ($t -eq $null) { Write-Output "TYPE NOT FOUND: $name"; continue }
    Write-Output "=== $($t.FullName) : $($t.BaseType.FullName) ==="
    Write-Output '--- FIELDS ---'
    $t.Fields | ForEach-Object { Write-Output ("  " + $_.FieldType.Name + " " + $_.Name) }
    Write-Output '--- METHODS ---'
    $t.Methods | ForEach-Object { Write-Output ("  " + $_.ReturnType.Name + " " + $_.Name + "(" + (($_.Parameters | ForEach-Object { $_.ParameterType.Name }) -join ",") + ")") }
}

# Also list any type whose name contains Preview
Write-Output '=== Types containing Preview ==='
$m.Types | Where-Object { $_.Name -like '*Preview*' } | ForEach-Object { Write-Output ("  " + $_.FullName) }
