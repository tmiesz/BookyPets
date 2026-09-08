import { Link } from "react-router-dom";
import "../styles/NavBar.css"
import { AuthContext } from "../context/AuthContext";
import { useContext } from "react";

export default function NavBar() {
    const authContext = useContext(AuthContext);
    if (!authContext) throw new Error("Auth must be used within AuthProvider");
    const auth = authContext;

    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar-links">
            {auth.token && (
                <>
                    <Link to="/session" className="navbar-link">Session</Link>
                    <Link to="/books" className="navbar-link">Books</Link>
                    <Link to="/pets" className="navbar-link">Pets</Link>
                </>
            )}
        </div>

        <div className="navbar-auth">
            <Link to="/auth" className="btn btn-secondary">Login</Link>
            <Link to="/auth" className="btn btn-primary">SignUp</Link>
        </div>
    </nav >
}
