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
        <div className="picker-overlay">
            <div className="picker">
                <button onClick={onClose}>Close</button>
                <p>Pick a book</p>
                {error && <div className="error-message">{error.detail}</div>}
                <div>
                    {entries.map((entry) => (
                        <button key={entry.book.id} className="picker-button" onClick={() => handleSelect(entry)}>
                            <LibraryEntryCard entry={entry} />
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}
