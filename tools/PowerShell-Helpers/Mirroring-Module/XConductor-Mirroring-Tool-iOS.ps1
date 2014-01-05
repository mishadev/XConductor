Import-Module './XConductor-Mirroring-Tool.psm1' -DisableNameChecking

Mirror-Projects -sourcePath "d:\XConductor\XConductor\" -sourcePattern "*.csproj" -destinationPath "d:\XConductor\XConductor.iOS\" -destinationPattern "*iOS.csproj"