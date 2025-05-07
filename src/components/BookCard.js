import React from 'react';
import { FaShoppingCart, FaHeart, FaStar } from 'react-icons/fa';
import { useNavigate } from 'react-router-dom';

const BookCard = ({ book, onAddToCart, onAddToWhitelist, showActions = true }) => {
  const navigate = useNavigate();

  const handleBookClick = () => {
    navigate(`/books/${book.id}`);
  };

  return (

    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300 h-full flex flex-col">
      <div
        className="h-48 bg-gray-100 flex items-center justify-center cursor-pointer relative"
        onClick={handleBookClick}
      >
        {book.bookImageUrl ? (
          <img
            src={`http://localhost:5098/${book.bookImageUrl}`}
            alt={book.title}
            className="h-full w-full object-cover"
          />
        ) : (
          <span className="text-gray-500">No Image Available</span>
        )}
        {book.isOnSale && (
          <div className="absolute top-2 right-2 bg-red-500 text-white text-xs font-bold px-2 py-1 rounded-full">
            SALE
          </div>
        )}
      </div>

      <div className="p-4 flex-grow flex flex-col">
        <div className="flex-grow">
          <h3
            className="text-lg font-semibold mb-1 cursor-pointer hover:text-blue-600 line-clamp-2"
            onClick={handleBookClick}
            title={book.title}
          >
            {book.title}
          </h3>
          <p className="text-gray-600 text-sm mb-2">by {book.author}</p>

          <div className="flex items-center mb-2">
            {[...Array(5)].map((_, i) => (
              <FaStar
                key={i}
                className={`text-sm ${i < (book.rating || 0) ? 'text-yellow-400' : 'text-gray-300'}`}
              />
            ))}
            <span className="text-gray-500 text-xs ml-1">
              ({book.reviewCount || 0} reviews)
            </span>
          </div>

          <div className="flex items-center mb-3">
            {book.isOnSale ? (
              <>
                <span className="text-red-500 font-bold mr-2">${book.salePrice}</span>
                <span className="text-gray-500 line-through text-sm">${book.price}</span>
              </>
            ) : (
              <span className="text-gray-800 font-bold">${book.price}</span>
            )}
          </div>

         <div className="flex flex-wrap gap-1 mb-3">
  {/* {(typeof book.tags === "string" ? book.tags.split(",") : book.tags)
    ?.slice(0, 3)
    .map((tag) => (
      <span key={tag} className="bg-gray-100 text-gray-600 text-xs px-2 py-1 rounded">
        {tag}
      </span>
    ))} */}
</div>

        </div>

        {showActions && (
          <div className="flex justify-between mt-auto">
            <button
              onClick={(e) => {
                e.stopPropagation();
                onAddToCart(book);
              }}
              className="flex items-center justify-center bg-blue-600 text-white px-3 py-2 rounded hover:bg-blue-700 flex-grow mr-2"
            >
              <FaShoppingCart className="mr-2" />
              <span className="text-sm">Add to Cart</span>
            </button>
            <button
              onClick={(e) => {
                e.stopPropagation();
                onAddToWhitelist(book.id);
              }}
              className="flex items-center justify-center bg-gray-200 text-gray-800 px-3 py-2 rounded hover:bg-gray-300"
              title="Add to wishlist"
            >
              <FaHeart />
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default BookCard;
