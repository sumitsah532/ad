import React, { useEffect, useState } from 'react';
import { getBooks, searchBooks } from '../services/books';
import BookCard from '../components/BookCard';
import SearchFilters from '../components/SearchFilters';

const HomePage = () => {
  const [books, setBooks] = useState([]);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [searchParams, setSearchParams] = useState({
    query: '',
    sort: 'title',
    order: 'asc',
    minPrice: undefined,
    maxPrice: undefined,
  });

  useEffect(() => {
    const fetchBooks = async () => {
      setLoading(true);
      try {
        let data;
        if (searchParams.query || searchParams.minPrice || searchParams.maxPrice) {
          data = await searchBooks(searchParams);
        } else {
          data = await getBooks(page);
        }
        setBooks(data);
      } catch (error) {
        console.error('Error fetching books:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, [page, searchParams]);

  const handleAddToCart = (bookId) => {
    // Implement cart functionality
    console.log('Added to cart:', bookId);
  };

  const handleAddToWhitelist = (bookId) => {
    // Implement whitelist functionality
    console.log('Added to whitelist:', bookId);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-6">Book Catalogue</h1>
      
      <SearchFilters 
        onSearch={(params) => {
          setSearchParams(params);
          setPage(1);
        }} 
      />
      
      {loading ? (
        <div className="flex justify-center py-8">
          <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {books.map((book) => (
            <BookCard
              key={book.id}
              book={book}
              onAddToCart={handleAddToCart}
              onAddToWhitelist={handleAddToWhitelist}
            />
          ))}
        </div>
      )}
      
      <div className="flex justify-between mt-8">
        <button
          onClick={() => setPage(p => Math.max(1, p - 1))}
          disabled={page === 1}
          className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
        >
          Previous
        </button>
        <span className="self-center">Page {page}</span>
        <button
          onClick={() => setPage(p => p + 1)}
          disabled={books.length < 10} // Assuming pageSize is 10
          className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
};

export default HomePage;