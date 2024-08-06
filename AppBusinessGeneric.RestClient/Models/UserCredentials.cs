using System;
using System.Text;
namespace AppBusinessGeneric.RestClient.Models;

public class UserCredentials
{
    public UserCredentials(string userName, string userGroup, string machineName, string password)
    {
        UserName = userName;
        UserGroup = userGroup;
        MachineName = machineName;
        Password = password;
    }
    public string UserName { get; private set; }
    public string UserGroup { get; private set; }
    public string MachineName { get; private set; }
    public string Password { get; private set; }
    public string ConsumerKey
    {
        get { return string.Format("{0}:{1}:{2}", UserName, UserGroup, MachineName); }
    }
    public override string ToString()
    {
        return string.Format("{0}:{1}:{2}:{3}", UserName, UserGroup, MachineName, Password);
    }
    public string GetAuthHeaderString()
    {
        var authInfo = ConsumerKey + ":" + Password;
        authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
        return "Basic " + authInfo;
    }
} 