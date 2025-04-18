//game specific api calls
//determine API Base URL dynamically
const API_BASE_URL = window._env_?.VITE_API_URL || import.meta.env.VITE_API_URL;

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

//POST - send selected starting player to server
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

//POST - request tile from deck
export const pickUpTile = async (gameId, playerId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/pick-up-tile`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ playerId })
        });

        if (!response.ok) throw new Error("Failed to set pick up tile")

        return await response.json();
    } catch (error) {
        console.error("Error picking up tile: ", error);
        return null;
    }
};

//POST - request tile placement in selecte position
export const placeTile = async (gameId, playerId, tile, position) => {
    try {
        console.log("Tile being sent to backend:", tile);

        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/player-place-tile/${playerId}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ playerId, tile, position }),
        });

        const result = await response.json();

        //display backend error message if illegal move attempted (or unknown error if no message)
        if (!response.ok) {
            return { success: false, message: result.message || "Unknown error" };
        }

        //update state after placement
        if (result.success) {
            console.log("Tile placed successfully.", result.message);
            return await fetchGameState(gameId);  
        }

        return result;
    } catch (error) {
        console.error("Error placing tile:", error);
        return { success: false, message: "An error occurred. Please try again." };
    }
}

//GET - receive game stats for single game (Not used currently)
export const fetchGameStats = async (gameId) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/game-details`);
        if (!response.ok) throw new Error("Failed to fetch game details");

        return await response.json();
    } catch (error) {
        console.error("Error fetching game stats: ", error);
        return null;
    }
};

//GET - receive game stats for all games (used for Viewing Stats)
export const fetchAllGameStats = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/Game/all-game-details`);
        if (!response.ok) throw new Error("Failed to fetch all game details");

        return await response.json();
    } catch (error) {
        console.error("Error fetching all game stats: ", error);
        return null;
    }
};

export default {
    fetchGameState,
    setStartingPlayer,
    pickUpTile,
    placeTile,
    fetchGameStats,
    fetchAllGameStats
}