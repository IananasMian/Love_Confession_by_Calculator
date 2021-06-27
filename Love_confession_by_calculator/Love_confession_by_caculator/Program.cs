using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;

namespace love_confession_by_calculator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Process.Start("calc.exe");

            AutomationElement eRoot = AutomationElement.RootElement;

            Thread.Sleep(1000);

            FindChild_DescendantsElement_exp(eRoot);

            //-----------------------------------------------//

            AutomationElement first_child_under_root = Findchild_under_root(eRoot);

            Console.WriteLine($"\"{first_child_under_root.Current.Name}\" {first_child_under_root.Current.ControlType.ProgrammaticName}");

            //use list
            String[] Names = new string[17] {"Two", "Five", "Zero", "Multiply by", "Two", "Plus", "Three", "Eight", "Minus",
                                                "One", "Seven", "Decimal Separator", "Eight", "Six", "Eight", "Six", "Equals"};

            for (int i = 0; i < Names.Length; i++)
            {
                Findelement_and_caculate(first_child_under_root, Names[i], ControlType.Button, TreeScope.Descendants);
            }

            AutomationElement Answer = Result(first_child_under_root, "CalculatorResults", TreeScope.Descendants);

            Console.WriteLine("\nAnswer:");
            Console.WriteLine(Answer.Current.Name);

            while (true)
            {
            }
        }

        private static AutomationElement Findchild_under_root(AutomationElement rootElement)
        {
            AndCondition conditionName_ControlType = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Calculator"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));

            AutomationElement echild_under_root = rootElement.FindFirst(TreeScope.Children, conditionName_ControlType);

            return echild_under_root;
        }

        private static void Findelement_and_caculate(AutomationElement findElement, String Name, ControlType CTtype, TreeScope TSsearch)
        {
            AndCondition conditionName_ControlType = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, Name),
                new PropertyCondition(AutomationElement.ControlTypeProperty, CTtype));

            //items under number pad & standard operators
            AutomationElement ebranch_under_root = findElement.FindFirst(
            TSsearch, conditionName_ControlType);

            var clickpattern = (InvokePattern)ebranch_under_root.GetCurrentPattern(InvokePattern.Pattern);
            clickpattern.Invoke();
        }

        private static AutomationElement Result(AutomationElement findElement, String Automation_Id, TreeScope TSsearch)
        {
            Condition propCondition = new PropertyCondition(
                AutomationElement.AutomationIdProperty, Automation_Id, PropertyConditionFlags.IgnoreCase);

            AutomationElementCollection branch_under_numberpad = findElement.FindAll(
            TSsearch, propCondition);

            foreach (AutomationElement autoElement in branch_under_numberpad)
            {
                //Console.WriteLine(autoElement);
            }

            AutomationElement ebranch_under_root = findElement.FindFirst(
                TSsearch, propCondition);

            //Console.WriteLine(ebranch_under_root.Current.AutomationId);

            return ebranch_under_root;
        }

        //-------------------------------------------------------------------------------------//

        private static AutomationElement FindChild_DescendantsElement_exp(AutomationElement rootElement)
        {
            if (rootElement == null)
            {
                throw new ArgumentException("Argument cannot be null or empty.");
            }

            //AndCondition: Use Name & ControlType instead of AutomationId.
            AndCondition conditionName_ControlType = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Calculator"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));

            //find caculator as the child of root
            AutomationElement echild_under_root = rootElement.FindFirst(
                TreeScope.Children, conditionName_ControlType);

            Thread.Sleep(300);

            //find number pad
            AndCondition condition_numberpad = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Number pad"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group));

            AutomationElement find_numberpad = echild_under_root.FindFirst(
                TreeScope.Descendants, condition_numberpad);

            //find standard operators
            AndCondition condition_standardoperators = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Standard operators"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Group));

            AutomationElement find_standardoperators = echild_under_root.FindFirst(
                TreeScope.Descendants, condition_standardoperators);

            //numberpad findall buttons
            AndCondition buttons_under_numberpad = new AndCondition(
                new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

            AutomationElementCollection child_under_numberpad = find_numberpad.FindAll(
                TreeScope.Children, buttons_under_numberpad);

            Console.WriteLine("------------number pad:");

            foreach (AutomationElement autoElement in child_under_numberpad)
            {
                Console.WriteLine(autoElement.Current.Name);
            }

            //standard operators findall buttons
            AndCondition buttons_under_standardoperators = new AndCondition(
                new PropertyCondition(AutomationElement.IsEnabledProperty, true),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

            AutomationElementCollection child_under_standardoperators = find_standardoperators.FindAll(
                TreeScope.Children, buttons_under_standardoperators);

            Console.WriteLine("\n------------standard operators:");

            foreach (AutomationElement autoElement2 in child_under_standardoperators)
            {
                Console.WriteLine(autoElement2.Current.Name);
            }

            //----------------------------- calculation demonstration -----------------------------//

            //0
            AndCondition condition_button_zero = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "Zero"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

            AutomationElement find_number_zero = find_numberpad.FindFirst(
                TreeScope.Descendants, condition_button_zero);

            var click0 = (InvokePattern)find_number_zero.GetCurrentPattern(InvokePattern.Pattern);
            //click0.Invoke();

            //1
            AndCondition condition_button_one = new AndCondition(
                new PropertyCondition(AutomationElement.NameProperty, "One"),
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button));

            AutomationElement find_number_one = find_numberpad.FindFirst(
                TreeScope.Descendants, condition_button_one);

            var click1 = (InvokePattern)find_number_one.GetCurrentPattern(InvokePattern.Pattern);
            //click1.Invoke();

            return rootElement.FindFirst(TreeScope.Descendants, conditionName_ControlType);
        }
    }
}