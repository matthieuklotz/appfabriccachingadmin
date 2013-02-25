$solutionfile = "..\AppFabric.Admin.sln"
$msbuild = $env:windir + "\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe"
$msbuildargs = $solutionfile + " /t:rebuild /property:Configuration=Release /fl /flp:logfile=releasebuild.log"
Start-Process $msbuild $msbuildargs
