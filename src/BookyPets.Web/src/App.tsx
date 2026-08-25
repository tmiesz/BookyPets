import './App.css'

function App() {

    return (
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
        </button>)
}
export default App
