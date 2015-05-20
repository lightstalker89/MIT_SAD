Start application --> HotSwapping.exe
(.Net 4.0)

Examples for the two textBoxes if you want to test HotSwapping:

1.

MessageBox.Show("I'm a new text");

2.

int a = 2;
int b = 3;
int result = a + b;

Console.WriteLine("Result is: " + result);
MessageBox.Show("Result is: " + result);

3.

// Create new instance of Form1 window and show it
Form1 test = new Form1();
test.Show();
