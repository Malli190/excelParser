using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExelParser
{
    public class searthController
    {
        Sett.settingsController settingsController;
        DataTableCollection collection = null;
        DataGridView currentGrid;
        CancellationTokenSource cts = new CancellationTokenSource();
        Task mainTask;

        DataTable currentTable;
        string currentFileName;
        string currentName;
        string loadedFile;

        bool abort_async;
        public bool searching;
        public searthController(Sett.settingsController settingsController)
        {
            this.settingsController = settingsController;
            currentTable = new DataTable();

            currentGrid = new DataGridView();
            currentGrid.Name = "currentGrid";
        }
        public void searth_file(string name, ListView listView, Label textBox)
        {
            string fileName = "";
            abort_async = false;
            for (int i = 0; i < settingsController.saver1.files.Count; i++)
            {
                Sett.saver_files sav_file = settingsController.saver1.files[i];

                if (sav_file.fileName.ToLower().Contains(name.ToLower()))
                {
                    fileName = sav_file.fileName;
                    break;
                }
            }
            if (name.Length > 0)
            {
                textBox.Text = fileName;
                //listBox.BeginInvoke(new Action(() => addFieldToList(fileName, list)));

                if (currentFileName != fileName)
                {
                    listView.Items.Clear();
                    currentFileName = fileName;
                }
            }
            else
            {
                listView.Items.Clear();
                textBox.Text = "-";
                listView.Items.Add("Нет совпадений");
                currentFileName = "";
                if (searching)
                    abort_async = true;
            }
        }
        public void searth_product(string name, ListView listView, ProgressBar progressBar) 
        {
            // Действие на поиск продукта. Чистим, запускаем поток поиска в выбранном файле
            listView.Items.Clear();
            
            if (searching)
                abort_async = true;

            if (name.Length > 0)
            {
                progressBar.Value = 0;
                currentName = name;
                
                listView.Invoke(new Action(() => addFieldToList(currentFileName, name, listView, progressBar)));
            }
            else
            {
                listView.Items.Add("Нет совпадений");
                abort_async = true;
            }
        }
        async void addFieldToList(string fileName, string name, ListView listView, ProgressBar progressBar)
        {
            string filePath = $"files/{fileName}";
            searching = true;
            if (loadedFile != currentFileName)
            {
                try
                {
                    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        collection = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            ConfigureDataTable = (x) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true,
                            }
                        }).Tables;

                        currentGrid.DataSource = collection[0];

                        await Task.Delay(1);

                        Console.WriteLine(collection[0].TableName);
                        loadedFile = currentFileName;
                    }
                    stream.Close();
                }
                catch (Exception ex)
                {
                    listView.Items.Add(ex.Message);
                }
            }
            if (collection != null)
            {
                // Ведет поиск по всем строкам в таблице
                progressBar.Maximum = collection[0].Rows.Count;

                for (int row = 0; row < collection[0].Rows.Count; row++)
                {
                    if (abort_async)
                    {
                        abort_async = false;
                        searching = false;
                        break;
                    }
                    bool canPlace = false;
                    ListViewItem item = new ListViewItem();
                    for (int col = 0; col < collection[0].Columns.Count; col++)
                    {
                        
                        object t = collection[0].Rows[row][col];

                        if (t.ToString().Length > 0)
                        {
                            item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = t.ToString() });
                        }
                        if (t.ToString().ToLower().Contains(name.ToLower()))
                            canPlace = true;
                    }

                    if (canPlace)
                        listView.Items.Add(item);

                    if (progressBar.Value < progressBar.Maximum)
                        progressBar.Value += 1;

                    await Task.Delay(1);
                }
            }
            progressBar.Value = 0;
            searching = false;
        }
        public int searth_files(Panel fileList)
        {
            List<string> realList = new List<string>();
            for(int i = 0; i < fileList.Controls.Count; i++)
            {
                Panel stringPanel = (Panel)fileList.Controls[i];

                CheckBox checkText = (CheckBox)stringPanel.Controls[0];
                
                if (checkText.Checked)
                    realList.Add(checkText.Text);
            }
            return realList.Count;
        }
        public void set_abort(bool abort)
        {
            abort_async = abort;
        }
    }
}
