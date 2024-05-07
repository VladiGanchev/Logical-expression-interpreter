using Microsoft.VisualBasic;
using Project;
using System.Linq.Expressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        MyDictionary<string, TreeNode> expressionsWithNode;
        MyDictionary<string, string> expressions;
        RPN rpnConverter;


        public Form1()
        {
            InitializeComponent();
            rpnConverter = new RPN();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            expressionsWithNode = new MyDictionary<string, TreeNode>();
            expressions = new MyDictionary<string, string>();

            try
            {
                string filePath = @"C:\Users\user\source\repos\Project\Project\bin\Debug\net6.0\commandsForInterface.txt";

                string[] lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    string[] data = line.Split(" ");
                    string name = data[0];
                    string expression = data[1];
                    expressions.Add(name, expression);

                    string rpnExpression = rpnConverter.InfixToRPN(expression);
                    TreeNode root = rpnConverter.FindRoot(rpnExpression);
                    expressionsWithNode.Add(name, root);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
            }

            SetUpComboBox();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();

            // Получаване на избраният елемент от ComboBox
            string selectedExpression = comboBox1.SelectedItem as string;

            // Проверка дали елементът не е null
            if (selectedExpression != null)
            {
                // Получаване на корена на дървото за избрания израз
                TreeNode root = expressionsWithNode[selectedExpression];

                // Извикване на метод, който ще нарисува дървото
                PopulateTreeView(root);
            }

        }

        private void SetUpComboBox()
        {
            comboBox1.Items.Clear();

            // Добавяне на опции в падащото меню
            foreach (var expression in expressions.Keys)
            {
                comboBox1.Items.Add(expression);
            }

        }

        private void PopulateTreeView(TreeNode root)
        {
            if (root == null)
            {
                return;
            }



            // Create a new TreeNode in the TreeView for the current node
            TreeNode treeNode = new TreeNode(root.Name);

            // Add child nodes to the TreeView directly
            foreach (TreeNode child in root.Children)
            {
                // Save the data for the current node
                string nodeName = root.Name;
                string childrenData = "";

                foreach (TreeNode childNode in root.Children)
                {
                    childrenData += childNode.Name + " ";
                }

                // Write the data for the current node 
                Console.WriteLine($"{nodeName} - {childrenData}");

                PopulateTreeView(child);
                treeView1.Nodes[treeView1.Nodes.Count - 1].Nodes.Add(new TreeNode(child.Name).ToString());
            }

            // Add the current TreeNode to the TreeView
            treeView1.Nodes.Add(treeNode.Name.ToString());
        }

    }
}