using Microsoft.Win32;
using System.IO;
using System.Text;

namespace SeenExtractor.Data.Models.Lectura
{
    public class ConvertFile
    {
        public async Task<bool> PickFile()
        {
            var filePicker = new OpenFileDialog();
            filePicker.Filter = "Archivos TXT (*.txt)|*.txt";
            if ((bool)filePicker.ShowDialog())
            {
                var file = filePicker.OpenFile();
                await ConvertDialogCP932(file);
                return true;
            }
            
            return false;
        }

        public async Task ConvertDialogCP932(Stream file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encodingCP932 = Encoding.GetEncoding(932);
            Encoding encodingANSI = Encoding.GetEncoding(1252);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if ((bool)saveFileDialog.ShowDialog())
            {
                using (StreamReader streamReader = new(file, encodingCP932))
                using (FileStream fileDest = new(saveFileDialog.FileName, FileMode.Create, FileAccess.Write))
                using (StreamWriter streamWriter = new(fileDest, encodingCP932))
                {
                    string line, endline;
                    while (ReadLineCus(streamReader,out line, out endline))
                    {
                        streamWriter.Write(line + endline);
                    }
                    
                }
            }
            
        }

        public bool ReadLineCus(StreamReader file, out string line, out string endline)
        {
            if (file.EndOfStream)
            {
                line = null;
                endline = null;
                return false;
            }

            int code;
            StringBuilder stringBuilder = new();

            while ((code = file.Read()) != -1)
            {
                char c = (char)code;
                if (c == '\n')
                {
                    endline = c.ToString();
                    line = stringBuilder.ToString();
                    return true;
                }
                else if (c == '\r') {
                    if (((char)file.Peek()) == '\n')
                    {
                        endline = c.ToString() + '\n';
                        line = stringBuilder.ToString();
                        file.Read();
                        return true;
                    }
                    else
                    {
                        endline = c.ToString();
                        line = stringBuilder.ToString();
                        return true;
                    }
                    
                }

                stringBuilder.Append(c);

            }

            line = stringBuilder.ToString();
            endline = null;
            return true;

        }

    }
}
