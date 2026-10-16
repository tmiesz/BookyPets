import type { ApiError } from "../types/ApiError";
import "../styles/ErrorMessage.css";

interface ErrorMessageProps {
    error: ApiError | null | undefined,
    className?: string
}

export default function ErrorMessage({ error, className }: ErrorMessageProps) {
    if (!error) return null;

    return (
        <div role="alert" className={className ? `error-message ${className}` : "error-message"}>
            {error.message}
        </div>
    );
}
