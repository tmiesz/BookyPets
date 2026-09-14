import "../styles/AcquireCard.css"
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
        <div className="acquire-card">
            {error ? <p>{error.detail}</p> :
                <button className="btn btn-secondary" onClick={handleAcquire}>Acquire Pet</button>
            }
            <PetCard pet={pet} />
        </div>
    )
}
