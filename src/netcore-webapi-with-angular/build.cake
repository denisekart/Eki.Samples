#load "../../build/scripts/args.cake"
#addin "nuget:?package=Cake.Npm&version=0.16.0"


#region ARGUMENTS
var args=new ArgBuilder(Context,"cake_","build.json")
.Help("This is an utility for setting up the environment")
.Help("-------------------------------------------------");

var target = args
   .Arg("target").Arg("t")
   .Env("target").File("build.target")
   .Default("Default")
   .Help(null,true)
   .Help("\tThe target to execute")
   .Bundle();

var configuration=args
    .Arg("configuration").Arg("c")
    .Env("configuration").File("build.configuration")
    .Default("Debug")
    .Help(null, true)
    .Help("\tThe configuration to use")
    .Bundle();

var output=args
    .Arg("output").Arg("o")
    .Env("output").File("build.output")
    .Default("./artifacts")
    .Help(null,true)
    .Help("\tThe relative output folder")
    .Bundle();

var package=args
    .Arg("packageDirectory").Arg("p")
    .Env("packageDirectory").File("build.packageDirectory")
    .Default("./packages")
    .Help(null,true)
    .Help("\tThe relative output folder")
    .Bundle();

#endregion //ARGUMENTS


#region TASKS
var cleanTask=Task("Clean")
   .Does(()=>{
      if(DirectoryExists(output.Build()))
         DeleteDirectory(output.Build(),true);
      if(DirectoryExists(package.Build()))
         DeleteDirectory(package.Build(),true);
   });

var installClientToolingTask=Task("InstallClientTooling")
.Does(()=>{
   Information("Installing angular cli...");
   var setup=new NpmInstallSettings{
      Global=true,
   };
   setup.Packages.Add("@angular/cli");
   NpmInstall(setup);
});
var restoreTask=Task("Restore")
.Does(()=>{
    Information("Running restore...");
    DotNetCoreRestore();
    Information("Running npm install...");
    NpmInstall(settings=>{
    settings.WorkingDirectory=Directory("./sample-client");
    settings.Production=configuration.Build()=="Release";
    });
    RunTarget("InstallClientTooling");
   });
var buildTask=Task("Build")
   //  .IsDependentOn("InstallClientTooling")
    .IsDependentOn("Restore")
    .Does(()=>{
       Information("Running build for server...");
       var relativeOutputRoot=Directory(output.Build());
       DotNetCoreBuild(File("SampleApi/SampleApi.csproj"),new DotNetCoreBuildSettings{
          Configuration=configuration.Build(),
          OutputDirectory=relativeOutputRoot,
          NoRestore=true
       });
      Information("Running build for client...");
      
      NpmRunScript(configuration.Build()=="Release"?"build-prod":"build",settings=>{
         settings.Arguments.Add("--base-href=/client/");
         settings.Arguments.Add("--output-path="+"./../"+relativeOutputRoot+"/sample-client");
         settings.WorkingDirectory=Directory("./sample-client");
      });

    });

var publishTask=Task("Publish")
   .IsDependentOn("Build")
   .Does(()=>{
      Information("Running publish...");
      var relativeOutputRoot=Directory(output.Build()).Path+"/package";

      CreateDirectory(package.Build());

      DotNetCorePublish("SampleApi/SampleApi.csproj",new DotNetCorePublishSettings{
         NoBuild=true,
         Configuration=configuration.Build(),
         OutputDirectory=relativeOutputRoot,
      });

      CopyDirectory(output.Build()+"/sample-client", relativeOutputRoot+"/sample-client");
      Zip(relativeOutputRoot, File(package.Build()+"/SampleApi.zip"));
      
      DeleteDirectory(relativeOutputRoot,new DeleteDirectorySettings{Recursive=true});

   });
var testTask=Task("Test")
    .IsDependentOn("Build")
    .Does(()=>{
    Information("Running tests...");
    });
var defaultTask=Task("Default")
    .IsDependentOn("Test")
    .Does(()=>{
    Information("Default task...");
    });

#endregion //TASKS

#region MISC
Task("Help")
    .Does(()=>{
   Information(args.Help());
   });
args.WithTaskDescriptions(restoreTask.Task,buildTask.Task,testTask.Task,defaultTask.Task);
#endregion //MISC

RunTarget(target.Build());