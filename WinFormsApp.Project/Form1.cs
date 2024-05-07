using Project;
namespace WinFormsApp.Project
{
    public partial class Form1 : Form
    {
        Engine engine = new Engine();
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach (var pair in engine.roots)
            {
                comboBoxTrees.Items.Add(pair.Key);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        //private void PopulateTreeView(TreeNodeCollection nodes, TreeNode treeNode)
        //{
        //    if (treeNode != null)
        //    {
        //        TreeNode newNode = new TreeNode(treeNode.Name);
        //        nodes.Add(newNode);
        //        PopulateTreeView(newNode.Nodes, treeNode.Left);
        //        PopulateTreeView(newNode.Nodes, treeNode.Right);
        //    }
        //}

        //private void UpdateTreeView()
        //{
        //    treeView1.Nodes.Clear(); // Изчистване на старите данни
        //    PopulateTreeView(treeView1.Nodes, engine.roots["тук поставете ключа на корена на дървото"]);
        //}
    }
}