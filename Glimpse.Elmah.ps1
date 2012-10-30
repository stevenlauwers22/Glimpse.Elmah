#properties ---------------------------------------------------------------------------------------------------------

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\Builds\NuSpec"
    $build_output_dir = "$base_dir\Builds\NuGet"
    $source_dir = "$base_dir\Source"
    $tools_dir = "$base_dir\Tools"
    $config = "release"
}

#tasks -------------------------------------------------------------------------------------------------------------

task default -depends build

task clean {
    "Cleaning Glimpse.Elmah bin and obj directories"
    delete_directory "$source_dir\Glimpse.Elmah\bin"
    delete_directory "$source_dir\Glimpse.Elmah\obj"

    "Cleaning Glimpse.Elmah.Sample bin and obj directories"
    delete_directory "$source_dir\Glimpse.Elmah.Sample\bin"
    delete_directory "$source_dir\Glimpse.Elmah.Sample\obj"
}

task build -depends clean {
    "Building Glimpse.Elmah.sln"
    exec { msbuild $base_dir\Glimpse.Elmah.sln /p:Configuration=$config }
}

task package -depends build {
    "Creating Glimpse.Elmah.nupkg"
    xcopy $source_dir\Glimpse.Elmah\bin\$config\Glimpse.Elmah.dll $build_dir\Glimpse.Elmah\lib\net40\Glimpse.Elmah.dll /T /E /Y
    xcopy $source_dir\Glimpse.Elmah\Readme.txt $build_dir\Glimpse.Elmah\Content\App_Readme\Glimpse.Elmah.txt /T /E /Y
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $build_dir\Glimpse.Elmah\Glimpse.Elmah.nuspec -OutputDirectory $build_output_dir }

    "Creating Glimpse.Elmah.Sample.nupkg"
    xcopy $source_dir\Glimpse.Elmah.Sample\Errors.aspx $build_dir\Glimpse.Elmah.Sample\Content\Errors.aspx /T /E /Y
    xcopy $source_dir\Glimpse.Elmah.Sample\Errors.aspx.cs $build_dir\Glimpse.Elmah.Sample\Content\Errors.aspx.cs /T /E /Y
    xcopy $source_dir\Glimpse.Elmah.Sample\Errors.aspx.designer.cs $build_dir\Glimpse.Elmah.Sample\Content\Errors.aspx.designer.cs /T /E /Y
    xcopy $source_dir\Glimpse.Elmah.Sample\Readme.txt $build_dir\Glimpse.Elmah.Sample\Content\App_Readme\Glimpse.Elmah.Sample.txt /T /E /Y
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $build_dir\Glimpse.Elmah.Sample\Glimpse.Elmah.Sample.nuspec -OutputDirectory $build_output_dir }
}

#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name) {
    rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}