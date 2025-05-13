import React, { useEffect, useState } from "react";
import { createBookmark } from "../services/bookmark";
import BookCard from "../components/BookCard";
import Navbar from "../components/NavBar";
import { useNavigate } from 'react-router-dom';
import { getOrderedBooks } from "../services/order";

const MyOrderBooks = () => {
  const [orderedBooks, setOrderedBooks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  // Get current user ID (you might need to adjust this based on your auth system)
  const userId = JSON.parse(localStorage.getItem('user')).id; // or from your auth context

  useEffect(() => {
    const fetchOrderedBooks = async () => {
      try {
        setLoading(true);
        const response = await getOrderedBooks();
        const books = response.data.$values || [];
        setOrderedBooks(books);
        
        if (books.length === 0) {
          setError("You haven't ordered any books yet.");
        }
      } catch (err) {
        console.error("Error fetching ordered books:", err);
        setError("Failed to load your ordered books. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchOrderedBooks();
  }, []);
const handleAddToBookmark = async (bookId) => {
  try {
    if (!userId) {
      alert('Please login to bookmark books');
      navigate('/login');
      return;
    }

    const formData = {
      userId,
      bookId
    };

    await createBookmark(formData);
    alert('Book added to your bookmarks!');

    // Update UI to show the book is bookmarked
    setOrderedBooks(prevBooks =>
      prevBooks.map(book =>
        book.id === bookId ? { ...book, isBookmarked: true } : book
      )
    );
  } catch (error) {
    if (error.status == 409){
    alert('Already added to the bookmark.');
    return;

    }
    console.error('Error adding bookmark:', error.status);
        alert('Failed to add bookmark. Please try again.');

  }
};


  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-2xl font-bold mb-6">My Ordered Books</h1>
          <p>Loading your ordered books...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-2xl font-bold mb-6">My Ordered Books</h1>
          <p className="text-red-500">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-2xl font-bold mb-6">My Ordered Books</h1>
        
        {orderedBooks.length > 0 ? (
          <div className="book-grid">
            {orderedBooks.map((book) => (
              <BookCard
                key={book.id}
                book={book}
                showAddToCart={false}
                showAddToWishlist={true}

  onAddToWhitelist={handleAddToBookmark} // ✅ Pass this prop
              />
            ))}
          </div>
        ) : (
          <p>You haven't ordered any books yet.</p>
        )}
      </div>
    </div>
  );
};

export default MyOrderBooks;