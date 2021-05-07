using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ExelParser.Sett
{
    public class settingsController
    {
        public string path = "sett.txt";
        public saver_mainClass saver1 = new saver_mainClass();
        void version()
        {
            saver1.version = "0.1.27";
        }
        public settingsController()
        {
            //saver1 = new saver_mainClass();
            saver1.name = "СеваParse";
            version();

            //saveSettingsAuthor();
        }
        public void add_file(string file)
        {
            string[] proverStr = file.Split('.');
            if (proverStr[1] == "xls")
            {
                Console.WriteLine($"Записан экселевский) {file}");
            }
            saver_files nfile = new saver_files();
            nfile.fileName = file;

            saver1.files.Add(nfile);

            saveSettings();
        }
        public void delete_file(string name)
        {
            for(int i = 0; i < saver1.files.Count; i++)
            {
                if (saver1.files[i].fileName == name)
                {
                    //saver_files f = saver1.files[i];

                    saver1.files.RemoveAt(i);
                }
            }
            saveSettings();
        }
        public void loadSettings()
        {
            try
            {
                using (StreamReader sr = new StreamReader(path, Encoding.Default, true))
                {
                    string result = sr.ReadLine();
                    saver1 = JsonConvert.DeserializeObject<saver_mainClass>(result);

                    Console.WriteLine(result);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void saveSettings()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
                {
                    version();
                    string result = JsonConvert.SerializeObject(saver1);
                    sw.WriteLine(result);
                    Console.WriteLine("типа записал " + result);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                MessageBox.Show(e.Message);
            }
        }
    }
    
    public class saver_mainClass
    {
        public string name { get; set; }
        public string version { get; set; }
        public string KP_Folder { get; set; }
        public List<saver_files> files { get; set; }

        public saver_mainClass() 
        {
            name = "";
            version = "";

            files = new List<saver_files>();
        }
    }
    public class saver_files
    {
        public string fileName { get; set; }

        public saver_files()
        {
            fileName = "";
        }
    }
}
