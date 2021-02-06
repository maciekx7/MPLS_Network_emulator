#!/bin/bash
 
 
osascript -e 'tell app "Terminal"
    do script " 
    cd Desktop/programowanie/cSharp/TSST/tsst_proj2
    dotnet ./Host/bin/Debug/netcoreapp3.1/Host.dll './Config/Host/host1.json'"
    set bounds of front window to {0, 0, 500, 300}
end tell'


osascript -e 'tell app "Terminal"
    do script " 
    cd Desktop/programowanie/cSharp/TSST/tsst_proj2
    dotnet ./Host/bin/Debug/netcoreapp3.1/Host.dll './Config/Host/host2.json'"
    set bounds of front window to {0, 0, 500, 300}
end tell'



 