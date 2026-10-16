import { useState } from "react";
import { useForm } from "react-hook-form";
import PetCard from "../components/PetCard";
import "../styles/Session.css";
import PetPicker from "../components/PetPicker";
import ErrorMessage from "../components/ErrorMessage";
import LibraryEntryPicker from "../components/LibraryEntryPicker";
import LibraryEntryCard from "../components/LibraryEntryCard";
import {
    useAbandonSession,
    useActiveSession,
    useCompleteSession,
    useLibrary,
    useReaderPets,
    useStartSession
} from "../hooks/queries";
import { useHeartbeat } from "../hooks/useHeartbeat";
import { useSessionTimer } from "../hooks/useSessionTimer";
import { toApiError, type ApiError } from "../types/ApiError";
import type { Session } from "../types/Session";

const MAX_PAGES_READ = 2_147_483_647;

interface PagesFormData {
    pagesRead: string
}

function validatePagesRead(value: string): true | string {
    const trimmed = value.trim();

    if (!/^\d+$/.test(trimmed) || Number(trimmed) > MAX_PAGES_READ) {
        return "Enter a whole number of pages (0 or more).";
    }

    return true;
}

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
    const libraryQuery = useLibrary();
    const petsQuery = useReaderPets();
    const activeSessionQuery = useActiveSession();

    const startMutation = useStartSession();
    const completeMutation = useCompleteSession();
    const abandonMutation = useAbandonSession();

    const library = libraryQuery.data ?? [];
    const pets = petsQuery.data ?? [];

    const [picker, setPicker] = useState<"book" | "pet" | null>(null);
    const [selectedProgressId, setSelectedProgressId] = useState<string | null>(null);
    const [selectedPetId, setSelectedPetId] = useState<string | null | undefined>(undefined);
    const [actionError, setActionError] = useState<ApiError | null>(null);

    const [session, setSession] = useState<Session | null>(null);
    const [startedAt, setStartedAt] = useState<number | null>(null);
    const [pendingHandled, setPendingHandled] = useState(false);

    const { register, handleSubmit, reset, formState: { errors } } = useForm<PagesFormData>({
        defaultValues: { pagesRead: "" }
    });

    const isActive = session?.status === "Active";
    const elapsedSeconds = useSessionTimer(isActive ? startedAt : null);
    useHeartbeat(isActive);

    const busy = startMutation.isPending || completeMutation.isPending || abandonMutation.isPending;
    const loading = libraryQuery.isPending || petsQuery.isPending;

    const selectedEntry = selectedProgressId === null
        ? library[0] ?? null
        : library.find((e) => e.progress.id === selectedProgressId) ?? null;
    const selectedPet = selectedPetId === undefined
        ? pets[0] ?? null
        : pets.find((p) => p.id === selectedPetId) ?? null;

    const pendingInfo = !isActive && !pendingHandled ? activeSessionQuery.data ?? null : null;
    const pendingEntry = pendingInfo ? library.find((e) => e.progress.id === pendingInfo.progressId) ?? null : null;

    const loadError = libraryQuery.error ?? petsQuery.error ?? activeSessionQuery.error;
    const error = actionError ?? (loadError ? toApiError(loadError) : null);

    async function handleStart() {
        if (!selectedEntry) return;

        setActionError(null);

        try {
            const started = await startMutation.mutateAsync({
                progressId: selectedEntry.progress.id,
                petId: selectedPet?.id ?? null
            });

            setSelectedProgressId(selectedEntry.progress.id);
            setSelectedPetId(selectedPet?.id ?? null);
            setStartedAt(Date.now());
            setSession(started);
        }
        catch (err) {
            setActionError(toApiError(err));
        }
    }

    async function handleComplete({ pagesRead }: PagesFormData) {
        if (!session) return;

        setActionError(null);

        try {
            await completeMutation.mutateAsync({ sessionId: session.id, pagesRead: Number(pagesRead.trim()) });
            resetSession();
        }
        catch (err) {
            setActionError(toApiError(err));
        }
    }

    async function handleAbandon() {
        if (!session) return;

        setActionError(null);

        try {
            await abandonMutation.mutateAsync(session.id);
            resetSession();
        }
        catch (err) {
            setActionError(toApiError(err));
        }
    }

    function resetSession() {
        setSession(null);
        setStartedAt(null);
        reset({ pagesRead: "" });
    }

    function handleResumePending() {
        if (!pendingInfo) return;

        setSelectedProgressId(pendingInfo.progressId);
        setSelectedPetId(pendingInfo.petId);
        setStartedAt(new Date(pendingInfo.startTime).getTime());
        setSession({ id: pendingInfo.id, status: "Active", pagesRead: 0, endTime: null });
        setPendingHandled(true);
    }

    async function handleDiscardPending() {
        if (!pendingInfo) return;

        setActionError(null);

        try {
            await abandonMutation.mutateAsync(pendingInfo.id);
            setPendingHandled(true);
        }
        catch (err) {
            setActionError(toApiError(err));
        }
    }

    return (
        <div className="page">
            <div className="container">
                <div className="session">
                    <h1 className="session__title">Session</h1>

                    <ErrorMessage error={error} className="session__error" />

                    {pendingInfo && (
                        <div className="session__resume">
                            <p className="session__resume-text">
                                {pendingInfo.isStale
                                    ? `You have an old reading session${pendingEntry ? ` for "${pendingEntry.book.title}"` : ""}. Resume or discard it?`
                                    : `You have an active reading session${pendingEntry ? ` for "${pendingEntry.book.title}"` : ""}. Resume?`}
                            </p>
                            <button type="button" className="btn session__resume-action" onClick={handleResumePending} disabled={busy}>Resume</button>
                            <button type="button" className="btn session__resume-action" onClick={handleDiscardPending} disabled={busy}>Discard</button>
                        </div>
                    )}

                    {!isActive && (
                        <div className="session__setup">
                            <div className="session__slots">
                                <section className="session__slot">
                                    <h2 className="session__label">Book</h2>
                                    {selectedEntry
                                        ? <LibraryEntryCard entry={selectedEntry} />
                                        : <div className="session__placeholder">{loading ? "Loading..." : "No book selected"}</div>}
                                    <button type="button" className="btn session__choose" onClick={() => setPicker("book")} disabled={!!pendingInfo}>
                                        {selectedEntry ? "Change book" : "Choose a book"}
                                    </button>
                                    {picker === "book" && (
                                        <LibraryEntryPicker
                                            entries={library}
                                            onClose={() => setPicker(null)}
                                            onSelect={(entry) => setSelectedProgressId(entry.progress.id)} />
                                    )}
                                </section>

                                <section className="session__slot">
                                    <div className="session__slot-header">
                                        <h2 className="session__label">Pet</h2>
                                        {selectedPet && (
                                            <button
                                                type="button"
                                                className="btn session__remove"
                                                onClick={() => setSelectedPetId(null)}
                                                disabled={!!pendingInfo}>
                                                Remove
                                            </button>
                                        )}
                                    </div>
                                    {selectedPet
                                        ? <PetCard pet={selectedPet} />
                                        : <div className="session__placeholder">{loading ? "Loading..." : "No pet selected"}</div>}
                                    <button type="button" className="btn session__choose" onClick={() => setPicker("pet")} disabled={!!pendingInfo}>
                                        {selectedPet ? "Change pet" : "Choose a pet"}
                                    </button>
                                    {picker === "pet" && (
                                        <PetPicker
                                            pets={pets}
                                            onClose={() => setPicker(null)}
                                            onSelect={(pet) => setSelectedPetId(pet.id)} />
                                    )}
                                </section>
                            </div>

                            <button type="button" className="btn session__start" disabled={!selectedEntry || busy || !!pendingInfo} onClick={handleStart}>
                                Start a session
                            </button>
                        </div>
                    )}

                    {isActive && (
                        <div className="session__active">
                            <div className="session__cards">
                                {selectedEntry && <LibraryEntryCard entry={selectedEntry} />}
                                {selectedPet && <PetCard pet={selectedPet} />}
                            </div>

                            <span className="session__clock" role="timer">{formatElapsed(elapsedSeconds)}</span>

                            <form className="session__form" onSubmit={handleSubmit(handleComplete)} noValidate>
                                <div className="session__pages">
                                    <label className="session__pages-label" htmlFor="pagesRead">Pages read</label>
                                    <input
                                        className="session__pages-input"
                                        id="pagesRead"
                                        type="number"
                                        inputMode="numeric"
                                        min={0}
                                        step={1}
                                        aria-invalid={errors.pagesRead ? "true" : "false"}
                                        aria-describedby={errors.pagesRead ? "pagesRead-error" : undefined}
                                        {...register("pagesRead", {
                                            required: "Enter how many pages you read.",
                                            validate: validatePagesRead
                                        })} />
                                    {errors.pagesRead && (
                                        <span id="pagesRead-error" role="alert" className="session__pages-error">
                                            {errors.pagesRead.message}
                                        </span>
                                    )}
                                </div>

                                <div className="session__actions">
                                    <button type="submit" className="btn session__finish" disabled={busy}>
                                        Finish session
                                    </button>
                                    <button type="button" className="btn session__abandon" onClick={handleAbandon} disabled={busy}>
                                        Abandon session
                                    </button>
                                </div>
                            </form>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
