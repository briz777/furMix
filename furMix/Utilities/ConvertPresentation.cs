﻿using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using furMix.DialogBoxes;
using System.Threading;

namespace furMix.Utilities
{
    class ConvertPresentation
    {
        static string file;
        static string folder;

        public ConvertPresentation(string filename)
        {
            file = filename;
            FileInfo fileinfo = new FileInfo(file);
            folder = Path.GetTempPath() + @"\" + fileinfo.Name;
        }

        public string FolderName { get => folder; }

        public List<string> Convert()
        {
            List<string> slides = new List<string>();
            Loading load = new Loading();
            load.Show();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            Application ppt = new Application();
            Presentation pres = ppt.Presentations.Open(file, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
            foreach (Slide slide in pres.Slides)
            {
                slide.Export(string.Format(folder + @"\Slide{0}.jpg", slide.SlideIndex), "jpg");
                slides.Add(string.Format(folder + @"\Slide{0}.jpg", slide.SlideIndex));
            }
            load.Close();
            load.Dispose();
            ppt.Quit();
            return slides;
        }
    }
}
