import { ApiError, type ValidationErrors } from "../types/ApiError";
import type { Book } from "../types/Book";
import type { LibraryEntry } from "../types/LibraryEntry";
import type { Pet } from "../types/Pet";
import type { Progress } from "../types/Progress";
import type { ActiveSession, Session } from "../types/Session";
import type { AuthResponse } from "../types/User";
import { getToken } from "./authStorage";

export const BASE_URL: string = (
    (import.meta.env.VITE_API_URL as string | undefined) ?? "http://localhost:5293"
).replace(/\/+$/, "");

interface ProblemDetails {
    title?: string,
    detail?: string,
    errors?: ValidationErrors,
    traceId?: string
}

interface RequestOptions {
    method?: "GET" | "POST",
    body?: unknown,
    query?: Record<string, string | number | undefined>,
    signal?: AbortSignal,
    authenticated?: boolean
}

let unauthorizedHandler: (() => void) | null = null;

export function setUnauthorizedHandler(handler: (() => void) | null): void {
    unauthorizedHandler = handler;
}

function fallbackMessage(status: number): string {
    if (status === 401) return "You need to log in to do that.";
    if (status === 403) return "You are not allowed to do that.";
    if (status === 404) return "That could not be found.";
    if (status >= 500) return "The server ran into a problem. Please try again later.";

    return `The request failed (${status}).`;
}

async function readError(response: Response): Promise<ApiError> {
    let problem: ProblemDetails | null = null;

    try {
        problem = (await response.json()) as ProblemDetails;
    } catch {
        problem = null;
    }

    const validationMessages = problem?.errors ? Object.values(problem.errors).flat() : [];
    const message = validationMessages.length > 0
        ? validationMessages.join(" ")
        : problem?.detail ?? problem?.title ?? fallbackMessage(response.status);

    return new ApiError(response.status, message, {
        title: problem?.title,
        errors: problem?.errors,
        traceId: problem?.traceId
    });
}

async function request<T>(path: string, options: RequestOptions = {}): Promise<T> {
    const { method = "GET", body, query, signal, authenticated = true } = options;

    const params = new URLSearchParams();
    for (const [key, value] of Object.entries(query ?? {})) {
        if (value !== undefined) params.set(key, String(value));
    }
    const queryString = params.toString();
    const url = `${BASE_URL}${path}${queryString ? `?${queryString}` : ""}`;

    const token = authenticated ? getToken() : null;
    const headers: Record<string, string> = {};
    if (token) headers.Authorization = `Bearer ${token}`;
    if (body !== undefined) headers["Content-Type"] = "application/json";

    let response: Response;
    try {
        response = await fetch(url, {
            method,
            headers,
            body: body === undefined ? undefined : JSON.stringify(body),
            signal
        });
    } catch (error) {
        if (error instanceof DOMException && error.name === "AbortError") throw error;

        throw new ApiError(0, "Could not reach the server. Check your connection and try again.");
    }

    if (!response.ok) {
        if (response.status === 401 && token) unauthorizedHandler?.();

        throw await readError(response);
    }

    if (response.status === 204) return undefined as T;

    const text = await response.text();
    if (!text) return undefined as T;

    try {
        return JSON.parse(text) as T;
    } catch {
        throw new ApiError(response.status, "The server sent an unexpected response.");
    }
}

export const loginUser = (email: string, password: string): Promise<AuthResponse> =>
    request<AuthResponse>("/authentication/login", {
        method: "POST",
        body: { email, password },
        authenticated: false
    });

export const registerUser = (firstName: string, lastName: string, email: string, password: string): Promise<AuthResponse> =>
    request<AuthResponse>("/authentication/register", {
        method: "POST",
        body: { firstName, lastName, email, password },
        authenticated: false
    });

export const getReaderBooks = (signal?: AbortSignal): Promise<Book[]> =>
    request<Book[]>("/readers/books", { signal });

export const getReaderPets = (signal?: AbortSignal): Promise<Pet[]> =>
    request<Pet[]>("/readers/pets", { signal });

export const getReaderProgresses = (signal?: AbortSignal): Promise<Progress[]> =>
    request<Progress[]>("/readers/progresses", { signal });

export const getBooks = (search?: string, limit?: number, signal?: AbortSignal): Promise<Book[]> =>
    request<Book[]>("/books", { query: { search: search?.trim() || undefined, limit }, signal });

export const getLibrary = (search?: string, signal?: AbortSignal): Promise<LibraryEntry[]> =>
    request<LibraryEntry[]>("/readers/library", { query: { search: search?.trim() || undefined }, signal });

export const getPets = (search?: string, limit?: number, signal?: AbortSignal): Promise<Pet[]> =>
    request<Pet[]>("/pets", { query: { search: search?.trim() || undefined, limit }, signal });

export const acquireBook = (bookId: string): Promise<void> =>
    request<void>(`/readers/books/${bookId}/acquire`, { method: "POST" });

export const acquirePet = (petId: string): Promise<void> =>
    request<void>(`/readers/pets/${petId}/acquire`, { method: "POST" });

export const startSession = (progressId: string, petId: string | null): Promise<Session> =>
    request<Session>("/sessions/start", { method: "POST", body: { progressId, petId } });

export const completeSession = (sessionId: string, pagesRead: number): Promise<Session> =>
    request<Session>("/sessions/complete", { method: "POST", body: { sessionId, pagesRead } });

export const abandonSession = (sessionId: string): Promise<Session> =>
    request<Session>("/sessions/abandon", { method: "POST", body: { sessionId } });

export const getActiveSession = async (signal?: AbortSignal): Promise<ActiveSession | null> =>
    (await request<ActiveSession | undefined>("/sessions/active", { signal })) ?? null;

export const heartbeatSession = (): Promise<void> =>
    request<void>("/sessions/heartbeat", { method: "POST" });
