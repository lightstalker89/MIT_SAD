package com.grill.customerserviceclient.connection;

import android.os.Handler;

import java.util.List;

/**
 * Created by Flo on 15.12.2014.
 */
public interface IConnection {
    void setIpAddress(String ip);
    void setActivityHandler(Handler handler);
    void addCustomer(String customerName);
    void getCustomers();
    void deleteCustomer(String customerName);
    void deleteOrder(String customerName, int orderIndex);
    void getOrders(String customerName);
    void addOrder(String customerName, List<String> order);
}
