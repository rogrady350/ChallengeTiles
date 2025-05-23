//Game landing page

//css
import "../styles/Home.css";

//Hooks: React functions that allow you to use state and other React features inside components
/*useEffect: handles side effects (things that happen outside React)
  useState: watches for change and re-renders page. format: const [value, setValue] = useState(initialValue);
  useNavigate: allows page navigation*/
import { useEffect, useState } from "react";
import { useNavigate, Link } from 'react-router-dom'; 

//services
import gameSetupService from '../services/gameSetupService';
import { handleLogin } from "../services/playerService";

const Home = () => {
    //values to be set for game
    const [players, setPlayers] = useState([]);
    const [player1, setPlayer1] = useState('');
    const [player2, setPlayer2] = useState('');
    const [numColors, setNumColors] = useState(2); //suggest playing with 2 colors
    const [numTiles, setNumTiles] = useState(7);   //suggest playing with Hands of 7 Tiles
    const [message, setMessage] = useState('');    //message from back end
    const navigate = useNavigate();                //initialize React Router navigation feature

    //values for logging in
    const [loginData, setLoginData] = useState({ username: '', password: '' }); //stores user login input. passed to backend via handleLogin
    const [loggedInUser, setLoggedInUser] = useState(null); //holds successfully authenticated user profile object
    const [loginError, setLoginError] = useState(''); //recieves error message from backend end to be displayed for failed loggin

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
            console.error("Error starting game: ", error); //debug message if game does not start
            setMessage("Failed to start game. Please try again.");
        }
    };

    //login function
    const handleLoginSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await handleLogin(loginData);

            if (response?.success) {
                setLoggedInUser(response.data);
                localStorage.setItem('loggedInUser', JSON.stringify(response.data));
                setLoginError('');
            } else {
                setLoginError("Invalid usernamename or password");
            }
        } catch (error) {
            console.error("Login error: ", error);
            setLoginError("An error occurred during login. Please try again");
        }
    };

    return (
        <div className="home-page">
            <div className="create-profile-container">
                <h1>Welcome!</h1>

                <Link to="/CreateProfile">Create New Profile</Link>
            </div>

            <h3>Play as a character or select your personal profile</h3>

            {/* dropdown-container to place buttons in columns*/}
            <div className="dropdown-container">
                {/* Player selection drop downs in first (left) column */}
                <div className="left-column">
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
                                    {player.name} 
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
                                    {player.name}
                                </option>
                            ))}
                        </select>
                    </div>
                </div> {/* End left column div */}

                {/* Tile selection dropwdowns in second (right) column */}
                <div className="right-column">
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
                </div> {/* End right column div */}
            </div> {/* dropdown container div */}

            {/* Play Game button */}
            <button className="start-button" onClick={startGame}>Start Game</button>
            {message && <p>{message}</p>} {/*render message if back end sends one*/}
        </div>
    );
};

export default Home;