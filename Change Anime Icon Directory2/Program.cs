﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

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




namespace Change_Anime_Icon_Directory //This program assumes: 1.) Icon Folder is located outside of Anime Folder; 2.) Icon and Anime Folder have the same name
{
    class Program
    {

        //[SecurityCritical]
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
            Console.WriteLine($"Press 1 to add; 2 to update...\n");

            var key = Console.ReadKey().KeyChar;

            try
            {
                if (key.ToString() == "1")
                {
                    AddIcons();
                }

                else if (key.ToString() == "2")
                {
                    UpdateIcons();
                }

                else
                {
                    Console.Write("DAIKON TAKUAN BURI DAIKON, BURI BURI BURI");
                }


            }

            
            catch (Exception e)
            {
                Console.WriteLine("\n\n" + e);

                
            }

            Console.WriteLine($"\n\nPress any key to terminate...");
            Console.ReadKey();

        }

        private static void AddIcons() //for adding folders with icons for the 1st time
        {
            int start, end;
            string filedir = Environment.CurrentDirectory, olddir = null, newdir = null, anime = null;

            string[] foldername = null, lines = { }, icon = null;

            IEnumerable<string> ini = null;

            foldername = Directory.GetDirectories(filedir, "*", SearchOption.AllDirectories); // * gets all directories and subdirectories

            foreach (var path in foldername)                                                    // iterates the list of directories
            {
                if (Path.GetFileNameWithoutExtension(path).Contains("icons") == false)
                {
                    
                
                    Console.WriteLine(path);

                    anime = Path.GetFileNameWithoutExtension(path);                                                // gets the filename of the directory

                    Console.WriteLine("\n\n" + anime);

                    //path = Path.GetDirectoryName(x);                                                           // get the directory name of the file

                    //Console.WriteLine(path);

                    icon = Directory.GetFiles(filedir, anime + ".ico", SearchOption.AllDirectories);                         // finds the icon file with the same name as the directory

                    //Console.WriteLine("\n\n" + filedir);
                    Console.WriteLine("\n\nICON:" + icon.First());                                  

                    ini = Directory.EnumerateFiles(path, "desktop.ini", SearchOption.AllDirectories); // check if directory has "desktop.ini"

                    

                    foreach (var y in icon)
                    {
                        Console.WriteLine("\n\nINI:" + y);
                    }


                    if (ini.Count() == 0)                                                                                      // if there's no desktop.ini file... create one
                    {
                        Console.WriteLine("NULL");

                        /*lines = new string[] { "[ViewState]", "Mode=", "Vid=", "FolderType=Videos", "[.ShellClassInfo]", "IconResource=" + icon.First() + ",0" };


                        File.WriteAllLines(path + @"\desktop.ini", lines, UnicodeEncoding.Unicode);                     //Creates a desktop ini
                        File.SetAttributes(path + @"\desktop.ini", FileAttributes.Hidden);*/

                        SHFOLDERCUSTOMSETTINGS FolderSettings = new SHFOLDERCUSTOMSETTINGS
                        {
                            dwMask = FolderCustomSettingsMask.IconFile,
                            pszIconFile = icon.First(),
                            iIconIndex = 0
                        };

                        uint hResult = SHGetSetFolderCustomSettings(
                            ref FolderSettings, path + char.MinValue, FolderCustomSettingsRW.ForceWrite);

                        Console.WriteLine(hResult);

                              
                        File.SetAttributes(path + @"\desktop.ini", FileAttributes.Hidden);

                    }

                    else
                    {
                        Console.WriteLine("NOT NULL");

                        File.SetAttributes(path + @"\desktop.ini", FileAttributes.Archive);

                        SHFOLDERCUSTOMSETTINGS FolderSettings = new SHFOLDERCUSTOMSETTINGS
                        {
                            dwMask = FolderCustomSettingsMask.IconFile,
                            pszIconFile = icon.First(),
                            iIconIndex = 0
                        };

                        uint hResult = SHGetSetFolderCustomSettings(
                            ref FolderSettings, path + char.MinValue, FolderCustomSettingsRW.ForceWrite);

                        Console.WriteLine(hResult);

                        File.SetAttributes(path + @"\desktop.ini", FileAttributes.Hidden);

                        /*foreach (var deskini in ini)
                        {
                            File.SetAttributes(deskini, FileAttributes.Archive);

                            foreach (var line in File.ReadAllLines(deskini))
                            {
                                if (line.Contains("IconResource"))
                                {
                                    start = line.IndexOf("icons");

                                    Console.WriteLine(line);
                                    Console.WriteLine(line.Length.ToString());
                                    Console.WriteLine(line.IndexOf("icons").ToString());

                                    if (line.Contains(","))
                                    {
                                        end = line.IndexOf(",");

                                        olddir = line.Substring(start, (end - start));

                                        Console.Write(end - start);
                                    }

                                    else
                                    {
                                        end = line.Length - 1;

                                        Console.Write(end);

                                        olddir = line.Substring(start, (end - start + 1));

                                        Console.Write(end - start);
                                    }

                                    Console.WriteLine("\n" + filedir);

                                    newdir = File.ReadAllText(deskini);

                                    newdir = newdir.Replace(line, "IconResource=" + filedir + "\\" + olddir + ",0"); // gets too much slashes when parent

                                    File.WriteAllText(deskini, newdir, Encoding.Unicode);

                                    Console.WriteLine("\n" + olddir);
                                    Console.WriteLine("\n" + newdir);

                                }

                            }

                            File.SetAttributes(deskini, FileAttributes.Hidden);
                        }*/

                    }
                    }
            }
        }

        private static void UpdateIcons() //for updating folder icons to new directory
        {
            int start, end;
            string filedir = Environment.CurrentDirectory, olddir = null, newdir = null;

 
            IEnumerable<string> Ini = Directory.EnumerateFiles(filedir, "desktop.ini", SearchOption.AllDirectories);

            foreach (string deskini in Ini)
            {
                File.SetAttributes(deskini, FileAttributes.Archive);
                
                foreach (var line in File.ReadAllLines(deskini))
                {
                    if (line.Contains("IconResource"))
                    {
                        start = line.IndexOf("icons");

                        Console.WriteLine(line);
                        Console.WriteLine(line.Length.ToString());
                        Console.WriteLine(line.IndexOf("icons").ToString());

                        if (line.Contains(","))
                        {
                            end = line.IndexOf(",");

                            olddir = line.Substring(start, (end - start));

                            Console.Write(end - start);
                        }

                        else
                        {
                            end = line.Length - 1;

                            Console.Write(end);

                            olddir = line.Substring(start, (end - start + 1));

                            Console.Write(end - start);
                        }

                        Console.WriteLine("\n" + filedir);

                        newdir = File.ReadAllText(deskini);

                        newdir = newdir.Replace(line, "IconResource=" + filedir + "\\" + olddir + ",0"); // gets too much slashes when parent

                        File.WriteAllText(deskini, newdir, Encoding.Unicode);

                        Console.WriteLine("\n" + olddir);
                        Console.WriteLine("\n" + newdir);

                    }

                }

                File.SetAttributes(deskini, FileAttributes.Hidden);

            }
        }
    }
}
