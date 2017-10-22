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
  let passed = "Ok, passed"
  msgs |> Seq.iter (function {Message=msg} when msg.Contains(passed) -> trace msg
                           | {Message=msg}               -> traceError msg)
  if msgs |> Seq.exists (fun {Message=msg} -> msg.Contains(passed) |> not) 
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
