import { Link, useNavigate } from "react-router-dom";
import "../styles/NavBar.css"
import { useAuth } from "../context/useAuth";

export default function NavBar() {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate("/");
    }

    return <nav className="navbar" aria-label="Main">
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
                        Hello, {user.firstname} {user.lastname}
                    </span>
                    <button type="button" className="btn navbar__logout" onClick={handleLogout}>Logout</button>
                </div>
            )
        }
    </nav >
}
