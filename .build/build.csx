using DotNetBuild.Core.Facilities.Logging;
using DotNetBuild.Core.Facilities.State;
using DotNetBuild.Tasks;
using DotNetBuild.Tasks.NuGet;

var dotNetBuild = Require<DotNetBuildScriptPackContext>();

dotNetBuild.AddTarget("ci", "Continuous integration target", c 
    => c.DependsOn("updateVersionNumber")
        .And("buildRelease")
        .And("createPackage")
);

dotNetBuild.AddTarget("updateVersionNumber", "Update version number", c 
    => c.Do(context => {
            var solutionDirectory = context.ConfigurationSettings.Get<String>("SolutionDirectory");
            const String assemblyMajorVersion = "1";
            const String assemblyMinorVersion = "1";
            const String assemblyBuildNumber = "1";
            var assemblyInfoTask = new AssemblyInfo
            {
                AssemblyInfoFiles = new[]
                {
                    Path.Combine(solutionDirectory, @"Glimpse.Elmah\Properties\AssemblyInfo.cs")
                },
                AssemblyInformationalVersion = String.Format("{0}.{1}.{2}", assemblyMajorVersion, assemblyMinorVersion, assemblyBuildNumber),
                UpdateAssemblyInformationalVersion = true,
                AssemblyMajorVersion = assemblyMajorVersion,
                AssemblyMinorVersion = assemblyMinorVersion,
                AssemblyBuildNumber = assemblyBuildNumber,
                AssemblyRevision = "0",
                AssemblyFileMajorVersion = assemblyMajorVersion,
                AssemblyFileMinorVersion = assemblyMinorVersion,
                AssemblyFileBuildNumber = assemblyBuildNumber,
                AssemblyFileRevision = "0"
            };

            var result = assemblyInfoTask.Execute();
            context.FacilityProvider.Get<ILogger>().LogInfo("Building assembly version: " + assemblyInfoTask.MaxAssemblyVersion);
            context.FacilityProvider.Get<ILogger>().LogInfo("Building assembly informational version: " + assemblyInfoTask.AssemblyInformationalVersion);
            context.FacilityProvider.Get<IStateWriter>().Add("VersionNumber", assemblyInfoTask.AssemblyInformationalVersion);

            return result;
		}));

dotNetBuild.AddTarget("buildRelease", "Build in release mode", c 
	=> c.Do(context => {
            var solutionDirectory = context.ConfigurationSettings.Get<String>("SolutionDirectory");
			var msBuildTask = new MsBuildTask
			{
				Project = Path.Combine(solutionDirectory, "Glimpse.Elmah.sln"),
				Target = "Rebuild",
				Parameters = "Configuration=Release"
			};

			return msBuildTask.Execute();
		}));

dotNetBuild.AddTarget("createPackage", "Create NuGet package", c 
    => c.Do(context => {
            var solutionDirectory = context.ConfigurationSettings.Get<String>("SolutionDirectory");
            var nugetExe = context.ConfigurationSettings.Get<String>("PathToNuGetExe");
            var nugetPackTask = new Pack
            {
                NuGetExe = Path.Combine(solutionDirectory, nugetExe),
                NuSpecFile = Path.Combine(solutionDirectory, @"Glimpse.Elmah\package.nuspec"),
                OutputDir = Path.Combine(solutionDirectory, @"packagesForNuGet\"),
                Version = context.FacilityProvider.Get<IStateReader>().Get<String>("VersionNumber")
            };

            return nugetPackTask.Execute();
		}));

dotNetBuild.AddTarget("deploy", "Deploy to NuGet", c
	=> c.DependsOn("publishPackage"));

dotNetBuild.AddTarget("publishPackage", "Publish NuGet package", c
	=> c.Do(context => {
            var solutionDirectory = context.ConfigurationSettings.Get<String>("SolutionDirectory");
            var nugetExe = context.ConfigurationSettings.Get<String>("PathToNuGetExe");
            var nugetApiKey = context.ConfigurationSettings.Get<String>("NuGetApiKey");
            var nupkgFile = string.Format(@"packagesForNuGet\Glimpse.Elmah.{0}.nupkg", context.ParameterProvider.Get("VersionNumber"));
            var nugetPackTask = new Push
            {
                NuGetExe = Path.Combine(solutionDirectory, nugetExe),
                NuPkgFile = Path.Combine(solutionDirectory, nupkgFile),
                ApiKey = nugetApiKey
            };

            return nugetPackTask.Execute();
        }));

dotNetBuild.AddConfiguration("defaultConfig", c 
	=> c.AddSetting("SolutionDirectory", @"..\")
        .AddSetting("PathToNuGetExe", @"packages\NuGet.CommandLine.2.8.2\tools\NuGet.exe")
        .AddSetting("NuGetApiKey", "")
);

dotNetBuild.RunFromScriptArguments();