//game specific api calls
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL; //url based on env



export const fetchTiles = async () => {
    try {
        const response = await fetch(`${API_BASE_URL}/api/game/tiles`);
        if (!response.ok) throw new Error("Failed to fetch tiles");
        return await response.json();
    } catch (error) {
        console.error("Error fetching tiles:", error);
        return [];
    }
};