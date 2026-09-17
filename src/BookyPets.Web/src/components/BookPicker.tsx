import { useEffect, useState } from "react";
import { getReaderBooks } from "../services/api";
import "../styles/Picker.css";
import type { Book } from "../types/Book";
import BookCard from "./BookCard";
import type { ApiError } from "../types/ApiError";

type Props = {
    onClose: () => void;
    onSelect: (book: Book) => void;
};

export default function BookPicker({ onClose, onSelect }: Props) {
    const [books, setBooks] = useState<Book[]>([]);
    const [error, setError] = useState<ApiError | null>(null);

    useEffect(() => {
        getReaderBooks().then(setBooks).catch((error) => setError(error))
    }, []);

    const handleSelect = (book: Book) => {
        onSelect(book);
        onClose();
    }

    return (
        <div className="picker-overlay">
            <div className="picker">
                <button onClick={onClose}>Close</button>
                <p>Pick a book</p>
                {error && <div className="error-message">{error.detail}</div>}
                <div>
                    {books.map((b) => (
                        <button key={b.id} className="picker-button" onClick={() => handleSelect(b)}>
                            <BookCard key={b.id} book={b} />
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}
