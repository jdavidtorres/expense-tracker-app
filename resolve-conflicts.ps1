# PowerShell script to resolve merge conflicts by keeping HEAD (local) version
$files = Get-ChildItem -Path ExpenseTracker\ -Recurse -Include *.cs,*.xaml | Where-Object { (Get-Content $_.FullName -Raw) -match "<<<<<<< HEAD" }

foreach ($file in $files) {
    Write-Host "Resolving conflicts in: $($file.FullName)"
    $content = Get-Content $file.FullName -Raw
    
    # Remove conflict markers and keep only HEAD (local) version
    # Pattern explanation:
    # <<<<<<< HEAD marks start of local version
    # ======= marks separation between local and remote
    # >>>>>>> commit marks end of remote version
    
    # This regex keeps everything between <<<<<<< HEAD and =======, removes everything else
    $resolved = $content -replace '(?s)<<<<<<< HEAD\r?\n(.*?)\r?\n=======\r?\n.*?\r?\n>>>>>>> [^\r\n]+\r?\n?', '$1'
    
    # Write resolved content back to file
    Set-Content -Path $file.FullName -Value $resolved -NoNewline
}

Write-Host "All conflicts resolved by keeping local (HEAD) versions"