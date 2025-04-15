//Page to display player profile and game stats

//css
import "../styles/viewStats.css";

//Hoooks
import { useEffect, useState } from "react";

//services
import { fetchAllGameStats } from "../services/gameService";

const ViewStats = () => {
    //hook for game data
    const [gameData, setGameData] = useState([]);

    useEffect(() => {
        const fetchGameData = async () => {
            const data = await fetchAllGameStats();
            setGameData(data);
        }

        fetchGameData();
    }, []); //only run once on game mount

    return (
        <div className = "view-stats-page">
            <h1>Game Stats</h1>
            <table>
                <thead>
                    <tr>
                        <th>Game ID</th>
                        <th>Score</th>
                        <th>Number of Colors</th>
                        <th>Number of Tiles</th>
                        <th>Player 1</th>
                        <th>Player 2</th>
                    </tr>
                </thead>
                <tbody>
                    {gameData.map((game) => (
                        <tr key={game.gameId}>
                            <td>{game.gameId}</td>
                            <td>{game.score}</td>
                            <td>{game.numberOfColors}</td>
                            <td>{game.numberOfTiles}</td>
                            <td>{game.players[0]?.name}</td>
                            <td>{game.players[1]?.name}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

        </div>
    )
}

export default ViewStats