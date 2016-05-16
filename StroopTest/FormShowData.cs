﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StroopTest
{
    public partial class FormShowData : Form
    {
        private StroopProgram program = new StroopProgram();
        private string path;

        public FormShowData(string dataFolderPath)
        {
            InitializeComponent();

            string[] filePaths = null;
            path = dataFolderPath;

            string[] headers = program.HeaderOutputFile.Split('\t');
            foreach (var columnName in headers)
            {
                dataGridView1.Columns.Add(columnName, columnName);
            }

            if (Directory.Exists(dataFolderPath))
            {
                filePaths = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
                for (int i = 0; i < filePaths.Length; i++)
                {
                    comboBox1.Items.Add(Path.GetFileNameWithoutExtension(filePaths[i]));
                }
            }
            else
            {
                Console.WriteLine("{0} é um diretório inválido!.", dataFolderPath);
            }
        }
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            string[] lines;
            
            try
            {
                lines = program.readDataFile(path + "/" + comboBox1.SelectedItem.ToString() + ".txt");
                if (lines.Count() > 0)
                {
                    foreach (var cellValues in lines)
                    {
                        var cellArray = cellValues.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (cellArray.Length == dataGridView1.Columns.Count)
                            dataGridView1.Rows.Add(cellArray);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string[] lines;

            saveFileDialog1.Filter = "csv (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    throw new Exception("Selecione um arquivo de dados!");
                }

                lines = program.readDataFile(path + "/" + comboBox1.SelectedItem.ToString() + ".txt");
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (TextWriter tw = new StreamWriter(saveFileDialog1.FileName))
                    {
                        tw.WriteLine(program.HeaderOutputFile);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            tw.WriteLine(lines[i]);
                        }
                        tw.Close();
                        MessageBox.Show("Arquivo exportado com sucesso!");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
