/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package hotswappingbytebuddy;

import hotswappingbytebuddy.classes.ConsoleWriter;
import hotswappingbytebuddy.classes.DocumentWriter;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import net.bytebuddy.ByteBuddy;
import net.bytebuddy.agent.ByteBuddyAgent;
import net.bytebuddy.dynamic.loading.ClassReloadingStrategy;

/**
 *
 * @author Flo
 */
public class HotSwappingByteBuddy {

    private static boolean toggle;
    
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        ByteBuddyAgent.installOnOpenJDK();
        char command = 's';
        BufferedReader br = new 
        BufferedReader(new InputStreamReader(System.in)); 
        
        System.out.println("Type q, to exit program. Enter for r for repeat:");
        System.out.println("");
        // read characters 
        do {
            try { 
                command = (char) br.read();
                if(command == 'r'){
                    System.out.println("Execute method.");
                    
                    if(toggle){
                        new ByteBuddy()
                        .redefine(ConsoleWriter.class)
                        .name(ConsoleWriter.class.getName())
                        .make()
                        .load(ConsoleWriter.class.getClassLoader(), ClassReloadingStrategy.fromInstalledAgent());
                    } else{
                         new ByteBuddy()
                        .redefine(DocumentWriter.class)
                        .name(ConsoleWriter.class.getName())
                        .make()
                        .load(ConsoleWriter.class.getClassLoader(), ClassReloadingStrategy.fromInstalledAgent());
                    }
                    
                    (new ConsoleWriter()).writeOutput("I am an unnecessary output");
                    toggle = !toggle;
                }
                } catch (IOException ex) {
                    System.out.println("Could not read input.");
            }
            
        } while(command != 'q');
    }
}
