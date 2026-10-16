import "../styles/Pet.css"
import PetCard from "./PetCard";
import ErrorMessage from "./ErrorMessage";
import type { Pet } from "../types/Pet";
import { useAcquirePet } from "../hooks/queries";
import { toApiError } from "../types/ApiError";

interface AcquirePetCardProps {
    pet: Pet,
    owned: boolean
}

export default function AcquirePetCard({ pet, owned }: AcquirePetCardProps) {
    const acquire = useAcquirePet();
    const isOwned = owned || acquire.isSuccess;

    return (
        <div className="pet__acquire">
            <PetCard pet={pet} />
            <button
                type="button"
                className="btn pet__acquire__button"
                onClick={() => acquire.mutate(pet.id)}
                disabled={isOwned || acquire.isPending}>
                {isOwned ? "In your collection" : acquire.isPending ? "Acquiring..." : "Acquire Pet"}
            </button>
            <ErrorMessage error={acquire.error ? toApiError(acquire.error) : null} />
        </div>
    )
}
