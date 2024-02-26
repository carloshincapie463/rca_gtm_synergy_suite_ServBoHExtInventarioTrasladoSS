$ServiceName = $args[0]
$ServicePath = $args[1]
$ServiceDescription = $args[2]

write-host "There are a total of $($args.count) arguments"

Write-Output $ServiceName
Write-Output $ServicePath
Write-Output $ServiceDescription