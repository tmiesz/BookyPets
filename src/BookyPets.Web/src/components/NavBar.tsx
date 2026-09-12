import { Link } from "react-router-dom";
import "../styles/NavBar.css"
import { AuthContext } from "../context/AuthContext";
import { useContext } from "react";

export default function NavBar() {
    const authContext = useContext(AuthContext);
    if (!authContext) throw new Error("Auth must be used within AuthProvider");
    const { token, user, logout } = authContext;

    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar-links">
            {token && (
                <>
                    <Link to="/session" className="navbar-link">Session</Link>
                    <Link to="/books" className="navbar-link">Books</Link>
                    <Link to="/pets" className="navbar-link">Pets</Link>
                </>
            )}
        </div>
        {!token ?
            (

                <div className="navbar-auth">
                    <Link to="/login" className="btn btn-secondary">Login</Link>
                    <Link to="/register" className="btn btn-primary">SignUp</Link>
                </div>
            )
            :
            (
                <div className="navbar-user">
                    <span className="navbar-greeting">
                        Hello, {user?.firstname} {user?.lastname}
                    </span>
                    <button className="btn btn-secondary" onClick={logout}>Logout</button>
                </div>
            )
        }
    </nav >
}
