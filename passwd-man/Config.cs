using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Linq;

public static class Config
{
    const string configFileName = "passwd_man_config.json";

    //Don't question my struct choices, i thought it would be easier to serialize like that, stop crying
    static Configuration config;

    static string configLocation;

    public static void Load()
    {
        configLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), configFileName);

        if (File.Exists(configLocation))
        {
            try
            {
                config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configLocation));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                New();
            }
        }
        else
        {
            New();
        }
    }

    static void New()
    {
        config = new();
        config.vaults = new();
        Save();
    }

    static void Save()
    {
        var str = JsonSerializer.Serialize(config);
        File.WriteAllText(configLocation, str);
    }



    public static string[] ListVaultNames()
    {
        return config.vaults.Select(v => v.name).ToArray();
    }

    public static string GetVaultPath(string name)
    {
        return config.vaults.Where(v => v.name == name).Last().path;
    }

    public static void AddVault(string name, string path)
    {
        config.vaults.Add(new Vault(name, path));
        Save();
    }

    public static void RemoveVault(string name)
    {
        for (int i = 0; i < config.vaults.Count; i++)
        {
            if (config.vaults[i].name == name)
            {
                config.vaults.RemoveAt(i);
            }
        }
        Save();
    }

    public static bool AreVaultsPresent()
    {
        if (config.vaults.Count > 0)
        {
            return true;
        }

        return false;
    }

    //designed like this for future extensibility

    class Configuration
    {
        public List<Vault> vaults { get; set; }
    }

    class Vault
    {
        public string name { get; set; }
        public string path { get; set; }

        public Vault(string name, string path)
        {
            this.name = name;
            this.path = path;
        }
    }
}
