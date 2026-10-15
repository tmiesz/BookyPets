export interface User {
    id: string,
    firstname: string;
    lastname: string;
    email: string;
}

export interface AuthResponse {
    id: string,
    firstName: string,
    lastName: string,
    email: string,
    token: string
}
