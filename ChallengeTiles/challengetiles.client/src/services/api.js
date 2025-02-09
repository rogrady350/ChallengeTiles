//api utility
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL; //set in .env file
console.log("Backend URL:", API_BASE_URL); //show which url is currently being used in console

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