//css
import "../styles/GameBoard.css";

import Tile from "./Tile";
import PropTypes from "prop-types";

//component to display game board and placed tiles
const GameBoard = ({ board, onTilePlacement, currentPlayer }) => {
    const gridSize = 10; //define board size

    return (
        <div className="game-board">
            {Array.from({ length: gridSize }).map((_, row) => (
                <div key={row} className="board-row">
                    {Array.from({ length: gridSize }).map((_, col) => {
                        const placedTile = board.find(tile => tile.x === col && tile.y === row); //placement position

                        return (
                            <div
                                key={`${row}-${col}`}
                                className={`board-cell ${currentPlayer ? "" : "disabled"}`} // Disable clicks if no current player
                                onClick={() => currentPlayer && onTilePlacement(col, row)}
                            >
                                {placedTile ? <Tile tile={placedTile.tile} onClick={() => onTilePlacement(col, row)} /> : null}
                            </div>
                        );
                    })}
                </div>
            ))}
        </div>
    );
};

//props validation
GameBoard.propTypes = {
    board: PropTypes.arrayOf(
        PropTypes.shape({
            x: PropTypes.number.isRequired,
            y: PropTypes.number.isRequired,
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