#r @"packages/FAKE/tools/FakeLib.dll"

open Fake
let buildDir = "./artifacts/"

Target "Clean" (fun _ -> 
  CleanDir buildDir
)

Target "BuildApp" (fun _ -> 
  !! "./ThereAndBack/*.csproj"
    |> MSBuildRelease buildDir "Build" 
    |> Log "AppBuild-Output: "
)

Target "Properties" (fun _ ->
  let (_, msgs) = executeFSI __SOURCE_DIRECTORY__ "properties.fsx" []
  
  msgs |> Seq.iter (function {Message=message} when message.Contains("Ok,") -> trace message
                           | {Message=message}               -> traceError message)
  if msgs |> Seq.exists (fun {Message=msg} -> msg.Contains("Ok,") |> not) 
  then failwith "A property has failed"
)

Target "Default" (fun _ -> 
  trace "Complete"
)

"Clean"
  ==> "BuildApp"
  ==> "Properties"
  ==> "Default"

// start build
RunTargetOrDefault "Default"
