import type { Book } from "../types/Book"
import type { Pet } from "../types/Pet";

export const BASE_URL = "http://localhost:5293"

function getToken(): string | null {
    return localStorage.getItem("token");
}

function authHeaders(): HeadersInit {
    const token = getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
}

export const getReaderBooks = async (): Promise<Book[]> => {
    const url = new URL(`${BASE_URL}/reader/books`);
    const response = await fetch(url, {
        method: "GET",
        headers: {
            ...authHeaders(),
        },
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch books: ${response.status} ${response.statusText}`)
    }

    const books: Book[] = await response.json();
    return books;
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
        throw new Error(`Failed to fetch books: ${response.status} ${response.statusText}`)
    }

    const books: Book[] = await response.json();
    return books;
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
        throw new Error(`Failed to fetch pets: ${response.status} ${response.statusText}`)
    }

    const pets: Pet[] = await response.json();
    return pets;
}

