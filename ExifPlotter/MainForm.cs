﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExifPlotter
{
    public partial class MainForm : Form
    {
        private string folderPath = null;
        private bool scanSubFolders = false;
        private PictureFiles picFiles = new PictureFiles();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if(folderPath != null)
            {
                folderBrowserDialog1.SelectedPath = folderPath;
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();

            if(result == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                btnGo.Enabled = true;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            Util util = new Util();

            if (lbFileTypes.SelectedItems.Count > 0)
            {

                util.FindFiles(folderPath, util.GetPattern(lbFileTypes.SelectedItems), chkSubfolders.Checked, picFiles);
            }
            else
            {
                MessageBox.Show("Please select at least one file type", "Select File Types", MessageBoxButtons.OK);
            }

            lblFound.Text = "Files found: " + picFiles.pictureCount;
            lblMissing.Text = "Exif missing: " + picFiles.noExif;
            //lblDuplicates.Text = "Duplicates: " + picFiles.duplicates;        
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            Statistics stats = new Statistics(picFiles);
            List<StatItem> statsList = stats.GenerateFocalStatsBar();
            statsList.Sort();

            new ChartForm(statsList, "bar").Show();
        }
    }
}
