/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package hotswappingjavaagent;

import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.hotswap.agent.config.PluginManager;

/**
 *
 * @author Flo
 */
public class HotSwappingJavaAgent {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        for (int i = 0; i < 15; i++) {
            System.out.println("Test meesk");
            System.out.println("12");
            PluginManager.getInstance();

            try {
                System.in.read();
            } catch (IOException ex) {
                Logger.getLogger(HotSwappingJavaAgent.class.getName()).log(Level.SEVERE, null, ex);
            }
        }
    }
}
