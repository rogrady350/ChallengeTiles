//game specific api calls
const API_BASE_URL = import.meta.env.VITE_API_URL; //url based on env

//GET - recieves current state of game objecs from server
export const fetchGameState = async (gameId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/game-state`);
        if (!response.ok) throw new Error("Failed to fetch game state");

        return await response.json();
    } catch (error) {
        console.error("Error fetching game state: ", error);
        return null;
    }
};

//Post - send selected starting player to server
export const setStartingPlayer = async (gameId, playerId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/set-starting-player/${playerId}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });

        if (!response.ok) throw new Error("Failed to set starting player")

        return await response.json();
    } catch (error) {
        console.error("Error setting starting player: ", error);
        return null;
    }
}

export default {
    fetchGameState,
    setStartingPlayer
}