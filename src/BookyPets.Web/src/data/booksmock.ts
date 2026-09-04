import type { Book } from "../types/Book";

const books: Book[] = [
    {
        id: "3a8d2ee9-2d3a-4abc-a72d-472d08b04780",
        title: "Meditations",
        author: "Marcus Aurelius",
        genre: "Philosophy",
        pageCount: 304,
    },
    {
        id: "a59721e6-adf2-4f43-b534-c0d4179901b0",
        title: "Dune",
        author: "Frank Herbert",
        genre: "Fiction",
        pageCount: 412,
    },
    {
        id: "9078f114-594d-4392-8d85-f5682207e5d0",
        title: "A Brief History of Time",
        author: "Stephen Hawking",
        genre: "Science",
        pageCount: 256,
    },
    {
        id: "e21f6b73-9c45-4a82-b1d7-5830e6f92410",
        title: "The Hobbit",
        author: "J.R.R. Tolkien",
        genre: "Fantasy",
        pageCount: 310,
    },
    {
        id: "6d8a42f1-35c7-4e91-a629-817b5c03d246",
        title: "The Psychology of Money",
        author: "Morgan Housel",
        genre: "Psychology",
        pageCount: 256,
    },
];

export function getBooks() {
    return books;
}
