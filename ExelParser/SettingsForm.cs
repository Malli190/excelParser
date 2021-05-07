using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExelParser
{
    public partial class SettingsForm : Form
    {
        Sett.settingsController settingsController;
        Sett.kp_creator KP;
        mainForm mainF;
        public SettingsForm(Sett.settingsController settingsController, mainForm mainF, Sett.kp_creator KP)
        {
            InitializeComponent();

            this.mainF = mainF;
            this.KP = KP;

            InitList(settingsController);
        }
        public void close_form(object sender, EventArgs e)
        {
            this.Close();
        }
        void InitList(Sett.settingsController settingsController) // Инициализируем список ощичаем его
        {
            this.settingsController = settingsController;

            updateFilesList();
        }
        private void changeKPFolderEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //KP.
                settingsController.saver1.KP_Folder = kpFolderTextBox.Text;
                settingsController.saveSettings();
            }
        }
        private void addNewFile(object sender, EventArgs e) // Кнопка добавить в список файл
        {
            bool canAddToList = fileNameTextBox.Text.Length > 0 ? true : false;
            string fileName = canAddToList ? fileNameTextBox.Text : "";

            if (canAddToList)
            {
                settingsController.add_file(fileName);

                updateFilesList();
            }
        }
        void updateFilesList()
        {
            clear_list();
            settingsController.loadSettings();
            
            kpFolderTextBox.Text = settingsController.saver1.KP_Folder;

            for (int i = 0; i < settingsController.saver1.files.Count; i++)
            {
                string result = settingsController.saver1.files[i].fileName;
                fileList.Items.Add(result);
                Console.WriteLine(result);
            }
        }
        private void deleteSelectFileClick(object sender, EventArgs e)
        {
            if (fileList.SelectedItem != null)
            {
                settingsController.delete_file(fileList.SelectedItem.ToString());
                updateFilesList();
                statusLabel.Text = "Удалено";
            }
            else
            {
                MessageBox.Show("ничего не выбрано");
            }
        }
        void clear_list()
        {
            fileList.Items.Clear();
        }

        private void onDeactivate(object sender, EventArgs e)
        {
            mainF.onCloseSettingsForm();
        }
        
    }
}
