open System
open BoardInterface
open Types
open Game

[<EntryPoint>]
let main argv =
    let board = generateBoard 

    let rec gameLoop (score: Score) (player: Player) (board: GameBoard) = 
        printBoard board
        let coordinate = coordinatePrompt "Please enter a coordinate"
        let inputTile = getTile coordinate board

        match inputTile with 
        | Some tile -> 
            // Tile exists on the board
            match tile.disk with 
            // No disk is occupying the tile
            | None -> 
                let cappedTileList = showCapturedTiles tile (player.color) board

                match cappedTileList with 
                | [] -> 
                    // Invalid move - No tiles were captured
                    gameLoop score player board
                | _ -> 
                    // Successful capture
                    let newBoard = 
                        setTileColor (player.color) (tile.position) board 
                        |> captureTiles cappedTileList (player.color)  

                    gameLoop score player newBoard
            | Some _ -> 
                // A disk is occupying the tile
                gameLoop score player board
        | None -> 
            // Tile does not exist
            gameLoop score player board

    gameLoop { white = 0; black = 0} { color = White } board

    0
