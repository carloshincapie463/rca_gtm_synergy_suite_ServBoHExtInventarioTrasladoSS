$IP_DEPLOY = $args[0]
$Origin = $args[1]
$USERNAME_DEPLOY = $args[2]
$PASSWORD_DEPLOY = $args[3]
$BASE = $args[4]
$DIRECTORYDESTINY = $args[5]

write-host "There are a total of $($args.count) arguments"

for ($i = 0; $i -lt $args.Length; $i++)
{
    # Output the current item
    Write-Host $args[$i]
}

Write-Output $Origin
Get-ChildItem $Origin