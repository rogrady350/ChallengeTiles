import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home'; // import Home component
import Header from './components/Header'; // import Header component

const RouterComponent = () => {
    return (
        <Router>
            <Header />
             <main>
                <Routes>
                    <Route path="/" element={<Home />} />
                </Routes>
            </main>
        </Router>
    );
};

export default RouterComponent;