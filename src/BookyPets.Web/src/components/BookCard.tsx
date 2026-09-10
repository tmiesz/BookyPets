import type { Book } from "../types/Book";
import "../styles/BookCard.css"
import { BASE_URL } from "../services/api";

interface BookCardProps {
    book: Book
}

function BookCard({ book }: BookCardProps) {

    return (
        <div className="book-card">
            <div className="book-info">
                <h3>{book.title}</h3>
                <p>{book.author}</p>
            </div>
            <div className="book-overlay">
                <div className="book-image">
                    <img src={`${BASE_URL}${book.iconUrl}`} />
                    <p>{book.genre}</p>
                </div>
            </div>
        </div>
    )
}

export default BookCard
