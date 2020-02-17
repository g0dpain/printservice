using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintService
{
    public class Printer
    {
        public PrintDocument Document { get; private set; }
        private string Path { get; set; }

        public Printer()
        {
            Document = new PrintDocument();
        }

        public void Load(string docPath)
        {
            Path = docPath;
        }

        public void Print(FileInfo value)
        {
            Process p = new Process();
            p.StartInfo.FileName = value.FullName;
            p.StartInfo.Verb = "Print";
            p.Start();
        }

        private void Document_PrintImagePage(object sender, PrintPageEventArgs e)
        {
            Image img = Image.FromFile(Path);
            Point loc = new Point(100, 100);
            e.Graphics.DrawImage(img, loc);
        }
    }
}
