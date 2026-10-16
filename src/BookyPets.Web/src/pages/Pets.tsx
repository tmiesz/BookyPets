import { useMemo, useState, type SubmitEvent } from "react";
import "../styles/Pets.css"
import AcquirePetCard from "../components/AcquirePetCard";
import ErrorMessage from "../components/ErrorMessage";
import { usePets, useReaderPets } from "../hooks/queries";
import { toApiError } from "../types/ApiError";

export default function Pets() {
    const [searchInput, setSearchInput] = useState("");
    const [query, setQuery] = useState("");

    const petsQuery = usePets({ search: query });
    const readerPetsQuery = useReaderPets();

    const pets = petsQuery.data ?? [];
    const ownedPetIds = useMemo(
        () => new Set((readerPetsQuery.data ?? []).map((pet) => pet.id)),
        [readerPetsQuery.data]);

    const handleSearch = (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()
        setQuery(searchInput.trim())
    };

    const showResults = !petsQuery.isPlaceholderData;

    return (
        <div className="pets">
            <form onSubmit={handleSearch} className="pets__form" role="search">
                <input
                    type="text"
                    placeholder="Search for pets..."
                    aria-label="Search for pets"
                    className="pets__input"
                    value={searchInput}
                    onChange={(e) => setSearchInput(e.target.value)} />
                <button type="submit" className="btn pets__search">Search</button>
            </form>

            <ErrorMessage error={petsQuery.error ? toApiError(petsQuery.error) : null} className="pets__error" />

            {petsQuery.isPending ? <div className="pets__loading" role="status">Loading...</div> :
                pets.length === 0 ? (!petsQuery.isError && showResults && (
                    <div className="pets__empty">{query ? `No pets found for "${query}".` : "No pets found."}</div>
                )) :
                    <>
                        {query && showResults && (
                            <p className="pets__summary" role="status">
                                {pets.length} {pets.length === 1 ? "result" : "results"} for "{query}"
                            </p>
                        )}
                        <div className="pets__grid">
                            {pets.map((pet) => (
                                <AcquirePetCard pet={pet} owned={ownedPetIds.has(pet.id)} key={pet.id} />
                            ))}
                        </div>
                    </>
            }
        </div>
    );
}
