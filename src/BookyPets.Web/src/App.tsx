import './css/App.css'
import Home from './pages/Home'
import Books from './pages/Books'
import Pets from './pages/Pets'
import { Routes, Route } from "react-router-dom"
import NavBar from "./components/NavBar"

function App() {
    return (
        <div>
            <NavBar />
            <main className="main-content">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/books" element={<Books />} />
                    <Route path="/pets" element={<Pets />} />
                </Routes>
            </main>
        </div>
    );
}

export default App
