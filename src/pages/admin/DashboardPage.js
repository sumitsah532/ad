import React from 'react';

const DashboardPage = () => {
  const stats = [
    { title: 'Total Books', value: '1,234', change: '+12%', trend: 'up' },
    { title: 'Total Users', value: '568', change: '+5%', trend: 'up' },
    { title: 'Today\'s Orders', value: '42', change: '-3%', trend: 'down' },
    { title: 'Revenue', value: '$8,752', change: '+18%', trend: 'up' },
  ];

  const recentOrders = [
    { id: '#1001', customer: 'John Doe', amount: '$45.99', status: 'Completed' },
    { id: '#1002', customer: 'Jane Smith', amount: '$32.50', status: 'Processing' },
    { id: '#1003', customer: 'Robert Johnson', amount: '$78.25', status: 'Shipped' },
    { id: '#1004', customer: 'Emily Davis', amount: '$124.99', status: 'Completed' },
  ];

  return (
    <div>
      <h1 className="text-2xl font-bold mb-6">Admin Dashboard</h1>

      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        {stats.map((stat, index) => (
          <div key={index} className="bg-white rounded-lg shadow p-6">
            <h3 className="text-gray-500 text-sm font-medium">{stat.title}</h3>
            <p className="text-2xl font-bold mt-2">{stat.value}</p>
            <div className={`flex items-center mt-2 ${
              stat.trend === 'up' ? 'text-green-500' : 'text-red-500'
            }`}>
              <span className="mr-1">{stat.trend === 'up' ? '↑' : '↓'}</span>
              {stat.change}
            </div>
          </div>
        ))}
      </div>

      {/* Recent Orders Table */}
      <div className="bg-white rounded-lg shadow overflow-hidden mb-8">
        <div className="p-6 border-b border-gray-200">
          <h2 className="text-lg font-semibold">Recent Orders</h2>
        </div>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Order ID</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Customer</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Amount</th>
                <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Status</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {recentOrders.map((order, index) => (
                <tr key={index}>
                  <td className="px-6 py-4 text-sm font-medium text-gray-900">{order.id}</td>
                  <td className="px-6 py-4 text-sm text-gray-500">{order.customer}</td>
                  <td className="px-6 py-4 text-sm text-gray-500">{order.amount}</td>
                  <td className="px-6 py-4">
                    <span className={`px-2 inline-flex text-xs font-semibold rounded-full ${
                      order.status === 'Completed' 
                        ? 'bg-green-100 text-green-800' 
                        : order.status === 'Processing' 
                          ? 'bg-yellow-100 text-yellow-800' 
                          : 'bg-blue-100 text-blue-800'
                    }`}>
                      {order.status}
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* Quick Actions */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold mb-4">Quick Actions</h3>
          <div className="space-y-3">
            <button className="w-full bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600">Add New Book</button>
            <button className="w-full bg-green-500 text-white py-2 px-4 rounded hover:bg-green-600">Create Discount</button>
            <button className="w-full bg-purple-500 text-white py-2 px-4 rounded hover:bg-purple-600">Send Announcement</button>
          </div>
        </div>

        <div className="bg-white rounded-lg shadow p-6 md:col-span-2">
          <h3 className="text-lg font-semibold mb-4">Recent Activity</h3>
          <div className="space-y-4">
            <div className="flex items-start">
              <div className="flex-shrink-0 bg-blue-500 rounded-full p-2 text-white"></div>
              <div className="ml-3">
                <p className="text-sm font-medium">New book added</p>
                <p className="text-sm text-gray-500">"Advanced React Patterns" was added to the catalog</p>
                <p className="text-xs text-gray-400">2 hours ago</p>
              </div>
            </div>
            <div className="flex items-start">
              <div className="flex-shrink-0 bg-green-500 rounded-full p-2 text-white"></div>
              <div className="ml-3">
                <p className="text-sm font-medium">New user registered</p>
                <p className="text-sm text-gray-500">"john.doe@example.com" created an account</p>
                <p className="text-xs text-gray-400">5 hours ago</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
