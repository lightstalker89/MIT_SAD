/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package hotswapping.swappingClass;

import javafx.scene.control.Alert;
import javafx.scene.control.Alert.AlertType;

/**
 *
 * @author Flo
 */
public class SwappingClass {
    
    public void swappingMethod(){
        Alert alert = new Alert(AlertType.INFORMATION);
        alert.setTitle("Information Dialog");
        alert.setHeaderText("This was a Dialog from the standard code");
        alert.setContentText("Try to swapp some the byte code!");

        alert.showAndWait();
    }
}
