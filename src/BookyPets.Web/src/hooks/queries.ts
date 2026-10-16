import { keepPreviousData, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
    abandonSession,
    acquireBook,
    acquirePet,
    completeSession,
    getActiveSession,
    getBooks,
    getLibrary,
    getPets,
    getReaderPets,
    startSession
} from "../services/api";

export const queryKeys = {
    books: (search: string, limit?: number) => ["books", { search, limit }] as const,
    pets: (search: string, limit?: number) => ["pets", { search, limit }] as const,
    library: ["library"] as const,
    readerPets: ["readerPets"] as const,
    activeSession: ["activeSession"] as const
};

interface CatalogueParams {
    search?: string,
    limit?: number
}

export function useBooks({ search = "", limit }: CatalogueParams = {}) {
    return useQuery({
        queryKey: queryKeys.books(search, limit),
        queryFn: ({ signal }) => getBooks(search, limit, signal),
        placeholderData: keepPreviousData
    });
}

export function usePets({ search = "", limit }: CatalogueParams = {}) {
    return useQuery({
        queryKey: queryKeys.pets(search, limit),
        queryFn: ({ signal }) => getPets(search, limit, signal),
        placeholderData: keepPreviousData
    });
}

export function useLibrary() {
    return useQuery({
        queryKey: queryKeys.library,
        queryFn: ({ signal }) => getLibrary(undefined, signal)
    });
}

export function useReaderPets() {
    return useQuery({
        queryKey: queryKeys.readerPets,
        queryFn: ({ signal }) => getReaderPets(signal)
    });
}

export function useActiveSession() {
    return useQuery({
        queryKey: queryKeys.activeSession,
        queryFn: ({ signal }) => getActiveSession(signal),
        staleTime: 0,
        gcTime: 0,
        refetchOnWindowFocus: false,
        refetchOnReconnect: false
    });
}

export function useAcquireBook() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (bookId: string) => acquireBook(bookId),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: queryKeys.library })
    });
}

export function useAcquirePet() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (petId: string) => acquirePet(petId),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: queryKeys.readerPets })
    });
}

export function useStartSession() {
    return useMutation({
        mutationFn: ({ progressId, petId }: { progressId: string, petId: string | null }) =>
            startSession(progressId, petId)
    });
}

export function useCompleteSession() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: ({ sessionId, pagesRead }: { sessionId: string, pagesRead: number }) =>
            completeSession(sessionId, pagesRead),
        onSuccess: () => Promise.all([
            queryClient.invalidateQueries({ queryKey: queryKeys.library }),
            queryClient.invalidateQueries({ queryKey: queryKeys.readerPets })
        ])
    });
}

export function useAbandonSession() {
    return useMutation({
        mutationFn: (sessionId: string) => abandonSession(sessionId)
    });
}
