import { useMemo, useState, type SubmitEvent } from "react";
import "../styles/Books.css"
import AcquireBookCard from "../components/AcquireBookCard";
import ErrorMessage from "../components/ErrorMessage";
import { useBooks, useLibrary } from "../hooks/queries";
import { toApiError } from "../types/ApiError";

export default function Books() {
    const [searchInput, setSearchInput] = useState("");
    const [query, setQuery] = useState("");

    const booksQuery = useBooks({ search: query });
    const libraryQuery = useLibrary();

    const books = booksQuery.data ?? [];
    const ownedBookIds = useMemo(
        () => new Set((libraryQuery.data ?? []).map((entry) => entry.book.id)),
        [libraryQuery.data]);

    const handleSearch = (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()
        setQuery(searchInput.trim())
    };

    const showResults = !booksQuery.isPlaceholderData;

    return (
        <div className="books">
            <form onSubmit={handleSearch} className="books__form" role="search">
                <input
                    type="text"
                    placeholder="Search for books..."
                    aria-label="Search for books"
                    className="books__input"
                    value={searchInput}
                    onChange={(e) => setSearchInput(e.target.value)} />
                <button type="submit" className="btn books__search">Search</button>
            </form>

            <ErrorMessage error={booksQuery.error ? toApiError(booksQuery.error) : null} className="books__error" />

            {booksQuery.isPending ? <div className="books__loading" role="status">Loading...</div> :
                books.length === 0 ? (!booksQuery.isError && showResults && (
                    <div className="books__empty">{query ? `No books found for "${query}".` : "No books found."}</div>
                )) :
                    <>
                        {query && showResults && (
                            <p className="books__summary" role="status">
                                {books.length} {books.length === 1 ? "result" : "results"} for "{query}"
                            </p>
                        )}
                        <div className="books__grid">
                            {books.map((book) => (
                                <AcquireBookCard book={book} owned={ownedBookIds.has(book.id)} key={book.id} />
                            ))}
                        </div>
                    </>
            }
        </div>
    );
}
