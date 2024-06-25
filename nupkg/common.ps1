# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

function Write-Info   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Black -BackgroundColor Green

	try 
	{
	   $host.UI.RawUI.WindowTitle = $text
	}		
	catch 
	{
		#Changing window title is not suppoerted!
	}
}

function Write-Error   
{
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $text
    )

	Write-Host $text -ForegroundColor Red -BackgroundColor Black 
}

function Seperator   
{
	Write-Host ("_" * 100)  -ForegroundColor gray 
}

function Get-Current-Version { 
	$commonPropsFilePath = resolve-path "../common.props"
	$commonPropsXmlCurrent = [xml](Get-Content $commonPropsFilePath ) 
	$currentVersion = $commonPropsXmlCurrent.Project.PropertyGroup.Version.Trim()
	return $currentVersion
}

function Get-Current-Branch {
	return git branch --show-current
}	   

function Read-File {
	param(
        [Parameter(Mandatory = $true)]
        [string]
        $filePath
    )
		
	$pathExists = Test-Path -Path $filePath -PathType Leaf
	if ($pathExists)
	{
		return Get-Content $filePath		
	}
	else{
		Write-Error  "$filePath path does not exist!"
	}
}

# List of solutions
$solutions = (
    "framework"
)

# List of projects
$projects = (

    # framework
    "framework/src/BBT.Prism.AspNetCore",
    "framework/src/BBT.Prism.AspNetCore.Dapr",
    "framework/src/BBT.Prism.AspNetCore.Dapr.EventBus",
    "framework/src/BBT.Prism.AspNetCore.HealthChecks",
    "framework/src/BBT.Prism.AspNetCore.HealthChecks.Dapr",
    "framework/src/BBT.Prism.AspNetCore.Serilog",
    "framework/src/BBT.Prism.Audingt.Contracts",
    "framework/src/BBT.Prism.AutoMapper",
    "framework/src/BBT.Prism.Core",
    "framework/src/BBT.Prism.Dapr",
    "framework/src/BBT.Prism.Data",
    "framework/src/BBT.Prism.Data.Seeding",
    "framework/src/BBT.Prism.Ddd.Application",
    "framework/src/BBT.Prism.Ddd.Application.Contracts",
    "framework/src/BBT.Prism.Ddd.Domain",
    "framework/src/BBT.Prism.Ddd.Domain.Shared",
    "framework/src/BBT.Prism.EntityFrameworkCore",
    "framework/src/BBT.Prism.EntityFrameworkCore.Sqlite",
    "framework/src/BBT.Prism.EntityFrameworkCore.PostgreSql",
    "framework/src/BBT.Prism.EntityFrameworkCore.SqlServer",
    "framework/src/BBT.Prism.EventBus",
    "framework/src/BBT.Prism.EventBus.Abstractions",
    "framework/src/BBT.Prism.EventBus.Dapr",
    "framework/src/BBT.Prism.ExceptionHandling",
    "framework/src/BBT.Prism.Mapper",
    "framework/src/BBT.Prism.TestBase",
    "framework/src/BBT.Prism.Threading",
    "framework/src/BBT.Prism.Uow"   
)
