//Service to make requests to backend when starting a new game
//determine API Base URL dynamically
const API_BASE_URL = window._env_?.VITE_API_URL || import.meta.env.VITE_API_URL;

//manage API calls for starting a new game (done from Home page)
console.log("Backend URL:", API_BASE_URL); //show which url is currently being used in console

//GET retrieve list of players (Client side for PlayerController.cs GetPlayers)
export const getPlayers = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/Player/players?asList=false`);
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
        const response = await fetch(`${API_BASE_URL}/Game/start-game`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(gameData),
            credentials: 'include'
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
