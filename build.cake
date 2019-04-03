#load "build/scripts/args.cake"

//#load "src/netcore-webapi-with-angular"

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



#endregion //ARGUMENTS

#region VARIABLES
//cakes available in this repository under path ./src/{route}/build.cake
var routes=new []{
   "netcore-webapi-with-angular"
};

//Gets the settings to push to another cake
CakeSettings GetSettings(string target){
   return new CakeSettings{
      Arguments=new Dictionary<string,string>(){
         {"target", target},
         {"configuration", configuration.Build()},
         {"exclusive", "true"}//runs only top level tasks - this is to ensure we can run individual tasks without relying on this one
      },
   };
}
//Gets the runner to invoke for specific task
Action<string> GetTaskRunner(string task){
   return (input)=>{
      Information($"Running {task} for {input}...");
      CakeExecuteScript(File($"src/{input}/build.cake"),GetSettings(task));
   };
}

#endregion //VARIABLES

#region TASKS
var restoreTask=Task("Restore")
   .Description("Runs the restore task for each project")
   .DoesForEach(routes,GetTaskRunner("Restore"));
var buildTask=Task("Build")
   .Description("Runs the build task for each project")
   .IsDependentOn("Restore")
   .DoesForEach(routes,GetTaskRunner("Build"));
var testTask=Task("Test")
   .Description("Runs the test task for each project")
   .IsDependentOn("Build")
   .DoesForEach(routes,GetTaskRunner("Test"));
var defaultTask=Task("Default")
   .Description("Runs the test task for each project")
   .IsDependentOn("Test")
   .Does(()=>{
      Information("Completed running tests...");
   });

#endregion //TASKS

#region MISC
Task("Help")
   .Does(()=>{
   Information(args.Help());
   }).DoesForEach(routes,route=>{
      Information($"Showing Help for {route}...");
      GetTaskRunner("Help").Invoke(route);
      });

args.WithTaskDescriptions(restoreTask.Task,buildTask.Task,testTask.Task,defaultTask.Task);
#endregion //MISC




RunTarget(target.Build());