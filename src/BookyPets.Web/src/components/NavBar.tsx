import { Link } from "react-router-dom";
import "../styles/NavBar.css"
import { useAuth } from "../context/AuthContext";

export default function NavBar() {
    const { user, logout } = useAuth();

    return <nav className="navbar">
        <div className="navbar__brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar__links">
            {user && (
                <>
                    <Link to="/session" className="navbar__link">Session</Link>
                    <Link to="/books" className="navbar__link">Books</Link>
                    <Link to="/pets" className="navbar__link">Pets</Link>
                </>
            )}
        </div>
        {!user ?
            (

                <div className="navbar__auth">
                    <Link to="/login" className="btn navbar__login">Login</Link>
                    <Link to="/register" className="btn navbar__signup">SignUp</Link>
                </div>
            )
            :
            (
                <div className="navbar__user">
                    <span className="navbar__greeting">
                        Hello, {user?.firstname} {user?.lastname}
                    </span>
                    <button className="btn navbar__logout" onClick={logout}>Logout</button>
                </div>
            )
        }
    </nav >
}
