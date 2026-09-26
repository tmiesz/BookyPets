import { useEffect, useRef, useState } from "react";
import PetCard from "../components/PetCard";
import "../styles/Session.css";
import PetPicker from "../components/PetPicker";
import type { Pet } from "../types/Pet";
import { abandonSession, completeSession, getActiveSession, getLibrary, getPets, heartbeatSession, startSession } from "../services/api";
import type { ApiError } from "../types/ApiError";
import type { LibraryEntry } from "../types/LibraryEntry";
import type { ActiveSession, Session } from "../types/Session";
import LibraryEntryPicker from "../components/LibraryEntryPicker";
import LibraryEntryCard from "../components/LibraryEntryCard";
import { useAuth } from "../context/AuthContext";

const HEARTBEAT_INTERVAL_MS = 60_000;

function formatElapsed(totalSeconds: number): string {
    const seconds = totalSeconds % 60;
    const totalMinutes = Math.floor(totalSeconds / 60);
    const minutes = totalMinutes % 60;
    const hours = Math.floor(totalMinutes / 60);

    const format = (value: number) => value.toString().padStart(2, "0");

    if (hours > 0) {
        return `${hours}:${format(minutes)}:${format(seconds)}`
    }

    return `${format(minutes)}:${format(seconds)}`
}

export default function Session() {
    const { user } = useAuth();

    const [picker, setPicker] = useState<"book" | "pet" | null>(null);
    const [selectedEntry, setSelectedEntry] = useState<LibraryEntry | null>(null);
    const [selectedPet, setSelectedPet] = useState<Pet | null>(null);
    const [error, setError] = useState<ApiError | null>(null);

    const [session, setSession] = useState<Session | null>(null);
    const [elapsedSeconds, setElapsedSeconds] = useState(0);
    const [pagesReadInput, setPagesReadInput] = useState("");
    const [submitting, setSubmitting] = useState(false);

    const [pendingSession, setPendingSession] = useState<{
        info: ActiveSession;
        entry: LibraryEntry | null;
        pet: Pet | null;
    } | null>(null);

    const startedAtRef = useRef<number | null>(null);

    useEffect(() => {
        Promise.all([refreshLibrary(), getPets(), getActiveSession()])
            .then(([entries, pets, activeSession]) => {
                if (!entries || !pets) return;

                if (activeSession) {
                    const matchedEntry = entries.find((e) => e.progress.id === activeSession.progressId) ?? null;
                    const matchedPet = pets.find((p) => p.id === activeSession.petId) ?? null;
                    setPendingSession({ info: activeSession, entry: matchedEntry, pet: matchedPet });
                } else {
                    setSelectedPet(pets[0] ?? null);
                }
            })
            .catch((error) => setError(error));
    }, []);

    useEffect(() => {
        if (session?.status !== "Active" || startedAtRef.current == null) return;

        const interval = setInterval(() => {
            setElapsedSeconds(Math.floor((Date.now() - startedAtRef.current!) / 1000));
        }, 1000);

        return () => clearInterval(interval);
    }, [session?.status]);


    useEffect(() => {
        if (session?.status !== "Active") return;

        const interval = setInterval(() => {
            heartbeatSession().catch((error) => setError(error));
        }, HEARTBEAT_INTERVAL_MS);

        return () => clearInterval(interval);
    }, [session?.status]);

    function refreshLibrary() {
        return getLibrary()
            .then((entries) => {
                setSelectedEntry((current) => {
                    if (!current) return entries[0] ?? null;
                    return entries.find((e) => e.progress.id == current.progress.id) ?? entries[0] ?? null;
                });
                return entries;
            })
            .catch((error) => setError(error));
    }

    async function handleStart() {
        if (!user || !selectedEntry) return;

        setError(null);
        setSubmitting(true);

        try {
            const started = await startSession(user.id, selectedEntry.progress.id, selectedPet?.id ?? null);
            startedAtRef.current = Date.now();
            setElapsedSeconds(0);
            setSession(started);
        }
        catch (err) {
            setError(err as ApiError);
        }
        finally {
            setSubmitting(false);
        }
    }

    async function handleComplete() {
        if (!session) return;

        const pagesRead = Number(pagesReadInput);

        setError(null);
        setSubmitting(true);

        try {
            await completeSession(session.id, pagesRead);
            resetSession();
            await refreshLibrary();
            const pets = await getPets();
            setSelectedPet((current) => pets.find((p) => p.id === current?.id) ?? pets[0] ?? null);
        }
        catch (err) {
            setError(err as ApiError);
        }
        finally {
            setSubmitting(false);
        }
    }

    async function handleAbandon() {
        if (!session) return;

        setError(null);
        setSubmitting(true);

        try {
            await abandonSession(session.id);
            resetSession();
        }
        catch (err) {
            setError(err as ApiError);
        }
        finally {
            setSubmitting(false);
        }
    }

    function resetSession() {
        setSession(null);
        startedAtRef.current = 0;
        setElapsedSeconds(0);
        setPagesReadInput("");
    }

    async function handleResumePending() {
        if (!pendingSession) return;

        const { info, entry, pet } = pendingSession;

        setSelectedEntry(entry);
        setSelectedPet(pet);
        startedAtRef.current = new Date(info.startTime).getTime();
        setElapsedSeconds(Math.floor((Date.now() - startedAtRef.current) / 1000));
        setSession({ id: info.id, status: "Active", pagesRead: 0, endTime: null });
        setPendingSession(null);
    }

    async function handleDiscardPending() {
        if (!pendingSession) return;

        setError(null);
        setSubmitting(true);

        try {
            await abandonSession(pendingSession.info.id);
            setPendingSession(null);

            const pets = await getPets();
            setSelectedPet(pets[0] ?? null);
        }
        catch (err) {
            setError(err as ApiError);
        }
        finally {
            setSubmitting(false);
        }
    }

    const isActive = session?.status === "Active";

    return (
        <div className="page">
            <div className="container">
                <div className="session-container">
                    <h1 className="page-title">Session</h1>

                    {error && <div className="error-message">{error.detail}</div>}

                    {pendingSession && (
                        <div className="session-resume-prompt">
                            <p>
                                {pendingSession.info.isStale
                                    ? `You have an old reading session${pendingSession.entry ? ` for "${pendingSession.entry.book.title}"` : ""}. Resume or discard it?`
                                    : `You have an active reading session${pendingSession.entry ? ` for "${pendingSession.entry.book.title}"` : ""}. Resume?`}
                            </p>
                            <button onClick={handleResumePending} disabled={submitting}>Resume</button>
                            <button onClick={handleDiscardPending} disabled={submitting}>Discard</button>
                        </div>
                    )}

                    <div className="session-layout">
                        <div className="session-item">
                            <p>Choose a book</p>
                            <button onClick={() => setPicker("book")} disabled={!!pendingSession}>Choose</button>
                            {selectedEntry && <LibraryEntryCard entry={selectedEntry} />}
                            {picker === "book" && <LibraryEntryPicker onClose={() => setPicker(null)} onSelect={setSelectedEntry} />}
                        </div>

                        <div className="session-item">
                            <button className="start-session-button" disabled={!selectedEntry || submitting || !!pendingSession} onClick={handleStart}>Start a session</button>
                        </div>

                        {isActive && (
                            <div className="session-active-controls">
                                <span className="session-clock">{formatElapsed(elapsedSeconds)}</span>

                                <div className="session-pages-input">
                                    <label htmlFor="pagesRead">Pages read</label>
                                    <input id="pagesRead" type="number" min={0} value={pagesReadInput} onChange={(e) => setPagesReadInput(e.target.value)} />
                                </div>

                                <button onClick={handleComplete} disabled={submitting || pagesReadInput === ""}>
                                    Finish session
                                </button>

                                <button onClick={handleAbandon} disabled={submitting}>
                                    Abandon session
                                </button>
                            </div>
                        )}


                        <div className="session-item">
                            <p>Choose a pet</p>
                            <button onClick={() => setPicker("pet")} disabled={!!pendingSession}>Choose</button>
                            {selectedPet && <PetCard pet={selectedPet} />}
                            {picker === "pet" && <PetPicker onClose={() => setPicker(null)} onSelect={setSelectedPet} />}
                        </div>
                    </div>

                </div>
            </div>

        </div>
    )
}
