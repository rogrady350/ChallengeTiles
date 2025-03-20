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
export const placeTile = async (gameId, playerId, tile, x, y) => {
    try {
        console.log("Tile being sent to backend:", tile);

        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/player-place-tile/${playerId}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ playerId, tile, x, y }),
        });

        const result = await response.json();

        //update state after placement
        if (result.success) {
            console.log("Tile placed successfully.");
            return await fetchGameState(gameId);  
        }
        //error if unsuccessful placement
        console.error("Tile placement failed:", result.message);
        return result;
    } catch (error) {
        console.error("Error placing tile:", error);
        return { success: false, message: "An error occurred. Please try again." };
    }
}

export default {
    fetchGameState,
    setStartingPlayer,
    pickUpTile,
    placeTile
}