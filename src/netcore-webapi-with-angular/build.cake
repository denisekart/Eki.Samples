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

#endregion //ARGUMENTS


#region TASKS

var restoreTask=Task("Restore")
.Does(()=>{
    Information("Running restore...");
    DotNetCoreRestore();
    Information("Running npm install...");
    NpmInstall(settings=>{
    settings.WorkingDirectory=Directory("./sample-client");
    settings.Production=configuration.Build()=="Release";
    });
   });
var buildTask=Task("Build")
    .IsDependentOn("Restore")
    .Does(()=>{});
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