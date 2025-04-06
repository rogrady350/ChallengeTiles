//Game landing page

//css
import "../styles/Home.css";

//Hooks: React functions that allow you to use state and other React features inside components
/*useEffect: handles side effects (things that happen outside React)
  useState: watches for change and re-renders page. format: const [value, setValue] = useState(initialValue);*/
import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom'; //allows page navigation

//services
import gameSetupService from '../services/gameSetupService.js';

const Home = () => {
    //values to be set for game
    const [players, setPlayers] = useState([]);
    const [player1, setPlayer1] = useState('');
    const [player2, setPlayer2] = useState('');
    const [numColors, setNumColors] = useState(2); //suggest playing with 2 colors
    const [numTiles, setNumTiles] = useState(7);   //suggest playing with Hands of 7 Tiles
    const [message, setMessage] = useState('');    //message from back end
    const navigate = useNavigate();                //initialize React Router navigation feature

    //fetch players from backend on page load
    useEffect(() => {
        gameSetupService.getPlayers() //call API function from to get list of players for drop downs
            .then(response => {
                setPlayers(response || []); //up state with Player's userId's from response if true, empty array if false
            })
            .catch(error => {
                console.error("Error fetching players", error);
            });
    }, []); //run component once

    //function to start a new game when Play Game button clicked
    const startGame = async () => {
        //validate all selections are made. numColors and numTiles have default recommended values used if not changed by user
        if (!player1 || !player2) {
            setMessage("Please select two players.");
            return;
        }

        //request body sent to backend
        const requestBody = {
            playerIds: [player1, player2], //list of playerId's
            numberOfColors: parseInt(numColors),
            numberOfTiles: parseInt(numTiles)
        };

        try {
            //call API function to start a new game when
            const response = await gameSetupService.startNewGame(requestBody);

            //redirect to newly created game, display returned messages passed in state on new page load
            navigate(`/game/${response.gameId}`, { state: { message: response.message } });
        } catch (error) {
            console.error("Error starting game:", error); //debug message if game does not start
            setMessage("Failed to start game. Please try again.");
        }
    };

    return (
        <div>
            <h2>Welcome!</h2>

            {/* Player 1 dropdown */}
            <div>
                <label>Player 1: </label>
                {/*event handler: display selected value in dropdown, update component state with selection*/}
                <select value={player1} onChange={(e) => setPlayer1(e.target.value)}>
                    <option value="">Select Player 1</option>
                    {/*loop thru each player sent from backend as JSON and create an option
                        key: playerId, display: username*/}
                    {players.map(player => (
                        <option key={player.playerId} value={player.playerId}>
                            {player.username} 
                        </option>
                    ))}
                </select>
            </div>

            {/* Player 2 dropdown */}
            <div>
                <label>Player 2: </label>
                <select value={player2} onChange={(e) => setPlayer2(e.target.value)}>
                    <option value="">Select Player 2</option>
                    {/*exclude selection of player 1 from options for player 2*/}
                    {players
                        .filter((p) => p.playerId !== parseInt(player1))
                        .map(player => (
                        <option key={player.playerId} value={player.playerId}>
                            {player.username}
                        </option>
                    ))}
                </select>
            </div>

            {/* Number of Colors dropdown */}
            <div>
                <label>Number of Colors: </label>
                <select value={numColors} onChange={(e) => setNumColors(e.target.value)}>
                    {/*can choose to play with 2-4 colors*/}
                    {[2, 3, 4].map(num => (
                        <option key={num} value={num}>
                            {num}
                        </option>
                    ))}
                </select>
            </div>

            {/* Number of Tiles dropdown */}
            <div>
                <label>Number of Tiles: </label>
                <select value={numTiles} onChange={(e) => setNumTiles(e.target.value)}>
                    {/*can choose to deal 3-10 Tiles per hand*/}
                    {[3, 4, 5, 6, 7, 8, 9, 10].map(num => (
                        <option key={num} value={num}>
                            {num}
                        </option>
                    ))}
                </select>
            </div>

            {/* Play Game button */}
            <button onClick={startGame}>Start Game</button>
            {message && <p>{message}</p>} {/*render message if back end sends one*/}
        </div>
    );
};

export default Home;