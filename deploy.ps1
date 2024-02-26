Set-Item wsman:\localhost\client\TrustedHosts -Value ${{ env.IP_DEPLOY }} -Force
Write-Output ${{ github.workspace }}
Get-ChildItem -Path ${{ github.workspace }}
$User = ${{ env.IP_DEPLOY }} + "\" + ${{ env.USERNAME_DEPLOY }}
$PWord = ConvertTo-SecureString -String ${{ env.PASSWORD_DEPLOY }} -AsPlainText -Force
$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $PWord
$Session = New-PSSession -ComputerName ${{ env.IP_DEPLOY }} -Credential $Credential
$ConfirmPreference = 'None'
$ServiceDescription = "Servicio Windows rca_gtm_synergy_suite_ServBoHExtInventarioTrasladoSS"
$Origin = ${{ github.workspace }}
$Base = ${{ env.BASE }}
$DirectoryDestiny = ${{ env.DIRECTORYDESTINY }}
$Destination = "$($Base)\$($DirectoryDestiny)\"
Write-Output "Destino:" + $Destination
$ServicePath = "$($Destination)\ServBoHExtInventarioTrasladoSS.exe"
$ServiceName = "Service_$($Destination)".Replace("\","_").Replace(":","").TrimEnd("_")
Write-Output "ServicePath: " + $ServicePath
get-Item WSMan:\localhost\Client\TrustedHosts
Test-NetConnection 172.191.231.77 -Port 5985


# $resultQueryDestiny = Invoke-Command -Session $Session –ScriptBlock {
#     param($Destination)
#     Get-ChildItem -Path $Destination -ErrorAction SilentlyContinue
# }-ArgumentList $Destination

# #Write-Output $resultQueryDestiny

# if(!($resultQueryDestiny))
# {
#     Invoke-Command -Session $Session –ScriptBlock {
#         param($Base, $DirectoryDestiny)
#         New-Item -Path $Base -Name $DirectoryDestiny -ItemType "directory" -ErrorAction SilentlyContinue
#     }-ArgumentList $Base, $DirectoryDestiny
# }

# $service = Invoke-Command -Session $Session –ScriptBlock {
#     param($ServiceName)
#     Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
# }-ArgumentList $ServiceName

# Write-Output $service.Status

# if(!($service))
# {
#     Write-Output "No hay servicio"
#     Copy-Item $Origin -Destination $Destination -ToSession $Session -Recurse -Force
#     Invoke-Command -Session $Session –ScriptBlock {
#         param($ServiceName,$ServicePath,$ServiceDescription)
#         New-Service -Name $ServiceName -BinaryPathName $ServicePath -Description $ServiceDescription
#         Set-Service -Name $ServiceName -StartupType Automatic
#     }-ArgumentList $ServiceName,$ServicePath,$ServiceDescription
# }
# else
# {
#     if (!($service.Started))
#     {
#         Write-Output "Servicio Existe"
#         Invoke-Command -Session $Session –ScriptBlock {
#             param($ServiceName)
#             Stop-Service -Name $ServiceName
#         }-ArgumentList $ServiceName
#         Copy-Item $Origin -Destination $Destination -ToSession $Session -Recurse -Force
#     } 
# }

# Invoke-Command -Session $Session –ScriptBlock {
#     param($ServiceName)
#     Start-Service -Name $ServiceName
# }-ArgumentList $ServiceName

# $service = Invoke-Command -Session $Session –ScriptBlock {
#     param($ServiceName)
#     Get-Service -Name $ServiceName -ErrorAction SilentlyContinue #$ServiceName
# }-ArgumentList $ServiceName

# Write-Output $service.Status