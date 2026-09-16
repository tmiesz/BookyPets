import type { Pet } from "../types/Pet";
import "../styles/Pet.css"
import { BASE_URL } from "../services/api";

interface PetCardProps {
    pet: Pet
}

export default function PetCard({ pet }: PetCardProps) {

    return (
        <div className="pet">

            <div className="pet__cover">
                <img src={`${BASE_URL}${pet.iconUrl}`} />
            </div>

            <div className="pet__info">
                <h3>{pet.name}</h3>
                {pet.favouriteGenre && <p>Loves {pet.favouriteGenre}</p>}
            </div>

        </div>
    )
}
