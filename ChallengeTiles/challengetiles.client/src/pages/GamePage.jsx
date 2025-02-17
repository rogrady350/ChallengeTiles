//Main game interface. Renders GameBoard, Hand, player info

import { useState, useEffect } from "react";
import GameBoard from "../components/GameBoard";
import { fetchTiles } from "../services/api"

const GamePage = () => {
    const [tiles, setTiles] = useState([]);

    useEffect(() => {
        fetchTiles().then(setTiles);
    }, []);

    return (
        <div className="game-page">
            <GameBoard tiles={tiles} />
        </div>
    );
};

export default GamePage;