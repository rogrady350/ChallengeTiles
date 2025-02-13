import "../styles/Home.css";
import { Link } from 'react-router-dom';

const Home = () => {
    return (
        <div>
            <h2>Welcome!</h2>
            <Link to="/game">Go to Game Page</Link>
        </div>
    );
};

export default Home;