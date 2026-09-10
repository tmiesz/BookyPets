import { useState, useEffect, type SubmitEvent } from "react";
import "../styles/Pets.css"
import type { Pet } from "../types/Pet";
import PetCard from "../components/PetCard";
import { getPets } from "../services/api";

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
            <form onSubmit={handleSearch} className="search-form">
                <input
                    type="text"
                    placeholder="Search for pets..."
                    className="search-input"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)} />
                <button type="submit" className="search-button">Search</button>
            </form>

            {error && <div className="error-message">{error}</div>}

            {loading ? <div className="loading">Loading...</div> :
                <div className="pets-grid">
                    {pets.map((pet) => (
                        <PetCard pet={pet} key={pet.id} />
                    ))}
                </div>
            }
        </div>
    );
}

