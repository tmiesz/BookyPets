import type { ApiError } from "../types/ApiError";
import type { Book } from "../types/Book"
import type { LibraryEntry } from "../types/LibraryEntry";
import type { Pet } from "../types/Pet";
import type { Progress } from "../types/Progress";
import type { Session } from "../types/Session";

export const BASE_URL = "http://localhost:5293"

function getToken(): string | null {
    return localStorage.getItem("token");
}

function authHeaders(): HeadersInit {
    const token = getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
}

export const getReaderBooks = async (): Promise<Book[]> => {
    const url = new URL(`${BASE_URL}/readers/books`);
    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const books: Book[] = await response.json();
    return books;
}

export const getReaderPets = async (): Promise<Pet[]> => {
    const url = new URL(`${BASE_URL}/readers/pets`);
    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const pets: Pet[] = await response.json();
    return pets;
}

export const getReaderProgresses = async (): Promise<Progress[]> => {
    const url = new URL(`${BASE_URL}/readers/progresses`);
    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const progresses: Progress[] = await response.json();
    return progresses;
}

export const getBooks = async (search?: string): Promise<Book[]> => {
    const url = new URL(`${BASE_URL}/books`);
    if (search?.trim()) {
        url.searchParams.set("search", search.trim())
    }

    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const books: Book[] = await response.json();
    return books;
}

export const getLibrary = async (search?: string): Promise<LibraryEntry[]> => {
    const url = new URL(`${BASE_URL}/readers/library`);
    if (search?.trim()) {
        url.searchParams.set("search", search.trim())
    }

    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const library: LibraryEntry[] = await response.json();
    return library;
}

export const getPets = async (search?: string): Promise<Pet[]> => {
    const url = new URL(`${BASE_URL}/pets`);
    if (search?.trim()) {
        url.searchParams.set("search", search.trim())
    }

    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const pets: Pet[] = await response.json();
    return pets;
}

export const acquireBook = async (bookId: string): Promise<void> => {
    const url = new URL(`${BASE_URL}/readers/books/${bookId}/acquire`);

    const response = await fetch(url, {
        method: "POST",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }
}

export const acquirePet = async (petId: string): Promise<void> => {
    const url = new URL(`${BASE_URL}/readers/pets/${petId}/acquire`);

    const response = await fetch(url, {
        method: "POST",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }
}

export const startSession = async (readerId: string, progressId: string, petId: string | null): Promise<Session> => {
    const url = new URL(`${BASE_URL}/sessions/start`);
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            ...authHeaders(),
        },
        body: JSON.stringify({ readerId, progressId, petId }),
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const session: Session = await response.json();
    return session;
}

export const completeSession = async (sessionId: string, pagesRead: number): Promise<Session> => {
    const url = new URL(`${BASE_URL}/sessions/complete`);
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            ...authHeaders(),
        },
        body: JSON.stringify({ sessionId, pagesRead }),
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const session: Session = await response.json();
    return session;
}

export const abandonSession = async (sessionId: string): Promise<Session> => {
    const url = new URL(`${BASE_URL}/sessions/abandon`);
    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            ...authHeaders(),
        },
        body: JSON.stringify({ sessionId }),
    });

    if (!response.ok) {
        const error: ApiError = await response.json();
        throw error;
    }

    const session: Session = await response.json();
    return session;
}
