
namespace CreditScore.Models
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            if (user != null)
            {
                Id = user.Id;
                CompanyId = user.CompanyId;
                FirstName = user.FirstName;
                SurName = user.SurName;
                Username = user.UserName;
                Role = user.Role;
                Token = token;
            }
        }
    }
}