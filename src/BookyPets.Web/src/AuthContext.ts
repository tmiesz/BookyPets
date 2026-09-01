import { createContext } from "react";
import type { User } from "./types/User";

interface AuthContextType {
    user: User;
    login: (name: string) => void;
    logout: () => void;
}
export const AuthContext = createContext<AuthContextType>({
    user: {
        name: "",
        isAuth: false,
    },
    login: () => { },
    logout: () => { },
});
