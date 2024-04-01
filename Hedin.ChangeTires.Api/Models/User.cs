namespace Hedin.ChangeTires.Api.Models
{
    public class User
    {
        public User(Guid id, string customerName)
        {
            Id = id;

            CustomerName = customerName;
        }

        public Guid Id { get; set; }
        public string CustomerName { get; set; }
    }
}