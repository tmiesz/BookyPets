import { useEffect, useState } from "react";
import "../styles/Picker.css";
import type { ApiError } from "../types/ApiError";
import type { Pet } from "../types/Pet";
import PetCard from "./PetCard";
import { getReaderPets } from "../services/api";

type Props = {
    onClose: () => void;
    onSelect: (pet: Pet) => void;
};

export default function PetPicker({ onClose, onSelect }: Props) {
    const [pets, setPets] = useState<Pet[]>([]);
    const [error, setError] = useState<ApiError | null>(null);

    useEffect(() => {
        getReaderPets().then(setPets).catch((error) => setError(error))
    }, []);

    const handleSelect = (pet: Pet) => {
        onSelect(pet);
        onClose();
    }

    return (
        <div className="picker-overlay">
            <div className="picker">
                <button onClick={onClose}>Close</button>
                <p>Pick a pet</p>
                {error && <div className="error-message">{error.detail}</div>}
                <div>
                    {pets.map((p) => (
                        <button key={p.id} className="picker-button" onClick={() => handleSelect(p)}>
                            <PetCard key={p.id} pet={p} />
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}

