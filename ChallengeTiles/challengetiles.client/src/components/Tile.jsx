import PropTypes from "prop-types";

const Tile = ({ tile }) => {
    return (
        <div className="tile" >
            <img src={tile.tileImageUrl} alt={`Tile ${tile.id}`} className="tile-image" />
        </div>
    );
};

// Validate props
Tile.propTypes = {
    tile: PropTypes.shape({
        id: PropTypes.number.isRequired,
        tileImageUrl: PropTypes.string.isRequired,
    }).isRequired,
};

export default Tile;