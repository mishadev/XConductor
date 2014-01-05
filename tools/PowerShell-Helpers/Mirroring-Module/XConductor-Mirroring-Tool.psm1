<#---------------------------------------------------
				EXPORTED FUNCTIONS
-----------------------------------------------------#>

function Mirror-Projects([string]$sourcePath, [string]$sourcePattern, [string]$destinationPath, [string]$destinationPattern)
{
    $sourceFiels = Get-Fiels -path $sourcePath -pattern $sourcePattern
    $destinationFiles = Get-Fiels -path $destinationPath -pattern $destinationPattern

    if(!$sourceFiels)
    {
        Write-Host "files not found by path $sourcePath" -ForegroundColor Red
        return
    }

    if(!$destinationFiles)
    {
        Write-Host "files not found by path $destinationPath" -ForegroundColor Red
        return
    }

    foreach($sourceFiel in $sourceFiels) 
    {
        $source = [IO.Path]::GetFileNameWithoutExtension($sourceFiel.FullName)
        foreach($destinationFile in $destinationFiles) 
        {
            $destination = [IO.Path]::GetFileNameWithoutExtension([IO.Path]::GetFileNameWithoutExtension($destinationFile.FullName))
            if([String]::Equals($source, $destination, [StringComparison]::InvariantCultureIgnoreCase))
            {
                Mirror-Fiels -sourceFile $sourceFiel -destinationFile $destinationFile
            }
        }
    }
}

<#---------------------------------------------------
				PRIVATE FUNCTIONS
-----------------------------------------------------#>

Add-Type -AssemblyName System.Xml.Linq
Add-Type -AssemblyName System

function Get-Fiels([string]$path, [string]$pattern)
{
    [Array]$fiels = @()
    
    if([System.IO.Directory]::Exists($path))
    {
        $fiels = Get-ChildItem -recurse ($path) -Filter $pattern
    }
    
    return $fiels
}

function Mirror-Fiels([IO.FileInfo]$sourceFile, [IO.FileInfo]$destinationFile)
{
    $sourceXml = [Xml.Linq.XDocument]::Load($sourceFile.FullName)
    $destinationXml = [Xml.Linq.XDocument]::Load($destinationFile.FullName)

    $path = Make-Relative-Path -fromPath $destinationFile.DirectoryName -toPath $sourceFile.DirectoryName 

    foreach($snode in $sourceXml.Descendants() | where { $_.Name.LocalName -eq "Compile" -and $_.Attribute("Include").Value.IndexOf("AssemblyInfo") -eq -1 })
    {
        $relPath = [IO.Path]::Combine("..\" + $path, $snode.Attribute("Include").Value);
        
        Remove-Exists-Files -node $snode -xml $destinationXml -relpath $relPath

        $group = ($destinationXml.Descendants() | where { $_.Name.LocalName -eq "Compile" } | select -f 1).Parent

        if($group)
        {
            Add-Files-Links -group $group -relpath $relPath -basepath $snode.Attribute("Include").Value
        }
    }
    
    $destinationXml.Save($destinationFile.FullName)
}

function Remove-Exists-Files([Xml.Linq.XElement] $node,[Xml.Linq.XDocument] $xml, [string] $relpath)
{
    foreach($dnode in $xml.Descendants() | where { $_.Name.LocalName -eq "Compile" -and ($_.Attribute("Include").Value -eq $node.Attribute("Include").Value -or $_.Attribute("Include").Value -eq $relpath) })
    {
        $dnode.Remove()
    }
}

function Add-Files-Links([Xml.Linq.XElement] $group, [string] $relpath, [string] $basepath)
{
    [System.Xml.Linq.XName] $xcompilename = "Compile"
    [System.Xml.Linq.XNamespace] $xcompilenamespace = $group.Name.Namespace   
    [System.Xml.Linq.XElement] $compile = $xcompilenamespace + $xcompilename

    [System.Xml.Linq.XName] $xlinkname = "Link"
    [System.Xml.Linq.XNamespace] $xlinknamespace = $group.Name.Namespace      
    [System.Xml.Linq.XElement] $link = $xlinknamespace + $xlinkname

    $compile.SetAttributeValue("Include", $relpath)
    $link.Add($basepath)

    $compile.Add($link)
    $group.Add($compile)
}

function Make-Relative-Path([string] $fromPath, [string] $toPath)
{
    [System.Uri] $fromUri = $fromPath
    [System.Uri] $toUri = $toPath
        
    $relativeUri = $fromUri.MakeRelativeUri($toUri);
    $relativePath = [Uri]::UnescapeDataString($relativeUri.ToString())

    return $relativePath.Replace("/", [IO.Path]::DirectorySeparatorChar)
}

<#---------------------------------------------------
				MODULE EXPORTS
-----------------------------------------------------#>

Export-ModuleMember Mirror-Projects 
