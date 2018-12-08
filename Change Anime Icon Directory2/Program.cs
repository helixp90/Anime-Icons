using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public enum FolderCustomSettingsMask : uint
{
    InfoTip = 0x00000004,
    Clsid = 0x00000008,
    IconFile = 0x00000010,
    Logo = 0x00000020,
    Flags = 0x00000040
}

public enum FolderCustomSettingsRW : uint
{
    Read = 0x00000001,
    ForceWrite = 0x00000002,
    ReadWrite = Read | ForceWrite
}




namespace Change_Anime_Icon_Directory //This program assumes: 1.) Icon Folder is located within Anime Folder directory; 2.) Icon and Anime Folder have the same name
{
    class Program
    {

        
        [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
        static extern uint SHGetSetFolderCustomSettings(ref SHFOLDERCUSTOMSETTINGS pfcs, string pszPath, FolderCustomSettingsRW dwReadWrite);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct SHFOLDERCUSTOMSETTINGS
        {
            public uint dwSize;
            public FolderCustomSettingsMask dwMask;
            public IntPtr pvid;
            public string pszWebViewTemplate;
            public uint cchWebViewTemplate;
            public string pszWebViewTemplateVersion;
            public string pszInfoTip;
            public uint cchInfoTip;
            public IntPtr pclsid;
            public uint dwFlags;
            public string pszIconFile;
            public uint cchIconFile;
            public int iIconIndex;
            public string pszLogo;
            public uint cchLogo;
        }


        static void Main(string[] args)
        {

            string filedir = Environment.CurrentDirectory, anime = null;

            string[] foldername = null, lines = { }, icon = null;

            IEnumerable<string> ini = null;

            foldername = Directory.GetDirectories(filedir, "*", SearchOption.AllDirectories); // * gets all directories and subdirectories

            Console.WriteLine("Starting...");
            Thread.Sleep(3000);

            try
            {
                foreach (var path in foldername)                                                    // iterates the list of directories
                {
                    if (Path.GetFileNameWithoutExtension(path).Contains("icons") == false)
                    {


                        Console.WriteLine("\nFolder path: " + path);                              


                        anime = Path.GetFileName(path).Replace(".ico", "");                          // gets the filename of the directory

                        Console.WriteLine("\nAnime: " + anime);

                       

                        icon = Directory.GetFiles(filedir, anime + ".ico", SearchOption.AllDirectories);                         // finds the icon file with the same name as the directory

                        
                        Console.WriteLine("\n\nICON: " + icon.First() + "\nlocated at " + path);

                        ini = Directory.EnumerateFiles(path, "desktop.ini", SearchOption.AllDirectories);   // check if directory has "desktop.ini"
                        
                        SHFOLDERCUSTOMSETTINGS FolderSettings = new SHFOLDERCUSTOMSETTINGS                  // Create or update desktop.ini using Powershell code
                        {
                            dwMask = FolderCustomSettingsMask.IconFile,
                            pszIconFile = icon.First(),                                                     // desktop.ini folder properties
                            iIconIndex = 0
                        };

                        uint hResult = SHGetSetFolderCustomSettings(
                            ref FolderSettings, path + char.MinValue, FolderCustomSettingsRW.ForceWrite);  //create or overwrite desktop.ini

                        Console.WriteLine("\n==================================================================================================");
                        
                    }


                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            

            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadKey();


        }

    }

}
