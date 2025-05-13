import React, { useEffect, useState } from "react";
import { getBookmarksByUserId, deleteBookmark } from "../services/bookmark";
import BookCard from "../components/BookCard";
import Navbar from "../components/NavBar";
import { useNavigate } from 'react-router-dom';

const BookmarksPage = () => {
  const [bookmarks, setBookmarks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  // Get current user ID
  const user = JSON.parse(localStorage.getItem('user'));
  const userId = user?.id;

  useEffect(() => {
    if (!userId) {
      navigate('/login');
      return;
    }

    const fetchBookmarks = async () => {
      try {
        setLoading(true);
        const response = await getBookmarksByUserId(userId);
        const bookmarksData = response.$values || [];
        setBookmarks(bookmarksData);
        
        if (bookmarksData.length === 0) {
          setError("You haven't bookmarked any books yet.");
        }
      } catch (err) {
        console.error("Error fetching bookmarks:", err);
        setError("Failed to load your bookmarks. Please try again later.");
      } finally {
        setLoading(false);
      }
    };

    fetchBookmarks();
  }, [userId, navigate]);

  const handleRemoveBookmark = async (bookmarkId) => {
    try {
      await deleteBookmark(bookmarkId);
      // Remove the deleted bookmark from state
      setBookmarks(prev => prev.filter(b => b.id !== bookmarkId));
      alert('Bookmark removed successfully!');
    } catch (error) {
      console.error('Error removing bookmark:', error);
      alert('Failed to remove bookmark. Please try again.');
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-2xl font-bold mb-6">My Bookmarks</h1>
          <p>Loading your bookmarks...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50">
        <Navbar />
        <div className="container mx-auto px-4 py-8">
          <h1 className="text-2xl font-bold mb-6">My Bookmarks</h1>
          <p className="text-red-500">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="container mx-auto px-4 py-8">
        <h1 className="text-2xl font-bold mb-6">My Bookmarks</h1>
        
        {bookmarks.length > 0 ? (
          <div className="book-grid">
            {bookmarks.map((bookmark) => (
              <div key={bookmark.id} className="relative">
                <BookCard
                  book={bookmark.book} // Assuming the API returns the book object nested in bookmark
                  showAddToCart={false}
                  showAddToWishlist={false}
                />
                <button
                  onClick={() => handleRemoveBookmark(bookmark.id)}
                  className="absolute top-2 right-2 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center hover:bg-red-600 transition-colors"
                  title="Remove bookmark"
                >
                  ×
                </button>
              </div>
            ))}
          </div>
        ) : (
          <p>You haven't bookmarked any books yet.</p>
        )}
      </div>
    </div>
  );
};

export default BookmarksPage;