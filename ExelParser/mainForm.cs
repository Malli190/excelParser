using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExelParser
{
    public partial class mainForm : Form
    {
        Sett.settingsController settingsController;
        SelectForm selectForm;
        SettingsForm settingsForm;
        KP_Viewer kp_viewer;

        Sett.kp_creator KP = new Sett.kp_creator();
        public mainForm()
        {
            InitializeComponent();

            InitSettings();
        }
        void InitSettings()
        {
            // Загружаем настройки и список файлов из списка sett.txt в корне
            settingsController = new Sett.settingsController();
            
            settingsController.loadSettings();
            this.Text = settingsController.saver1.name + " ver: " + settingsController.saver1.version;
        }
        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (settingsForm == null)
            {
                settingsForm = new SettingsForm(settingsController, this, KP);
                settingsForm.Show();
            }
            else
            {
                settingsForm.Focus();
            }
        }
        private void ButtonNewZakaz(object s, EventArgs e) // Кнопка новый ---------------
        {
            if (selectForm == null)
            {
                selectForm = new SelectForm(settingsController, this);
                selectForm.Show();
            }
            else
            {
                selectForm.Focus();
            }
            
        }
        private void openKPViewer(object s, EventArgs e) // открыть окно формирования КП
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DataTable table = new DataTable();

                foreach (var col in dataGridView1.Columns)
                    table.Columns.Add(col.ToString());

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataRow dRow = table.NewRow();

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    table.Rows.Add(dRow);
                }

                Console.WriteLine("rows: " + table.Rows.Count);
                if (kp_viewer == null)
                {

                    kp_viewer = new KP_Viewer(this, KP, table);
                    kp_viewer.Show();
                }
                else
                {
                    kp_viewer.Focus();
                }
            }
            else
                MessageBox.Show("Таблица пуста");
        }
        public void add_Item_to_form(string data) // Вызывается selectForm'ой  добавляет в таблицу строку из поиска
        {
            dataGridView1.Rows.Add(data);

            itemsCountLabel.Text = (dataGridView1.Rows.Count - 1).ToString();
        }
        public void add_Item_to_form(ListViewItem item)
        {
            DataGridViewRow row = new DataGridViewRow();
            string[] cells = new string[item.SubItems.Count];
            for(int i = 0; i < item.SubItems.Count; i++)
            {
                cells[i] = item.SubItems[i].Text;
            }
            dataGridView1.Rows.Add(cells);

            itemsCountLabel.Text = (dataGridView1.Rows.Count).ToString();

            Focus();
        }
        public void onCloseShowForm()
        {
            selectForm = null;
        }
        public void onCloseKPViewer()
        {
            kp_viewer = null;
        }
        public void onCloseSettingsForm()
        {
            settingsForm = null;
        }
        public string get_projectName()
        {
            return projectNameLabel.Text;
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void onEditProjectNameLabel(object sender, EventArgs e)
        {
            projectNameLabel.Text = textBox1.Text;
            //searthController.searth_file(textBox1.Text);
        }
        private void clearDataGridButtonClick(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            itemsCountLabel.Text = "0";

            MessageBox.Show("Таблица очищена");
        }
    }
}
