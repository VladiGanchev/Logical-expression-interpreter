namespace Project;

public class RPN
{
    public int GetPrecedence(char op)
    {
        switch (op)
        {
            case '|':
                return 1;
            case '&':
                return 2;
            case '!':
                return 3;
            default:
                return 0;
        }
    }

    public bool IsOperator(char c)
    {
        return c == '|' || c == '&' || c == '!';
    }

    public string InfixToRPN(string infixExpression)
    {
        MyStack<char> operatorStack = new MyStack<char>();
        MyList<char> output = new MyList<char>();

        foreach (char token in infixExpression)
        {
            if (Char.IsLetterOrDigit(token))
            {
                output.Add(token);
            }
            else if (token == '(')
            {
                operatorStack.Push(token);
            }
            else if (token == ')')
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Pop(); 
            }
            else if (IsOperator(token))
            {
                while (operatorStack.Count > 0 && IsOperator(operatorStack.Peek()) && GetPrecedence(operatorStack.Peek()) >= GetPrecedence(token))
                {
                    output.Add(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
            else if (token == '!')
            {
                operatorStack.Push(token);
            }
        }

        while (operatorStack.Count > 0)
        {
            output.Add(operatorStack.Pop());
        }

        return new string(output.ToArray());
    } 

    public TreeNode FindRoot(string rpnExpression)
    {
        MyStack<TreeNode> operandStack = new MyStack<TreeNode>();

        foreach (char token in rpnExpression)
        {
            if (Char.IsLetterOrDigit(token))
            {
                operandStack.Push(new TreeNode(token.ToString()));
            }
            else if (IsOperator(token))
            {
                if (token == '!')
                {
                    if (operandStack.Count < 1)
                    {
                        Console.WriteLine("Error: Not enough operands for operator " + token);
                        return null;  
                    }

                    TreeNode operand = operandStack.Pop();
                    TreeNode operatorNode = new TreeNode(token.ToString());
                    operatorNode.Left = operand;

                    operandStack.Push(operatorNode);
                }
                else
                {
                    if (operandStack.Count < 2)
                    {
                        Console.WriteLine("Error: Not enough operands for operator " + token);
                        return null;  
                    }

                    TreeNode rightOperand = operandStack.Pop();
                    TreeNode leftOperand = operandStack.Pop();

                    TreeNode operatorNode = new TreeNode(token.ToString());
                    operatorNode.Left = leftOperand;
                    operatorNode.Right = rightOperand;

                    operandStack.Push(operatorNode);
                }
            }
        }

        if (operandStack.Count != 1)
        {
            Console.WriteLine("Error: Operand stack should have exactly one element at the end.");
            return null;  
        }

        return operandStack.Pop();
    }
}