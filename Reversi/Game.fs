module Game
open Types
open BoardInterface
open System
open System.Text.RegularExpressions

//Player 1 is white
//Plater 2 is black

let private (|Coordinate|_|) input =
    let result = Regex(@"([A-Ha-h])\s*(?:,?\s*)?([1-8])").Match(input)

    match result.Success with 
    | true -> 
        result.Groups
        |> (fun g -> (g.[1].Value |> char, g.[2].Value |> int))
        |> Some
    | false -> None

let rec coordinatePrompt (message: string) = 
    printf "%s: " message
    let response = Console.ReadLine()

    match response with 
    | Coordinate (ch, i) ->
        (ch, i)
    | _ ->
        printfn "Invalid Coordinate"
        coordinatePrompt message

let getValidMoves (currentColor: Disk) = ()