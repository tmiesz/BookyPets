import type { Pet } from "../types/Pet";

const pets: Pet[] = [
    {
        id: "cc552c65-7129-4e91-9eff-89a1e76c8b89",
        name: "Tom",
        favouriteGenre: null,
        level: 1,
    },
    {
        id: "b5ed214b-b84e-4710-bfa2-24128ccc2c31",
        name: "Alice",
        favouriteGenre: "Fantasy",
        level: 5,
    },
    {
        id: "faa5e0a9-1fd1-4552-9fa1-3318c9b146db",
        name: "James",
        favouriteGenre: "Science",
        level: 3,
    },
    {
        id: "15bc75ee-3f3b-43bc-b09b-9a9b2572d30d",
        name: "Sophie",
        favouriteGenre: "Philosophy",
        level: 7,
    },
    {
        id: "7c4e8a21-6d92-4f35-b8a1-2e7c9d51f604",
        name: "Daniel",
        favouriteGenre: "History",
        level: 2,
    },
];

export function getPets() {
    return pets;
}
