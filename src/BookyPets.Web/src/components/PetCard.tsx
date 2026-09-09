import type { Pet } from "../types/Pet";
import "../styles/PetCard.css"

interface PetCardProps {
    pet: Pet
}

export default function PetCard({ pet }: PetCardProps) {

    function onAcquireClick() {
        alert("clickd");
    }

    return (
        <div className="pet-card">
            <div className="pet-info">
                <h3>{pet.name}</h3>
                <p>{pet.species}</p>
                {pet.favouriteGenre && <p>Loves {pet.favouriteGenre}</p>}
            </div>
            <div className="pet-overlay">
                <button className="acquire-button" onClick={onAcquireClick}>
                    +
                </button>
                <div className="pet-image">
                    <img src={`http://localhost:5293/${pet.iconUrl}`} />
                </div>
            </div>
        </div>
    )
}
