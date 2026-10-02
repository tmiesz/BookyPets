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
        <div className="picker" onClick={(e) => { if (e.target === e.currentTarget) onClose(); }}>
            <div className="picker__dialog" role="dialog" aria-modal="true" aria-labelledby="pet-picker-title">
                <div className="picker__header">
                    <h2 className="picker__title" id="pet-picker-title">Pick a pet</h2>
                    <button className="btn picker__close" onClick={onClose}>Close</button>
                </div>

                {error && <div className="picker__error">{error.detail}</div>}

                <div className="picker__list">
                    {pets.map((p) => (
                        <button key={p.id} className="picker__option" onClick={() => handleSelect(p)}>
                            <PetCard pet={p} />
                        </button>
                    ))}
                </div>
            </div>
        </div>
    );
}

