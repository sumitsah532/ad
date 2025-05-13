import React, { useState } from 'react';
// import { useHistory } from 'react-router-dom';

const AddBannerAnnouncement = () => {
  const [title, setTitle] = useState('');
  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(false);
//   const history = useHistory();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      const bannerData = { title, message };
      const response = await AddBannerAnnouncement(bannerData); // API to add banner
      console.log("Banner added:", response.data);
      alert('Banner added successfully!');
    //   history.push('/admin/dashboard'); // Redirect after adding banner
    } catch (error) {
      console.error("Error adding banner:", error);
      alert('Failed to add banner.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <h2 className="text-2xl font-semibold mb-6">Add Banner Announcement</h2>

      <form onSubmit={handleSubmit} className="max-w-2xl mx-auto bg-white p-6 shadow-lg rounded-lg">
        <div className="mb-4">
          <label htmlFor="title" className="block text-sm font-medium text-gray-700">Title</label>
          <input
            id="title"
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="mt-1 block w-full p-2 border border-gray-300 rounded-md"
            required
          />
        </div>

        <div className="mb-4">
          <label htmlFor="message" className="block text-sm font-medium text-gray-700">Message</label>
          <textarea
            id="message"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
            className="mt-1 block w-full p-2 border border-gray-300 rounded-md"
            required
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-600 transition-colors disabled:bg-gray-400"
        >
          {loading ? 'Adding Banner...' : 'Add Banner'}
        </button>
      </form>
    </div>
  );
};

export default AddBannerAnnouncement;
