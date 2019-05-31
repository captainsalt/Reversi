module Types

type DiskColor = 
| White 
| Black

type Position = char * int

type Player = {
    //playerId: 
    diskColor: DiskColor
}

type Tile = {
    position: Position
    disk: DiskColor option
}

type GameBoard = Tile list list
