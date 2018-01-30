/*   Copyright 2018 Timothy Brown

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversomate
{   
    
    public partial class Reversomate : Form
    {
        Version version = Assembly.GetExecutingAssembly().GetName().Version;

        public Reversomate()
        {
            InitializeComponent();
            label4.Text ="Reversomate\r\nversion " + $"{version}"  + "\r\nCopyright 2018 Timothy Brown, Licensed under Apache License 2.0\r\nhttp://www.apache.org/licenses/LICENSE-2.0";
        }
        public void ExportAvalible()
        {
            if (string.IsNullOrEmpty(outputBox.Text)) exportButton.Enabled = false;
            else exportButton.Enabled = true;
        }
        public void runEngine()
        {
            if (!string.IsNullOrEmpty(seperatorBox.Text) && !string.IsNullOrEmpty(inputBox.Text))
            {
                char seperator = seperatorBox.Text.First();
                string[] Input = inputBox.Text.Split(seperator);
                Array.Reverse(Input);
                string output = "";
                foreach (string segment in Input)
                {
                    output += segment;
                }
                outputBox.Text = output;
                ExportAvalible();
            }
            else if (string.IsNullOrEmpty(inputBox.Text))
            {
                MessageBox.Show("Cannot run with no input, please provide input text in the box provided.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
                inputBox.Select();
            }
            else
            {
                MessageBox.Show("Please input a character to the 'Seperate by' box.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                seperatorBox.Select();
            }
        }
        private void run_Click(object sender, EventArgs e)
        {
            runEngine();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK) inputBox.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
            }
            catch(Exception)
            {
                MessageBox.Show("Sorry, something went wrong somwhere. Please try again or try something else.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            inputBox.Select();
            
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            inputBox.ReadOnly = true;
            outputBox.ReadOnly = false;
            seperatorBox.ReadOnly = true;
            run.Enabled = false;
            importButton.Enabled = false;
            doneButton.Visible = true;
            exportButton.Visible = false;
            
            if (MessageBox.Show("Please perform any edits you wish to make before export. Click the 'Done' button when ready.", "Edit output for export", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                inputBox.ReadOnly = false;
                outputBox.ReadOnly = true;
                seperatorBox.ReadOnly = false;
                run.Enabled = true;
                importButton.Enabled = true;
                doneButton.Visible = false;
                exportButton.Visible = true;
                ExportAvalible();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                inputBox.ScrollBars = ScrollBars.Both;
                inputBox.WordWrap = false;
                outputBox.ScrollBars = ScrollBars.Both;
                outputBox.WordWrap = false;
            }
            else
            {
                inputBox.ScrollBars = ScrollBars.Vertical;
                inputBox.WordWrap = true;
                outputBox.ScrollBars = ScrollBars.Vertical;
                outputBox.WordWrap = true;
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK) System.IO.File.WriteAllText(saveFileDialog1.FileName, outputBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, something went wrong somwhere. Please try again or try something else.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                inputBox.ReadOnly = false;
                outputBox.ReadOnly = true;
                seperatorBox.ReadOnly = false;
                run.Enabled = true;
                importButton.Enabled = true;
                doneButton.Visible = false;
                exportButton.Visible = true;
                ExportAvalible();
            }
        }

        private void seperatorBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13) runEngine();
        }


    }
}
