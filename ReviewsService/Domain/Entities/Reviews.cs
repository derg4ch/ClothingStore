using Domain.Common;
using Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public int CustomerId { get; private set; }
        public int ProductId { get; private set; }
        public int? OrderId { get; private set; }
        public Rating Rating { get; private set; }
        public string Comment { get; private set; }
        public string ProductName { get; private set; }
        public string CustomerName { get; private set; }
        public bool IsVerifiedPurchase { get; private set; }
        public List<string> HelpfulVotes { get; private set; } = new List<string>();
        public DateTime? UpdatedAt { get; private set; }

        private Review() { }

        public Review(int customerId, int productId, Rating rating, string comment, string productName = null, string customerName = null, int? orderId = null, bool isVerifiedPurchase = false)
        {
            if (customerId <= 0)
                throw new ArgumentException("CustomerId must be greater than 0.");
            if (productId <= 0)
                throw new ArgumentException("ProductId must be greater than 0.");
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be empty.");

            CustomerId = customerId;
            ProductId = productId;
            OrderId = orderId;
            Rating = rating;
            Comment = comment;
            ProductName = productName;
            CustomerName = customerName;
            IsVerifiedPurchase = isVerifiedPurchase;
        }

        public void Update(int rating, string comment)
        {
            Rating = new Rating(rating);
            Comment = comment;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddHelpfulVote(string userId)
        {
            if (!HelpfulVotes.Contains(userId))
            {
                HelpfulVotes.Add(userId);
            }
        }

        public void RemoveHelpfulVote(string userId)
        {
            HelpfulVotes.Remove(userId);
        }
    }
}
