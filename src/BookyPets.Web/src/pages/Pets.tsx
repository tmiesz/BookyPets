import { useState, useEffect, type SubmitEvent } from "react";
import "../styles/Pets.css"
import type { Pet } from "../types/Pet";
import { getPets } from "../services/api";
import AcquirePetCard from "../components/AcquirePetCard";
import type { ApiError } from "../types/ApiError";

export default function Pets() {
    const [searchQuery, setSearchQuery] = useState("");
    const [pets, setPets] = useState<Pet[]>([]);
    const [error, setError] = useState<ApiError | null>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const loadPets = async () => {
            try {
                const pets = await getPets()
                setPets(pets)
            } catch (err) {
                setError(err as ApiError)
            }
            finally {
                setLoading(false)
            }
        }

        loadPets()
    }, [])

    const handleSearch = async (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()

        if (loading) return
        const query = searchQuery.trim()

        setLoading(true);

        try {
            const result = query ? await getPets(query) : await getPets()
            setPets(result)
            setError(null)
        } catch (err) {
            setError(err as ApiError)
        } finally {
            setLoading(false)
        }

        setSearchQuery("")
    };

    return (
        <div className="pets">
            <form onSubmit={handleSearch} className="pets__form">
                <input
                    type="text"
                    placeholder="Search for pets..."
                    className="pets__input"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)} />
                <button type="submit" className="btn pets__search">Search</button>
            </form>

            {error && <div className="pets__error">{error.detail}</div>}

            {loading ? <div className="pets__loading">Loading...</div> :
                pets.length === 0 ? (!error && <div className="pets__empty">No pets found.</div>) :
                    <div className="pets__grid">
                        {pets.map((pet) => (
                            <AcquirePetCard pet={pet} key={pet.id} />
                        ))}
                    </div>
            }
        </div>
    );
}

