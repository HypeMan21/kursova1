using Microsoft.AspNetCore.Identity;

namespace YourNamespace.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public int CarId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        // Додайте інші властивості за потреби

        public virtual Car Car { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
