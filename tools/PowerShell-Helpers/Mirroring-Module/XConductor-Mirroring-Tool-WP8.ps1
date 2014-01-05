Import-Module './XConductor-Mirroring-Tool.psm1' -DisableNameChecking

Mirror-Projects -sourcePath "d:\XConductor\XConductor\" -sourcePattern "*.csproj" -destinationPath "d:\XConductor\XConductor.WP8\" -destinationPattern "*WP8.csproj"