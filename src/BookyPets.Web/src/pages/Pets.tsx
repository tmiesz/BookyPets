import { useContext } from "react"
import { AuthContext } from "../AuthContext"

function Pets() {
    const {user} = useContext(AuthContext)
    return <div>
        <h2>Pets for {user.name}</h2>
    </div>
}

export default Pets
