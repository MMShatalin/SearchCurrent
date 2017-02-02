using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System.Drawing.Printing;

//псевдонимы
using SD = System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace Converter
{
    public partial class Form1 : Form
    {

        private System.Collections.ArrayList customers = new System.Collections.ArrayList();
        private MyVirtualClass customerInEdit;
        private int rowInEdit = -1;
        private bool rowScopeCommit = true;
        public Form1()
        {
            InitializeComponent();




            chart1.Series["Series1"].Color = Color.Blue;
            chart1.Series["Series2"].Color = Color.Red;
            chart1.Series["Series3"].Color = Color.Lime;
            chart1.Series["Series4"].Color = Color.Aqua;

            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.DarkGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.DarkGray;

            chart1.Legends["Legend1"].BorderColor = Color.Black;
        }

        //  МЕТОД ОЧИСТКИ РАБОЧЕЙ ЗОНЫ 

        MyListOfSensors MyAllSensors = new MyListOfSensors();
        int indexSVBU;
        double HowMuchMegobite;

        int indexTOK1;
        int indexTOK2;
        int indexR1;
        int indexR2;
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            r.Clear();
            //   progressBar2.Maximum = 0;
            MyListBegin.Clear();
            MyListEnd.Clear();

            checkedListBox1.Items.Clear();
            MyAllSensors.Clear();
            Y.Clear();
            //  chart1.ChartAreas.Clear();
            NumberSeries = 0;
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
                chart1.Series[i].LegendText = "";
            }

            //убирает все галочки с чеклистбоксов
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            chart1.Titles.Clear();

            for (int i = 1; i < chart1.Series.Count - 2; i++)
            {
                chart1.Series["Series" + i].IsVisibleInLegend = false;
                chart1.Series["Series" + i].Points.Clear();
            }
            //  openFileDialog1.Filter = "txt файлы (*.txt)|*.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                очиститьToolStripMenuItem.Enabled = true;
                открытьToolStripMenuItem.Enabled = false;

                //   System.IO.StreamReader streamReader;
                //   streamReader = new System.IO.StreamReader(openFileDialog1.FileName, System.Text.Encoding.GetEncoding("Utf-8"));

                //     textBox1.Text = Convert.ToString(openFileDialog1.FileName);

                FileInfo File = new FileInfo(openFileDialog1.FileName);
                HowMuchMegobite = File.Length / 1024 / 1024;

                foreach (var item in openFileDialog1.FileNames)
                {
                    MyAllSensors.LoadFromFile(item, MyAllSensors);
                }

                MyAllSensors.Sort();

                
                
                //textBox2.Text = progressBar2.Maximum + " " + MyAllSensors[0].MyListRecordsForOneKKS.Count;
                for (int i = 0; i < MyAllSensors.Count; i++)
                {
                    checkedListBox1.Items.Add(MyAllSensors[i].KKS_Name);
                }
                for (int j = 0; j < MyAllSensors.Count; j++)
                {
                    if (MyAllSensors[j].KKS_Name == "254  tok1")
                    {
                        indexTOK1 = j;
                    }
                    if (MyAllSensors[j].KKS_Name == "255  tok2")
                    {
                        indexTOK2 = j;
                    }
                    if (MyAllSensors[j].KKS_Name == "251  r1")
                    {
                        indexR1 = j;
                    }
                    if (MyAllSensors[j].KKS_Name == "252  r2")
                    {
                        indexR2 = j;
                    }
                }
           //     MessageBox.Show(indexTOK1 + " " + indexTOK2 + " " + indexR1 + " " + indexR2);
                

            }//End openFileDialog1.ShowDialog()
        }
        Excel.Application xlApp;
        private Excel.Workbook xlWorkBook;

        DateTime MyBeginner;
        DateTime MyFinished;
        private void button1_Click_1(object sender, EventArgs e)
        {
        }



        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            MyAllSensors.Clear();
            очиститьToolStripMenuItem.Enabled = false;
            открытьToolStripMenuItem.Enabled = true;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void очиститьГрафикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 main = this.Owner as Form2;
            rr.Clear();
            rrr.Clear();

            Y.Clear();
            NumberSeries = 0;
            for (int i = 0; i < chart1.Series.Count; i++)
            {
                chart1.Series[i].Points.Clear();
                chart1.Series[i].LegendText = "";
                chart1.Series[i].YAxisType = AxisType.Primary;
                chart1.Series[i].XValueType = ChartValueType.Double;

            }

            //убирает все галочки с чеклистбоксов
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            chart1.Titles.Clear();

            for (int i = 1; i < chart1.Series.Count - 2; i++)
            {
                chart1.Series["Series" + i].IsVisibleInLegend = false;
                chart1.Series["Series" + i].Points.Clear();
            }
            
            //ОЧИСТКА ПОДПИСЕЙ ПАРАМЕТРОВ ОСЕЙ
            chart1.Annotations.Clear();
            listaver.Clear();
            chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chart1.ChartAreas[0].Position.Auto = true;      
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                checkedListBox1.ContextMenuStrip = contextMenuStrip1;
                checkedListBox1.SelectedIndex = checkedListBox1.IndexFromPoint(e.X, e.Y);
            }
        }

        private void добатьбToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        System.Windows.Forms.DataVisualization.Charting.ChartArea newchar = new System.Windows.Forms.DataVisualization.Charting.ChartArea();


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
        List<double> Y = new List<double>();
        private void добавитьНаОсьXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.MinorTickMark.Enabled = true;
            chart1.ChartAreas[0].AxisY.MinorTickMark.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY.MinorTickMark.LineColor = Color.LightGray;

            AddGrafAnotherVremeni(chart1.Series[NumberSeries].Name);
            NumberSeries++;
        }

        public void NextAxis(string b)
        {

            b = chart1.Series[NumberSeries].Name;
            //   b = chart1.Series[NumberSeries].Name;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.Auto;
            for (int j = 0; j < MyAllSensors.Count; j++)
            {
                if ((string)checkedListBox1.Text == MyAllSensors[j].KKS_Name)
                {
                    for (int i = 0; i < MyAllSensors[j].MyListRecordsForOneKKS.Count; i++)
                    {
                        chart1.Series[chart1.Series[NumberSeries].Name].Points.AddXY(MyAllSensors[j].MyListRecordsForOneKKS[i].DateTime, MyAllSensors[j].MyListRecordsForOneKKS[i].Value);
                        //    Y.Add(MyAllSensors[j].MyListRecordsForOneKKS[i].Value);

                    }
                    chart1.Series[NumberSeries].IsVisibleInLegend = true;
                    chart1.Series[NumberSeries].LegendText = MyAllSensors[j].KKS_Name;
                    chart1.Series[NumberSeries].XValueType = ChartValueType.Time;


                    chart1.ChartAreas[0].AxisX.Title = "Время, чч:мм:сс";

                    chart1.Series[chart1.Series[NumberSeries].Name].YAxisType = AxisType.Secondary;
                    chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                    chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.LightGray;
                }
            }
            chart1.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Triangle;
            chart1.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Triangle;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.LightGray;
        }








        List<double> listaver = new List<double>();
        private void добавитьНаОсьYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.VirtualMode = true;

            DataGridViewTextBoxColumn companyNameColumn = new
                DataGridViewTextBoxColumn();
            companyNameColumn.HeaderText = "Время";
            companyNameColumn.Name = "Время";
            this.dataGridView1.Columns.Add(companyNameColumn);
            DataGridViewTextBoxColumn companyNameColumn1 = new
               DataGridViewTextBoxColumn();
            companyNameColumn1.HeaderText = "Значение";
            companyNameColumn1.Name = "Значение";

            this.dataGridView1.Columns.Add(companyNameColumn1);
          
            chart1.Series[0].LegendText = checkedListBox1.Text;

            for (int i = 1; i < 11; i++)
                chart1.Series["Series" + i].IsVisibleInLegend = false;

            chart1.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Times New Roman", 14, FontStyle.Regular);
            chart1.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 14, FontStyle.Regular);
           
        }

        private void dataGridView1_CellValueNeeded(object sender,
        System.Windows.Forms.DataGridViewCellValueEventArgs e)
        {
            //this.dataGridView1.RowCount = 1;
            // If this is the row for new records, no values are needed.
            if (e.RowIndex == this.dataGridView1.RowCount - 1) return;

            MyVirtualClass customerTmp = null;

            // Store a reference to the Customer object for the row being painted.
            if (e.RowIndex == rowInEdit)
            {
                customerTmp = this.customerInEdit;
            }
            else
            {
                customerTmp = (MyVirtualClass)this.customers[e.RowIndex];
            }

            // Set the cell value to paint using the Customer object retrieved.
            switch (this.dataGridView1.Columns[e.ColumnIndex].Name)
            {
                case "Время":
                    e.Value = customerTmp.time;
                    break;

                case "Значение":
                    e.Value = customerTmp.value;
                    break;
            }
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Visible = false;
                Sencors myOneKKS = new Sencors();
                myOneKKS = MyAllSensors.getOneKKSByIndex(checkedListBox1.SelectedIndex);
                this.dataGridView1.CellValueNeeded += new
                    DataGridViewCellValueEventHandler(dataGridView1_CellValueNeeded);

                dataGridView1.Rows.Clear();
                customers.Clear();

                for (int i = 0; i < myOneKKS.MyListRecordsForOneKKS.Count; i++)
                {
                    this.customers.Add(new MyVirtualClass(myOneKKS.MyListRecordsForOneKKS[i].DateTime.ToString(), myOneKKS.MyListRecordsForOneKKS[i].Value.ToString()));

                }
                if (this.dataGridView1.RowCount == 0)
                {
                    this.dataGridView1.RowCount = 1;

                }

                this.dataGridView1.RowCount = myOneKKS.MyListRecordsForOneKKS.Count;
                dataGridView1.Visible = true;
            }
            catch (Exception ex0)
            {
                MessageBox.Show(ex0.Message);
            }
            //if (indexSVBU == 1 && HowMuchMegobite > 3)
            //{
            //    показатьДанныуToolStripMenuItem.Enabled = true;
            //}
            //if (indexSVBU == 1 && HowMuchMegobite <= 3)
            //{
            //    показатьДанныуToolStripMenuItem.Enabled = false;
            //    Sencors myOneKKS = new Sencors();
            //    myOneKKS = MyAllSensors.getOneKKSByIndex(checkedListBox1.SelectedIndex);
            //    System.Data.DataTable dt = new System.Data.DataTable();
            //    dt.Columns.Add("Дата");
            //    dt.Columns.Add("Значение");
            //    foreach (Record item in myOneKKS.MyListRecordsForOneKKS)
            //    {
            //        dt.Rows.Add(item.DateTime, item.Value);
            //    }
            //    dataGridView1.DataSource = dt;
            //    показатьДанныуToolStripMenuItem.Enabled = false;
            //}
            //if (indexSVBU == 0)
            //{
            //    показатьДанныуToolStripMenuItem.Enabled = false;
            //    Sencors myOneKKS = new Sencors();
            //    myOneKKS = MyAllSensors.getOneKKSByIndex(checkedListBox1.SelectedIndex);
            //    System.Data.DataTable dt = new System.Data.DataTable();
            //    dt.Columns.Add("Дата");
            //    dt.Columns.Add("Значение");
            //    foreach (Record item in myOneKKS.MyListRecordsForOneKKS)
            //    {
            //        dt.Rows.Add(item.ValueTimeForDAT, item.Value);
            //    }
            //    dataGridView1.DataSource = dt;
            //    показатьДанныуToolStripMenuItem.Enabled = false;
            //}



            //if (checkedListBox1.CheckedItems.Count > 0)
            //{
            //    button1.Enabled = true;
            //}
        }
        double value;

        List<DateTime> fff = new List<DateTime>();

        List<DateTime> beg = new List<DateTime>();
        List<DateTime> end = new List<DateTime>();
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.LineColor = Color.Black;

            if (e.Button == MouseButtons.Right)
            {
                chart1.ContextMenuStrip = contextMenuStrip2;
            }

            if (e.Button == MouseButtons.Left)
            {


            }
        }


        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(saveFileDialog1.FileName + ".TIFF", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }

        private void параметрыГрафикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 ChangeParametrImage = new Form2();
            ChangeParametrImage.Owner = this;
            ChangeParametrImage.Show();
        }

        private void добавитьНаВспомагательнуюОсьYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void добавитьНаОсновнуюОсьYToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        int NumberSeries = 0;

        private void AddGrafAnotherVremeni(string b)
        {
            b = chart1.Series[NumberSeries].Name;

            for (int j = 0; j < MyAllSensors.Count; j++)
            {
                if ((string)checkedListBox1.Text == MyAllSensors[j].KKS_Name)
                {
                    for (int i = 0; i < MyAllSensors[j].MyListRecordsForOneKKS.Count; i++)
                    {
                        chart1.Series[b].Points.AddXY(MyAllSensors[j].MyListRecordsForOneKKS[i].ValueTimeForDAT, MyAllSensors[j].MyListRecordsForOneKKS[i].Value);
                    }
                    chart1.Series[NumberSeries].IsVisibleInLegend = true;
                    chart1.Series[NumberSeries].LegendText = MyAllSensors[j].KKS_Name;;
                }
            }
        }

        private void убратьПодписиКривыхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Legends["Legend1"].Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void добавитьНаВспомогательнуюОсьXToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }




        private void добавитьНаВспомогательнуюОсьXToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {

        }

        List<double> rrr = new List<double>();
        List<double> rr = new List<double>();
        //   public CreateAxis WorkingWithAxis = new CreateAxis();
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void инструкцияПоПрименениюToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }
        //Word.Application word = new Word.Application();
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            //Excel.Application excel = new Excel.Application();
            // fileExcel = excel.Workbooks.Open(saveFileDialog3.FileName + ".xlsx");
            //Excel._Workbook fileExcel;
            //  Microsoft.Office.Interop.Word.Document t;
            //t = word.Documents.Open("D:\\Инструкция.docx");
            //word.Visible = true;
            System.Diagnostics.Process.Start("D:\\Инструкция.docx");

        }

        private void очиститьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();
        }

        private void jToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void осьY1ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void осьY2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // WorkingWithAxis.CollectionAxis[0].AxisY.LabelStyle.Format = "G";
        }

        private void осьY3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WorkingWithAxis.CollectionAxis[1].AxisY.LabelStyle.Format = "G";
        }

        private void осьY4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  WorkingWithAxis.CollectionAxis[2].AxisY.LabelStyle.Format = "G";
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void форматВремениДоСекундToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void поменятьФорматОсиXНаВременнойToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                chart1.ChartAreas[0].CursorX.LineColor = Color.Black;
                chart1.ChartAreas[0].CursorX.Interval = 0.000000001;


            }
            if (checkBox4.Checked == false)
            {
                chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
                chart1.ChartAreas[0].CursorX.LineColor = Color.Black;
                chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = false;

            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
        }
        public int AmountPoint;
        private void button2_Click_1(object sender, EventArgs e)
        {




            //  textBox2.Text = progressBar2.Maximum.ToString() + " " + MyAllSensors[0].MyListRecordsForOneKKS.Count;
        }
        private void copySelectedRowsToClipboard(DataGridView dgv)
        {
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            Clipboard.Clear();
            if (dgv.GetClipboardContent() != null)
            {
                Clipboard.SetDataObject(dgv.GetClipboardContent());
                Clipboard.GetData(DataFormats.Text);
                IDataObject dt = Clipboard.GetDataObject();
                if (dt.GetDataPresent(typeof(string)))
                {
                    string tb = (string)(dt.GetData(typeof(string)));
                    Encoding encoding = Encoding.GetEncoding(1251);
                    byte[] dataStr = encoding.GetBytes(tb);
                    Clipboard.SetDataObject(encoding.GetString(dataStr));
                }
            }
            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
        }
        private void dataGridView2_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void wxToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (((DataGridView)sender).SelectedCells.Count > 0)
                {
                    copySelectedRowsToClipboard((DataGridView)sender);
                }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
                if (((DataGridView)sender).SelectedCells.Count > 0)
                {
                    copySelectedRowsToClipboard((DataGridView)sender);
                }
        }

        private void показатьДанныуToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //  показатьДанныуToolStripMenuItem.Enabled = true;

            Sencors myOneKKS = new Sencors();
            myOneKKS = MyAllSensors.getOneKKSByIndex(checkedListBox1.SelectedIndex);
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Дата");
            dt.Columns.Add("Значение");
            foreach (Record item in myOneKKS.MyListRecordsForOneKKS)
            {
                dt.Rows.Add(item.DateTime, item.Value);
            }
            dataGridView1.DataSource = dt;

        }


        List<DateTime> rrrg = new List<DateTime>();
        private void button3_Click(object sender, EventArgs e)
        {
        }

        List<int> r = new List<int>();


        List<DateTime> MyListBegin = new List<DateTime>();
        List<DateTime> MyListEnd = new List<DateTime>();
        //    r.Clear();
        private void button4_Click(object sender, EventArgs e)
        {
        }
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (var item in openFileDialog1.FileNames)
                {
                    MyAllSensors.LoadFromFile(item, MyAllSensors);
                }

                checkedListBox1.Items.Clear();

                for (int i = 0; i < MyAllSensors.Count; i++)
                {
                    checkedListBox1.Items.Add(MyAllSensors[i].KKS_Name);

                }
                MyAllSensors.Sort();
                //   textBox2.Text = MyAllSensors.Count.ToString();
            }
        }

        private void пересчитатьРеактивностиToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void построитьРеактивностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyAllSensors[3].MyListRecordsForOneKKS.Count; i++)
            {
                chart1.Series[0].Points.AddXY(MyAllSensors[3].MyListRecordsForOneKKS[i].ValueTimeForDAT, MyAllSensors[3].MyListRecordsForOneKKS[i].Value);
            }
        }

        private void добавитьНаВспомательнуюОсьYОтВремениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NextAxis(chart1.Series[NumberSeries].Name);
            chart1.ChartAreas[0].AxisY2.MinorTickMark.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MinorTickMark.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas[0].AxisY2.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MinorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas[0].AxisY2.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY2.MinorTickMark.LineColor = Color.LightGray;

            chart1.ChartAreas[0].AxisY2.ArrowStyle = AxisArrowStyle.Triangle;

            NumberSeries++;
        }

        public List<System.Windows.Forms.DataVisualization.Charting.ChartArea> CollectionAxis = new List<System.Windows.Forms.DataVisualization.Charting.ChartArea>();
        public System.Windows.Forms.DataVisualization.Charting.ChartArea areaAxis;
        public System.Windows.Forms.DataVisualization.Charting.ChartArea areaSeries;
        public System.Windows.Forms.DataVisualization.Charting.Chart chart;
        public System.Windows.Forms.DataVisualization.Charting.ChartArea area;
        public void CreateYAxis(System.Windows.Forms.DataVisualization.Charting.Chart chart, System.Windows.Forms.DataVisualization.Charting.ChartArea area, System.Windows.Forms.DataVisualization.Charting.Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);

            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;

            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());
            CollectionAxis.Add(areaAxis);

            // Create a copy of specified series
            System.Windows.Forms.DataVisualization.Charting.Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;

            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // Disable drid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.TitleFont = new System.Drawing.Font("Times New Roman", 14, FontStyle.Regular);
            areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;
            areaAxis.AxisY.IsLabelAutoFit = true;
            areaAxis.AxisX.IsLabelAutoFit = true;

            // Adjust area position
            chart.ChartAreas[0].Position.Auto = false;

            areaAxis.Position.X -= labelsSize - 50;
            areaAxis.InnerPlotPosition.X += labelsSize + 50;
        }


        private void добавитьНаДополнительнуюОсьYОтВремениToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void вклДополнительныеОсиToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void ljfpoefodjcfToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sdsadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void asdadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
        int indexAKR1;
        int indexAKR2;
        int indexAKR3;
        int indexAKR4;
        int indexAKR5;
        int indexAKR6;
        int indexAKR7;
        int indexAKR8;

        List<string> MyAKR = new List<string>();
        double OtnosOtklonenie;
        double Pogreshnost;
        double AverageAKR;
        private void найтиРеактивностиПоТокуИПостроитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyAKR.Add(MyAllSensors[192].KKS_Name);
            MyAKR.Add(MyAllSensors[193].KKS_Name);
            MyAKR.Add(MyAllSensors[194].KKS_Name);
            MyAKR.Add(MyAllSensors[195].KKS_Name);
            MyAKR.Add(MyAllSensors[196].KKS_Name);
            MyAKR.Add(MyAllSensors[197].KKS_Name);
            MyAKR.Add(MyAllSensors[198].KKS_Name);
            MyAKR.Add(MyAllSensors[200].KKS_Name);

            for (int j = 0; j < MyAllSensors.Count; j++)
            {
                if (MyAllSensors[j].KKS_Name == MyAKR[0])
                {
                    indexAKR1 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[1])
                {
                    indexAKR2 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[2])
                {
                    indexAKR3 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[3])
                {
                    indexAKR4 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[4])
                {
                    indexAKR5 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[5])
                {
                    indexAKR6 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[6])
                {
                    indexAKR7 = j;
                }
                if (MyAllSensors[j].KKS_Name == MyAKR[7])
                {
                    indexAKR8 = j;
                }
            }//192
           
            for (int j = 0; j < MyAllSensors.Count; j++)
            {
                if (checkedListBox1.Text == MyAllSensors[j].KKS_Name)
                {
                    IndexTok = j;
                }
            }

            Reactivity MyR = new Reactivity();
     
            MyR.PSI0(MyAllSensors[IndexTok].MyListRecordsForOneKKS[0].Value);
            for (int i = 1; i < MyAllSensors[IndexTok].MyListRecordsForOneKKS.Count; i++)
            {
                double R1 = MyR.SearchR(MyAllSensors[IndexTok].MyListRecordsForOneKKS[i].ValueTimeForDAT, MyAllSensors[IndexTok].MyListRecordsForOneKKS[i - 1].ValueTimeForDAT, MyAllSensors[IndexTok].MyListRecordsForOneKKS[i].Value, MyAllSensors[IndexTok].MyListRecordsForOneKKS[i - 1].Value, Const.l_metodiki, Const.aAPIK);
                chart1.Series[NumberSeries].Points.AddXY(MyAllSensors[indexTOK1].MyListRecordsForOneKKS[i].ValueTimeForDAT, R1);
            }
        }
        int IndexTok;
        private void индексКакойToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_2(object sender, EventArgs e)
        {
               
        }

        private void индексToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < MyAllSensors.Count; i++)
            {
                if(checkedListBox1.Text == MyAllSensors[i].KKS_Name)
                {
                    MessageBox.Show(i.ToString());
                }
            }
        }

        private void расчитатьРеактивностьИПостроитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Current Y = new Current();
            List<double> MyListReactivity = new List<double>();
            int indexTok = Sencors.getOneKKSByIndex(checkedListBox1.Text, MyAllSensors);
            Y.AddData(MyAllSensors[indexTok], MyListReactivity);
            for (int i = 0; i < MyAllSensors[indexTok].MyListRecordsForOneKKS.Count - 1; i++)
            {
                chart1.Series[NumberSeries].Points.AddXY(MyAllSensors[indexTok].MyListRecordsForOneKKS[i].ValueTimeForDAT, MyListReactivity[i]);
            }

            chart1.Series[NumberSeries].IsVisibleInLegend = true;
            chart1.Series[NumberSeries].LegendText = MyAllSensors[indexTok].KKS_Name;
            chart1.Series[NumberSeries].YAxisType = AxisType.Secondary;
          
            chart1.ChartAreas[0].AxisY2.MinorTickMark.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MinorTickMark.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MinorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas[0].AxisY2.MinorTickMark.LineColor = Color.LightGray;

            chart1.ChartAreas[0].AxisY2.MajorTickMark.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MajorTickMark.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas[0].AxisY2.MajorTickMark.LineColor = Color.Gray;
       
            NumberSeries++;
        }
    }
}

