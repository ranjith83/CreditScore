using CreditScore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreditScore.Interface
{
    public interface IUserService
    {
        bool AuthenticateUsers(string username, string password);

        AuthenticateResponse Authenticate(AuthenticateRequest model);

        User GetById(int id);

        User AddUsers(UserDetail userDetail);

        List<User> GetUsers(long companyID);

        bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck);
    }
}
