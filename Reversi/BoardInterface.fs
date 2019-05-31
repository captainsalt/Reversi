module BoardInterface
open Types

// Generate an 8x8 board
let generateBoard = 
    let initTiles = function
    | ('e', 4) | ('d', 5) -> Some White
    | ('e', 5) | ('d', 4) -> Some Black
    | _ -> None

    [
        for row in 'a'..'h' do 
            yield [ for col in 1..8 ->  { position = (row, col); disk = initTiles (row,col) }]
    ]

let getTileState (position: Position) (board: GameBoard) = 
    board 
    |> List.find (fun row -> fst row.Head.position = fst position) 
    |> List.find (fun tile -> tile.position = position)
    |> (fun tile -> tile.disk)
    
let setTileState (newColor: DiskColor) (position: Position) (board: GameBoard) = 
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

// Print the board to the console
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