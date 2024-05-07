using System;
using System.IO;
using System.Windows.Forms;
using Project;
using MyExtensions;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Trees
{
    public partial class Form1 : Form
    {

        private RPN rpnConverter;
        private Engine engine;
        MyDictionary<string, string> expressions;
        MyDictionary<string, TreeNode> expressionsWithNode;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(RPN rpnConverter, Engine engine) : base()
        {
            this.rpnConverter = rpnConverter;
            this.engine = engine;

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
                    expressionsWithNode.Add(expression, root);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
            }

            comboBox1_SelectedIndexChanged_1(sender, e);

        }

       

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            // Изчистване на предишните опции
            comboBox1.Items.Clear();

            // Добавяне на опции в падащото меню
            foreach (var expression in expressions.Keys)
            {
                comboBox1.Items.Add(expression);
            }

            // Ако искате инициално да изберете първата опция
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }
    }
}