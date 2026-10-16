import type { LibraryEntry } from "../types/LibraryEntry";
import LibraryEntryCard from "./LibraryEntryCard";
import PickerDialog from "./PickerDialog";

type Props = {
    entries: LibraryEntry[];
    onClose: () => void;
    onSelect: (entry: LibraryEntry) => void;
};

export default function LibraryEntryPicker({ entries, onClose, onSelect }: Props) {
    const handleSelect = (entry: LibraryEntry) => {
        onSelect(entry);
        onClose();
    }

    return (
        <PickerDialog title="Pick a book" onClose={onClose}>
            {entries.length === 0 ? (
                <p className="picker__empty">Your library is empty. Acquire a book on the Books page first.</p>
            ) : (
                <ul className="picker__list">
                    {entries.map((entry) => (
                        <li key={entry.book.id} className="picker__item">
                            <LibraryEntryCard entry={entry} />
                            <button
                                type="button"
                                className="picker__option"
                                aria-label={`Select ${entry.book.title}`}
                                onClick={() => handleSelect(entry)} />
                        </li>
                    ))}
                </ul>
            )}
        </PickerDialog>
    );
}
