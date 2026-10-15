import type { Genre } from "./Book";

export type Species =
    | "Bear"
    | "Bee"
    | "Bird"
    | "Butterfly"
    | "Dog"
    | "Lion"
    | "Tiger"
    | "Whale";

export interface Pet {
    id: string,
    name: string,
    species: Species,
    iconUrl: string,
    favouriteGenre: Genre | null,
    level: number,
    experience: number,
    experienceToNextLevel: number
}
