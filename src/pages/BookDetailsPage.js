import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { FaHeart, FaStar } from 'react-icons/fa';
import Navbar from '../components/NavBar';
import {
  getBookmarksByUserId,
  createBookmark,
  checkBookmarkExists,
  deleteBookmark
} from '../services/bookmark';
import axios from 'axios';

const BookDetailsPage = () => {
  const { id } = useParams();
  const [book, setBook] = useState(null);
  const [wishlistAdded, setWishlistAdded] = useState(false);
  const [bookmarks, setBookmarks] = useState([]);

  const userId = JSON.parse(localStorage.getItem('user')).id;

  useEffect(() => {
    const fetchBook = async () => {
      try {
        const response = await axios.get(`http://localhost:5098/api/book/${id}`);
        setBook(response.data);
      } catch (error) {
        console.error('Error fetching book:', error);
      }
    };

    fetchBook();
  }, [id]);

  useEffect(() => {
    const fetchBookmarks = async () => {
      try {
        const data = await getBookmarksByUserId(userId);
        setBookmarks(data);
        setWishlistAdded(data.some(bookmark => bookmark.bookId === id));
      } catch (error) {
        console.error('Error fetching bookmarks:', error);
      }
    };

    fetchBookmarks();
  }, [id, userId]);

  const handleAddToWishlist = async () => {
    if (wishlistAdded) {
      try {
        const bookmarkToRemove = bookmarks.find(bookmark => bookmark.bookId === id);
        await deleteBookmark(bookmarkToRemove.id);
        setWishlistAdded(false);
        alert('Book removed from your wishlist!');
      } catch (error) {
        console.error('Error removing bookmark:', error);
      }
    } else {
      try {
        const formData = new FormData();
        formData.append('UserId', userId);
        formData.append('BookId', id);

        await createBookmark(formData);
        setWishlistAdded(true);
        alert('Book added to your wishlist!');
      } catch (error) {
        console.error('Error adding bookmark:', error);
      }
    }
  };

  const getReviewsArray = () => {
    return book?.reviews?.$values || [];
  };

  const calculateAverageRating = () => {
    const reviewsArray = getReviewsArray();
    if (reviewsArray.length === 0) return 0;
    const total = reviewsArray.reduce((sum, review) => sum + review.rating, 0);
    return (total / reviewsArray.length).toFixed(1);
  };

  if (!book) return <div className="p-6 text-center">Loading...</div>;

  const reviewsArray = getReviewsArray();

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="max-w-4xl mx-auto p-6 bg-white shadow rounded-lg mt-4">
        <div className="flex flex-col md:flex-row gap-6">
          <img
            src={`http://localhost:5098${book.bookImageUrl}`}
            alt={book.title}
            className="w-60 h-80 object-cover rounded shadow"
          />
          <div className="flex-1 space-y-4">
            <h2 className="text-3xl font-bold">{book.title}</h2>
            <p className="text-gray-700"><strong>Author:</strong> {book.author}</p>
            <p className="text-gray-700"><strong>ISBN:</strong> {book.isbn}</p>
            <p className="text-gray-700"><strong>Category:</strong> {book.category}</p>
            {book.tags && book.tags.length > 0 && (
              <p className="text-gray-700">
                <strong>Tags:</strong> {book.tags.join(', ')}
              </p>
            )}

            <div>
              {book.isOnSale ? (
                <div className="text-red-600 font-semibold">
                  On Sale: ${book.salePrice}{' '}
                  <span className="line-through text-gray-500">${book.price}</span>
                </div>
              ) : (
                <div className="text-gray-800 font-semibold">Price: ${book.price}</div>
              )}
              <div className="text-sm text-gray-500">In Stock: {book.stockQuantity}</div>
            </div>

            <div className="flex items-center gap-2 text-yellow-500">
              <span className="text-sm text-gray-600">Average Rating:</span>
              {[...Array(5)].map((_, i) => (
                <FaStar
                  key={i}
                  className={i < Math.round(calculateAverageRating()) ? 'text-yellow-400' : 'text-gray-300'}
                />
              ))}
              <span className="text-sm text-gray-600">({calculateAverageRating()})</span>
            </div>

            <button
              onClick={handleAddToWishlist}
              disabled={wishlistAdded}
              className={`mt-2 px-4 py-2 rounded text-white flex items-center gap-2 ${wishlistAdded ? 'bg-gray-500' : 'bg-pink-600 hover:bg-pink-700'}`}
            >
              <FaHeart />
              {wishlistAdded ? 'Added to Wishlist' : 'Add to Wishlist'}
            </button>
          </div>
        </div>

        <div className="mt-8">
          <h3 className="text-xl font-bold mb-2">Reviews</h3>
          {reviewsArray.length === 0 ? (
            <p className="text-gray-500">No reviews yet.</p>
          ) : (
            reviewsArray.map((review, index) => (
              <div key={index} className="border-t pt-2 mt-2">
                <div className="flex items-center">
                  {[...Array(5)].map((_, i) => (
                    <FaStar
                      key={i}
                      className={i < review.rating ? 'text-yellow-400' : 'text-gray-300'}
                    />
                  ))}
                  <span className="ml-2 text-sm text-gray-600">
                    by {review.user?.name || 'Anonymous'}
                  </span>
                </div>
                <p className="text-gray-700 mt-1">{review.comment}</p>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default BookDetailsPage;
