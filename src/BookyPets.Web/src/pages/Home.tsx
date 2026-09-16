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
            <div className="home">
                <h1 className="home__title">Welcome to Booky Pets</h1>
                <p className="home__subtitle">Every book deservse a companion</p>
            </div>

            <div className="container">
                <h2 className="container__title">Books</h2>
                <div className="container__grid">
                    {books.map((book) => (
                        <div className="container__item">
                            <BookCard book={book} key={book.id} />
                        </div>
                    ))}
                </div>
            </div>

            <div className="container">
                <h2 className="container__title">Pets</h2>
                <div className="container__grid">
                    {pets.map((pet) => (
                        <div className="container__item">
                            <PetCard pet={pet} key={pet.id} />
                        </div>
                    ))}
                </div>
            </div>
        </div>
    )
}
