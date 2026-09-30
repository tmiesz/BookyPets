import "../styles/Pet.css"
import { useState } from "react";
import type { ApiError } from "../types/ApiError";
import PetCard from "./PetCard";
import type { Pet } from "../types/Pet";
import { acquirePet } from "../services/api";

interface PetCardProps {
    pet: Pet
}

export default function AcquirePetCard({ pet }: PetCardProps) {
    const [error, setError] = useState<ApiError | null>(null);

    const handleAcquire = async () => {
        setError(null);

        try {
            await acquirePet(pet.id);
        } catch (error) {
            setError(error as ApiError);
        }
    };

    return (
        <div className="pet__acquire">
            <PetCard pet={pet} />
            <button className="btn pet__acquire__button" onClick={handleAcquire}>Acquire Pet</button>
            <p className="acquire__error">{error?.detail}</p>
        </div>
    )
}
