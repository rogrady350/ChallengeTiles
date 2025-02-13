import PropTypes from "prop-types";

const Tile = ({ tile }) => {
    return (
        <div className="tile" >
            <img src={tile.tileImageUrl} alt={`Tile ${tile.id}`} className="tile-image" />
        </div>
    );
};

//Validate props (attributes for Tile)
Tile.propTypes = {
    tile: PropTypes.shape({
        id: PropTypes.number.isRequired,           //must be a number
        number: PropTypes.number.isRequired,       //must be a number
        color: PropTypes.string.isRequired,        //must be a string
        tileImageUrl: PropTypes.string.isRequired, //must be a string
    }).isRequired,
};

export default Tile;