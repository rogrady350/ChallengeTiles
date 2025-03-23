import { Link } from "react-router-dom";
import "../styles/Header.css"; // Import header styles

const Header = () => {
    return (
        <header>
            <nav>
                <Link to="/">Home</Link>
                <Link to="/About">About</Link>
                <Link to="/Instructions">Instructions</Link>
                <Link to="/ViewStats">View Stats</Link>
            </nav>
            <img
                src={"../images/logo.png"}
                alt="Challenge Tiles"
                className="logo"
            />
        </header>
    );
};

export default Header;