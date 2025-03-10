//Main game interface. Renders GameBoard, Hand, player info
import { useParams } from 'react-router-dom';

const GamePage = () => {
    const { gameId } = useParams();
    const message = location.state?.message || "";

    return (
        <div>
            <h2>Game #{gameId}</h2>
            {message && <p>{message}</p>}
        </div>
    );
};

export default GamePage;