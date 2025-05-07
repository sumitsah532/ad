import React, { useEffect, useState } from 'react';
import { clearCart, getCart, removeCartItem, updateCartItem } from '../services/cart';
import Navbar from '../components/NavBar';

const CartPage = () => {
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchCart = async () => {
      try {
        const data = await getCart();
        console.log("Fetched Cart:", data);
        setCart(data);
      } catch (error) {
        console.error('Error fetching cart:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchCart();
  }, []);

  const handleRemoveItem = async (cartItemId) => {
    try {
      await removeCartItem(cartItemId);
      setCart((prevCart) => ({
        ...prevCart,
        cartItems: {
          ...prevCart.cartItems,
          $values: prevCart.cartItems.$values.filter((item) => item.id !== cartItemId),
        },
      }));
    } catch (error) {
      console.error('Error removing item from cart:', error);
    }
  };

  const handleUpdateQuantity = async (cartItemId, newQuantity) => {
    try {
      const updatedItem = { newQuantity };
      await updateCartItem(cartItemId, updatedItem);
      const updatedCart = await getCart();
      setCart(updatedCart);
    } catch (error) {
      console.error('Error updating item quantity:', error);
    }
  };

  const handleClearCart = async () => {
    try {
      await clearCart();
      setCart({ ...cart, cartItems: { $values: [] } });
    } catch (error) {
      console.error('Error clearing cart:', error);
    }
  };

  // 🧹 Filter only isActive cart items (active cart, not completed orders)
  const activeCartItems = cart?.isActive ? cart.cartItems?.$values || [] : [];


  // ✅ Get total quantity of active cart items
  const getTotalQuantity = () => {
    return activeCartItems.reduce((total, item) => total + item.quantity, 0);
  };

  // ✅ Get total price
  const getTotalPrice = () => {
    return activeCartItems.reduce((total, item) => total + item.price * item.quantity, 0);
  };

  // 🧠 Calculate discount
  const getDiscount = () => {
    let discount = 0;
    if (getTotalQuantity() >= 5) {
      discount += 0.05;
    }

    // 🎯 Assume `cart.successfulOrderCount` is passed from backend
    if (cart?.successfulOrderCount >= 10) {
      discount += 0.10;
    }

    return discount;
  };

  const getFinalPrice = () => {
    const discount = getDiscount();
    const total = getTotalPrice();
    return total * (1 - discount);
  };

  if (loading) return <div>Loading...</div>;

  return (
    <div className="min-h-screen bg-gray-50">
      <Navbar />
      <div className="container mx-auto py-8 px-4">
        <h1 className="text-3xl font-bold text-center mb-6">Your Shopping Cart</h1>

        {activeCartItems.length > 0 ? (
          <div>
            <div className="flex flex-col space-y-4">
              {activeCartItems.map((item) => (
                <div key={item.id} className="flex justify-between items-center border-b pb-4 mb-4">
                  <div className="flex items-center space-x-4">
                    <img
                      src={item.book.bookImageUrl}
                      alt={item.book.title}
                      className="w-20 h-28 object-cover rounded"
                    />
                    <div>
                      <h3 className="text-lg font-semibold">{item.book.title}</h3>
                      <p className="text-sm text-gray-500">{item.book.author}</p>
                      <p className="text-sm text-gray-500">Price: ${item.price}</p>
                    </div>
                  </div>

                  <div className="flex items-center space-x-2">
                    <button
                      onClick={() => handleUpdateQuantity(item.id, item.quantity - 1)}
                      className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
                      disabled={item.quantity <= 1}
                    >
                      -
                    </button>
                    <span>{item.quantity}</span>
                    <button
                      onClick={() => handleUpdateQuantity(item.id, item.quantity + 1)}
                      className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
                    >
                      +
                    </button>
                  </div>

                  <button
                    onClick={() => handleRemoveItem(item.id)}
                    className="text-red-500 hover:text-red-700"
                  >
                    Remove
                  </button>
                </div>
              ))}
            </div>

            {/* Clear Cart Button */}
            <div className="text-right mt-4">
              <button
                onClick={handleClearCart}
                className="px-6 py-2 bg-red-500 text-white rounded hover:bg-red-700"
              >
                Clear Cart
              </button>
            </div>

            {/* Summary Section */}
            <div className="mt-6 text-right border-t pt-4 space-y-2">
              <p>Total Quantity: {getTotalQuantity()}</p>
              <p>Total Price: ${getTotalPrice().toFixed(2)}</p>
              {getTotalQuantity() >= 5 && (
                <p className="text-green-600">📦 5% discount applied for 5+ books</p>
              )}
              {cart?.successfulOrderCount >= 10 && (
                <p className="text-blue-600">🏅 10% loyalty discount (10+ successful orders)</p>
              )}
              {getDiscount() > 0 && (
                <p className="text-green-600">
                  Total Discount: {(getDiscount() * 100).toFixed(0)}% (-${(getTotalPrice() * getDiscount()).toFixed(2)})
                </p>
              )}
              <p className="font-bold text-xl">Final Price: ${getFinalPrice().toFixed(2)}</p>

              <button
                onClick={() => alert("Order placed! (connect to API here)")}
                className="mt-4 px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
              >
                Make Order
              </button>
            </div>
          </div>
        ) : (
          <div className="text-center text-lg text-gray-500">Your cart is empty.</div>
        )}
      </div>
    </div>
  );
};

export default CartPage;
