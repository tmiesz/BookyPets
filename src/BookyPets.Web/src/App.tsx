import './css/App.css'
import Home from './pages/Home'
import Books from './pages/Books'
import Pets from './pages/Pets'
import NotFound from './pages/NotFound'
import NavBar from "./components/NavBar"
import { Routes, Route } from "react-router-dom"
import { useState } from 'react'
import { AuthContext } from './AuthContext.ts'

function App() {
    const [user, setUser] = useState({ name: "", isAuth: false })

    function login(name: string) {
        setUser({ name: name, isAuth: true })
    }

    function logout() {
        setUser({ name: "", isAuth: false })
    }

    return (
        <div>
            <AuthContext.Provider value={{ user, login, logout }}>
                <NavBar />

                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/books" element={<Books />} />
                    <Route path="/pets" element={<Pets />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </AuthContext.Provider>
        </div>
    );
}

export default App
