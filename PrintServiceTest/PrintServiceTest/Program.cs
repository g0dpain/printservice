using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintServiceTest
{
    class Program
    {
        static FileSystemWatcher fsw = new FileSystemWatcher();
        static Configuration config;
        static Printer printer;

        static void Main(string[] args)
        {
            config = Configuration.GetInstance();
            printer = new Printer();
            fsw.Path = config.Work;
            fsw.Created += Fsw_Created;
            fsw.EnableRaisingEvents = true;
            Console.WriteLine("Press q to quit");
            while (Console.ReadKey().KeyChar != 'q') { }
        }

        private static void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            string backupFile = Path.Combine(config.Backup, e.Name);
            File.Copy(filePath, backupFile+DateTime.Now.ToFileTime());

            FileInfo fi = new FileInfo(filePath);

            Console.WriteLine("Printing " + fi.FullName);
            printer.SendToPrinter(e.FullPath);
            Console.WriteLine("Done printing..");

            if (!config.CanDelete)
                File.Move(filePath, Path.Combine(config.Finished, e.Name));
            else
                File.Delete(filePath);
        }
    }
}
