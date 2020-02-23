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
      var user = new User()
      {
        UserName = username,
        Password = password
      };
      Users.Add(user);
    }

    // Check login info
    public void CheckUsername(string username)
    {
      var checkUser = Users.Any(user => user.UserName == username);
      // validate username and password
      while (checkUser)
      {
        System.Console.WriteLine("Username already taken incorrect, please try again.");

        if (checkUser)
        {

        }
        checkUser = !Users.Any(user => user.UserName == username);
        // if (checkUser)
        // {
        //   System.Console.WriteLine("Username already taken incorrect, please try again.");
        // }
        // else
        // {
        //   checkUser = false;
        // }
      }
    }

    public void CheckPassword(string username, string password)
    {
      var checkPassword = Users.Any(user => user.Password == password);

    }

  }


}