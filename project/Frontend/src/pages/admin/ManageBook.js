import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { FaEdit, FaTrash, FaPlus, FaSearch } from 'react-icons/fa';
import { deleteBook, getAllBooks } from '../../services/books';

const ManageBooksPage = () => {
  const [books, setBooks] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const booksPerPage = 10;

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        setLoading(true);
        const data = await getAllBooks();
        console.log(data.$values)
        setBooks(data.$values);
      } catch (error) {
        console.error("Failed to fetch books:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchBooks();
  }, []);

  const handleDelete = async (bookId) => {
    if (window.confirm('Are you sure you want to delete this book?')) {
      try {
        const response = await deleteBook(bookId);
        console.log("Responseee")
        console.log(response);
        if (!response) {
          setBooks(books.filter(book => book.id !== bookId));
          alert("Book removed successfully");
        } else {
          alert("Failed to delete the book.");
        }
      } catch (error) {
        console.error("Failed to delete book:", error);
        alert("An error occurred while deleting the book.");
      }
    }
  };
  

  const filteredBooks = books.filter(book =>
    book.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
    book.author.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const indexOfLastBook = currentPage * booksPerPage;
  const indexOfFirstBook = indexOfLastBook - booksPerPage;
  const currentBooks = filteredBooks.slice(indexOfFirstBook, indexOfLastBook);
  const totalPages = Math.ceil(filteredBooks.length / booksPerPage);

  console.log(books);
  return (
    <div className="container mx-auto px-4 py-6">
      <h1 className="text-2xl font-bold mb-6">Manage Books</h1>

      {/* Header Actions */}
      <div className="flex justify-between items-center mb-6">
        <div className="w-full max-w-md">
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <FaSearch className="text-gray-400" />
            </div>
            <input
              type="text"
              placeholder="Search by title or author..."
              className="pl-10 pr-10 py-2 w-full border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            {searchTerm && (
              <button
                className="absolute inset-y-0 right-0 pr-3 flex items-center text-gray-400"
                onClick={() => setSearchTerm('')}
              >
                ✕
              </button>
            )}
          </div>
        </div>

        <Link
          to="/admin/add-book"
          className="bg-blue-500 hover:bg-blue-600 text-white py-2 px-4 rounded flex items-center ml-4"
        >
          <FaPlus className="mr-2" /> Add New Book
        </Link>
      </div>

      {/* Table */}
      {loading ? (
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-10 w-10 border-t-2 border-b-2 border-blue-500" />
        </div>
      ) : (
        <div className="bg-white rounded-lg shadow overflow-hidden">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Cover</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Title</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Author</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Price</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Stock</th>
                <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {currentBooks.length > 0 ? (
                currentBooks.map((book) => (
                  <tr key={book.id}>
                    <td className="px-6 py-4">
                      <img
                        src={'http://localhost:5098'+book.bookImageUrl || 'https://via.placeholder.com/50x70?text=No+Image'}
                        alt={'http://localhost:5098'+book.bookImageUrl}
                        className="h-16 w-auto rounded"
                      />
                    </td>
                    <td className="px-6 py-4 font-medium text-gray-900">{book.title}</td>
                    <td className="px-6 py-4 text-gray-500">{book.author}</td>
                    <td className="px-6 py-4 text-gray-500">${book.price.toFixed(2)}</td>
                    <td className="px-6 py-4">
                      <span className={`px-2 inline-flex text-xs font-semibold rounded-full ${
                        book.stock > 10
                          ? 'bg-green-100 text-green-800'
                          : book.stock > 0
                          ? 'bg-yellow-100 text-yellow-800'
                          : 'bg-red-100 text-red-800'
                      }`}>
                        {book.stock}
                      </span>
                    </td>
                    <td className="px-6 py-4 text-right space-x-2">
                      <Link to={`/admin/books/edit/${book.id}`} className="text-blue-500 hover:text-blue-700">
                        <FaEdit />
                      </Link>
                      <button
                        onClick={() => handleDelete(book.id)}
                        className="text-red-500 hover:text-red-700"
                      >
                        <FaTrash />
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="6" className="text-center px-6 py-4 text-gray-500">
                    No books found.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="mt-6 flex justify-center">
          <div className="inline-flex rounded-md shadow-sm" role="group">
            {[...Array(totalPages)].map((_, i) => (
              <button
                key={i}
                onClick={() => setCurrentPage(i + 1)}
                className={`px-4 py-2 text-sm font-medium border ${
                  currentPage === i + 1
                    ? 'bg-blue-500 text-white'
                    : 'bg-white text-gray-700 hover:bg-gray-100'
                }`}
              >
                {i + 1}
              </button>
            ))}
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageBooksPage;
