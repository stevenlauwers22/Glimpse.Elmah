#properties ---------------------------------------------------------------------------------------------------------

properties {
    $base_dir = resolve-path .
    $build_dir = "$base_dir\Builds"
    $source_dir = "$base_dir\Source"
    $tools_dir = "$base_dir\Tools"
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
    exec { msbuild $base_dir\Glimpse.Elmah.sln /p:Configuration=Release }
}

task package -depends build {
    "Creating Glimpse.Elmah.nupkg"
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $source_dir\Glimpse.Elmah\Package.nuspec -OutputDirectory $build_dir }

    "Creating Glimpse.Elmah.Sample.nupkg"
    exec { & $tools_dir\NuGet.CommandLine.2.1.0\tools\nuget.exe pack  $source_dir\Glimpse.Elmah.Sample\Package.nuspec -OutputDirectory $build_dir }
}

#functions ---------------------------------------------------------------------------------------------------------

function global:delete_directory($directory_name) {
    rd $directory_name -recurse -force -ErrorAction SilentlyContinue | out-null
}