module BoardInterface
open Types

let getOppositeDisk = function | White -> Black | Black -> White

let generateBoard = 
    let initTiles = function
    | ('e', 4) | ('d', 5) -> Some White
    | ('e', 5) | ('d', 4) -> Some Black
    | _ -> None

    [
        for row in 'a'..'h' do 
            yield [ for col in 1..8 ->  { position = (row, col); disk = initTiles (row, col) } ]
    ]

let getTile (position: Position) (board: GameBoard) = 
    try 
        board 
        |> List.find (fun row -> fst row.Head.position = fst position) 
        |> List.find (fun tile -> tile.position = position)
        |> Some
    with 
    | :? System.Collections.Generic.KeyNotFoundException ->
        None

let setTileColor (newColor: DiskColor) (position: Position) (board: GameBoard) = 
    board 
    |> List.map 
        (fun row -> 
            if fst row.Head.position = fst position then
                row 
                |> List.map 
                    (fun tile ->
                        if tile.position = position then
                            { tile with disk = Some newColor }
                        else
                            tile
                    )
            else
                row
        )

let captureSurroundingTiles (startingTilePos: Position) (board: GameBoard) = 
    let addDirection (pos: Position) (dir: int * int) = 
        let ( +> ) (ch: char) (i: int) = int ch + i |> char
        (fst pos +> fst dir, snd pos + snd dir)

    let directions = [
        (-1, -1 ); (-1, 0 ); (-1, 1 )
        ( 0, -1 ); ( 0, 1 )
        ( 1, -1 ); ( 1,  0); ( 1, 1 )
    ]

    let opponentColor = getOppositeDisk ((getTile startingTilePos board).Value.disk.Value)

    let rec getTileCaptives (refTilePos: Position) (opponentColor: DiskColor) (direction: int * int) (capturedTiles: Tile list) = 
           let pos = addDirection refTilePos direction
           let tileToCapture = getTile pos board

           match tileToCapture with 
           | Some tile ->
                match tile.disk with 
                | Some color when color = opponentColor -> 
                    let newRefTile = addDirection refTilePos direction
                    getTileCaptives newRefTile opponentColor direction (tile :: capturedTiles)
                | Some color when color = getOppositeDisk opponentColor -> 
                    capturedTiles
                | _ ->
                    []
           | None -> 
                []
    
    directions
    |> List.collect (fun dir -> getTileCaptives startingTilePos opponentColor dir []) 
    |> List.collect (fun tile -> setTileColor (getOppositeDisk opponentColor) tile.position board)

let printBoard (board: GameBoard) = 
    for row in 0..7  do
        let rec printRow colIndex colString = 
            if colIndex < 8 then 
                let tile = board.[row].[colIndex]
                let tileString = 
                    match tile.disk with 
                    | Some Black -> "B"
                    | Some White -> "W"
                    | None -> tile.position ||> sprintf "%c%i"

                let newColString = sprintf "%s %2s" colString tileString
                printRow (colIndex + 1) newColString
            else 
                printfn "%s" colString 

        printRow 0 ""