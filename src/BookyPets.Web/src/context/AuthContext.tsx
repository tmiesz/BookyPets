import { createContext, useState, type ReactNode } from "react";
import type { User } from "../types/User";
import { BASE_URL } from "../services/api.ts"
import type { ApiError } from "../types/ApiError.ts";

interface AuthContextType {
    user: User | null;
    token: string | null;
    signUp: (firstName: string, lastName: string, email: string, password: string) => Promise<boolean>;
    login: (email: string, password: string) => Promise<boolean>;
    logout: () => void;
    loading: boolean;
    error: ApiError | null;
}

const AuthContext = createContext<AuthContextType | null>(null);

interface AuthResponse {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    token: string
}

export default function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [token, setToken] = useState<string | null>(() => localStorage.getItem("token"))
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<ApiError | null>(null);


    async function signUp(firstname: string, lastname: string, email: string, password: string): Promise<boolean> {
        setLoading(true);
        setError(null);

        try {
            const res = await fetch(`${BASE_URL}/authentication/register`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    FirstName: firstname,
                    LastName: lastname,
                    Email: email,
                    Password: password
                })
            });

            if (!res.ok) {
                const data: ApiError = await res.json();
                setError(data);
                return false;
            }
            const data: AuthResponse = await res.json();
            handleAuthSuccess(data);

            return true;
        } catch (err) {
            setError({
                type: "",
                title: "Error",
                status: 0,
                detail: "Something went wrong",
                traceId: ""
            });

            return false;
        }
        finally {
            setLoading(false)
        }
    }

    async function login(email: string, password: string): Promise<boolean> {
        setLoading(true);
        setError(null);

        try {
            const res = await fetch(`${BASE_URL}/authentication/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    Email: email,
                    Password: password
                })
            });

            if (!res.ok) {
                const data: ApiError = await res.json();
                setError(data);
                return false;
            }
            const data: AuthResponse = await res.json();
            handleAuthSuccess(data);

            return true;
        } catch (err) {
            setError({
                type: "",
                title: "Error",
                status: 0,
                detail: "Something went wrong",
                traceId: ""
            });

            return false;
        }
        finally {
            setLoading(false)
        }
    }

    function handleAuthSuccess(data: AuthResponse) {
        setToken(data.token);
        localStorage.setItem("token", data.token);
        setUser({
            firstname: data.firstName,
            lastname: data.lastName,
            email: data.email
        })
    }

    function logout() {
        setToken(null);
        setUser(null);
        localStorage.removeItem("token");
    }

    return (
        <AuthContext.Provider value={{ user, token, signUp, login, logout, loading, error }}>
            {children}
        </AuthContext.Provider>
    );
}

export { AuthContext };
