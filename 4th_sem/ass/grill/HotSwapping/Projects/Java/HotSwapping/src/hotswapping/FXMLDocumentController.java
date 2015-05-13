/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package hotswapping;

import com.sun.jdi.connect.IllegalConnectorArgumentsException;
import hotswapping.swappingClass.AddCalculator;
import hotswapping.swappingClass.SubstractCalculator;
import hotswapping.swappingClass.SwappingClass;
import java.io.IOException;
import java.net.URL;
import java.util.ResourceBundle;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Label;
import javafx.scene.control.TextArea;
import javafx.scene.control.TextField;
import javassist.CannotCompileException;
import javassist.ClassPool;
import javassist.CtClass;
import javassist.CtMethod;
import javassist.NotFoundException;
import javassist.util.HotSwapper;
import net.bytebuddy.ByteBuddy;
import net.bytebuddy.dynamic.loading.ClassReloadingStrategy;

/**
 *
 * @author Flo
 */
public class FXMLDocumentController implements Initializable {
    
    // JavaAssist
    private SwappingClass swappingClass;
    private HotSwapper swap;
    
    // Byte Buddy
    private boolean toggleSwapping;
    
    @FXML
    private Label javassistSwappingError;
    @FXML
    private Label javassistError;
    @FXML
    private Label calcSign;
    @FXML
    private Label result;
    @FXML
    private Label usedClassInfo;
    @FXML
    private TextField field1;
    @FXML
    private TextField field2;
    @FXML
    private TextArea javassistTextArea; 
    
    public FXMLDocumentController(){
        this.swappingClass = new SwappingClass();
        try {
            this.swap = new HotSwapper("8000");
        } catch (IOException ex) {
            Logger.getLogger(FXMLDocumentController.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IllegalConnectorArgumentsException ex) {
            Logger.getLogger(FXMLDocumentController.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    @Override
    public void initialize(URL url, ResourceBundle rb) {
        field1.textProperty().addListener(field1ChangeListener);
        field2.textProperty().addListener(field2ChangeListener);
    }
    
    @FXML
    private void handleJavassistButton(ActionEvent event) throws IllegalConnectorArgumentsException {
        javassistError.setVisible(false);
        javassistSwappingError.setVisible(false);
        if(!javassistTextArea.getText().equals("")){
            if(this.swap != null){
                ClassPool pool = ClassPool.getDefault();
                try {

                    CtClass target = pool.get("hotswapping.swappingClass.SwappingClass");
                    target.defrost();
                    CtMethod targetMethod = target.getDeclaredMethod("swappingMethod");
                    targetMethod.setBody("{" + javassistTextArea.getText() + "}");
                    swap.reload("hotswapping.swappingClass.SwappingClass", target.toBytecode());
                } catch (CannotCompileException ex) {
                    javassistSwappingError.setVisible(true);
                } catch (NotFoundException ex) {
                    javassistSwappingError.setVisible(true);
                } catch (SecurityException ex) {
                    javassistSwappingError.setVisible(true);
                } catch (IllegalArgumentException ex) {
                    javassistSwappingError.setVisible(true);
                } catch (IOException ex) {
                    javassistSwappingError.setVisible(true);
                }
            } else{
                System.out.println("Error");
                javassistSwappingError.setVisible(true);
            }
        } else{
            javassistError.setVisible(true);
        }
    }
    
    @FXML
    private void handleByteBuddyButton(ActionEvent event) {
        if(toggleSwapping){
            calcSign.setText("+");
            usedClassInfo.setText("Use AddCalculator class");
            new ByteBuddy()
                .redefine(AddCalculator.class)
                .name(AddCalculator.class.getName())
                .make()
                .load(AddCalculator.class.getClassLoader(), ClassReloadingStrategy.fromInstalledAgent());
        } else{
            calcSign.setText("-");
            usedClassInfo.setText("Use SubstractCalculator class");
            new ByteBuddy()
                .redefine(SubstractCalculator.class)
                .name(AddCalculator.class.getName())
                .make()
                .load(AddCalculator.class.getClassLoader(), ClassReloadingStrategy.fromInstalledAgent());
        }
        toggleSwapping = !toggleSwapping;
    }
    
    @FXML
    private void executeCodeJavaAssist(ActionEvent event) {
        swappingClass.swappingMethod();
    }
    
    @FXML
    private void executeCodeByteBuddy(ActionEvent event) {
        if(!field1.getText().equals("") && !field2.getText().equals("")){
            int a = Integer.parseInt(field1.getText());
            int b = Integer.parseInt(field2.getText());
            int value = new AddCalculator().calc(a, b);
            result.setText(Integer.toString(value));
        }
    }
    
    ChangeListener<String> field1ChangeListener = new ChangeListener<String>() {
        @Override
        public void changed(ObservableValue<? extends String> observable, String oldValue, String newValue) {
            if (!newValue.matches("\\d*")) {
                field1.setText(oldValue);
            }
        }
    };
    
    ChangeListener<String> field2ChangeListener = new ChangeListener<String>() {
        @Override
        public void changed(ObservableValue<? extends String> observable, String oldValue, String newValue) {
            if (!newValue.matches("\\d*")) {
                field2.setText(oldValue);
            }
        }
    };
    
}
