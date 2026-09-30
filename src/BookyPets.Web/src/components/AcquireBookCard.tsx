import type { Book } from "../types/Book";
import "../styles/Book.css"
import BookCard from "./BookCard";
import { acquireBook } from "../services/api";
import { useState } from "react";
import type { ApiError } from "../types/ApiError";

interface BookCardProps {
    book: Book
}

export default function AcquireBookCard({ book }: BookCardProps) {
    const [error, setError] = useState<ApiError | null>(null);

    const handleAcquire = async () => {
        setError(null);

        try {
            await acquireBook(book.id);
        } catch (error) {
            setError(error as ApiError);
        }
    };

    return (
        <div className="acquire">
            <BookCard book={book} />
            {error ? <p>{error.detail}</p> :
                <button className="btn acquire__button" onClick={handleAcquire}>Acquire Book</button>
            }
        </div>
    )
}
