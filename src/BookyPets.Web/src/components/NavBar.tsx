import { Link } from "react-router-dom";
import "../styles/NavBar.css"
import { AuthContext } from "../AuthContext";
import { useContext } from "react";

export default function NavBar() {
    const { user } = useContext(AuthContext)

    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar-links">
            {user.isAuth && (
                <>
                    <Link to="/session">Session</Link>
                    <Link to="/books">Books</Link>
                    <Link to="/pets">Pets</Link>
                </>
            )}
        </div>

        <div className="navbar-auth">
            <Link to="/auth" className="btn-secondary">Login</Link>
            <Link to="/auth" className="btn-primary">SignUp</Link>
        </div>
    </nav >
}
