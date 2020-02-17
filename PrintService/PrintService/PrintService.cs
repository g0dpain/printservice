using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PrintService
{
    public partial class PrintService : ServiceBase
    {
        private readonly FileSystemWatcher fsw = new FileSystemWatcher();
        private Configuration config;
        private Printer printer;
        public PrintService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            config = Configuration.GetInstance();
            printer = new Printer();
            fsw.Path = config.Work;
            fsw.Created += Fsw_Created;
            fsw.EnableRaisingEvents = true;

        }

        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            string backupFile = Path.Combine(config.Backup, e.Name);
            File.Copy(filePath, backupFile);

            FileInfo fi = new FileInfo(filePath);

            //printer.Print(fi);
            Console.WriteLine(fi.FullName);

            if (!config.CanDelete)
                File.Move(filePath, Path.Combine(config.Finished, e.Name));
            else
                File.Delete(filePath);
        }

        protected override void OnStop()
        {
            fsw.Created -= Fsw_Created;
            fsw.Dispose();
        }
    }
}
