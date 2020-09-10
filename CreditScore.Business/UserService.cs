using CreditScore.Interface;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using CreditScore.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CreditScore.Helpers;
using System.Security.Claims;
using AutoMapper;

namespace CreditScore.Business
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public UserService(DatabaseContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public bool AuthenticateUsers(string username, string password)
        {
            bool isAuthenticated = false;
            var result = (from user in _context.User
                          where user.UserName == username
                          select user).FirstOrDefault();

            if (result != null)
                isAuthenticated = VerifyPassword(password, result.PasswordHash);

            return isAuthenticated; 
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            string token = "";
            var userResult = (from user in _context.User
                              where user.UserName == model.Username
                              select user).FirstOrDefault();

            if (userResult == null)
                return null;

            var isPasswordValid = VerifyPassword(userResult.PasswordHash, model.Password);

            // return null if user not found
            if (!isPasswordValid) return null;

            // authentication successful so generate jwt token
            token = generateJwtToken(userResult);

            return new AuthenticateResponse(userResult, token);
        }


        public User AddUpdateUser(UserViewModel userDetail)
        {
            if (userDetail == null)
                return null;

            var user = _context.User.SingleOrDefault(s => s.Id == userDetail.Id);

            if (user == null)
            {
                user = new User();
                user.PasswordHash = HashPassword(userDetail.Password);
            }
            user.FirstName = userDetail.FirstName;
            user.SurName = userDetail.SurName;
            user.UserName = userDetail.UserName;
            user.CreatedDate = DateTime.Now;
            user.Createdby = userDetail.UserId;
            user.Modifiedby = userDetail.UserId;
            user.CompanyId = userDetail.CompanyId;
            user.Active = true;
            user.Role = "USER";//userDetail.Role;
            user.Id = userDetail.Id;

            if (userDetail.Id <= 0)
                _context.User.Add(user);

            _context.SaveChanges();

            return user;

        }

        public List<UserViewModel> GetUsers(long companyID)
        {
            if (companyID <= 0)
                return null;

            //var user = from tUser in _context.User
            //            join tCompany in _context.Company
            //            on tUser.CompanyId equals tCompany.Id
            //            select new { tUser };

            var users = _context.User
                //.Where(a => a.CompanyId == companyID)
                .ToList();

            return _mapper.Map<List<UserViewModel>>(users);



        }


        private string HashPassword(string password, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                // generate a 128-bit salt using a secure PRNG
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (needsOnlyHash) return hashed;
            // password will be concatenated with salt using ':'
            return $"{hashed}:{Convert.ToBase64String(salt)}";
        }

        public bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck)
        {
            // retrieve both salt and password from 'hashedPasswordWithSalt'
            var passwordAndHash = hashedPasswordWithSalt.Split(':');
            if (passwordAndHash == null || passwordAndHash.Length != 2)
                return false;
            var salt = Convert.FromBase64String(passwordAndHash[1]);
            if (salt == null)
                return false;
            // hash the given password
            var hashOfpasswordToCheck = HashPassword(passwordToCheck, salt, true);
            // compare both hashes
            if (String.Compare(passwordAndHash[0], hashOfpasswordToCheck) == 0)
            {
                return true;
            }
            return false;
        }

        public User GetById(int id)
        {
            return (from user in _context.User
                    where user.Id == id
                    select user).FirstOrDefault();
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
