export type Genre =
    | "Science"
    | "Technology"
    | "Philosophy"
    | "History"
    | "Psychology"
    | "Fiction"
    | "Fantasy"
    | "Biography"
    | "Educational";

export interface Book {
    id: string,
    title: string,
    author: string,
    genre: Genre,
    iconUrl: string,
    pageCount: number
}
