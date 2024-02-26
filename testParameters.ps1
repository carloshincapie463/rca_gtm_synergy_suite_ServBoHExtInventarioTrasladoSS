# $ServiceName = $args[0]
# $ServicePath = $args[1]
# $ServiceDescription = $args[2]

write-host "There are a total of $($args.count) arguments"

for ($i = 0; $i -lt $args.Length; $i++)
{
    # Output the current item
    Write-Host $args[$i]
}

# Write-Output $ServiceName
# Write-Output $ServicePath
# Write-Output $ServiceDescription