import type { Book } from "../types/Book"

const API_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQWRtaW4iLCJmYW1pbHlfbmFtZSI6IkFkbWluIiwiZW1haWwiOiJhZG1pbjJAYm9va3lwZXRzLmNvbSIsImlkIjoiNDgxYTc2MTUtOGY5OC00OWFjLTliODItZThlYmViMjA5YjA5IiwicGVybWlzc2lvbnMiOlsiYm9va3M6YWNxdWlyZSIsInBldHM6YWNxdWlyZSIsInNlc3Npb25zOnN0YXJ0Iiwic2Vzc2lvbnM6ZmluaXNoIl0sImV4cCI6MTc4ODEwNTUzMCwiaXNzIjoiQm9va3lQZXRzIiwiYXVkIjoiQm9va3lQZXRzIn0.FRyA4q18B3E9i1lxxfW5IlmnyS3cOrHwiFHP4dvs_m4"
const BASE_URL = "http://localhost:5293"

export const getBooks = async (search?: string): Promise<Book[]> => {
    const url = new URL(`${BASE_URL}/books`);
    if (search?.trim()) {
        url.searchParams.set("search", search.trim())
    }

    const response = await fetch(url, {
        method: "GET",
        headers: {
            Authorization: `Bearer ${API_TOKEN}`,
        },
    });

    if (!response.ok) {
        throw new Error(`Failed to fetch books: ${response.status} ${response.statusText}`)
    }

    const books: Book[] = await response.json();
    return books;
}

