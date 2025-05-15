//Register new Player form page

//css
import "../styles/createProfile.css";

//Hooks
import { useState } from "react";
import { useNavigate } from 'react-router-dom';

//services
import playerService from "../services/playerService";

const CreateProfile = () => {
    const [formData, setFormData] = useState({ username: "", password: "", name: "", email: "" }); //hook to manage form field values. store as states.
    const [message, setMessage] = useState("");
    const navigate = useNavigate();

    //dynamically update form field values
    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleCreateProfile = async (e) => {
        e.preventDefault(); //prevent browswer from reloading whole page. js manually handles form submission

        try {
            //Call API function to send Player profile data
            const response = await playerService.registerPlayer({
                UserName: formData.username,
                Password: formData.password,
                Name: formData.name,
                Email: formData.email
            })

            if (response) {
                setMessage('Profile created successfully!');
                navigate('/'); //redirect to home page
            } else {
                setMessage('Failed to create profile.');
            }
            //input length validations


        } catch (error) {
            console.error('Error creating profile:', error);
            setMessage('An error occurred. Please try again.');
        }
    };

    return (
        <div className="create-profile-container">
            <h1>Create Your Profile</h1>

            <form onSubmit={handleCreateProfile}>
                <div className="input-group">
                    <label>Username:</label>
                    <input type="text" name="username" value={formData.username} onChange={handleChange} required />
                </div>

                <div className="input-group">
                    <label>Password:</label>
                    <input type="password" name="password" value={formData.password} onChange={handleChange} required />
                </div>

                <div className="input-group">
                    <label>Name:</label>
                    <input type="text" name="name" value={formData.name} onChange={handleChange} required />
                </div>

                <div className="input-group">
                    <label>Email:</label>
                    <input type="email" name="email" value={formData.email} onChange={handleChange} required />
                </div>

                <button type="submit">Create Profile</button>
            </form>
            {message && <p className="message">{message}</p>}
        </div>
    );
};

export default CreateProfile;