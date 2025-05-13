import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { FaHeart, FaStar } from 'react-icons/fa';
import Navbar from '../components/NavBar';
import {
  getBookmarksByUserId,
  createBookmark,
  deleteBookmark
} from '../services/bookmark';
import { 
  getReviewsByBookId,
  createReview,
  hasUserPurchasedBook 
} from '../services/review';
import axios from 'axios';
import { getAllBooks } from '../services/books';

const BookDetailsPage = () => {
  const { id } = useParams();
  const [book, setBook] = useState(null);
  const [wishlistAdded, setWishlistAdded] = useState(false);
  const [bookmarks, setBookmarks] = useState([]);
  const [reviews, setReviews] = useState([]);
  const [canReview, setCanReview] = useState(false);
  const [newReview, setNewReview] = useState({
    rating: 5,
    comment: ''
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const user = JSON.parse(localStorage.getItem('user'));
  const userId = user?.id;

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const [bookResponse, bookmarksResponse, reviewsResponse] = await Promise.all([
          axios.get(`http://localhost:5098/api/book/${id}`),
          getBookmarksByUserId(userId),
          getReviewsByBookId(id)
        ]);

        setBook(bookResponse.data);
        setBookmarks(bookmarksResponse);
        setReviews(reviewsResponse.$values || []);
        // setWishlistAdded(bookmarksResponse.some(bookmark => bookmark.bookId === id));

        if (userId) {
          const hasPurchased = await getAllBooks();
          setCanReview(hasPurchased);
        }
      } catch (error) {
        console.error('Error fetching data:', error);
        setError('Failed to load book details');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, userId]);

  const handleAddToWishlist = async () => {
    if (!userId) {
      alert('Please login to add to wishlist');
      return;
    }

    try {
      if (wishlistAdded) {
        const bookmarkToRemove = bookmarks.find(bookmark => bookmark.bookId === id);
        await deleteBookmark(bookmarkToRemove.id);
        setWishlistAdded(false);
        alert('Book removed from your wishlist!');
      } else {
        const formData = new FormData();
        formData.append('UserId', userId);
        formData.append('BookId', id);
        await createBookmark(formData);
        setWishlistAdded(true);
        alert('Book added to your wishlist!');
      }
    } catch (error) {
      console.error('Error updating wishlist:', error);
      alert(error.response?.data?.message || 'Failed to update wishlist');
    }
  };

  const handleReviewSubmit = async (e) => {
    e.preventDefault();
    if (!userId) {
      alert('Please login to submit a review');
      return;
    }

    try {
      const response = await createReview({
        userId,
        bookId: id,
        rating: newReview.rating,
        comment: newReview.comment
      });

      setReviews(prev => [...prev, response]);
      setNewReview({ rating: 5, comment: '' });
      alert('Review submitted successfully!');
    } catch (error) {
      console.error('Error submitting review:', error);
      alert(error.response?.data || 'Failed to submit review');
    }
  };

  const calculateAverageRating = () => {
    if (reviews.length === 0) return 0;
    const total = reviews.reduce((sum, review) => sum + review.rating, 0);
    return (total / reviews.length).toFixed(1);
  };

  if (loading) return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="p-6 text-center">Loading book details...</div>
    </div>
  );

  if (error) return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="p-6 text-center text-red-500">{error}</div>
    </div>
  );

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
            {book.tags?.$values?.length > 0 && (
              <p className="text-gray-700">
                <strong>Tags:</strong> {book.tags.$values.join(', ')}
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
              <span className="text-sm text-gray-600">
                ({calculateAverageRating()} from {reviews.length} reviews)
              </span>
            </div>

            <button
              onClick={handleAddToWishlist}
              className={`mt-2 px-4 py-2 rounded text-white flex items-center gap-2 ${wishlistAdded ? 'bg-gray-500' : 'bg-pink-600 hover:bg-pink-700'}`}
            >
              <FaHeart />
              {wishlistAdded ? 'Added to Wishlist' : 'Add to Wishlist'}
            </button>
          </div>
        </div>

        <div className="mt-8">
          <h3 className="text-xl font-bold mb-4">Reviews</h3>
          
          {/* Add Review Form (only for users who purchased) */}
          {canReview && (
            <div className="mb-6 p-4 bg-gray-50 rounded-lg">
              <h4 className="font-medium mb-3">Write a Review</h4>
              <form onSubmit={handleReviewSubmit}>
                <div className="mb-3">
                  <label className="block mb-1">Rating</label>
                  <div className="flex gap-1">
                    {[1, 2, 3, 4, 5].map((star) => (
                      <button
                        key={star}
                        type="button"
                        onClick={() => setNewReview({...newReview, rating: star})}
                        className={`text-2xl ${star <= newReview.rating ? 'text-yellow-400' : 'text-gray-300'}`}
                      >
                        <FaStar />
                      </button>
                    ))}
                  </div>
                </div>
                <div className="mb-3">
                  <label className="block mb-1">Comment</label>
                  <textarea
                    value={newReview.comment}
                    onChange={(e) => setNewReview({...newReview, comment: e.target.value})}
                    className="w-full p-2 border rounded"
                    rows="3"
                    required
                  />
                </div>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
                >
                  Submit Review
                </button>
              </form>
            </div>
          )}

          {/* Reviews List */}
          {reviews.length === 0 ? (
            <p className="text-gray-500">No reviews yet.</p>
          ) : (
            <div className="space-y-4">
              {reviews.map((review) => (
                <div key={review.id} className="border-t pt-4">
                  <div className="flex items-center justify-between">
                    <div className="flex items-center gap-2">
                      <div className="font-medium">
                        {review.user?.name || 'Anonymous'}
                      </div>
                      <div className="flex text-yellow-400">
                        {[...Array(5)].map((_, i) => (
                          <FaStar key={i} className={i < review.rating ? 'text-yellow-400' : 'text-gray-300'} />
                        ))}
                      </div>
                    </div>
                    <div className="text-sm text-gray-500">
                      {new Date(review.createdAt).toLocaleDateString()}
                      {review.updatedAt && ' (edited)'}
                    </div>
                  </div>
                  <p className="mt-2 text-gray-700">{review.comment}</p>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default BookDetailsPage;