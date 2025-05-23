//Main app component

import './styles/App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home'; // import Home component
import GamePage from './pages/GamePage'
import Header from './components/Header'; // import Header component
import About from './pages/About';
import Instructions from './pages/Instructions';
import ViewStats from './pages/ViewStats';
import CreateProfile from './pages/CreateProfile';

const App = () => {
    return (
        <Router>
            <Header />
            <main>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/game/:gameId" element={<GamePage />} />
                    <Route path="/About" element={<About />} />
                    <Route path="/Instructions" element={<Instructions />} />
                    <Route path="/ViewStats" element={<ViewStats />} />
                    <Route path="/CreateProfile" element={<CreateProfile />} />
                </Routes>
            </main>
        </Router>
    );
};

export default App;