//Service to make GET requests to back end

const API_URL = import.meta.env.VITE_API_URL; //gets url based on dev or production environment

export const getPlayers = async () => {
    try {
        const response = await fetch(`${API_URL}/players?asList=true`);
        if (!response.ok) {
            throw new Error("Failed to fetch players");
        }
        return await response.json();
    } catch (error) {
        console.error("Error fetching players:", error);
        return [];
    }
};