# EcommerceBook Order API Documentation

## Base URL
- **Production**: `https://api.ecommercebook.com`  
- **Development**: `http://localhost:5000`

## Authentication
All endpoints require JWT authentication:  
`Authorization: Bearer <your_token>`

## Order Status
- **Pending** – Order placed but not processed  
- **Processing** – Order being prepared  
- **Shipped** – Order dispatched  
- **Completed** – Order delivered  
- **Cancelled** – Order cancelled  

## Endpoints

### 1. Create Order
**POST** `/api/orders`  
Create new order from cart items

**Request Body**
```json
{
  "items": [
    {
      "bookId": "uuid",
      "quantity": 2
    }
  ]
}
```

**Response (201)**
```json
{
  "id": "uuid",
  "orderDate": "2023-08-20T10:30:00Z",
  "status": "Pending",
  "totalPrice": 49.98,
  "items": []
}
```

---

### 2. Get Order by ID  
**GET** `/api/orders/{id}`  
Get order details

**Response (200)**
```json
{
  "id": "uuid",
  "status": "Completed",
  "totalPrice": 49.98,
  "claimCode": "A1B2C3D4E5F6"
}
```

---

### 3. Get User Orders  
**GET** `/api/orders/my-orders`  
Get authenticated user's orders

**Response (200)**
```json
[
  {
    "id": "uuid",
    "orderDate": "2023-08-20T10:30:00Z",
    "status": "Completed"
  }
]
```

---

### 4. Cancel Order  
**PUT** `/api/orders/{id}/cancel`  
Cancel pending order

**Response (204)**: No content

---

### 5. Fulfill Order  
**PUT** `/api/orders/fulfill/{claimCode}`  
(Admin/Staff only) Mark order as fulfilled

**Response (204)**: No content

---

### 6. Get All Orders  
**GET** `/api/orders`  
(Admin only) Get all orders

**Response (200)**
```json
[
  {
    "id": "uuid",
    "userEmail": "user@example.com",
    "status": "Completed"
  }
]
```

---

## Error Responses

| Code | Description              |
|------|--------------------------|
| 400  | Invalid request          |
| 401  | Unauthorized             |
| 403  | Forbidden                |
| 404  | Not found                |
| 500  | Server error             |
