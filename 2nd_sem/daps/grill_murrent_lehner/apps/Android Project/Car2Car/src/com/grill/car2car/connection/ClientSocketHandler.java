
package com.grill.car2car.connection;

import android.os.Handler;
import android.util.Log;

import java.io.IOException;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;

import com.grill.car2car.MainActivity;

public class ClientSocketHandler extends Thread {

    private static final String TAG = "ClientSocketHandler";
    private Handler handler;
    private CommunicationManager chat;
    private InetAddress mAddress;
    private String macAddress;

    public ClientSocketHandler(Handler handler, InetAddress groupOwnerAddress, String macAddress) {
        this.handler = handler;
        this.mAddress = groupOwnerAddress;
        this.macAddress = macAddress;
    }

    @Override
    public void run() {
        Socket socket = new Socket();
        try {
            socket.bind(null);
            socket.connect(new InetSocketAddress(mAddress.getHostAddress(),
                    MainActivity.SERVER_PORT), 5000);
            Log.d(TAG, "Launching the I/O handler");
            chat = new CommunicationManager(socket, handler, macAddress);
            new Thread(chat).start();
        } catch (IOException e) {
            e.printStackTrace();
            try {
                socket.close();
            } catch (IOException e1) {
                e1.printStackTrace();
            }
            return;
        }
    }
}
