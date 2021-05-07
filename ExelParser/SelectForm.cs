using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExelParser
{
    public partial class SelectForm : Form
    {
        Sett.settingsController settingsController;
        searthController searthController;
        mainForm _mainForm;
        ListViewItem selectItem;

        public SelectForm(Sett.settingsController settingsController, mainForm _mainForm)
        {
            InitializeComponent();

            this.settingsController = settingsController;
            this._mainForm = _mainForm;
            searthController = new searthController(settingsController);

            filesCountLabel.Text = this.settingsController.saver1.files.Count.ToString();

            Console.WriteLine($"controller check {this.settingsController.saver1.files.Count}");

            create_file_list(this.settingsController.saver1.files.ToArray());

            updateFilesList();
        }
        private void onSearthCompanyName(object o, EventArgs e) // На поиске по файлу
        {
            searthController.searth_file(textBox1.Text, listView1, resultLabel);
        }
        private void onSearthProdName(object o, EventArgs e) // На поиске по наименованию 
        {
            selectedItemLabel.Text = "-";
            searthController.searth_product(textBox2.Text, listView1, progressBar1);
        }
        private void onSearthReference(object o, KeyPressEventArgs e) // На поиске по референсам
        {
            char number = e.KeyChar;

            if (Char.IsDigit(number))
            {
                searthController.searth_product(textBox3.Text, listView1, progressBar1);
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) // Событие в момент выбора из списка
        {
            selectedItemLabel.Text = listView1.SelectedItems.Count.ToString();
        }
        void updateFilesList() 
        {
            clear_list();
        }
        void clear_list()
        {
            listView1.Items.Clear();
        }
        private void addToMainFormButtonClick(object sender, EventArgs e) // Добавить к форме выбранную строку
        {
            if (selectItem != null)
            {
                _mainForm.add_Item_to_form(selectItem);

                close_window();
            }
        }
        public void Close_Wind(object sender, EventArgs e)
        {
            close_window();
        }
        void close_window()
        {
            searthController.set_abort(true);
            this.Close();
            _mainForm.onCloseShowForm();
        }

        private void listViewSelectItem(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item.SubItems.Count > 1)
            {
                selectItem = e.Item;
                selectedItemLabel.Text = e.Item.SubItems[2].Text;
            }
        }
        private void onDeactiovate(object sender, EventArgs e)
        {
            //close_window();
        }
        private void onCheckBoxChange(object sender, EventArgs e)
        {
            loadFilesLabel.Text = searthController.searth_files(panel1).ToString();
            
        }
        void create_file_list(Sett.saver_files[] list) // Создает список файлов с чекбоксом
        {
            for (int i = 0; i < list.Length; i++)
            {
                Panel check_panel = new Panel(); // 
                CheckBox checkBox = new CheckBox();
                
                check_panel.Width = panel1.Width;
                check_panel.Height = 25;
                //check_panel.BackColor = Color.LightGray;
                
                if(i > 0)
                {
                    check_panel.Location = new Point(0, panel1.Controls[i - 1].Location.Y + panel1.Controls[i - 1].Height);
                }
                checkBox.CheckedChanged += onCheckBoxChange;
                checkBox.Location = new Point(5, 0);
                checkBox.Text = list[i].fileName;
                checkBox.Width = (int)(checkBox.Text.Length * 8);

                check_panel.Controls.Add(checkBox);

                panel1.Controls.Add(check_panel);
            }
        }
        private void onTimerTick(object sender, EventArgs e) // 
        {
            
        }
    }
}
