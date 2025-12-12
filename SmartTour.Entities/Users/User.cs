
namespace SmartTour.Entities.Users
{
    public class User
    {
        public Guid Id { get; private set; }

        public required string FullName { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }

        public bool IsPhoneVerified { get; set; } = false;

        public required string PasswordHash { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime? LockoutUntil { get; set; }

        public DateTime CreatedAt { get; private set; }=DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }
    }



}

