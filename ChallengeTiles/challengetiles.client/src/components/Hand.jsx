//component for hand object. shows tiles in players hand
import Tile from "./Tile";
import PropTypes from "prop-types";

const Hand = ({ tiles }) => {
    return (
        <div className="hand">
            {tiles.map((tile) => (
                <Tile key={tile.id} tile={tile} />
            ))}
        </div>
    );
};

//Validate props
Hand.propTypes = {
    tiles: PropTypes.arrayOf(
        PropTypes.shape({
            id: PropTypes.number.isRequired,
            number: PropTypes.number.isRequired,
            color: PropTypes.string.isRequired,
            tileImageUrl: PropTypes.string.isRequired,
        })
    ).isRequired,
};

export default Hand;
