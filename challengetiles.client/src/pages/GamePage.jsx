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
import GameBoard from "../components/GameBoard";

const GamePage = () => {
    const {gameId} = useParams(); //gameId sent in url
    const location = useLocation(); //initialize use location
    const message = location.state?.message || ""; //extract message if passed
    const [gameState, setGameState] = useState(null); //store game data
    const [selectedTile, setSelectedTile] = useState(null) //track tile that player selects to place on board
    const [errorMessage, setErrorMessage] = useState(""); //track error messages from backend

    //fetch gameState on page load
    useEffect(() => {
        const fetchState = async () => {
            try {
                const data = await gameService.fetchGameState(gameId);
                if (data) setGameState(data);
            } catch (error) {
                console.error("Error fetching game state:", error);
            }
        };

        fetchState();
    }, [gameId]); //gameId does not change, will onlly render once

    //handle starting player selection
    const handleSetStartingPlayer = async (playerId) => {
        try {
            const response = await gameService.setStartingPlayer(gameId, playerId);
            if (response) {
                console.log("Starting player set successfully");
                const updatedState = await gameService.fetchGameState(gameId);
                setGameState(updatedState);
            }
        } catch (error) {
            console.error("Error setting starting player:", error);
        }
    };

    //handle picking up a Tile from the TileDeck
    const handlePickUpTile = async (playerId) => {
        try {
            const response = await gameService.pickUpTile(gameId, playerId);
            if (response) {
                const updatedState = await gameService.fetchGameState(gameId);
                console.log("Tile picked up successfully:", updatedState);
                setGameState(updatedState);
            }
        } catch (error) {
            console.error("Error picking up tile:", error);
        }
    };

    //select tile to place by clicking
    const handleTileClick = (tile) => {
        setSelectedTile(tile);
        setErrorMessage(""); //clear error on tile selection
    };

    //manage process of placing a Tile on the Game Board
    const handleTilePlacement = async (x, y) => {
        console.log(`Tile placement attempted at (${x}, ${y})`);
        //handle selecting a board position before selecting a tile
        if (!selectedTile) {
            setErrorMessage("Please select a tile to place");
            return;
        }

        try {
            const result = await gameService.placeTile(gameId, gameState.currentPlayerId, selectedTile, x, y);

            if (result.success) {
                const updatedState = await gameService.fetchGameState(gameId)
                setGameState(updatedState) //refresh board with new Tile Placement
                setSelectedTile(null) //clear selection after placement
                setErrorMessage("") //clear errors after placement
            } else {
                //backend sends message based on response set in GameService.PlayerPlaceTile
                setErrorMessage(result.message)
            }
        } catch (error) {
            console.error("Error placing tile:", error);
            setErrorMessage("An error occurred. Please try again.");
        }
    }

    //loading message while building game
    if (!gameState) {
        return <div>Loading game</div>;
    }

    //render Game Page
    return (
        <div>
            <h1>{message && <p>{message}</p>}</h1> {/*render welcome message*/}

            <h3>Score: {gameState.currentScore}</h3>

            {/*render error messages*/}
            {errorMessage && <p className="error-message">{errorMessage}</p>}


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

            {/*Game Board - manage tile placement*/}
            <GameBoard board={gameState.gameBoard || []} onTilePlacement={handleTilePlacement} />

            {/*Player area - shows name, hand, turn*/}
            <div className="players-section">
                {gameState.players.map((player, index) => (
                    <div key={player.playerId} className="player-info">
                        <Hand
                            player={player}
                            tiles={gameState.hands[index].handTiles}
                            onTileClick={handleTileClick} //set as selected tile when clicked
                        />
                        {/*Show who has the current turn*/}
                        {
                            gameState.currentPlayerId === player.playerId &&
                            <p className="turn-indicator">Current Turn</p>
                        }
                    </div>
                ))}
            </div>

            {/*Deck image - click to draw tile*/}
            <h3>Tile Deck - click to draw Tile</h3>
            <div className="deck-section">
                <img
                    src={gameState.tileDeckImageUrl}
                    alt="Tile Deck"
                    className="deck-image"
                    onClick={() => handlePickUpTile(gameState.currentPlayerId)}
                />
            </div>
        </div>
    );
};

export default GamePage;