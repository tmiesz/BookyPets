import type { Book } from "../types/Book";
import "../styles/BookCard.css"
import { BASE_URL } from "../services/api";

interface BookCardProps {
    book: Book
}

function BookCard({ book }: BookCardProps) {

    function onAcquireClick() {
        alert("clickd");
    }

    return (
        <div className="book-card">
            <div className="book-info">
                <h3>{book.title}</h3>
                <p>{book.author}</p>
                <p>{book.genre}</p>
                <p>{book.pageCount} pages</p>
            </div>
            <div className="book-overlay">
                <button className="acquire-button" onClick={onAcquireClick}>
                    +
                </button>
                <div className="book-image">
                    <img src={`${BASE_URL}${book.iconUrl}`} />
                </div>
            </div>
        </div>
    )
}

export default BookCard
