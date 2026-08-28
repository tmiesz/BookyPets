import { Link } from "react-router-dom";
import "../css/NavBar.css"

function NavBar() {
    return <nav className="navbar">
        <div className="navbar-brand">
            <Link to="/">Booky Pets</Link>
        </div>

        <div className="navbar-links">
            <Link to="/" className="nav-link">Home</Link>
            <Link to="/books" className="nav-link">Books</Link>
            <Link to="/pets" className="nav-link">Pets</Link>
        </div>
    </nav>
}

export default NavBar

