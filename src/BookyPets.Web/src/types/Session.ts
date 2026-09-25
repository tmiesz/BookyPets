export interface Session {
    id: string,
    status: "Active" | "Completed" | "Dropped",
    pagesRead: number,
    endTime: string | null
}

export interface ActiveSession {
    id: string,
    progressId: string,
    petId: string | null,
    startTime: string,
    isStale: boolean
}
