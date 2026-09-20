import "../styles/LibraryEntryCard.css"
import type { LibraryEntry } from "../types/LibraryEntry";
import BookCard from "./BookCard";

interface LibraryEntryCardProps {
    entry: LibraryEntry
}

export default function LibraryEntryCard({ entry }: LibraryEntryCardProps) {

    const { currentPage, totalPages } = entry.progress;
    const percent = totalPages > 0 ? Math.min(100, (currentPage / totalPages) * 100) : 0;

    return (
        <div className="library-entry-card">
            <BookCard book={entry.book} />
            <div className="library-entry-card__progress">
                <div className="library-entry-card__progress-track">
                    <div className="library-entry-card__progress-fill" style={{ width: `${percent}%` }} />
                </div>
                <span className="library-entry-card__progress-label">{currentPage} of {totalPages} pages</span>
            </div>
        </div>)
}
