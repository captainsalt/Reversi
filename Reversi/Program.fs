open System
open BoardInterface
open Types
open Game

[<EntryPoint>]
let main argv =
    let board = generateBoard 
    let players = getPlayers

    //|> setTileColor White ('c', 4) 
    //|> captureSurroundingTiles ('c', 4)
    //|> printBoard

    0
