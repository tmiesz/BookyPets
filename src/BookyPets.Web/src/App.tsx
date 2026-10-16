import Home from './pages/Home'
import Books from './pages/Books'
import Pets from './pages/Pets'
import NotFound from './pages/NotFound'
import NavBar from "./components/NavBar"
import { Routes, Route } from "react-router-dom"
import Session from './pages/Session.tsx'
import AuthProvider from './context/AuthContext.tsx'
import Login from './pages/Login.tsx'
import Register from './pages/Register.tsx'
import RequireAuth from './components/RequireAuth.tsx'
import GuestOnly from './components/GuestOnly.tsx'

function App() {
    return (
        <AuthProvider>
            <div className='app'>
                <NavBar />
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route element={<GuestOnly />}>
                        <Route path="/login" element={<Login />} />
                        <Route path="/register" element={<Register />} />
                    </Route>
                    <Route element={<RequireAuth />}>
                        <Route path="/session" element={<Session />} />
                        <Route path="/books" element={<Books />} />
                        <Route path="/pets" element={<Pets />} />
                    </Route>
                    <Route path="*" element={<NotFound />} />
                </Routes>
            </div>
        </AuthProvider>
    );
}

export default App
