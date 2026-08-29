import type { Book } from "../types/Book"

const API_TOKEN = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiQWRtaW4iLCJmYW1pbHlfbmFtZSI6IkFkbWluIiwiZW1haWwiOiJhZG1pbjJAYm9va3lwZXRzLmNvbSIsImlkIjoiNDgxYTc2MTUtOGY5OC00OWFjLTliODItZThlYmViMjA5YjA5IiwicGVybWlzc2lvbnMiOlsiYm9va3M6YWNxdWlyZSIsInBldHM6YWNxdWlyZSIsInNlc3Npb25zOnN0YXJ0Iiwic2Vzc2lvbnM6ZmluaXNoIl0sImV4cCI6MTc4ODAxMzY3OCwiaXNzIjoiQm9va3lQZXRzIiwiYXVkIjoiQm9va3lQZXRzIn0.bB_08-dJD3H4X7lmknoWT3gYkj7Y1mqskGtBtU7fzm4"
const BASE_URL = "http://localhost:5293"

export const getBooks = async (): Promise<Book[]> => {
    const response = await fetch(`${BASE_URL}/books`, {
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

