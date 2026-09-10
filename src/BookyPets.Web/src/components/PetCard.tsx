import type { Pet } from "../types/Pet";
import "../styles/PetCard.css"
import { BASE_URL } from "../services/api";

interface PetCardProps {
    pet: Pet
}

export default function PetCard({ pet }: PetCardProps) {

    return (
        <div className="pet-card">
            <div className="pet-overlay">
                <div className="pet-image">
                    <img src={`${BASE_URL}${pet.iconUrl}`} />
                </div>
            </div>
            <div className="pet-info">
                <h3>{pet.name}</h3>
                {pet.favouriteGenre && <p>Loves {pet.favouriteGenre}</p>}
            </div>
        </div>
    )
}
