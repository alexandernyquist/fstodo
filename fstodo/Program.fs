open System;
open System.IO;

// Remove Nth item from file
let remove path num =
    let n = Int32.Parse(num) - 1

    File.ReadAllLines(path)
    |> Array.toList
    |> List.mapi (fun i line -> (i, line)) // Create a tuple with (index, line)
    |> List.filter (fun x -> fst x <> n) // Filter Nth item (first tuple member)
    |> List.map (fun x -> snd x) // Take line (second tuple member)
    |> List.toArray
    |> fun(lines) -> File.WriteAllLines(path, lines)
    |> ignore

// Show todo list
let display path =
    File.ReadAllLines(path)
    |> Array.toList
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
        | [||] -> () // No arguments, skip
        | _ -> match argv.[0] with
               | "done" -> remove path argv.[1] // How the fuck do i call Int32.Parse argv.[1] and call remove (like remove (path, Int32.Parse(argv.[1])))?
               | _ -> add path argv

    display path

    0 // return an integer exit code
