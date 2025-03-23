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
    const [selectedTile, setSelectedTile] = useState({ tile:null, playerId: null }) //track tile that player selects to place on board
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

    //select tile to place by clicking. store the tile that was clicked + playerthat clicked it
    const handleTileClick = (tile, playerId) => {
        console.log("Tile selected:", tile, "PlayerId:", playerId);
        setSelectedTile({ tile, playerId });
        setErrorMessage(""); //clear error on tile selection
    };

    //manage process of placing a Tile on the Game Board
    const handleTilePlacement = async (position) => {
        console.log(`Tile placement attempted at (${position})`);
        //Frontend logic checks
        //handle selecting a board position before selecting a tile
        if (!selectedTile.tile) {
            setErrorMessage("Please select a tile to place");
            return;
        }

        //handle attempt to place tile before selecting a starting player
        if (!gameState.currentPlayerId) {
            setErrorMessage("Please select a starting player");
            return;
        }

        //handle tile placement attempt out of turn
        const currentPlayer = gameState.currentPlayerId;
        if (currentPlayer !== selectedTile.playerId) {
            setErrorMessage("Its not your turn");
            return;
        }

        //Backend verification of move
        try {
            console.log("Placing tile: ", selectedTile);
            const placementResult = await gameService.placeTile(
                gameId,
                gameState.currentPlayerId,
                selectedTile.tile,
                position);

            //if placementResult contains false success key, this means player made an illegal move
            if (placementResult.success === false) {
                setErrorMessage(placementResult.message);
                return;
            }

            //safety check: prevents crash and points to reason for error
            if (!placementResult || !placementResult.players || !placementResult.hands) {
                console.error("Invalid game state returned:", {
                    placementResult,
                    hasPlayers: !!placementResult?.players,
                    hasHands: !!placementResult?.hands,
                });
                setErrorMessage("Failed to update game state.");
                return;
            }

            //otherwise, result is raw GameDTO, 
            //(after the frontend recieves the updated gamestate the success message from tile placement is no longer in memory)
            setGameState(placementResult);
            setSelectedTile(null);
            setErrorMessage("");
            console.log("Game state updated:", placementResult);
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
        <div className="game-page">
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
            <GameBoard
                board={gameState.gameBoard || []}
                onTilePlacement={handleTilePlacement}
                currentPlayer={gameState.currentPlayerId}
            />

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

            {/*Deck image*/}
            <h3>Tile Deck - click to draw a Tile</h3>
            <div className="deck-section">
                <img
                    //src={gameState.tileDeckImageUrl}
                    src={"../images/logo.png"}
                    alt="Tile Deck"
                    className="deck-image"
                    onClick={() => handlePickUpTile(gameState.currentPlayerId)}
                />
            </div>
        </div>
    );
};

export default GamePage;