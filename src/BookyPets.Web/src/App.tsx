import './App.css'

function App() {

    return (
        <>
            <div className="menu-bar">
                <div className="menu-item">
                    <h3>Login</h3>
                </div>
                <div className="menu-item">
                    <h3>Session</h3>
                </div>
                <div className="menu-item">
                    <h3>Books</h3>
                </div>
                <div className="menu-item">
                    <h3>Pets</h3>
                </div>
            </div>

            <button onClick={() => {
                fetch('http://localhost:5293/authentication/login', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ Email: 'admin2@bookypets.com', Password: 'Admin123!@#A' }),
                })
                    .then(response => response.json())
                    .then(data => console.log(data))
            }}>
                Test login
            </button>
        </>
    );
}
export default App
