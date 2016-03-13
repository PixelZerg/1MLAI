using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1MLVisual
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private void UI_Load(object sender, EventArgs e)
        {
            Console.SetOut(new ListBoxWriter(listBox1));
        }

        private void UI_Resize(object sender, EventArgs e)
        {
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.ProcessName == "cmd")
                {
                    MoveWindow(proc.Handle, 10, 10, 10, 10, true);
                        break;
                }
            }
        }
        public void UpdateTree(BrainNet.NeuralFramework.INeuralNetwork n)
        {
            // treeView1.Nodes.Clear();
            //  string json = Newtonsoft.Json.JsonConvert.SerializeObject(n);
            // TreeNode parent = Json2Tree(JObject.Parse(json));
            // parent.Text = "root";
            //// treeView1.Nodes.Add(parent);
            //listBox2.Items.Clear();
            //foreach (BrainNet.NeuralFramework.Neuron neuron in n.OutputLayer)
            //{
            //    listBox2.Items.Add(Newtonsoft.Json.JsonConvert.SerializeObject(neuron));   
            //}
        }

        private TreeNode Json2Tree(JObject obj)
        {
            //create the parent node
            TreeNode parent = new TreeNode();
            //loop through the obj. all token should be pair<key, value>
            foreach (var token in obj)
            {
                //change the display Content of the parent
                parent.Text = token.Key.ToString();
                //create the child node
                TreeNode child = new TreeNode();
                child.Text = token.Key.ToString();
                //check if the value is of type obj recall the method
                if (token.Value.Type.ToString() == "Object")
                {
                    // child.Text = token.Key.ToString();
                    //create a new JObject using the the Token.value
                    JObject o = (JObject)token.Value;
                    //recall the method
                    child = Json2Tree(o);
                    //add the child to the parentNode
                    parent.Nodes.Add(child);
                }
                //if type is of array
                else if (token.Value.Type.ToString() == "Array")
                {
                    int ix = -1;
                    //  child.Text = token.Key.ToString();
                    //loop though the array
                    foreach (var itm in token.Value)
                    {
                        //check if value is an Array of objects
                        if (itm.Type.ToString() == "Object")
                        {
                            TreeNode objTN = new TreeNode();
                            //child.Text = token.Key.ToString();
                            //call back the method
                            ix++;

                            JObject o = (JObject)itm;
                            objTN = Json2Tree(o);
                            objTN.Text = token.Key.ToString() + "[" + ix + "]";
                            child.Nodes.Add(objTN);
                            //parent.Nodes.Add(child);
                        }
                        //regular array string, int, etc
                        else if (itm.Type.ToString() == "Array")
                        {
                            ix++;
                            TreeNode dataArray = new TreeNode();
                            foreach (var data in itm)
                            {
                                dataArray.Text = token.Key.ToString() + "[" + ix + "]";
                                dataArray.Nodes.Add(data.ToString());
                            }
                            child.Nodes.Add(dataArray);
                        }

                        else
                        {
                            child.Nodes.Add(itm.ToString());
                        }
                    }
                    parent.Nodes.Add(child);
                }
                else
                {
                    //if token.Value is not nested
                    // child.Text = token.Key.ToString();
                    //change the value into N/A if value == null or an empty string 
                    if (token.Value.ToString() == "")
                        child.Nodes.Add("N/A");
                    else
                        child.Nodes.Add(token.Value.ToString());
                    parent.Nodes.Add(child);
                }
            }
            return parent;

        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  new ExportDialog(Newtonsoft.Json.JsonConvert.SerializeObject(Program.network)).ShowDialog();
        }
    }
}
