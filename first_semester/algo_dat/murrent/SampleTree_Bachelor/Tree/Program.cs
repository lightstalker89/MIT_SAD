using System;

namespace TreeII
{
    class Program
    {
        private static CustomTree ctree = new CustomTree("");
        private static XGetopt getops = new XGetopt();

        private static bool hFlag;
        private static bool sFlag;
        private static bool bFlag;
        private static bool wFlag;

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                char c;

                while ((c = getops.Getopt(args.Length, args, "bsh")) != '\0')
                {
                    switch (c)
                    {
                        case 'h':
                            hFlag = true;
                            break;

                        case 's':
                            sFlag = true;
                            break;

                        case 'b':
                            bFlag = true;
                            break;

                        default:
                            wFlag = true;
                            break;
                    }
                }

                if (wFlag)
                {
                    Console.WriteLine("Tree: Wrong or unknown parameter: try -h for more information");
                    System.Environment.Exit(64);
                }
                else if (hFlag)
                {
                    Help();
                }
                else if (bFlag || sFlag)
                {
                    if (sFlag)
                    {
                        SplitIntoWords(Console.In.ReadToEnd(), false);
                    }

                    if (bFlag)
                    {
                        Console.WriteLine("digraph \n{");
                        Console.WriteLine("node [shape=record];");
                        Console.WriteLine("D [ label=\"{<w>Wort|<l>Links}|{<h>Haeufigkeit|<r>Rechts}\" ]");
                        Console.WriteLine();
                        SplitIntoWords(Console.In.ReadToEnd(), true);
                        Console.WriteLine("}");
                    }
                }
            }
            else
            {
                SplitIntoWords(Console.In.ReadToEnd(), false);
                System.Environment.Exit(0);
            }           
        }

        private static void SplitIntoWords(string userinput, bool dotfile)
        {
            string[] words = userinput.Split(new char[] {' ','\n','\r',',','.'}, StringSplitOptions.RemoveEmptyEntries);

            GoThroughArray(words, dotfile);
        }

        private static void GoThroughArray(string[] words, bool dotfile)
        {
            CustomTree cdhead = new CustomTree(string.Empty);
            CustomTree cdtree = new CustomTree(cdhead);

            for (int i = 0; i < words.Length; i++)
            {
                cdtree.AVLSort(words[i].Trim(), ref cdtree);
            }

            cdtree.Print(dotfile);
        }

        private static void Help()
        {
            Console.WriteLine("---- Help ----");
            Console.WriteLine("-b: save the tree as .dot file");
            Console.WriteLine("-s: prints the tree sorted");
            Console.WriteLine("-h: shows the help");
            Console.WriteLine();
            Console.WriteLine("Usage: tree [-b | -s | -h] < [file]");

        }
    }
}
