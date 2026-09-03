import { useContext, useState, type SubmitEvent } from "react";
import { AuthContext } from "../AuthContext";


export default function Auth() {
    const [name, setName] = useState("")
    const { user, login } = useContext(AuthContext)

    function handleSubmit(e: SubmitEvent) {
        e.preventDefault();
        if (!name.trim()) return
        login(name);
    }

    return (

        <div className="login">
            <form onSubmit={handleSubmit}>
                <label>Name
                    <input
                        type="text"
                        placeholder="Type any name..."
                        value={name}
                        onChange={(e) => setName(e.target.value)} />
                </label>
                <button type="submit">
                    Log in
                </button>
            </form>

            {user.isAuth && <p>User logged in.</p>}
        </div>
    )
}
