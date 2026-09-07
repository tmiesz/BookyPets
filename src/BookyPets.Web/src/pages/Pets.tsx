import { useContext } from "react"
import { AuthContext } from "../context/AuthContext"

function Pets() {
    const authContext = useContext(AuthContext);
    if (!authContext) throw new Error("Auth must be used within AuthProvider");
    const auth = authContext;

    return <div>
        <h2>Pets for {auth.user?.firstname} {auth.user?.lastname}</h2>
    </div>
}

export default Pets
