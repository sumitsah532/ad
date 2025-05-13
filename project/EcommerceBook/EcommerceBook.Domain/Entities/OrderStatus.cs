using System;

namespace EcommerceBook.Domain.Entities
{
    /// <summary>
    /// Represents the different stages of an order's lifecycle.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order has been placed but not yet processed.
        /// </summary>
        Pending,

        /// <summary>
        /// Order is currently being processed.
        /// </summary>
        Processing,

        /// <summary>
        /// Order has been completed and fulfilled.
        /// </summary>
        Completed,

        /// <summary>
        /// Order has been cancelled.
        /// </summary>
        Cancelled
    }
}
