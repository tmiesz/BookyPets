import BookCard from "../components/BookCard"

function Home() {

    const books = [
        {
            id: "1",
            title: "book 1",
            author: "author 1",
            genre: "Fantasy",
            pagecount: 100
        },
        {
            id: "2",
            title: "book 2",
            author: "author 2",
            genre: "Sci-Fi",
            pagecount: 200
        },
        {
            id: "3",
            title: "book 3",
            author: "author 3",
            genre: "Horror",
            pagecount: 300
        }
    ];

    const handleSearch = () => {

    }

    return (
        <div className="home">
            <form onSubmit={handleSearch} className="search-form">
                <input type="text" placeholder="Search for books..." className="search-input" />
                <button type="submit" className="search-btn">Search</button>
            </form>

            <div className="books-grid">
                {books.map((book) => (
                    <BookCard book={book} key={book.id} />
                ))}
            </div>
        </div>
    );
}

export default Home
