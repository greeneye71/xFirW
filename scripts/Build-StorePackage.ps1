# SPDX-License-Identifier: EUPL-1.2
[CmdletBinding()]
param(
    [string]$IdentityName,
    [string]$Publisher,
    [string]$PublisherDisplayName,
    [string]$DisplayName = 'xFirW',
    [string]$PackageVersion = '1.0.0.0',
    [switch]$Development
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path $PSScriptRoot -Parent

if ($Development) {
    $IdentityName = 'xFirW.Development'
    $Publisher = 'CN=xFirW Development'
    $PublisherDisplayName = 'xFirW Development'
} elseif ([string]::IsNullOrWhiteSpace($IdentityName) -or
          [string]::IsNullOrWhiteSpace($Publisher) -or
          [string]::IsNullOrWhiteSpace($PublisherDisplayName)) {
    throw 'Provide IdentityName, Publisher and PublisherDisplayName from Partner Center, or use -Development for local validation only.'
}
if ($IdentityName -notmatch '^[A-Za-z0-9.-]{3,50}$' -or $Publisher -notmatch '^CN=') {
    throw 'Invalid package identity. Copy the exact Name and Publisher from Partner Center.'
}
$parsedVersion = $null
if ($PackageVersion -notmatch '^\d+\.\d+\.\d+\.0$' -or
    -not [Version]::TryParse($PackageVersion, [ref]$parsedVersion)) {
    throw 'PackageVersion must contain four numbers and end in .0 (for example 1.0.0.0).'
}
if ($parsedVersion.Major -lt 1 -or $parsedVersion.Major -gt 65535 -or
    $parsedVersion.Minor -gt 65535 -or $parsedVersion.Build -gt 65535) {
    throw 'Store version: major must be 1..65535; minor/build must be 0..65535.'
}

$sdkRoot = Join-Path ${env:ProgramFiles(x86)} 'Windows Kits\10\bin'
$makeAppx = Get-ChildItem -LiteralPath $sdkRoot -Directory |
    Where-Object { $_.Name -match '^10\.0\.\d+\.0$' } |
    Sort-Object { [Version]$_.Name } -Descending |
    ForEach-Object { Join-Path $_.FullName 'x64\makeappx.exe' } |
    Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
if (-not $makeAppx) { throw 'Install the Windows SDK including MakeAppx.exe.' }
$dotnet = Join-Path $repo '.tools\dotnet\dotnet.exe'
if (-not (Test-Path -LiteralPath $dotnet)) { $dotnet = (Get-Command dotnet -ErrorAction Stop).Source }

# A fresh directory prevents stale binaries from entering the package; no existing output is deleted.
$kind = if ($Development) { 'development' } else { 'store' }
$buildRoot = Join-Path $repo ('artifacts\msix\{0}-{1}-{2}' -f $kind, $PackageVersion, [Guid]::NewGuid().ToString('N'))
$layout = Join-Path $buildRoot 'layout'
New-Item -ItemType Directory -Path $layout -Force | Out-Null
Push-Location $repo
try {
    & $dotnet publish src/Xfir.App/Xfir.App.csproj -c Release -r win-x64 --self-contained true `
        -p:RestoreLockedMode=true -p:PublishSingleFile=false -p:PublishTrimmed=false `
        -p:DebugType=None -p:DebugSymbols=false -o $layout
    if ($LASTEXITCODE -ne 0) { throw 'dotnet publish failed.' }
} finally { Pop-Location }
foreach ($required in @('xFirW.exe', 'coreclr.dll', 'hostfxr.dll', 'PresentationFramework.dll', 'LICENSE', 'LICENSE-IT.txt', 'THIRD-PARTY-NOTICES.md')) {
    if (-not (Test-Path -LiteralPath (Join-Path $layout $required))) { throw "Missing package component: $required" }
}
# User documents (real formulari, exported copies) must never be shipped inside the package.
$documents = Get-ChildItem -LiteralPath $layout -Recurse -File | Where-Object { $_.Extension -in '.xfir', '.pdf' }
if ($documents) { throw "Documents found in the package layout: $($documents.FullName -join ', ')" }

# Simple vector-style document mark, rendered at each required size without external assets.
Add-Type -AssemblyName System.Drawing
$assetDir = Join-Path $layout 'Assets'
New-Item -ItemType Directory -Path $assetDir | Out-Null
foreach ($asset in @(@('StoreLogo', 50), @('Square44x44Logo', 44), @('Square150x150Logo', 150))) {
    $size = [int]$asset[1]
    $bitmap = New-Object System.Drawing.Bitmap $size, $size
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $paper = New-Object System.Drawing.SolidBrush ([System.Drawing.Color]::FromArgb(244, 249, 250))
    $ink = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(22, 60, 74)), ([single]($size * 0.035))
    $accent = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(51, 189, 151)), ([single]($size * 0.06))
    try {
        $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
        $graphics.Clear([System.Drawing.Color]::FromArgb(22, 60, 74))
        $graphics.FillRectangle($paper, [single]($size * .25), [single]($size * .15), [single]($size * .5), [single]($size * .7))
        foreach ($y in @(.32, .43, .54)) {
            $graphics.DrawLine($ink, [single]($size * .35), [single]($size * $y), [single]($size * .65), [single]($size * $y))
        }
        $graphics.DrawLine($accent, [single]($size * .36), [single]($size * .69), [single]($size * .45), [single]($size * .77))
        $graphics.DrawLine($accent, [single]($size * .45), [single]($size * .77), [single]($size * .67), [single]($size * .61))
        $bitmap.Save((Join-Path $assetDir ($asset[0] + '.png')), [System.Drawing.Imaging.ImageFormat]::Png)
    } finally { $accent.Dispose(); $ink.Dispose(); $paper.Dispose(); $graphics.Dispose(); $bitmap.Dispose() }
}

[xml]$manifest = Get-Content -LiteralPath (Join-Path $repo 'packaging\windows\AppxManifest.xml') -Raw
$manifest.Package.Identity.Name = $IdentityName
$manifest.Package.Identity.Publisher = $Publisher
$manifest.Package.Identity.Version = $PackageVersion
$manifest.Package.Properties.DisplayName = $DisplayName
$manifest.Package.Properties.PublisherDisplayName = $PublisherDisplayName
$ns = New-Object System.Xml.XmlNamespaceManager $manifest.NameTable
$ns.AddNamespace('uap', 'http://schemas.microsoft.com/appx/manifest/uap/windows10')
$manifest.SelectSingleNode('//uap:VisualElements', $ns).SetAttribute('DisplayName', $DisplayName)
$manifest.Save((Join-Path $layout 'AppxManifest.xml'))

$package = Join-Path $buildRoot ("xFirW-$PackageVersion-x64-$kind.msix")
& $makeAppx pack /d $layout /p $package /o
if ($LASTEXITCODE -ne 0) { throw 'MakeAppx validation/packaging failed.' }
Get-FileHash -LiteralPath $package -Algorithm SHA256 | Format-List
Write-Host "Unsigned MSIX: $package"
if ($Development) { Write-Warning 'Development identity: DO NOT upload this package to Partner Center.' }
Write-Host 'Store signs accepted packages. Local installation requires a trusted test signature or developer registration of the layout.'
