import { QueryClient } from "@tanstack/react-query";
import { ApiError } from "../types/ApiError";

export const queryClient = new QueryClient({
    defaultOptions: {
        queries: {
            staleTime: 30_000,
            retry: (failureCount, error) => {
                const isClientError = error instanceof ApiError && error.status >= 400 && error.status < 500;

                return !isClientError && failureCount < 2;
            }
        }
    }
});
