//player related api calls
const API_BASE_URL = window._env_?.VITE_API_URL || import.meta.env.VITE_API_URL;

//POST - send player data to server
export const registerPlayer = async (playerData) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Player/register-player`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(playerData),
        });

        //PlayerService.RegisterProfile validates profile data using ValidateInput method and returns response if invalid.
        const result = await response.json();
        if (!response.ok) throw new Error(result.message || "Failed to register player");

        return result;
    } catch (error) {
        console.error("Error registering player: ", error);
        return null;
    }
};

//POST - send login data to server
export  const handleLogin = async (loginData) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Player/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginData),
        });

        const result = await response.json();
        if (!response.ok) throw new Error(result.message || "Failed to login");

        return result;
    } catch (error) {
        console.error("Error logging in: ", error);
        return null;
    }
};

export default {
    registerPlayer,
    handleLogin
};