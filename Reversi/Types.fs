module Types

type Disk = 
| White 
| Black

type Score = {
    white: int
    black: int
}

type Position = char * int

type Player = {
    color: Disk
}

type Tile = {
    position: Position
    disk: Disk option
}

type GameBoard = Tile list list
