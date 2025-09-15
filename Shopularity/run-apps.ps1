if ($PSVersionTable.PSEdition -eq 'Desktop' -or [System.Runtime.InteropServices.RuntimeInformation]::IsOSPlatform([System.Runtime.InteropServices.OSPlatform]::Windows)) 
{
    # ---- Windows ----
    Start-Process cmd.exe -ArgumentList "/k cd /d Shopularity.Admin && title Shopularity.Admin && dotnet run -c Debug" -NoNewWindow:$false -Wait:$false
    Start-Process cmd.exe -ArgumentList "/k cd /d Shopularity.Public && title Shopularity.Public && dotnet run -c Debug" -NoNewWindow:$false -Wait:$false
}
else 
{
	 # ---- MAC ----
    Start-Process bash -ArgumentList "-c 'cd Shopularity.Admin && dotnet run -c Debug; exec bash'" -NoNewWindow:$false
    Start-Process bash -ArgumentList "-c 'cd Shopularity.Public && dotnet run -c Debug; exec bash'" -NoNewWindow:$false
}


