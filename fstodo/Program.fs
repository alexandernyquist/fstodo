open System;
open System.IO;

let read path = File.ReadAllLines path |> Array.toList

// Remove Nth item from file
let remove (path, num) =
    read path
    |> List.mapi (fun i line -> (i <> num, line)) // Create a tuple with (shouldkeep, line)
    |> List.filter fst // Filter Nth item (first tuple member)
    |> List.map snd // Take line (second tuple member)
    |> List.toArray
    |> fun(lines) -> File.WriteAllLines(path, lines)
    |> ignore

// Show todo list
let display path =
    read path
    |> List.mapi (fun i line -> (i+1, line))
    |> List.iter (fun (i, line) -> printf "%d. %s\n" i line)
    |> ignore

// Add an item to the list
let add path (items:seq<string>) =
    items
    |> fun items -> String.Join(" ", items) + Environment.NewLine
    |> fun item -> File.AppendAllText(path, item)
    |> ignore

[<EntryPoint>]
let main argv = 
    let path = Environment.GetEnvironmentVariable("TODO_FILE", EnvironmentVariableTarget.User)

    match argv with
        | [||] -> ()
        | [|"done"; num|] -> remove(path, Int32.Parse(num) - 1)
        | _ -> add path argv

    display path
    0