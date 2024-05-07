using MyExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Engine : IEngine
    {
         public MyDictionary<string, TreeNode> roots = new();
        public void Run()
        {
            //Обратен полски запис
            RPN rpnConverter = new RPN();

            //запазвам
            MyDictionary<string, string> expressions = new MyDictionary<string, string>();
            MyList<string> names = new MyList<string>();
            MyDictionary<string, TreeNode> findData = new MyDictionary<string, TreeNode>();
            MyList<string> operators = new MyList<string>()
            {
                    "&&",
                    "||",
                    "|&",
                    "&|"
            };


            //изпълнявам 4те команди от документа - DEFINE, SOLVE, ALL, FIND, принтирам дърво на конзолата и чета данни от файл
            while (true)
            {

                string[] data = Console.ReadLine().MySplit(" ");

                if (data[0] == "End")
                {
                    break;
                }

                if (data[0] == "DEFINE")
                {
                    DEFINE(rpnConverter, expressions, roots, names, data);
                }
                else if (data[0] == "SOLVE")
                {
                    SOLVE(rpnConverter, expressions, roots, names, data);
                }
                else if (data[0] == "PRINT")
                {
                    PRINT(rpnConverter, expressions, data);
                }
                else if (data[0] == "ReadFromFile")
                {
                    string filePath = "commands.txt";
                    ReadFromFile(filePath, roots, expressions, rpnConverter, names);
                }
                else if (data[0] == "ALL")
                {
                    ALL(rpnConverter, expressions, roots, data);
                }
                else if (data[0] == "FIND")
                {
                    FIND(rpnConverter, operators);
                }
            }

        }

        public void DEFINE(RPN rpnConverter, MyDictionary<string, string> expressions, MyDictionary<string, TreeNode> roots, MyList<string> names, string[] data)
        {
            string funcName = GetName(data[1]);
            string expression = GetExpression(data[2]);

            bool contains = ExpressionContainsAnotherFuncs(expressions, expression);

            string fileName = "commandsForInterface.txt";

            //без вложен func
            if (!contains)
            {
                if (ChechParametersEquality(data))
                {
                    expressions.Add(funcName, expression);
                    string rpn = rpnConverter.InfixToRPN(expression);
                    TreeNode root = rpnConverter.FindRoot(rpn);
                    roots.Add(funcName, root);
                    names.Add(funcName);
                    using (StreamWriter writer = new StreamWriter(fileName, true))
                    {
                        writer.WriteLine($"{funcName} {expression}");
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input");
                }
            }
            //с вложен func
            else
            {
                foreach (var ex in expressions)
                {
                    string nestedFunction = ex.Key;
                    string nestedFunctionValue = ex.Value;

                    // Проверка дали вложената функция се среща в израза
                    if (expression.MyContains(nestedFunction))
                    {
                        // Заместване на вложената функция със съответната стойност на съществуващата в Dictionary
                        expression = expression.Replace(nestedFunction, $"({nestedFunctionValue})");
                    }
                }

                //проверка дали след заместването е останал някоЙ func => e имал невалидни параметри => е невалиден
                if (expression.MyContains("func"))
                {
                    Console.WriteLine("Wrong input");
                }
                else
                {

                    expressions.Add(funcName, expression);
                    string rpn = rpnConverter.InfixToRPN(expression);
                    TreeNode root = rpnConverter.FindRoot(rpn);
                    roots.Add(funcName, root);
                    names.Add(funcName);
                    using (StreamWriter writer = new StreamWriter(fileName, true))
                    {
                        writer.WriteLine($"{funcName} {expression}");
                    }
                }


            }
        }
        private  string GetName(string expression)
        {
            int idxParenthesis = expression.MyIndexOf(":");
            string name = expression.MySubstring(0, idxParenthesis);
            return name;
        }
        public  string GetExpression(string v)
        {
            string expression = v.MySubstring(1, v.Length - 2);
            return expression;
        }

        private  bool ExpressionContainsAnotherFuncs(MyDictionary<string, string> expressions, string expression)
        {
            foreach (var item in expressions)
            {
                if (expression.MyContains(item.Key))
                {
                    return true;
                }
            }

            return false;
        }
        private  bool ChechParametersEquality(string[] data)
        {
            string[] parameters = FindParameters(data[1]);
            string[] expressionParameters = FindParametersFromExpression(data[2]);

            if (parameters.Length != expressionParameters.Length)
            {
                return false;
            }

            foreach (var item in parameters)
            {
                if (!expressionParameters.MyContains(item))
                {
                    return false;
                }
            }

            return true;
        }
        private  string[] FindParameters(string v)
        {
            int parenthesis = v.MyIndexOf("(");
            int closingPar = v.MyIndexOf(")");
            string needed = v.MySubstring(parenthesis + 1, closingPar - parenthesis - 1);
            string[] paramss = needed.MySplit(",");
            return paramss;
        }
        private  string[] FindParametersFromExpression(string v)
        {
            MyList<char> paramss = new MyList<char>();
            foreach (char ch in v)
            {
                if ((int)ch >= 97 && (int)ch <= 122)
                {
                    paramss.Add(ch);
                }
            }

            string[] stringArray = new string[paramss.Count];

            for (int i = 0; i < paramss.Count; i++)
            {
                stringArray[i] = paramss[i].ToString();
            }

            return stringArray;
        }


        //Методи на SOLVE
        public void SOLVE(RPN rpnConverter, MyDictionary<string, string> expressions, MyDictionary<string, TreeNode> roots, MyList<string> names, string[] data)
        {
            string nameee = data[1].MySubstring(0, 5);

            // Прочети цялата команда, включително функцията и аргументите
            string fullCommand = data[1];
            string? addLaterKey = null;
            string? addLaterExpression = null;
            // Извлечи само аргументите
            int startIndex = fullCommand.MyIndexOf("(") + 1;
            int endIndex = fullCommand.MyIndexOf(")");
            string arguments = fullCommand.MySubstring(startIndex, endIndex - startIndex);

            // Сплитване на аргументите
            string[] values = GetValues(arguments);
            int idx = fullCommand.MyIndexOf("(");
            string name = fullCommand.MySubstring(0, idx);
            string funcName = GetNewName(name, names);

            foreach (var item in roots)
            {
                if (item.Key.Contains(nameee))
                {
                    addLaterKey = item.Key;
                    addLaterExpression = expressions[addLaterKey];

                }
            }

            if (roots.ContainsKey(funcName))
            {
                TreeNode root = roots[funcName];
                string result = SolveExpression(root, values);

                // Проверка за коректен резултат
                if (result.ToLower() == "true" || result.ToLower() == "false")
                {
                    Console.WriteLine($"Result: {result}");
                }
                else
                {
                    Console.WriteLine("Invalid result: " + result);
                }
            }
            else
            {
                Console.WriteLine("Undefined function: " + funcName);
            }

            roots.Remove(addLaterKey);
            string rpn = rpnConverter.InfixToRPN(addLaterExpression);
            TreeNode root2 = rpnConverter.FindRoot(rpn);
            roots.Add(addLaterKey, root2);
        }
        private  string[] GetValues(string v)
        {
            // Изтриваме непотребните символи и спейсове
            v = v.Replace("SOLVE", "").Replace("func1", "").Replace("(", "").Replace(")", "").Replace(":", "").Replace(" ", "");

            MyList<string> values = new MyList<string>();

            // Сплитваме останалата част по запетайки
            string[] valueStrings = v.MySplit(",");

            foreach (var value in valueStrings)
            {
                values.Add(value);
            }

            return values.ToArray();
        }
        private  string GetNewName(string fullCommand, MyList<string> names)
        {
            foreach (var item in names)
            {
                if (item.MyContains(fullCommand))
                {
                    return item;
                }
            }

            return null;
        }
        private  string SolveExpression(TreeNode node, string[] values)
        {
            if (node == null)
            {
                return "";
            }

            // Ако възелът съдържа буква (параметър), присвой му стойност
            if (Char.IsLetterOrDigit(node.Name[0]))
            {
                int index = node.Name[0] - 'a';
                if (index >= 0 && index < values.Length)
                {
                    // Преобразуване на "1" и "0" в "true" и "false" съответно
                    if (values[index] == "1")
                    {
                        node.Name = "true";
                    }
                    else if (values[index] == "0")
                    {
                        node.Name = "false";
                    }
                    else
                    {
                        node.Name = values[index];
                    }
                }
            }

            // Рекурсивно извикване за ляво и за дясно поддърво
            string leftResult = SolveExpression(node.Left, values);
            string rightResult = SolveExpression(node.Right, values);

            // Изчисли стойността на текущия възел и върни я
            if (Char.IsDigit(node.Name[0]) || node.Name.ToLower() == "true" || node.Name.ToLower() == "false")
            {
                return node.Name;
            }
            else if (node.Name == "&")
            {
                return (bool.Parse(leftResult) && bool.Parse(rightResult)).ToString();
            }
            else if (node.Name == "|")
            {
                return (bool.Parse(leftResult) || bool.Parse(rightResult)).ToString();
            }
            else if (node.Name == "!")
            {
                return (!bool.Parse(leftResult)).ToString();
            }
            else
            {
                return node.Name;
            }
        }


        //Методи на ALL
        public void ALL(RPN rpnConverter, MyDictionary<string, string> expressions, MyDictionary<string, TreeNode> roots, string[] data)
        {
            string funcName = GetNameForALL(data[1]);
            string expression = null;

            // Проверка дали има такава запазена функция в Dictionary
            if (expressions.ContainsKey(funcName))
            {
                // Намерете записа на функцията
                expression = expressions[funcName];

                // Намерете аргументите на функцията
                string[] parameters = FindParametersFromExpression(expression);

                // Намерете всички възможни комбинации от стойности за аргументите
                List<List<string>> allVariations = GenerateAllVariations(parameters);

                // Решете функцията за всеки ред в таблицата
                foreach (var variation in allVariations)
                {
                    // Използвайте реализираният метод за решаване на функция
                    string[] values = variation.ToArray();
                    string result = SolveExpression(roots[funcName], values);

                    // Принтирайте текущия ред в таблицата
                    Console.WriteLine($"{string.Join(" , ", values)} : {result}");

                    roots.Remove(funcName);
                    string rpn = rpnConverter.InfixToRPN(expression);
                    TreeNode root3 = rpnConverter.FindRoot(rpn);
                    roots.Add(funcName, root3);

                }
            }
            else
            {
                Console.WriteLine("Undefined function: " + funcName);
            }
        }//not okay list
        private  string GetNameForALL(string text)
        {
            string first = text.MySubstring(0, 5);
            int idx = text.MyIndexOf(":");
            string second = text.MySubstring(7, idx - 7);
            string full = $"{first}({second})";
            return full;
        }
        private  List<List<string>> GenerateAllVariations(string[] parameters)
        {
            List<List<string>> allVariations = new List<List<string>>();
            GenerateVariationsRecursive(parameters, new List<string>(), allVariations);
            return allVariations;
        }// not okay list
        private  void GenerateVariationsRecursive(string[] parameters, List<string> currentVariation, List<List<string>> allVariations)
        {
            if (currentVariation.Count == parameters.Length)
            {
                // Добавете текущата вариация към списъка
                allVariations.Add(new List<string>(currentVariation));
                return;
            }

            foreach (string value in new[] { "0", "1" })
            {
                // Добавете стойността към текущата вариация и рекурсивно генерирайте следващите стойности
                currentVariation.Add(value);
                GenerateVariationsRecursive(parameters, currentVariation, allVariations);
                currentVariation.RemoveAt(currentVariation.Count - 1);
            }
        }//not okay list



        //Методи на FIND
        public void FIND(RPN rpnConverter, MyList<string> operators)
        {
            string filePath = "combinations_with_results.txt";
            MyList<string> lines = ReadFileLines(filePath);
            string[] combinations =
            {
                    "&&",
                    "||",
                    "|&",
                    "&|"
                };
            string neededResult = null;

            foreach (var line in lines)
            {
                foreach (var combination in combinations)
                {
                    string[] checkExpression = GenerateCombination(line, combination);
                    string rpn = rpnConverter.InfixToRPN(checkExpression[0]);
                    TreeNode root = rpnConverter.FindRoot(rpn);

                    string[] args = line.Split(":");
                    string[] values = args[0].Split(",");
                    bool result = SolveExpression(root);
                    if (result == false)
                    {
                        neededResult = "0";
                    }
                    else
                    {
                        neededResult = "1";
                    }

                    if (neededResult != checkExpression[1])
                    {
                        operators.Remove(combination);
                    }
                }
            }

            string finalOperators = operators[0];
            Console.WriteLine($"a{finalOperators[0]}b{finalOperators[1]}c");
        }
         MyList<string> ReadFileLines(string filePath)
        {
            MyList<string> lines = new MyList<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Грешка при четене на файла: " + e.Message);
            }

            return lines;
        }
        private  string[] GenerateCombination(string line, string combination)
        {
            string[] parameters = line.MySplit(":");
            string[] neededParameters = parameters[0].MySplit(",");

            string needed = $"{neededParameters[0]}{combination[0]}{neededParameters[1]}{combination[1]}{neededParameters[2]}";
            string result = $"{parameters[1]}";

            return new[] { needed, result };
        }
        private  bool SolveExpression(TreeNode root)
        {
            if (root == null)
            {
                return false; // Връщаме стойност false при липса на дърво
            }

            // Ако възелът съдържа стойност (0 или 1), връщаме я
            if (root.Name == "0" || root.Name == "1")
            {
                return root.Name == "1";
            }

            // Рекурсивно извикване за ляво и за дясно поддърво
            bool leftResult = SolveExpression(root.Left);
            bool rightResult = SolveExpression(root.Right);

            // Изчисляване на стойността на текущия възел и връщане на резултата
            switch (root.Name)
            {
                case "&":
                    return leftResult && rightResult;
                case "|":
                    return leftResult || rightResult;
                case "!":
                    return !leftResult;
                default:
                    throw new ArgumentException("Невалиден оператор в дървото");
            }
        }



        //PRINT
        private void PRINT(RPN rpnConverter, MyDictionary<string, string> expressions, string[] data)
        {
            string func = data[1];

            //проверяваме дали има такава запазена функция в Dictionary
            if (expressions.ContainsKey(func))
            {
                string rpn = rpnConverter.InfixToRPN(expressions[func]);
                TreeNode root = rpnConverter.FindRoot(rpn);

                // Принтиране на структурата на дървото
                Console.WriteLine("Expression Tree:");
                root.PrintTree();
            }
            else
            {
                Console.WriteLine("Wrong input");
            }
        }


        //ReadFromFile
        private  void ReadFromFile(string filePath, MyDictionary<string, TreeNode> roots, MyDictionary<string, string> expressions, RPN rpnConverter, MyList<string> names)
        {
            try
            {
                // Прочитане на всички редове от файла
                string[] lines = File.ReadAllLines(filePath);

                // Обработка на всяка команда от файла
                foreach (var line in lines)
                {
                    ProcessCommand(line, roots, expressions, rpnConverter, names);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading from file: " + ex.Message);
            }
        }
        private  void ProcessCommand(string command, MyDictionary<string, TreeNode> roots, MyDictionary<string, string> expressions, RPN rpnConverter, MyList<string> names)
        {
            // Разделяне на командата на токени
            string[] data = command.MySplit(" ");

            if (data[0] == "DEFINE")
            {
                string funcName = GetName(data[1]);
                string expression = GetExpression(data[2]);

                bool contains = ExpressionContainsAnotherFuncs(expressions, expression);

                //без вложен func
                if (!contains)
                {
                    if (ChechParametersEquality(data))
                    {
                        expressions.Add(funcName, expression);
                        string rpn = rpnConverter.InfixToRPN(expression);
                        TreeNode root = rpnConverter.FindRoot(rpn);
                        roots.Add(funcName, root);
                        names.Add(funcName);
                    }
                    else
                    {
                        Console.WriteLine("Wrong input");
                    }
                }
                //с вложен func
                else
                {
                    foreach (var ex in expressions)
                    {
                        string nestedFunction = ex.Key;
                        string nestedFunctionValue = ex.Value;

                        // Проверка дали вложената функция се среща в израза
                        if (expression.MyContains(nestedFunction))
                        {
                            // Заместване на вложената функция със съответната стойност на съществуващата в Dictionary
                            expression = expression.Replace(nestedFunction, $"({nestedFunctionValue})");
                        }
                    }

                    //проверка дали след заместването е останал някоЙ func => e имал невалидни параметри => е невалиден
                    if (expression.MyContains("func"))
                    {
                        Console.WriteLine("Wrong input");
                    }
                    else
                    {
                        expressions.Add(funcName, expression);
                        string rpn = rpnConverter.InfixToRPN(expression);
                        TreeNode root = rpnConverter.FindRoot(rpn);
                        roots.Add(funcName, root);
                        names.Add(funcName);
                    }
                }
            }
            else if (data[0] == "SOLVE")
            {
                // Прочети цялата команда, включително функцията и аргументите
                string fullCommand = data[1];

                // Извлечи само аргументите
                int startIndex = fullCommand.MyIndexOf("(") + 1;
                int endIndex = fullCommand.MyIndexOf(")");
                string arguments = fullCommand.Substring(startIndex, endIndex - startIndex);

                // Сплитване на аргументите
                string[] values = GetValues(arguments);
                int idx = fullCommand.MyIndexOf("(");
                string name = fullCommand.MySubstring(0, idx);
                string funcName = GetNewName(name, names);

                if (roots.ContainsKey(funcName))
                {
                    TreeNode root = roots[funcName];
                    string result = SolveExpression(root, values);

                    // Проверка за коректен резултат
                    if (result.ToLower() == "true" || result.ToLower() == "false")
                    {
                        string fileName = "output.txt";

                        // Отваряне или създаване на файл за запис
                        using (StreamWriter writer = new StreamWriter(fileName, true))
                        {
                            // Записване на резултата във файла
                            writer.WriteLine($"Result for {funcName}: {result}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid result: " + result);
                    }
                }
                else
                {
                    Console.WriteLine("Undefined function: " + funcName);
                }
            }
            else if (data[0] == "End")
            {
                return;
            }
        }
    }
}

