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

            //Console.Write(key.ToString());

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
            

            Console.WriteLine($"\nPress any key to terminate...");
            Console.ReadKey();


        }

        private static void AddIcons() //for adding folders with icons for the 1st time
        {
            //int start, end;
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


                        newdir = File.ReadAllText(deskini);

                        newdir = newdir.Replace(line, "IconResource=" + filedir + "\\" + olddir + ",0");

                        File.WriteAllText(deskini, newdir, Encoding.Unicode);

                        Console.WriteLine(olddir);
                        Console.WriteLine(newdir);

                    }

                }

                File.SetAttributes(deskini, FileAttributes.Hidden);

            }
        }
    }
}
