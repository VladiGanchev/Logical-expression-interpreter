public class TreeNode
{

    public string Name { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    public List<TreeNode> Children { get; set; }
    public TreeNode(string value)
    {
        Name = value;
        Left = null;
        Right = null;
        Children = new List<TreeNode>();
    }

    public void PrintTree(string indent = "")
    {
        Console.WriteLine(indent + Name);

        if (Left != null)
        {
            Left.PrintTree(indent + "  ");
        }

        if (Right != null)
        {
            Right.PrintTree(indent + "  ");
        }
    }

    public void PrintTreeIndented(TreeNode node, string indent = "")
    {
        if (node == null)
        {
            return;
        }

        Console.WriteLine(indent + node.Name);

        if (node.Left != null || node.Right != null)
        {
            PrintTreeIndented(node.Left, indent + "  ");
            PrintTreeIndented(node.Right, indent + "  ");
        }
    }
    public string[] GetFunctionParameters()
    {
        List<string> parameters = new List<string>();
        GetFunctionParametersHelper(this.Left, parameters);
        return parameters.ToArray();
    }

    private void GetFunctionParametersHelper(TreeNode node, List<string> parameters)
    {
        if (node == null)
        {
            return;
        }

        if (Char.IsLetterOrDigit(node.Name[0]))
        {
            // Възелът съдържа буква или цифра, следователно е параметър
            parameters.Add(node.Name);
        }

        // Рекурсивно извикване за ляво и за дясно поддърво
        GetFunctionParametersHelper(node.Left, parameters);
        GetFunctionParametersHelper(node.Right, parameters);
    }

    public void AddChild(TreeNode child)
    {
        Children.Add(child);
    }
}