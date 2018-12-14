using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Скачивание
{
    public partial class Form1 : Form
    {
        public int u;
        public int n;
        public int star;
        public int kol;
        BackgroundWorker[] BW;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            kol = 0;
            star = Convert.ToInt32(textBox1.Text);

            string path = Application.StartupPath + "\\Загрузки\\";
            Directory.CreateDirectory(path);
            string remoteUri = "http://ihtik.lib.ru/2011.07_ihtik_hudlit-ru/";

            u = 0;
            n = 0;
            //73335

            BW = new System.ComponentModel.BackgroundWorker[73335 - star];
            for (int i = Convert.ToInt32(textBox1.Text); i <= 73335; i++)
            {
                int flag = 0;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "Form1")
                        flag = 1;
                }
                if (flag == 0)
                    break;
                while (kol > 0)
                    Application.DoEvents();

                string fileName = "2011.07_ihtik_hudlit-ru_" + i + ".rar";
                BW[i - star] = new BackgroundWorker();
                BW[i - star].DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
                BW[i - star].RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);
                Downloading dd = new Downloading(remoteUri, fileName);
                BW[i - star].RunWorkerAsync(dd);
                kol++;
            }
            textBox1.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label5.Text = "Будет скачано " +
                  ((73335 - Convert.ToInt32(textBox1.Text) > 0) ?
                  (73335 - Convert.ToInt32(textBox1.Text) + 1) : 0) +
                    " файлов";
            }
        }

        public class Downloading
        {
            public string dpath;
            public string ad;
            public string file;
            public int u;

            public Downloading(string ad, string file)
            {
                this.dpath = Application.StartupPath + "\\Загрузки\\" + file;
                this.ad = ad + file;
                this.file = file;
            }
            public int SD()
            {
                try
                {
                    WebClient myWebClient = new WebClient();
                    myWebClient.DownloadFile(ad, dpath);
                    u++;
                }
                catch (WebException ex)
                {
                    u--;
                }
                return u;
            }
        }
        public struct resul
        {
            public int u;
            public string file;
        }
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Downloading D = (Downloading)e.Argument;
            resul re = new resul();
            re.u = D.SD();
            re.file = D.file;
            e.Result = re;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            resul A = (resul)e.Result;
            if (A.u > 0)
            {
                u++;
                textBox2.Text = u + "(" + Math.Round((double)u / ((double)73335 - (double)star), 3) + "%)";
                listBox1.Items.Add(A.file + "....Упех.");
            }
            else
            {
                n++;
                textBox3.Text = n + "(" + Math.Round((double)n / ((double)73335 - (double)star), 3) + "%)";
                listBox1.Items.Add(A.file + "....ОШИБКА.");
            }
            kol--;
        }
    }
}
