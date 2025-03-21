import { Link } from "react-router-dom";
import "../styles/Header.css"; // Import header styles

const Header = () => {
    return (
        <header>
            <nav>
                <Link to="/">Home</Link>
                <Link to="/">About</Link>
                <Link to="/">Instructions</Link>
                <Link to="/">View Stats</Link>
            </nav>
            <h1>Chalenge Tiles</h1>
        </header>
    );
};

export default Header;