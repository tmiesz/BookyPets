import { useEffect, useState, type ReactNode } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { loginUser, registerUser, setUnauthorizedHandler } from "../services/api";
import { clearStoredAuth, getToken, loadStoredAuth, storeAuth } from "../services/authStorage";
import type { AuthResponse, User } from "../types/User";
import { AuthContext } from "./authContext";

export default function AuthProvider({ children }: { children: ReactNode }) {
    const queryClient = useQueryClient();
    const [user, setUser] = useState<User | null>(() => loadStoredAuth()?.user ?? null);
    const [sessionExpired, setSessionExpired] = useState(false);

    useEffect(() => {
        setUnauthorizedHandler(() => {
            if (!getToken()) return;

            clearStoredAuth();
            setUser(null);
            setSessionExpired(true);
            queryClient.clear();
        });

        return () => setUnauthorizedHandler(null);
    }, [queryClient]);

    function handleAuthSuccess(data: AuthResponse) {
        const authenticatedUser: User = {
            id: data.id,
            firstname: data.firstName,
            lastname: data.lastName,
            email: data.email
        };

        queryClient.clear();
        storeAuth(data.token, authenticatedUser);
        setUser(authenticatedUser);
        setSessionExpired(false);
    }

    async function signUp(firstName: string, lastName: string, email: string, password: string): Promise<void> {
        handleAuthSuccess(await registerUser(firstName, lastName, email, password));
    }

    async function login(email: string, password: string): Promise<void> {
        handleAuthSuccess(await loginUser(email, password));
    }

    function logout() {
        clearStoredAuth();
        setUser(null);
        queryClient.clear();
    }

    return (
        <AuthContext.Provider value={{ user, sessionExpired, signUp, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}
