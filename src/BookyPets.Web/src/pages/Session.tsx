import { useState } from "react";
import BookCard from "../components/BookCard";
import PetCard from "../components/PetCard";
import { getBooks } from "../data/booksmock";
import { getPets } from "../data/petsmock";
import "../styles/Session.css";
import BookPicker from "../components/BookPicker";
import PetPicker from "../components/PetPicker";

const books = getBooks();
const pets = getPets();

export default function Session() {
    const [picker, setPicker] = useState<"book" | "pet" | null>(null);

    return (
        <div className="page">
            <div className="container">
                <div className="session-container">
                    <h1 className="page-title">Session</h1>

                    <div className="session-layout">
                        <div className="session-item">
                            <p>Choose a book</p>
                            <button onClick={() => setPicker("book")}>Choose</button>
                            <BookCard book={books[0]} />
                            {picker === "book" && <BookPicker onClose={() => setPicker(null)} />}
                        </div>
                        <div className="session-item">
                            <p>Start a session</p>
                        </div>
                        <div className="session-item">
                            <p>Choose a pet</p>
                            <button onClick={() => setPicker("pet")}>Choose</button>
                            <PetCard pet={pets[1]} />
                            {picker === "pet" && <PetPicker onClose={() => setPicker(null)}/>}
                        </div>
                    </div>

                </div>
            </div>

        </div>
    )
}
