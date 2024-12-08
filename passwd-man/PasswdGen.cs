using System;
using System.Security.Cryptography;
using System.Text;

namespace passwd_man;

public static class PasswdGen
{
    const string CharSet = "ABCDEFGHIJKLMNOPQRSTUWVXYZabcdefghijklmnopqrstuwvxyz1234567890!@#$%&?*_^-;:/";
    public static string Generate(int Length)
    {
        var passwd = new StringBuilder();

        var rng = RandomNumberGenerator.Create();
        
        byte[] bytes = new byte[Length];

        rng.GetBytes(bytes);

        foreach(byte b in bytes)
        {
            passwd.Append(CharSet[b % CharSet.Length]);
        }

        return passwd.ToString();
    }
}
