import BookCard from "../components/BookCard";
import PetCard from "../components/PetCard";
import { getBooks } from "../data/booksmock";
import { getPets } from "../data/petsmock";
import "../styles/Home.css"

export default function Home() {
    const books = getBooks();
    const pets = getPets();

    return (
        <div className="page">
            <div className="home-hero">
                <h1 className="home-title">Welcome to Booky Pets</h1>
                <p className="home-subtitle">Every book deservse a companion</p>
            </div>

            <div className="container">
                <h2 className="page-title">Books</h2>
                <div className="container-grid">
                    {books.map((book) => (
                        <BookCard book={book} key={book.id} />
                    ))}
                </div>
            </div>

            <div className="container">
                <h2 className="page-title">Pets</h2>
                <div className="container-grid">
                    {pets.map((pet) => (
                        <PetCard pet={pet} key={pet.id} />
                    ))}
                </div>
            </div>
        </div>
    )
}
