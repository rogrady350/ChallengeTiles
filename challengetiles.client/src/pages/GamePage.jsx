//Main game interface. Renders GameBoard, Hand, player info

//css
import "../styles/GamePage.css";

//Hooks
import { useEffect, useState } from "react";
/*useParams: extracts values from url segments when fetching data based on params sent in the url
  useLocation: accesses data passed navigate*/
import { useParams, useLocation } from 'react-router-dom';

//services, components
import Hand from "../components/Hand";
import gameService from "../services/gameService";

const GamePage = () => {
    const {gameId} = useParams(); //gameId sent in url
    const location = useLocation(); //initialize use location
    const message = location.state?.message || ""; //extract message if passed
    const [gameState, setGameState] = useState(null); //store game data

    //fetch gameState on page load
    useEffect(() => {
        gameService.fetchGameState(gameId) //call API function to get current state of game objecs
            .then(data => {
                if (data) setGameState(data); //update gameState to trigger re-render of game components
            })
            .catch(error => {
                console.error("Error fetching players", error);
            });
    }, [gameId]); //gameId does not change, will onlly render once

    //loading message while building game
    if (!gameState) {
        return <div>Loading game</div>;
    }

    return (
        <div>
            <h2>Game #{gameId}</h2>
            {message && <p>{message}</p>} {/*render message if back end sends one*/}

            <h3>Score: {gameState.currentScore}</h3>

            <div className="players-section">
                {gameState.players.map((player, index) => (
                    <Hand key={player.playerId} player={player} tiles={gameState.hands[index].handTiles} />
                ))}
            </div>
        </div>
    );
};

export default GamePage;