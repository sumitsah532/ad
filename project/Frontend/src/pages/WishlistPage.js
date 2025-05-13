import React, { useEffect, useState } from 'react';
import Navbar from '../components/NavBar';
import { getBookmarksByUserId, deleteBookmark } from '../services/bookmark';
import { FaTimes } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

const WishlistPage = () => {
    const [wishlistItems, setWishlistItems] = useState([]);
    const userId = JSON.parse(localStorage.getItem('user'))?.id;
    const token = localStorage.getItem('token');
    const navigate = useNavigate();

    useEffect(() => {
        if (!userId) {
            navigate('/login'); // Redirect to login if user is not logged in
            return;
        }
        const fetchWishlist = async () => {
            try {
                const data = await getBookmarksByUserId(userId, token);
                console.log(data)
                setWishlistItems(data?.$values || []);
            } catch (error) {
                console.error('Error fetching wishlist:', error);
            }
        };
        fetchWishlist();
    }, [userId, token, navigate]);

    const handleRemoveFromWishlist = async (bookmarkId) => {
        try {
            await deleteBookmark(bookmarkId, token);
            setWishlistItems((prevItems) =>
                prevItems.filter((item) => item.id !== bookmarkId)
            );
            alert('Book removed from your wishlist!');
        } catch (error) {
            console.error('Error removing bookmark:', error);
        }
    };

    if (!userId) {
        return <div className="p-6 text-center">Please log in to view your wishlist.</div>;
    }

    if (wishlistItems.length === 0) {
        return (
            <div className="min-h-screen bg-gray-50">
                <Navbar />
                <div className="max-w-4xl mx-auto p-6 bg-white shadow rounded-lg mt-4">
                    <h2 className="text-2xl font-semibold mb-4">Your Wishlist</h2>
                    <p className="text-gray-500">Your wishlist is currently empty.</p>
                </div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-50">
            <Navbar />
            <div className="max-w-6xl mx-auto p-6 mt-4">
                <h2 className="text-2xl font-semibold mb-4 bg-white p-4 rounded-lg shadow">Your Wishlist</h2>
                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4">
                    {wishlistItems.map((item) => (
                        <div key={item.id} className="bg-white rounded-lg shadow-md overflow-hidden relative">
                            {item.book && (
                                <>
                                    <img
                                        src={`http://localhost:5098${item.book.bookImageUrl}`}
                                        alt={item.book.title}
                                        className="w-full h-48 object-cover"
                                    />
                                    <div className="p-4">
                                        <h3 className="text-lg font-semibold mb-2">{item.book.title}</h3>
                                        <p className="text-gray-700 text-sm">Author: {item.book.author}</p>
                                        <p className="text-gray-700 text-sm">
                                            Price: $
                                            {item.book.isOnSale ? item.book.salePrice : item.book.price}
                                        </p>
                                    </div>
                                    <button
                                        onClick={() => handleRemoveFromWishlist(item.id)}
                                        className="absolute top-2 right-2 bg-gray-200 hover:bg-gray-300 rounded-full p-1 text-gray-600"
                                    >
                                        <FaTimes />
                                    </button>
                                </>
                            )}
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
};

export default WishlistPage;
