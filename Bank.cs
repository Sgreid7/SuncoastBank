using System.Collections.Generic;
using System.Linq;

namespace SuncoastBank
{
  public class Bank
  {
    public List<User> Users { get; set; } = new List<User>();

    // Create user method
    public void CreateUser(string username, string password)
    {
      var newUser = new User()
      {
        UserName = username,
        Password = password
      };
      Users.Add(newUser);
    }

  }

}