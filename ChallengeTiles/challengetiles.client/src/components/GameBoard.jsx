//component to display game board and placed tiles
import Hand from "./Hand"
import PropTypes from "prop-types";

const GameBoard = ({ tiles }) => {
    return (
        <div className="game-board">
            <h2>Game Board</h2>
            <Hand tiles={tiles} />
        </div>
    );
};

GameBoard.propTypes = {
    tiles: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.number.isRequired,
            number: PropTypes.number.isRequired,
            color: PropTypes.string.isRequired,
            tileImageUrl: PropTypes.string.isRequired,
        })
    ).isRequired,
};

export default GameBoard;