import type { Book } from "../types/Book";
import "../styles/Book.css"
import { BASE_URL } from "../services/api";

interface BookCardProps {
    book: Book
}

export default function BookCard({ book }: BookCardProps) {

    return (
        <div className="book">

            <div className="book__info">
                <h3>{book.title}</h3>
                <p>{book.author}</p>
                <p>{book.genre}</p>
            </div>

            <div className="book__cover">
                <img src={`${BASE_URL}${book.iconUrl}`} />
            </div>

        </div>
    )
}
