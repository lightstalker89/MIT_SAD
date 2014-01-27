using System;

namespace TreeII
{
    public class CustomTree
    {
        public CustomTree(string word)
        {
            this.word = word;
            this.count = 1;
        }

        public CustomTree(CustomTree ct)
        {
            word = null;
            left = null;
            right = null;
            balance = 0;
            height = 0;
            count = 0;
        }

        CustomTree left;
        CustomTree right;

        int count;
        int balance;
        int height;

        string word;

        public void SearchOrInsert(string InputWord)
        {
            if (this.word == string.Empty)
            {
                this.word = InputWord;
                return;
            }
            else
            {
                if (this.word.CompareTo(InputWord) == 0)
                {
                    this.count++;
                    return;
                }
                else if (this.word.CompareTo(InputWord) < 0)
                {
                    if (this.right == null)
                    {
                        this.right = new CustomTree(InputWord);
                    }
                    else
                    {
                        this.right.SearchOrInsert(InputWord);
                    }
                    return;
                }
                else if (this.word.CompareTo(InputWord) > 0)
                {
                    if (this.left == null)
                    {
                        this.left = new CustomTree(InputWord);
                    }
                    else
                    {
                        this.left.SearchOrInsert(InputWord);
                    }
                    return;
                }
            }
        }

        public void AVLSort(string key, ref CustomTree dummytree)
        {
            //A1
            CustomTree T = dummytree; //always points to the father of S
            CustomTree S = dummytree.right; //point to the place where rebalancing may be necessary
            CustomTree P = dummytree.right; //move down the tree
            CustomTree Q = new CustomTree(string.Empty); //used in A3/A4
            CustomTree R;

            while (true)
            {
                //A2
                if (key.CompareTo(P.word) < 0)
                {
                    //go to A3
                    Q = P.left;
                    if (Q == null)
                    {
                        Q = new CustomTree(key);
                        P.left = Q;
                        break;
                    }
                    if (Q.balance != 0)
                    {
                        T = P;
                        S = Q;
                        P = Q;
                        continue;
                    }

                }
                else if (key.CompareTo(P.word) > 0)
                {
                    //go to A4
                    Q = P.right;
                    if (Q == null)
                    {
                        Q = new CustomTree(key);
                        P.right = Q;
                        break;
                    }
                    if (Q.balance != 0)
                    {
                        T = P;
                        S = Q;
                        P = Q;
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
            //A5 nicht nötig dank Construktor!
            //A6
            if (key.CompareTo(S.word) < 0)
            {
                R = P = S.left;
            }
            else
            {
                R = P = S.right;
            }

            //A6
            while (P != Q)
            {
                if (key.CompareTo(P.word) < 0)
                {
                    P.balance = -1;
                    P = P.left;
                }
                else if (key.CompareTo(P.word) > 0)
                {
                    P.balance = 1;
                    P = P.right;
                }
                else
                {
                    break;
                }
            }

            //A7
            int alpha;

            if (key.CompareTo(S.word) < 0)
            {
                alpha = -1;
            }
            else
            {
                alpha = 1;
            }

            //A7
            if (S.balance == 0)
            {
                S.balance = alpha;
                dummytree.right.height = dummytree.right.height + 1;
                return;
            }
            if (S.balance == -alpha)
            {
                S.balance = 0;
                return;
            }
            if (S.balance == alpha)
            {

                if (R.balance == alpha)
                {
                    //A8
                    P = R;
                    if (alpha < 0)
                    {
                        S.left = -alpha < 0 ? R.left : R.right;
                    }
                    else
                    {
                        S.right = -alpha < 0 ? R.left : R.right;
                    }

                    if (-alpha < 0)
                    {
                        R.left = S;
                    }
                    else
                    {
                        R.right = S;
                    }

                    S.balance = R.balance = 0;
                }
                else if (R.balance == -alpha)
                {
                    //A9
                    if (P.balance == alpha)
                    {
                        S.balance = -alpha;
                        R.balance = 0;
                    }
                    else if (P.balance == 0)
                    {
                        S.balance = R.balance = 0;
                    }
                    else
                    {
                        S.balance = 0;
                        R.balance = alpha;
                    }

                    P.balance = 0;
                }
            }

            //A10
            if (S == T.right)
            {
                T.right = P;
            }
            else
            {
                T.left = P;
            }
        }

        public void Print(bool dotFile)
        {
            if (dotFile)
            {
                Console.WriteLine(this.word + " [ label=\"{<w>" + this.word + "|<l>}|{<h>1|<r>}\" ]");

                if (this.left != null)
                {
                    Console.WriteLine(this.word + ":r -> " + this.left.word + ";");
                    this.left.Print(true);                   
                }

                if (this.right != null)
                {
                    Console.WriteLine(this.word + ":l -> " + this.right.word + ";");
                    this.right.Print(true);
                }
            }
            else
            {
                if (this.left != null)
                {
                    this.left.Print(dotFile);
                }

                Console.WriteLine("{0} {1}", this.word, this.count);

                if (this.right != null)
                {
                    this.right.Print(dotFile);
                }
            }
        }
    }
}
