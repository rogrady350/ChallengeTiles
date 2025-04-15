//Page to display player profile and game stats

//css
import "../styles/viewStats.css";

//Hoooks
import { useEffect } from "react";

//services
import { fetchAllGameStats } from "../services/gameService";

const ViewStats = () => {
    //hook for game data

    useEffect(() => {
        
    }, []) //only run once on game mount

    return (
        <div>
            <h1>Game Stats</h1>


        </div>
    )
}

export default ViewStats