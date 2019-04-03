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
   .Help("\tPrints the help")
   .Bundle();



#endregion //ARGUMENTS

#region VARIABLES
var routes=new []{
   "src/netcore-webapi-with-angular"
};


#endregion //VARIABLES

#region TASKS
Task("Default")
.Does(()=>{
   Information("Default task...");
});
Task("Restore")
.Does(()=>{
   
});
Task("Build")
.IsDependentOn("Restore")
.Does(()=>{});
Task("Test")
.IsDependentOn("Build")
.Does(()=>{
   Information("Running tests...");
});


#endregion //TASKS

#region MISC
Task("Help")
.Does(()=>{
   Information(args.Help());
   
   });

#endregion //MISC




RunTarget(target.Build());