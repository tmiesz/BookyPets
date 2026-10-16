import type { User } from "../types/User";

const TOKEN_KEY = "token";
const USER_KEY = "user";

export interface StoredAuth {
    token: string,
    user: User
}

function decodeBase64Url(value: string): string {
    const base64 = value.replace(/-/g, "+").replace(/_/g, "/");
    const padded = base64.padEnd(Math.ceil(base64.length / 4) * 4, "=");
    const binary = atob(padded);

    return new TextDecoder().decode(Uint8Array.from(binary, (char) => char.charCodeAt(0)));
}

function isExpired(token: string): boolean {
    try {
        const payload = JSON.parse(decodeBase64Url(token.split(".")[1])) as { exp?: number };

        return typeof payload.exp !== "number" || payload.exp * 1000 <= Date.now();
    } catch {
        return true;
    }
}

export function getToken(): string | null {
    return localStorage.getItem(TOKEN_KEY);
}

export function loadStoredAuth(): StoredAuth | null {
    try {
        const token = localStorage.getItem(TOKEN_KEY);
        const rawUser = localStorage.getItem(USER_KEY);

        if (token && rawUser && !isExpired(token)) {
            const user = JSON.parse(rawUser) as User;

            if (typeof user.id === "string") {
                return { token, user };
            }
        }
    } catch { }

    clearStoredAuth();
    return null;
}

export function storeAuth(token: string, user: User): void {
    localStorage.setItem(TOKEN_KEY, token);
    localStorage.setItem(USER_KEY, JSON.stringify(user));
}

export function clearStoredAuth(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
}
