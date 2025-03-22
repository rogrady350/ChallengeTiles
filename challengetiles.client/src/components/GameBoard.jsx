//css
import "../styles/GameBoard.css";

import Tile from "./Tile";
import PropTypes from "prop-types";

//component to display game board and placed tiles - expand horizontally only with wrap
const GameBoard = ({ board, onTilePlacement, currentPlayer }) => {
    const tilesPerRow = 15; //define width before wrap - 7 tiles left and right + center starting tile

    //board details
    /*array of position of tiles on board 
      spead operator makes copy of board array of placed tiles 
      avoids modifying original array*/
    const sortedTiles = [...board].sort((a, b) => a.position - b.position); // sort array in ascending order by position
    const positions = sortedTiles.map(p => p.position);

    //min and max positions - range of tiles on board (renders both tiles and empty spaces)
    const minPosition = Math.min(...positions);
    const maxPosition = Math.max(...positions);

    //number of cells on each side of center tile
    const buffer = 7

    //create board
    //1. create array of placement positions (including empty positions)
    const fullBoardRange = [];
    for (let i = minPosition - buffer; i <= maxPosition + buffer; i++) {
        fullBoardRange.push(i);
    }

    //2. map board range to tile or null (empty space)
    const fullBoardWithTiles = fullBoardRange.map(position => {
        const found = sortedTiles.find(p => p.position === position);
        return {
            position,
            tile: found?.tile || null,
        }
    })

    //3. wrap tiles into rows to prevent extending too far
    const wrappedRows = [];
    for (let i = 0; i < fullBoardWithTiles.length; i += tilesPerRow) {
        wrappedRows.push(fullBoardWithTiles.slice(i, i + tilesPerRow)); //slice rows and add to new row for wrap
    }

    return (
        <div className="game-board">
            {/*array of rows (2D - tiles expand horizontaly only*/}
            {wrappedRows.map((row, rowIndex) => (
                <div key={rowIndex} className="board-row">
                    {/*loop thru tiles in row and render tiles on board
                       if no tile placed, tile will be null for that position (renders empty space)
                       dims board positions before starting player selected (disables clicks)
                       GamePage.handleTilePlacement will also display message if placement attempt made before starting player selected*/}
                    {row.map(({ position, tile }) => (
                        <div
                            key={position}
                            className={`board-cell ${tile ? "occupied" : "empty"} ${currentPlayer ? "" : "disabled"}`}
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