//game specific api calls
const API_BASE_URL = import.meta.env.VITE_API_URL; //url based on env

//GET - recieves current state of game objecs from server
export const fetchGameState = async (gameId) => {
    try {
        const requestUrl = `${API_BASE_URL}/Game/${gameId}/game-state`;
        console.log("Fetching game state from:", requestUrl)
        console.log(`Fetching game state for Game ID: ${gameId}`);

        const response = await fetch(`${API_BASE_URL}/Game/${gameId}/game-state`);

        console.log("Response status:", response.status);
        console.log("Response headers:", response.headers);

        if (!response.ok) throw new Error("Failed to fetch game state");

        const text = await response.text();
        console.log("Text response:", text); //text response before parsing JSON

        const data = JSON.parse(text); //convert only if it's valid JSON
        console.log("Fetched game state:", data);
        return data;
    } catch (error) {
        console.error("Error fetching game state:", error);
        return null;
    }
};

export default {
    fetchGameState
}