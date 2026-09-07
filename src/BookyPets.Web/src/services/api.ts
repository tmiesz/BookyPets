import type { Book } from "../types/Book"

export const BASE_URL = "http://localhost:5293"

function getToken(): string | null {
    return localStorage.getItem("token");
}

function authHeaders(): HeadersInit {
    const token = getToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
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

