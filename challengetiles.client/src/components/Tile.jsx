import PropTypes from 'prop-types'

const Tile = ({ tile, onClick }) => {
    return (
        <div className="tile" onClick={() => onClick(tile)}>
            <img src={tile.tileImageUrl} alt={`${tile.number} ${tile.color}`} />
        </div>
    );
};

//Props validation
Tile.propTypes = {
    tile: PropTypes.shape({
        number: PropTypes.number.isRequired,
        color: PropTypes.string.isRequired,
        tileImageUrl: PropTypes.string.isRequired, // Make sure this matches the backend attribute
    }).isRequired,
    onClick: PropTypes.func.isRequired, // Expecting a function
};

export default Tile;