Import-Module './XConductor-Mirroring-Tool.psm1' -DisableNameChecking

Mirror-Projects -sourcePath "d:\XConductor\XConductor\" -sourcePattern "*.csproj" -destinationPath "d:\XConductor\XConductor.W8\" -destinationPattern "*W8.csproj"