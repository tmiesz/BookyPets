import type { Book } from "./Book";
import type { Progress } from "./Progress";

export interface LibraryEntry {
    book: Book,
    progress: Progress
}
