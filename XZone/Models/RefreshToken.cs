namespace XZone.Models
{
    public class RefreshToken
    {


        public string  Token { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpiresON { get; set; }

        public bool IsExpired => DateTime.UtcNow > ExpiresON;

        public DateTime? RevokeOn { get; set; }

        public bool IsActive => RevokeOn == null && !IsExpired;
    }
}
