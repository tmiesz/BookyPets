import type { Book } from "../types/Book";
import "../css/BookCard.css"
interface BookCardProps {
    book: Book
}

function BookCard({ book }: BookCardProps) {

    function onAcquireClick() {
        alert("clickd");
    }

    return <div className="book-card">
        <div className="book-image">
            <div className="book-overlay">
                <button className="acquire-button" onClick={onAcquireClick}>
                    +
                </button>
            </div>
        </div>

        <div className="book-info">
            <h3>{book.title}</h3>
            <p>{book.author}</p>
            <p>{book.genre}</p>
            <p>{book.pageCount} pages</p>
        </div>
    </div>
}

export default BookCard
