Start application --> startApplication.bat
(Need Java 8)
Limitation: Require that both classes apply the same class schema before redefinition. There is no way to add new methods or new fields to a class when reloading.

Examples for the textBoxes if you want to test HotSwapping:

1.

javafx.scene.control.Alert alert = new javafx.scene.control.Alert(javafx.scene.control.Alert.AlertType.INFORMATION);
alert.setTitle("Hot swapping");
alert.setHeaderText("This was a new Dialog created by hot swapping");
alert.setContentText("I have swapped the byte code!");

alert.showAndWait();

2.

int a = 2;
int b = 3;
int result = a + b;

javafx.scene.control.Alert alert = new javafx.scene.control.Alert(javafx.scene.control.Alert.AlertType.INFORMATION);
alert.setTitle("Hot swapping");
alert.setHeaderText("This result of the calculation is: " + result );
alert.setContentText("I have swapped the byte code and add a calculation!");

alert.showAndWait();




