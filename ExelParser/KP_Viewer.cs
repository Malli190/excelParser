using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExelParser
{
    public partial class KP_Viewer : Form
    {
        mainForm mainF;
        Sett.kp_creator excel_creator;
        public KP_Viewer(mainForm mainF, Sett.kp_creator creator, DataTable table)
        {
            InitializeComponent();

            this.mainF = mainF;
            this.excel_creator = creator;

            tableNameLabel.Text = table.TableName;

            excel_creator.create_excel(table, mainF.get_projectName());

            label2.Text = "готово";
        }

        private void onDeactivate(object sender, EventArgs e)
        {
            mainF.onCloseKPViewer();
        }
        private void openKPFolderButtonClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", Directory.GetCurrentDirectory() + "\\" + Sett.kp_creator.default_folder);

        }
    }
}
