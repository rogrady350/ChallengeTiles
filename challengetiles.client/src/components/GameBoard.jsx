//css
import "../styles/GameBoard.css";

import Tile from "./Tile";
import PropTypes from "prop-types";

//component to display game board and placed tiles - expand horizontally only with wrap
const GameBoard = ({ board, onTilePlacement, currentPlayer }) => {
    const totalTiles = 52; //tiles in deck
    const tilesPerRow = 15; //7 tiles left and right + center starting tile
    const half = Math.floor(totalTiles / 2); //position values (-26 to 25)
   
      
    //create board
    //1. create range of positions
    const fullBoardRange = Array.from({ length: totalTiles }, (_, i) => i - half);

    //2. build visual representation - map tile data to board position if a tile is placed in it
    const fullBoardWithTiles = fullBoardRange.map(position => {
        const found = board.find(p => p.position === position);
        return {
            position,
            tile: found?.tile || null,
        }
    })

    //3. split into rows of 15
    const wrappedRows = [];
    for (let i = 0; i < fullBoardWithTiles.length; i += tilesPerRow) {
        const row = fullBoardWithTiles.slice(i, i + tilesPerRow); //slice board array into smaller row arrays
        const shouldReverse = Math.floor(i / tilesPerRow) % 2 === 1; //reverse every other row

        //create forward row if shouldReverse is even and reverse row if odd
        wrappedRows.push(shouldReverse ? row.reverse() : row);
    }

    return (
        <div className="game-board">
            {/*array of rows (2D - tiles expand horizontaly only*/}
            {wrappedRows.map((row, rowIndex) => {
                const oddRow = rowIndex % 2 === 1;
                //If row is odd, make copy of row (spread opperator) and reverse it. If even use as-is
                const displayRow = oddRow ? [...row].reverse() : row;

                return (
                    <div key={rowIndex} className="board-row">
                        {/*Loop thru tiles in row and render tiles on board.
                           If no tile placed, tile will be null for that position (renders empty space).
                           Dims board positions before starting player selected (disables clicks).
                           GamePage.handleTilePlacement will also display message if placement attempt made before starting player selected.*/}
                        {displayRow.map(({ position, tile }) => (
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
                );
            })}
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