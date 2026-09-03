import { Link } from "react-router-dom";
import "../styles/NavBar.css"
import { AuthContext } from "../AuthContext";
import { useContext } from "react";

export default function NavBar() {
    const { user, logout } = useContext(AuthContext)

    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar-links">
            <Link to="/" className="nav-link">Home</Link>
            {!user.isAuth && <Link to="/auth" className="nav-link">Login</Link>}
            {user.isAuth && <Link to="/session" className="nav-link">Session</Link>}
            {user.isAuth && <Link to="/books" className="nav-link">Books</Link>}
            {user.isAuth && <Link to="/pets" className="nav-link">Pets</Link>}
            {user.isAuth && <button onClick={logout}>Logout</button>}
        </div>
    </nav>
}
