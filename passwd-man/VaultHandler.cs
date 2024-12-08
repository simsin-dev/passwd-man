using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Data.Converters;

namespace passwd_man;

public static class VaultHandler
{
    public static string? CredentialsSetToEdit; //There is probably a better way to do this. I don not care

    static Vault vault;
    static string location = "";
    static string password = "";
    static bool vaultUpdated = false;
    static bool open = false;

    public static bool IsOpen()
    {
        return open;
    }

    public static void CreateVault(string path, string passwd)
    {
        if (open)
        {
            CloseVault();
        }

        vault = new();
        vault.Credentials = [];

        location = path;
        password = passwd;

        open = true;

        Save();
    }

    public static string GetPassword(string name)
    {
        var i = GetIndexOf(name);
        if (i == -1)
        { return ""; }

        return vault.Credentials[i].password;
    }

    public static string? GetLink(string name)
    {
        var i = GetIndexOf(name);
        if (i == -1)
        { return null; }

        return vault.Credentials[i].link;
    }

    public static string? GetUsername(string name)
    {
        var i = GetIndexOf(name);
        if (i == -1)
        { return null; }

        return vault.Credentials[i].username;
    }

    static int GetIndexOf(string name)
    {
        for (int i = 0; i < vault.Credentials.Count; i++)
        {
            if (vault.Credentials[i].name == name)
            {
                return i;
            }
        }

        return -1;
    }

    public static string[] ListCreds()
    {
        if (!open)
        { return null; }
        return vault.Credentials.Select(cred => cred.name).ToArray();
    }

    /// <summary>
    /// Removes a credential set from the vault, and returns if the operation was successful.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool RemoveCreds(string name)
    {
        if (!open)
        {
            return false;
        }

        for (int i = 0; i < vault.Credentials.Count; i++)
        {
            if (vault.Credentials[i].name == name)
            {
                vault.Credentials.RemoveAt(i);

                vaultUpdated = true;
                Save();

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Adds a credential set to the vault, and returns if the operation was successful.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="passwd"></param>
    /// <param name="link"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    public static bool AddCreds(string name, string passwd, string? link, string? username)
    {
        if (!open)
        {
            return false;
        }

        for (int i = 0; i < vault.Credentials.Count; i++)
        {
            if (vault.Credentials[i].name == name)
            {
                vault.Credentials[i].link = link;
                vault.Credentials[i].username = username;
                vault.Credentials[i].password = passwd;

                return true;
            }
        }

        var creds = new CredentialsSet
        {
            name = name,
            password = passwd,
            link = link,
            username = username
        };

        vault.Credentials.Add(creds);

        vaultUpdated = true;
        Save();

        return true;
    }

    // true means successfull operation
    public static async Task<bool> Open(string passwd, string path)
    {
        if (open) { CloseVault(); }

        location = path;
        password = passwd;

        if (!File.Exists(location))
        {
            return false;
        }

        try
        {
            using (FileStream input = new FileStream(location, FileMode.Open, FileAccess.Read))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CFB;
                    aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));

                    byte[] iv = new byte[aes.BlockSize / 8];
                    await input.ReadAsync(iv, 0, iv.Length);

                    aes.IV = iv;

                    var decryptor = aes.CreateDecryptor();

                    using (CryptoStream cs = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                    using (StreamReader reader = new StreamReader(cs))
                    {
                        var json = await reader.ReadToEndAsync();

                        vault = JsonSerializer.Deserialize<Vault>(json);
                    }
                }
            }

            open = true;
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }



    static void CloseVault()
    {
        if (vaultUpdated)
        {
            Save();
        }

        vault = new();
        location = "";
        open = false;
    }

    private static async void Save()
    {
        using (FileStream outs = new FileStream(location, FileMode.Create, FileAccess.Write))
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CFB;
                aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password));
                aes.GenerateIV();

                await outs.WriteAsync(aes.IV, 0, aes.IV.Length);

                var encryptor = aes.CreateEncryptor();

                var json = JsonSerializer.Serialize(vault);

                using (CryptoStream cs = new CryptoStream(outs, encryptor, CryptoStreamMode.Write))
                {
                    await cs.WriteAsync(Encoding.UTF8.GetBytes(json));
                    await cs.FlushAsync();
                }
            }
        }

        vaultUpdated = false;
    }

    class Vault
    { //structured like that bc might add more functionality down the line
        public List<CredentialsSet> Credentials { get; set; }
    }

    public class CredentialsSet
    {
        public string name { get; set; }

        public string? username { get; set; }

        public string? link { get; set; }
        public string password { get; set; }
    }
}
