import { createContext } from "react";
import type { User } from "../types/User";

export interface AuthContextType {
    user: User | null;
    sessionExpired: boolean;
    signUp: (firstName: string, lastName: string, email: string, password: string) => Promise<void>;
    login: (email: string, password: string) => Promise<void>;
    logout: () => void;
}

export const AuthContext = createContext<AuthContextType | null>(null);
