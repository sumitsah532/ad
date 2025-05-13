import React, { useEffect, useState } from "react";
import { fetchBanner, getAllBooks } from "../services/books";
import BookCard from "../components/BookCard";
import Navbar from "../components/NavBar";
import { addCartItem } from "../services/cart";
import { useNavigate } from 'react-router-dom';

const fetchActiveBanner = async () => {
  try {
    const response = await fetchBanner();
    const data = response.$values.length > 0 ? response.$values[0] : null;
    return data;

  } catch (error) {
    console.error("Error fetching banner:", error);
    return null;
  }
};

const HomePage = () => {
  const [books, setBooks] = useState([]);
  const [filteredBooks, setFilteredBooks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const booksPerPage = 8;
  const [searchParams, setSearchParams] = useState({
    query: "",
    minPrice: "",
    maxPrice: "",
    category: "",
  });
  const navigate = useNavigate();

  // State for banner
  const [banner, setBanner] = useState(null);


  useEffect(() => {
    // Fetch books
    const fetchBooks = async () => {
      setLoading(true);
      try {
        const data = await getAllBooks();
        setBooks(data.$values);
        setFilteredBooks(data.$values);
      } catch (error) {
        console.error("Error fetching books:", error);
        navigate('/something-went-wrong'); // Redirects on fetch error
        return;

      } finally {
        setLoading(false);
      }
    };

    // Fetch active banner
    const getBanner = async () => {
      try{

        const activeBanner = await fetchBanner();
        console.log(activeBanner.$values[0]);
        setBanner(activeBanner.$values[0]);
      }catch{
        navigate('/something-went-wrong'); // Redirects on fetch error
return;
      }
    };

    fetchBooks();
    getBanner();
  }, []);

  const handleSearch = (params) => {
    setSearchParams(params);
    const filtered = books.filter((book) => {
      const matchQuery = params.query
        ? book.title.toLowerCase().includes(params.query.toLowerCase())
        : true;
      const matchMinPrice = params.minPrice
        ? book.price >= parseFloat(params.minPrice)
        : true;
      const matchMaxPrice = params.maxPrice
        ? book.price <= parseFloat(params.maxPrice)
        : true;
      const matchCategory = params.category
        ? book.category.toLowerCase() === params.category.toLowerCase()
        : true;
      return matchQuery && matchMinPrice && matchMaxPrice && matchCategory;
    });
    setFilteredBooks(filtered);
    setPage(1);
  };

  const booksToDisplay = filteredBooks;
  const totalPages = Math.ceil(booksToDisplay.length / booksPerPage);
  const paginatedBooks = booksToDisplay.slice(
    (page - 1) * booksPerPage,
    page * booksPerPage
  );

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    const updatedParams = { ...searchParams, [name]: value };
    handleSearch(updatedParams);
  };

  const handleAddToCart = async (book) => {
    const cartItem = {
      bookId: book.id,
      quantity: 1,
      price: book.price,
    };
    try {
      const response = await addCartItem(cartItem);
      console.log("Response from server:", response.data);
      alert(`Added "${book.title}" to cart`);
    } catch (error) {
      console.error("Error adding to cart:", error);
      alert("Failed to add item to cart.");
    }
  };

  const handleAddToWishlist = (bookId) => {
    alert(`Add to wishlist clicked for book ID: ${bookId}`);
  };
  console.log("Banner")
  console.log(banner)

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />

      {/* Banner Section */}
      {banner && (
        <div className="banner-container">
          <div className="banner bg-blue-500 text-white text-center py-4">
            <h2>Title: {banner.title}</h2>
            <p>Message: {banner.message}</p>
          </div>
        </div>
      )}

      <div className="container mx-auto px-4 py-8">
        <h2>Books</h2>

        <div className="search-filter">
          <input
            type="text"
            name="query"
            placeholder="Search by title"
            value={searchParams.query}
            onChange={handleInputChange}
          />
          <input
            type="number"
            name="minPrice"
            placeholder="Min Price"
            value={searchParams.minPrice}
            onChange={handleInputChange}
          />
          <input
            type="number"
            name="maxPrice"
            placeholder="Max Price"
            value={searchParams.maxPrice}
            onChange={handleInputChange}
          />
          <input
            type="text"
            name="category"
            placeholder="Category"
            value={searchParams.category}
            onChange={handleInputChange}
          />
        </div>

        {loading ? (
          <p>Loading books...</p>
        ) : paginatedBooks.length > 0 ? (
          <div className="book-grid">
            {paginatedBooks.map((book) => (
              <BookCard
                key={book.id}
                book={book}
                onAddToCart={handleAddToCart}
                onAddToWhitelist={handleAddToWishlist}
              />
            ))}
          </div>
        ) : (
          <p>No books found.</p>
        )}

        <div className="pagination">
          <button
            onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
            disabled={page === 1}
          >
            Previous
          </button>
          <span>
            Page {page} of {totalPages}
          </span>
          <button
            onClick={() => setPage((prev) => Math.min(prev + 1, totalPages))}
            disabled={page === totalPages}
          >
            Next
          </button>
        </div>

        <style>{`
          .container {
            padding: 20px;
          }

          .search-filter {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
            flex-wrap: wrap;
          }

          .search-filter input {
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 6px;
          }

          .book-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 20px;
          }

          .pagination {
            margin-top: 20px;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
          }

          .pagination button {
            padding: 6px 12px;
            background: #2A004E;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
          }

          .pagination button[disabled] {
            background: #aaa;
            cursor: not-allowed;
          }

          .banner-container {
            overflow: hidden;
          }

          .banner {
            background: #2A004E;
            color: white;
            padding: 10px;
            margin-bottom: 20px;
            animation: slideIn 3s ease-out;
          }

          @keyframes slideIn {
            0% {
              transform: translateX(-100%);
            }
            100% {
              transform: translateX(0);
            }
          }
        `}</style>
      </div>
    </div>
  );
};

export default HomePage;
