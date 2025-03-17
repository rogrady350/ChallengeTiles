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

    //handle starting player selection
    const handleSetStartingPlayer = async (playerId) => {
        const response = await gameService.setStartingPlayer(gameId, playerId)

        if (response) {
            console.log("Starting player set successfully!");
            gameService.fetchGameState(gameId).then(data => setGameState(data)); //call API function to send selected player}
        }
    };

    //loading message while building game
    if (!gameState) {
        return <div>Loading game</div>;
    }

    return (
        <div>
            <h1>{message && <p>{message}</p>}</h1> {/*render welcome message*/}

            <h3>Score: {gameState.currentScore}</h3>

            {/*display message to set starting player on game load, hide once selected*/}
            {!gameState.currentPlayerId && (
                <div className="starting-player-selection">
                    <h3>Please select a starting player:</h3>

                    {/*buttons to select player*/}
                    {gameState.players.map(player => (
                        <button key={player.playerId} onClick={() => handleSetStartingPlayer(player.playerId)}>
                            {player.name}
                        </button>
                    ))}
                </div>
            )}

            <div className="players-section">
                {gameState.players.map((player, index) => (
                    <div key={player.playerId} className="player-info">
                        <Hand player={player} tiles={gameState.hands[index].handTiles} />
                        {/*Show who has the current turn*/}
                        {
                            gameState.currentPlayerId === player.playerId &&
                            <p className="turn-indicator">Current Turn</p>
                        }
                    </div>
                ))}
            </div>
        </div>
    );
};

export default GamePage;