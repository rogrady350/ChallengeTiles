//component for hand object. shows tiles in players hand
import PropTypes from "prop-types";
import Tile from "./Tile";

const Hand = ({ player, tiles, onTileClick }) => {
    return (
        <div className="player-hand">
            <h3>{player.name}</h3>
            <div className="tiles-container">
                {tiles.map(tile => (
                    <Tile
                        key={tile.tileId}
                        tile={tile}
                        onClick={() => onTileClick(tile, player.playerId)} //call handleTileClick when clicked
                    />
                ))}
            </div>
        </div>
    )
};

//props validation
Hand.propTypes = {
    player: PropTypes.shape({
        name: PropTypes.string.isRequired,
        playerId: PropTypes.number.isRequired
    }).isRequired,
    tiles: PropTypes.arrayOf(
        PropTypes.shape({
            tileId: PropTypes.number.isRequired,
            number: PropTypes.number.isRequired,
            color: PropTypes.string.isRequired,
            tileImageUrl: PropTypes.string.isRequired,
        })
    ).isRequired,
    onTileClick: PropTypes.func.isRequired,
};

export default Hand;
