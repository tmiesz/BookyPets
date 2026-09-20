import { useEffect, useState } from "react";
import PetCard from "../components/PetCard";
import "../styles/Session.css";
import PetPicker from "../components/PetPicker";
import type { Pet } from "../types/Pet";
import { getLibrary, getPets } from "../services/api";
import type { ApiError } from "../types/ApiError";
import type { LibraryEntry } from "../types/LibraryEntry";
import LibraryEntryPicker from "../components/LibraryEntryPicker";
import LibraryEntryCard from "../components/LibraryEntryCard";

export default function Session() {
    const [picker, setPicker] = useState<"book" | "pet" | null>(null);
    const [selectedEntry, setSelectedEntry] = useState<LibraryEntry | null>(null);
    const [selectedPet, setSelectedPet] = useState<Pet | null>(null);
    const [error, setError] = useState<ApiError | null>(null);

    useEffect(() => {
        getLibrary()
            .then((books) => setSelectedEntry(books[0] ?? null))
            .catch((error) => setError(error));

        getPets()
            .then((pets) => setSelectedPet(pets[0] ?? null))
            .catch((error) => setError(error));
    }, []);

    return (
        <div className="page">
            <div className="container">
                <div className="session-container">
                    <h1 className="page-title">Session</h1>

                    {error && <div className="error-message">{error.detail}</div>}

                    <div className="session-layout">
                        <div className="session-item">
                            <p>Choose a book</p>
                            <button onClick={() => setPicker("book")}>Choose</button>
                            {selectedEntry && <LibraryEntryCard entry={selectedEntry} />}
                            {picker === "book" && <LibraryEntryPicker onClose={() => setPicker(null)} onSelect={setSelectedEntry} />}
                        </div>

                        <div className="session-item">
                            <button className="start-session-button" disabled={!selectedEntry} onClick={() => { /* todo */ }}>Start a session</button>
                        </div>

                        <div className="session-item">
                            <p>Choose a pet</p>
                            <button onClick={() => setPicker("pet")}>Choose</button>
                            {selectedPet && <PetCard pet={selectedPet} />}
                            {picker === "pet" && <PetPicker onClose={() => setPicker(null)} onSelect={setSelectedPet} />}
                        </div>
                    </div>

                </div>
            </div>

        </div>
    )
}
