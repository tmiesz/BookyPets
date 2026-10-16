import type { Pet } from "../types/Pet";
import "../styles/Pet.css"
import { BASE_URL } from "../services/api";

interface PetCardProps {
    pet: Pet
}

export default function PetCard({ pet }: PetCardProps) {
    const { experience, experienceToNextLevel } = pet;
    const percent = experienceToNextLevel > 0 ? Math.min(100, (experience / experienceToNextLevel) * 100) : 0;

    return (
        <div className="pet">

            <div className="pet__cover">
                <img src={`${BASE_URL}${pet.iconUrl}`} alt={pet.species} loading="lazy" />
            </div>

            <div className="pet__info">
                <h3>{pet.name}</h3>
                <p>Level {pet.level}</p>
                {pet.favouriteGenre && <p>Loves {pet.favouriteGenre}</p>}
            </div>

            <div className="pet__progress">
                <div
                    className="pet__progress-track"
                    role="progressbar"
                    aria-label={`${pet.name} experience`}
                    aria-valuemin={0}
                    aria-valuemax={experienceToNextLevel}
                    aria-valuenow={experience}>
                    <div className="pet__progress-fill" style={{ width: `${percent}%` }} />
                </div>
                <span className="pet__progress-label">{experience} / {experienceToNextLevel} XP</span>
            </div>

        </div>
    )
}
