import { Navigate, Outlet, useLocation, type Location } from "react-router-dom";
import { useAuth } from "../context/useAuth";

export default function GuestOnly() {
    const { user } = useAuth();
    const location = useLocation();

    if (user) {
        const from = (location.state as { from?: Location } | null)?.from;

        return <Navigate to={from ?? "/"} replace />;
    }

    return <Outlet />;
}
