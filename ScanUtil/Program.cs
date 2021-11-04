using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace ScanUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)   //check if path wasn't given in command line
            {
                Console.WriteLine("Path is not given");
            }
            else
            {
                string path = args[0];
                int counter_1 = 0, counter_2 = 0, counter_3 = 0, files_counter = 0, errors = 0;
                string sample1 = "<script>evil_script()</script>";
                string sample2 = "rm -rf " + path;
                string sample3 = "Rundll32 sus.dll SusEntry";
                var watch = Stopwatch.StartNew();
                if (Directory.Exists(path))
                {
                    string[] fileEntries = Directory.GetFiles(path);  //get all files from given directory
                    foreach (string fileName in fileEntries)
                    {
                        files_counter++;
                        try
                        {
                            if (File.Exists(fileName))
                            {
                                string text = File.ReadAllText(fileName);
                                string extension = Path.GetExtension(fileName);
                                if (extension == ".js")   //check is file a js file
                                {
                                    if (text.Contains(sample1))   //check if file contains a string "<script>exile_script()</script>"
                                    {
                                        counter_1++;
                                    }
                                }
                                else if (text.Contains(sample2))  //check if file contains a string "rm -rf %directory_path%"
                                {
                                    counter_2++;
                                }
                                else if (text.Contains(sample3))  //check if file contains a string "Rundll32 sus.dll SusEntry"
                                {
                                    counter_3++;
                                }
                            }
                        }
                        catch (Exception)          //catch all errors while analyzing files
                        {
                            errors++;
                        }
                    }
                    watch.Stop();
                    var elapsed = watch.Elapsed;
                    //print results
                    Console.WriteLine("====== Scan result ======");
                    Console.WriteLine("Processed files: " + files_counter);
                    Console.WriteLine("JS detects: " + counter_1);
                    Console.WriteLine("rm -rf detects: " + counter_2);
                    Console.WriteLine("Rundll32 detects: " + counter_3);
                    Console.WriteLine("Errors: " + errors);
                    Console.WriteLine("Execution time: {0:00}:{1:00}:{2:00}", elapsed.Minutes, elapsed.Seconds, elapsed.Milliseconds);
                    Console.WriteLine("=========================");
                } else
                {
                    Console.WriteLine("Path doesn't exist");
                }
            }       
        }
    }
}
