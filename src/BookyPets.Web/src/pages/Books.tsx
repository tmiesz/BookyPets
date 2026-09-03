import BookCard from "../components/BookCard"
import { useState, useEffect, type SubmitEvent } from "react";
import { getBooks } from "../services/api";
import type { Book } from "../types/Book";
import "../styles/Books.css"

function Home() {
    const [searchQuery, setSearchQuery] = useState("");
    const [books, setBooks] = useState<Book[]>([]);
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const loadBooks = async () => {
            try {
                const books = await getBooks()
                setBooks(books)
            } catch (err) {
                console.log(err)
                setError("Failed to load books...")
            }
            finally {
                setLoading(false)
            }
        }

        loadBooks()
    }, [])

    const handleSearch = async (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()

        if (!searchQuery.trim()) return
        if (loading) return

        setLoading(true);

        try {
            const searchResult = await getBooks(searchQuery)
            setBooks(searchResult)
            setError(null)
        } catch (err) {
            console.log(err)
            setError("Failed to search books...")
        } finally {
            setLoading(false)
        }

        setSearchQuery("")
    };

    return (
        <div className="books">
            <form onSubmit={handleSearch} className="search-form">
                <input
                    type="text"
                    placeholder="Search for books..."
                    className="search-input"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)} />
                <button type="submit" className="search-button">Search</button>
            </form>

            {error && <div className="error-message">{error}</div>}

            {loading ? <div className="loading">Loading...</div> :
                <div className="books-grid">
                    {books.map((book) => (
                        <BookCard book={book} key={book.id} />
                    ))}
                </div>
            }
        </div>
    );
}

export default Home
