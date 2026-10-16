using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace SeenExtractor.Data.Models.Files
{
    public class PickFile
    {
        OpenFileDialog _openFile;
        SaveFileDialog _saveFile;
        OpenFolderDialog _openFolder;
        public PickFile(OpenFileDialog openFile, SaveFileDialog saveFile, OpenFolderDialog openFolder)
        {
            openFile.Filter = "Archivos TXT | *.txt";
            saveFile.Filter = "Archivos TXT | *.txt";
            _openFile = openFile;
            _saveFile = saveFile;
            _openFolder = openFolder;
        }

        public OpenFileDialog PickFileSeen() => _openFile;

        public SaveFileDialog SaveFileSeen() => _saveFile;

        public OpenFolderDialog OpenFolderSeen() => _openFolder;

    }
}
