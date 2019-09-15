module BoardInterface
open Types

let getOppositeDisk = function | White -> Black | Black -> White

let generateBoard = 
    let initTiles = function
    | ('E', 4) | ('D', 5) -> Some White
    | ('E', 5) | ('D', 4) -> Some Black
    | _ -> None

    [
        for row in 'A'..'H' do 
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

let setTileColor (newColor: Disk) (position: Position) (board: GameBoard) = 
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

let ( +> ) (dir: int * int) (pos: Position) = 
    let letterCoord = int (fst pos) + fst dir |> char
    let numberCoord = snd pos + snd dir
    (letterCoord, numberCoord)

/// Returns a list of tiles that get captured when you place a disk on startigTile's position
let showCapturedTiles (startingTile: Tile) (disk: Disk) (board: GameBoard) = 
    let directions = [
        (-1, -1 ); (-1, 0 ); (-1, 1 )
        ( 0, -1 );           ( 0, 1 )
        ( 1, -1 ); ( 1,  0); ( 1, 1 )
    ]

    let opponentColor = getOppositeDisk disk

    let rec getTileCaptives (refTilePos: Position) (direction: int * int) (capturedTiles: Tile list) = 
           let targetTile = getTile (direction +> refTilePos) board

           match targetTile with 
           | Some tile ->
                match tile.disk with 
                | Some color when color = opponentColor -> 
                    let newRefTile = direction +> refTilePos
                    getTileCaptives newRefTile direction (tile :: capturedTiles)
                | Some color when color = getOppositeDisk opponentColor -> 
                    capturedTiles
                | _ ->
                    []
           | None -> 
                []
    
    directions
    |> List.collect (fun dir -> getTileCaptives startingTile.position dir []) 

/// Returns a new board with the tiles captured when a disk is placed 
let captureTiles (tiles: Tile list) (color: Disk) (board: GameBoard) = 
    tiles
    |> List.collect (fun tile -> setTileColor color tile.position board)

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
