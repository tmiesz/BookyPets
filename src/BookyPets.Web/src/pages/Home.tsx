import type { ReactNode } from "react";
import { Link } from "react-router-dom";
import BookCard from "../components/BookCard";
import ErrorMessage from "../components/ErrorMessage";
import PetCard from "../components/PetCard";
import { useAuth } from "../context/useAuth";
import { useBooks, usePets } from "../hooks/queries";
import { toApiError } from "../types/ApiError";
import "../styles/Home.css"

const HOME_ITEM_LIMIT = 5;

interface CatalogueSectionProps {
    title: string,
    isPending: boolean,
    error: Error | null,
    isEmpty: boolean,
    emptyText: string,
    children: ReactNode
}

function CatalogueSection({ title, isPending, error, isEmpty, emptyText, children }: CatalogueSectionProps) {
    return (
        <div className="container">
            <h2 className="container__title">{title}</h2>
            <ErrorMessage error={error ? toApiError(error) : null} />
            {isPending ? <p className="container__message" role="status">Loading...</p> :
                isEmpty ? (!error && <p className="container__message">{emptyText}</p>) :
                    <div className="container__grid">{children}</div>}
        </div>
    );
}

function HomeCatalogue() {
    const booksQuery = useBooks({ limit: HOME_ITEM_LIMIT });
    const petsQuery = usePets({ limit: HOME_ITEM_LIMIT });
    const books = booksQuery.data ?? [];
    const pets = petsQuery.data ?? [];

    return (
        <>
            <CatalogueSection
                title="Books"
                isPending={booksQuery.isPending}
                error={booksQuery.error}
                isEmpty={books.length === 0}
                emptyText="No books available yet.">
                {books.map((book) => (
                    <div className="container__item" key={book.id}>
                        <BookCard book={book} />
                    </div>
                ))}
            </CatalogueSection>

            <CatalogueSection
                title="Pets"
                isPending={petsQuery.isPending}
                error={petsQuery.error}
                isEmpty={pets.length === 0}
                emptyText="No pets available yet.">
                {pets.map((pet) => (
                    <div className="container__item" key={pet.id}>
                        <PetCard pet={pet} />
                    </div>
                ))}
            </CatalogueSection>
        </>
    );
}

export default function Home() {
    const { user } = useAuth();

    return (
        <div className="page">
            <div className="home">
                <h1 className="home__title">Welcome to Booky Pets</h1>
                <p className="home__subtitle">Every book deserves a companion</p>

                {!user && (
                    <div className="home__cta">
                        <Link to="/login" className="btn home__login">Login</Link>
                        <Link to="/register" className="btn home__signup">Sign up</Link>
                    </div>
                )}
            </div>

            {user && <HomeCatalogue />}
        </div>
    )
}
