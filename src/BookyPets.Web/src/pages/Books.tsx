import { useState, useEffect, type SubmitEvent } from "react";
import { getBooks } from "../services/api";
import type { Book } from "../types/Book";
import "../styles/Books.css"
import AcquireBookCard from "../components/AcquireBookCard";
import type { ApiError } from "../types/ApiError";

export default function Books() {
    const [searchQuery, setSearchQuery] = useState("");
    const [books, setBooks] = useState<Book[]>([]);
    const [error, setError] = useState<ApiError | null>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const loadBooks = async () => {
            try {
                const books = await getBooks()
                setBooks(books)
            } catch (err) {
                setError(err as ApiError)
            }
            finally {
                setLoading(false)
            }
        }

        loadBooks()
    }, [])

    const handleSearch = async (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()

        if (loading) return;
        const query = searchQuery.trim()

        setLoading(true);

        try {
            const result = query ? await getBooks(searchQuery) : await getBooks()
            setBooks(result)
            setError(null)
        } catch (err) {
            setError(err as ApiError)
        } finally {
            setLoading(false)
        }

        setSearchQuery("")
    };

    return (
        <div className="books">
            <form onSubmit={handleSearch} className="books_form">
                <input
                    type="text"
                    placeholder="Search for books..."
                    className="books_input"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)} />
                <button type="submit" className="btn books_search">Search</button>
            </form>

            {error && <div className="books__error">{error.detail}</div>}

            {loading ? <div className="books__loading">Loading...</div> :
                books.length === 0 ? (!error && <div className="books__empty">No books found.</div>) :
                    <div className="books-grid">
                        {books.map((book) => (
                            <AcquireBookCard book={book} key={book.id} />
                        ))}
                    </div>
            }
        </div>
    );
}
