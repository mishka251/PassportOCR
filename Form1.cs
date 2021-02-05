using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace ComputerVision
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dr = this.openFileDialog1.ShowDialog();
            if (dr == DialogResult.Yes || dr == DialogResult.OK)
            {
                //var image = new Bitmap(openFileDialog1.FileName);
                var img = Pix.LoadFromFile(openFileDialog1.FileName);
                label1.Text = "";
                var engine = new TesseractEngine(@"F:\repos\ComputerVision\tessdata", "rus", EngineMode.Default);
                using (var page = engine.Process(img))
                {
                    var text = page.GetText();
                    label1.Text+=("Mean confidence: {0}", page.GetMeanConfidence())+"\r\n";

                    label1.Text+=("Text (GetText): \r\n{0}", text);
                    label1.Text+=("Text (iterator):");
                    using (var iter = page.GetIterator())
                    {
                        iter.Begin();

                        do
                        {
                            do
                            {
                                do
                                {
                                    do
                                    {
                                        if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                        {
                                            label1.Text+=("<BLOCK>");
                                        }

                                        label1.Text+=(iter.GetText(PageIteratorLevel.Word));
                                        label1.Text+=(" ");

                                        if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                        {
                                            label1.Text+=("\r\n");
                                        }
                                    } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                    if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                    {
                                        label1.Text+=("\r\n");
                                    }
                                } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                            } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                        } while (iter.Next(PageIteratorLevel.Block));
                    }
                }
            }
        }
    }
}