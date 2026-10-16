import { useEffect } from "react";
import { heartbeatSession } from "../services/api";

const HEARTBEAT_INTERVAL_MS = 60_000;

export function useHeartbeat(active: boolean): void {
    useEffect(() => {
        if (!active) return;

        const interval = setInterval(() => {
            heartbeatSession().catch(() => { });
        }, HEARTBEAT_INTERVAL_MS);

        return () => clearInterval(interval);
    }, [active]);
}
