
package com.grill.car2car.connection;
import android.os.Handler;
import android.util.Log;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import com.grill.car2car.MainActivity;

/**
 * Handles reading and writing of messages with socket buffers. Uses a Handler
 * to post messages to UI thread for UI updates.
 */
public class CommunicationManager implements Runnable {

	private String macAddress;
    private Socket socket = null;
    private Handler handler;

    public CommunicationManager(Socket socket, Handler handler, String macAddress) {
        this.socket = socket;
        this.handler = handler;
        this.macAddress = macAddress;
    }

    private InputStream iStream;
    private OutputStream oStream;
    private static final String TAG = "ChatHandler";

    @Override
    public void run() {
        try {

            iStream = socket.getInputStream();
            oStream = socket.getOutputStream();
            byte[] buffer = new byte[1024];
            int bytes;
            handler.obtainMessage(MainActivity.MY_HANDLE, this)
                    .sendToTarget();

            while (true) {
                try {
                    // Read from the InputStream
                    bytes = iStream.read(buffer);
                    if (bytes == -1) {
                        break;
                    }
                    
                    
                    // Send the obtained bytes to the UI Activity
                    Log.d(TAG, "Rec:" + String.valueOf(buffer));
                    handler.obtainMessage(MainActivity.MESSAGE_READ,
                            bytes, -1, buffer).sendToTarget();
                } catch (IOException e) {
                    Log.e(TAG, "disconnected", e);
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                socket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    public void write(byte[] buffer) {
        try {
        	byte[] message = concatArray(buffer, macAddress.getBytes());
            oStream.write(message);
        } catch (IOException e) {
            Log.e(TAG, "Exception during write", e);
        }
    }
    
    public byte[] concatArray(byte[] A, byte[] B) {
    	   int aLen = A.length;
    	   int bLen = B.length;
    	   byte[] C= new byte[aLen+bLen];
    	   System.arraycopy(A, 0, C, 0, aLen);
    	   System.arraycopy(B, 0, C, aLen, bLen);
    	   return C;
    	}
}
