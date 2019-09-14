open System
open BoardInterface
open Types
open Game

[<EntryPoint>]
let main argv =
    let board = generateBoard 
    let players = getPlayers

    let rec gameLoop (score: Score) (player: Player) (board: GameBoard) = 
        printBoard board
        let coordinate = coordinatePrompt "Please enter a coordinate"
        let tile = getTile coordinate board

        match tile with 
        | Some t -> 
            match t.disk with 
            | None -> 
                let newBoard = setTileColor player.color (t.position) board
                gameLoop score player newBoard
            | Some _ -> 
                gameLoop score player board
        | None -> 
            gameLoop score player board

    gameLoop { white = 0; black = 0} { color = White } board

    0
