import { useEffect, useState } from "react";

export function useSessionTimer(startedAt: number | null): number {
    const [now, setNow] = useState(() => Date.now());

    useEffect(() => {
        if (startedAt === null) return;

        const firstTick = setTimeout(() => setNow(Date.now()), 0);
        const interval = setInterval(() => setNow(Date.now()), 1000);

        return () => {
            clearTimeout(firstTick);
            clearInterval(interval);
        };
    }, [startedAt]);

    return startedAt === null ? 0 : Math.max(0, Math.floor((now - startedAt) / 1000));
}
