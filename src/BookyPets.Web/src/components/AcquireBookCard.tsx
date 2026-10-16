import type { Book } from "../types/Book";
import "../styles/Book.css"
import BookCard from "./BookCard";
import ErrorMessage from "./ErrorMessage";
import { useAcquireBook } from "../hooks/queries";
import { toApiError } from "../types/ApiError";

interface AcquireBookCardProps {
    book: Book,
    owned: boolean
}

export default function AcquireBookCard({ book, owned }: AcquireBookCardProps) {
    const acquire = useAcquireBook();
    const isOwned = owned || acquire.isSuccess;

    return (
        <div className="book__acquire">
            <BookCard book={book} />
            <button
                type="button"
                className="btn book__acquire__button"
                onClick={() => acquire.mutate(book.id)}
                disabled={isOwned || acquire.isPending}>
                {isOwned ? "In your library" : acquire.isPending ? "Acquiring..." : "Acquire Book"}
            </button>
            <ErrorMessage error={acquire.error ? toApiError(acquire.error) : null} />
        </div>
    )
}
