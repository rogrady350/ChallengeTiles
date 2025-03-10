//Service to make GET requests to backend

const API_URL = import.meta.env.VITE_API_URL; //gets url based on dev or production environment
//manage API calls for starting a new game (done from Home page)
console.log("Backend URL:", API_URL); //show which url is currently being used in console

//GET retrieve list of players (Client side for PlayerController.cs GetPlayers)
export const getPlayers = async () => {
    try {
        const response = await fetch(`${API_URL}/players?asList=false`);
        if (!response.ok) {
            throw new Error("Failed to fetch players");
        }
        return await response.json();
    } catch (error) {
        console.error("Error fetching players:", error);
        return [];
    }
};

//POST start a new game (Client side for GameController StartNewGame)
export const startNewGame = async (gameData) => {
    try {
        const response = await fetch(`${API_URL}/games/start-game`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(gameData)
        });

        if (!response.ok) {
            throw new Error("Failed to start game");
        }

        return await response.json();
    } catch (error) {
        console.error("Error starting game:", error);
        throw error;
    }
};

export default {
    getPlayers,
    startNewGame
};