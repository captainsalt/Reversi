open System
open BoardInterface
open Types

[<EntryPoint>]
let main argv =
    generateBoard 
    |> printBoard
    0
