export type ValidationErrors = Record<string, string[]>;

interface ApiErrorOptions {
    title?: string,
    errors?: ValidationErrors | null,
    traceId?: string | null
}

export class ApiError extends Error {
    status: number;
    title: string;
    errors: ValidationErrors | null;
    traceId: string | null;

    constructor(status: number, message: string, options: ApiErrorOptions = {}) {
        super(message);
        this.name = "ApiError";
        this.status = status;
        this.title = options.title ?? message;
        this.errors = options.errors ?? null;
        this.traceId = options.traceId ?? null;
    }
}

export function toApiError(error: unknown): ApiError {
    if (error instanceof ApiError) return error;

    return new ApiError(0, "Something went wrong. Please try again.");
}
