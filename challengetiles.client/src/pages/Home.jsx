import "../styles/Home.css";
import { useEffect, useState } from "react";
import GameSetupService from '../service/GameSetupService';

const Home = () => {
    //values to be set for game
    //useState watches for change and re-renders page. format: const [value, setValue] = useState(initialValue);
    const [players, setPlayers] = useState([]);
    const [player1, setPlayer1] = useState('');
    const [player2, setPlayer2] = useState('');
    const [numColors, setNumColors] = useState(2); //suggest playing with 2 colors
    const [numTiles, setNumTiles] = useState(7);   //suggest playing with Hands of 7 Tiles

    //fetch players from backend on page load
    //useEffect handles side effects (things that happen outside React)
    useEffect(() => {
        GameSetupService.getAllPlayers() //call backend API function
            .then(response => {
                setPlayers(response.data); //render drop downs with Player's
            })
            .catch(error => {
                console.error("Error fetching players", error);
            });
    }, []);

    return (
        <div>
            <h2>Welcome!</h2>

            {/* Player 1 dropdown */}

        </div>
    );
};

export default Home;