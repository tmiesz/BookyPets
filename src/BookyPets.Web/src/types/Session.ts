export interface Session {
    id: string,
    status: "Active" | "Completed" | "Dropped",
    pagesRead: number,
    endTime: string | null
}
