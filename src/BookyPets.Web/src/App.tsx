import './styles/App.css'
import Home from './pages/Home'
import Books from './pages/Books'
import Pets from './pages/Pets'
import NotFound from './pages/NotFound'
import NavBar from "./components/NavBar"
import { Routes, Route } from "react-router-dom"
import Auth from './pages/Auth.tsx'
import Session from './pages/Session.tsx'
import AuthProvider from './context/AuthContext.tsx'

function App() {
    return (
        <AuthProvider>
            <div className='app'>
                <NavBar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/auth" element={<Auth />} />
                    <Route path="/session" element={<Session />} />
                    <Route path="/books" element={<Books />} />
                    <Route path="/pets" element={<Pets />} />
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </div>
        </AuthProvider >
    );
}

export default App
