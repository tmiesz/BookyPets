import { useState, useEffect, type SubmitEvent } from "react";
import "../styles/Pets.css"
import type { Pet } from "../types/Pet";
import { getPets } from "../services/api";
import AcquirePetCard from "../components/AcquirePetCard";

export default function Pets() {
    const [searchQuery, setSearchQuery] = useState("");
    const [pets, setPets] = useState<Pet[]>([]);
    const [error, setError] = useState<string | null>(null)
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const loadPets = async () => {
            try {
                const pets = await getPets()
                setPets(pets)
            } catch (err) {
                console.log(err)
                setError("Failed to load pets...")
            }
            finally {
                setLoading(false)
            }
        }

        loadPets()
    }, [])

    const handleSearch = async (e: SubmitEvent<HTMLFormElement>) => {
        e.preventDefault()

        if (!searchQuery.trim()) return
        if (loading) return

        setLoading(true);

        try {
            const searchResult = await getPets(searchQuery)
            setPets(searchResult)
            setError(null)
        } catch (err) {
            console.log(err)
            setError("Failed to search pets...")
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

            {error && <div className="pets__error">{error}</div>}

            {loading ? <div className="pets__loading">Loading...</div> :
                <div className="pets__grid">
                    {pets.map((pet) => (
                        <AcquirePetCard pet={pet} key={pet.id} />
                    ))}
                </div>
            }
        </div>
    );
}

