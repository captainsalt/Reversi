module Types

type DiskColor = 
| White 
| Black

type Score = {
    white: int
    black: int
}

type Position = char * int

type Player = {
    color: DiskColor
}

type Tile = {
    position: Position
    disk: DiskColor option
}

type GameBoard = Tile list list
