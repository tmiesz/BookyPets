import { useEffect, useState } from "react";
import { getLibrary } from "../services/api";
import "../styles/Picker.css";
import type { ApiError } from "../types/ApiError";
import type { LibraryEntry } from "../types/LibraryEntry";
import LibraryEntryCard from "./LibraryEntryCard";

type Props = {
    onClose: () => void;
    onSelect: (entry: LibraryEntry) => void;
};

export default function LibraryEntryPicker({ onClose, onSelect }: Props) {
    const [entries, setEntries] = useState<LibraryEntry[]>([]);
    const [error, setError] = useState<ApiError | null>(null);

    useEffect(() => {
        getLibrary().then(setEntries).catch((error) => setError(error))
    }, []);

    const handleSelect = (entry: LibraryEntry) => {
        onSelect(entry);
        onClose();
    }

    return (
        <div className="picker" onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}>
            <div className="picker__dialog" role="dialog" aria-modal="true" aria-labelledby="book-picker-title">
                <div className="picker__header">
                    <h2 className="picker__title" id="book-picker-title">Pick a book</h2>
                    <button className="btn picker__close" onClick={onClose}>Close</button>
                </div>

                {error && <div className="picker__error">{error.detail}</div>}

                <div className="picker__list">
                    {entries.map((entry) => (
                        <button key={entry.book.id} className="picker__option" onClick={() => handleSelect(entry)}>
                            <LibraryEntryCard entry={entry} />
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}
