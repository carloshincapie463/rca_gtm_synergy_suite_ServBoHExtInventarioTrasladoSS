
$filePath = $args[0]
$urlApiColas = $args[1]
$apiConexionDB = $args[2]
$nombreServicio = $args[3]
$servicioDestino = $args[4]
$conexionDB = $args[5]
$conexionDB2 = $args[6]
$correo = $args[7]
$idMenu = $args[8]
$ambiente = $args[9]
$directorioBitacoraErrores = $args[10]

write-host "There are a total of $($args.count) arguments"

for ($i = 0; $i -lt $args.Length; $i++)
{
    # Output the current item
    Write-Host $args[$i]
}

# Write-Output $filePath

Get-Content -Path $filePath

#Write-Output $urlApiColas
# file not found, nothing to do
if (-Not (Test-Path $filePath))
{
	Write-Output "File not found: $filePath"
	return
}

# read the config in xml
[System.Xml.XmlDocument]$doc = new-object System.Xml.XmlDocument
$doc.Load($filePath)

# find the connection string 
foreach($item in $doc.get_DocumentElement().appSettings.add)
{
	# use your name here
	Write-Output "$($item.key):$($item.value)"
	switch($item.key)
	{
		"urlApiColas" {$item.value = $urlApiColas}
		"apiConexionDB" {$item.value = $apiConexionDB}
		"nombreServicio" {$item.value = $nombreServicio}
		"servicioDestino" {$item.value = $servicioDestino}
		"conexionDB" {$item.value = $conexionDB}
		"conexionDB2" {$item.value = $conexionDB2}
		"correo" {$item.value = $correo}
		"idMenu" {$item.value = $idMenu}
		"ambiente" {$item.value = $ambiente}
		"directorioBitacoraErrores" {$item.value = $directorioBitacoraErrores}
	}        
}

# save the new file
$doc.Save($filePath)