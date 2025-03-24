//css
import "../styles/GameBoard.css";

import Tile from "./Tile";
import PropTypes from "prop-types";

//component to display game board and placed tiles - expand horizontally only with wrap
const GameBoard = ({ board, onTilePlacement, currentPlayer }) => {
    const totalPositions = 75; //tiles in deck
    const tilesPerRow = 15; //7 tiles left and right + center starting tile
    const half = Math.floor(totalPositions / 2); //position values (-26 to 25)
   
      
    //create board
    //1. create range of positions
    const fullBoardRange = Array.from({ length: totalPositions }, (_, i) => i - half);

    //2. build visual representation - map tile data to board position if a tile is placed in it
    const fullBoardWithTiles = fullBoardRange.map(position => {
        const found = board.find(p => p.position === position);
        return {
            position,
            tile: found?.tile || null,
        }
    })

    //3. Slice board array into smaller row arrays of 15
    const wrappedRows = [];
    for (let i = 0; i < fullBoardWithTiles.length; i += tilesPerRow) {
        const rawRow = fullBoardWithTiles.slice(i, i + tilesPerRow);  //sliced row before flipping
        const rowIndex = Math.floor(i / tilesPerRow);

        const reversedCopy = [...rawRow].reverse(); //spread operator copies rawRow before reversing to avoid modifying original rawRow array

        //reverse both row rendering order AND logical position mapping value for odd rows
        const row = (rowIndex % 2 === 1) //check if odd or even row index
            ? reversedCopy.map((tile, index) => ({ //iterate over each tile in reversed row copy
                ...tile, //create a copy of tile to reassign position in reverse
                position: rawRow[rawRow.length - 1 - index].position //assign position by reversing the original positions
            }))
            : rawRow; //if even render as is

        wrappedRows.push(row); //push to final board layout
    }

    return (
        <div className="game-board">
            {/*array of rows (2D - tiles expand horizontaly only*/}
            {wrappedRows.map((row, rowIndex) => (
                <div key={rowIndex} className="board-row">
                    {/*Loop thru tiles in row and render tiles on board.
                           If no tile placed, tile will be null for that position (renders empty space).
                           Dims board positions before starting player selected (disables clicks).
                           GamePage.handleTilePlacement will also display message if placement attempt made before starting player selected.*/}
                    {row.map(({ position, tile }) => (
                        <div
                            key={position}
                            //className={`board-cell ${tile ? "occupied" : "empty"} ${currentPlayer ? "" : "disabled"}`}
                            //highlight center cell to make sure its staying centered for debugging
                            className={`board-cell ${tile ? "occupied" : "empty"} ${position === 0 ? "center" : ""} ${currentPlayer ? "" : "disabled"}`}
                            onClick={() => {
                                if (!tile && onTilePlacement) {
                                    onTilePlacement(position);
                                }
                            }}
                        >
                            {tile && (
                                <Tile
                                    tile={tile}
                                    onClick={() => onTilePlacement(position)}
                                />
                            )}
                        </div>
                    ))}
                </div>
            ))}
        </div>
    );
};

//props validation
GameBoard.propTypes = {
    board: PropTypes.arrayOf(
            PropTypes.shape({
                position: PropTypes.number.isRequired,
            tile: PropTypes.shape({
                number: PropTypes.number.isRequired,
                color: PropTypes.string.isRequired,
                tileImageUrl: PropTypes.string.isRequired,
            }).isRequired
        })
    ).isRequired,
    onTilePlacement: PropTypes.func.isRequired,
    currentPlayer: PropTypes.number
};

export default GameBoard;