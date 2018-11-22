using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Change_Anime_Icon_Directory
{
    class Program
    {


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
            string filedir = Environment.CurrentDirectory, olddir = null, newdir = null, anime;

            string[] foldername = null;

            IEnumerable<string> ini = null;

            foldername = Directory.GetDirectories(filedir, "*", SearchOption.AllDirectories); // * denotes all (i think)

            foreach (var x in foldername)
            {
                anime = Path.GetFileName(x);

                Console.WriteLine(anime);

                ini = Directory.EnumerateFiles(Path.GetDirectoryName(x), "desktop.ini", SearchOption.AllDirectories);

                Console.WriteLine(ini);

                foreach (var deskini in ini)
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

                            newdir = newdir.Replace(line, "IconResource=" + filedir + olddir + ",0"); // gets too much slashes when parent

                            File.WriteAllText(deskini, newdir, Encoding.Unicode);

                            Console.WriteLine("\n" + olddir);
                            Console.WriteLine("\n" + newdir);

                        }

                        else
                        {

                        }

                    }

                    File.SetAttributes(deskini, FileAttributes.Hidden);
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

                        newdir = newdir.Replace(line, "IconResource=" + filedir + olddir + ",0"); // gets too much slashes when parent

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
