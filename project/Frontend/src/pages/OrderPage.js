import React, { useEffect, useState } from 'react';
import { getMyOrders } from '../services/order'; // Adjust the import path as needed
import Navbar from '../components/NavBar';

const MyOrderPage = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const data = await getMyOrders();
        console.log("Fetched Orders:", data);
        
        // Handle the $values array and $ref references
        const processedOrders = data.$values.map(order => {
          // Skip reference objects (they start with $ref)
          if (order.$ref) return null;
          
          return {
            id: order.id,
            orderDate: order.orderDate,
            status: order.status,
            totalPrice: order.totalPrice,
            discountTotal: order.discountTotal,
            claimCode: order.claimCode,
            isFulfilled: order.isFulfilled,
            fulfilledDate: order.fulfilledDate,
            cancelledDate: order.cancelledDate
          };
        }).filter(order => order !== null); // Remove null entries from $ref items

        setOrders(processedOrders);
      } catch (err) {
        console.error('Error fetching orders:', err);
        setError('Failed to load orders. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchOrders();
  }, []);

  const getStatusText = (statusCode) => {
    switch(statusCode) {
      case 0: return 'Pending';
      case 1: return 'Processing';
      case 2: return 'Completed';
      case 3: return 'Cancelled';
      default: return 'Unknown';
    }
  };

  const formatDate = (dateString) => {
    if (!dateString) return 'N/A';
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  if (loading) return <div className="min-h-screen bg-gray-50"><Navbar /><div className="container mx-auto py-8 px-4">Loading...</div></div>;
  if (error) return <div className="min-h-screen bg-gray-50"><Navbar /><div className="container mx-auto py-8 px-4 text-red-500">{error}</div></div>;

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="container mx-auto py-8 px-4">
        <h1 className="text-3xl font-bold text-center mb-8">My Orders</h1>
        
        {orders.length === 0 ? (
          <div className="text-center text-lg text-gray-500">You haven't placed any orders yet.</div>
        ) : (
          <div className="overflow-x-auto">
            <table className="min-w-full bg-white rounded-lg overflow-hidden">
              <thead className="bg-gray-100">
                <tr>
                  <th className="py-3 px-4 text-left">Order ID</th>
                  <th className="py-3 px-4 text-left">Date</th>
                  <th className="py-3 px-4 text-left">Status</th>
                  <th className="py-3 px-4 text-left">Total</th>
                  <th className="py-3 px-4 text-left">Discount</th>
                  <th className="py-3 px-4 text-left">Claim Code</th>
                  <th className="py-3 px-4 text-left">Fulfilled</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200">
                {orders.map((order) => (
                  <tr key={order.id} className="hover:bg-gray-50">
                    <td className="py-4 px-4">{order.id.substring(0, 8)}...</td>
                    <td className="py-4 px-4">{formatDate(order.orderDate)}</td>
                    <td className="py-4 px-4">
                      <span className={`px-2 py-1 rounded-full text-xs ${
                        order.status === 2 ? 'bg-green-100 text-green-800' :
                        order.status === 3 ? 'bg-red-100 text-red-800' :
                        'bg-yellow-100 text-yellow-800'
                      }`}>
                        {getStatusText(order.status)}
                      </span>
                    </td>
                    <td className="py-4 px-4">${order.totalPrice}</td>
                    <td className="py-4 px-4">${order.discountTotal}</td>
                    <td className="py-4 px-4 font-mono">{order.claimCode}</td>
                    <td className="py-4 px-4">
                      {order.isFulfilled ? 
                        <span className="text-green-600">Yes ({formatDate(order.fulfilledDate)})</span> : 
                        <span className="text-gray-500">No</span>}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default MyOrderPage;