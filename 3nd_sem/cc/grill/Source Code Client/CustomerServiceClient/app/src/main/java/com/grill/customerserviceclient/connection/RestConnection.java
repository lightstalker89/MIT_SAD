package com.grill.customerserviceclient.connection;

import android.os.Handler;

import com.grill.customerserviceclient.helper.StateMsgs;

import org.json.JSONArray;
import org.springframework.http.HttpMethod;
import org.springframework.http.ResponseEntity;
import org.springframework.http.client.SimpleClientHttpRequestFactory;
import org.springframework.http.converter.json.MappingJackson2HttpMessageConverter;
import org.springframework.web.client.RestTemplate;


import java.text.MessageFormat;
import java.util.List;

/**
 * Created by Flo on 18.12.2014.
 */
public class RestConnection implements IConnection {

    // Standard ip
    private String ipAddress = "10.0.0.6";

    private String REST_ACTION_GET_CUSTOMERS = "http://{0}:8280/GetCustomers";
    private String REST_ACTION_ADD_CUSTOMERS = "http://{0}:8280/AddCustomer/{customerName}";
    private String REST_ACTION_DELETE_CUSTOMER = "http://{0}:8280/DeleteCustomer/{customerName}";
    private String REST_ACTION_GET_ORDERS = "http://{0}:8280/GetOrders/{customerName}";
    private String REST_ACTION_DELETE_ORDER = "http://{0}:8280/DeleteOrder/{customerName}/orderIndex/{orderIndex}";
    private String REST_ACTION_ADD_ORDERS = "http://{0}:8280/AddOrder/{customerName}/order/{order}";

    RestTemplate restTemplate;

    private static RestConnection restInstance;
    private Handler mHandler;

    private RestConnection(){
        setIpAddress(); // Set standard
        restTemplate = new RestTemplate();
        ((SimpleClientHttpRequestFactory)restTemplate.getRequestFactory()).setReadTimeout(5000);
        ((SimpleClientHttpRequestFactory)restTemplate.getRequestFactory()).setConnectTimeout(5000);
        restTemplate.getMessageConverters().add(new MappingJackson2HttpMessageConverter());
    }

    public static RestConnection getInstance() {
        if (restInstance == null) {
            restInstance = new RestConnection();
        }
        return restInstance;
    }

    private void setIpAddress() {
        REST_ACTION_GET_CUSTOMERS = MessageFormat.format("http://{0}:8280/GetCustomers", ipAddress);
        REST_ACTION_ADD_CUSTOMERS = MessageFormat.format("http://{0}:8280/AddCustomer/{1}", ipAddress, "{customerName}");
        REST_ACTION_DELETE_CUSTOMER = MessageFormat.format("http://{0}:8280/DeleteCustomer/{1}", ipAddress, "{customerName}");
        REST_ACTION_GET_ORDERS = MessageFormat.format("http://{0}:8280/GetOrders/{1}", ipAddress, "{customerName}");
        REST_ACTION_DELETE_ORDER = MessageFormat.format("http://{0}:8280/DeleteOrder/{1}/orderIndex/{2}", ipAddress, "{customerName}", "{orderIndex}");
        REST_ACTION_ADD_ORDERS = MessageFormat.format("http://{0}:8280/AddOrder/{1}/order/{2}", ipAddress, "{customerName}", "{order}");
    }

    @Override
    public void setIpAddress(String ip) {
        if(!ip.equals(this.ipAddress)) {
            this.ipAddress = ip;
            setIpAddress();
        }
    }

    @Override
    public void setActivityHandler(Handler handler) {
        this.mHandler = handler;
    }

    @Override
    public void addCustomer(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                try {
                    String response = restTemplate.postForObject(REST_ACTION_ADD_CUSTOMERS, null, String.class, customerName);
                    mHandler.obtainMessage(StateMsgs.REST_ADD_CUSTOMER_RESULT.ordinal(), response)
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void getCustomers() {
        new Thread(new Runnable() {
            public void run() {
                try {
                    String response = restTemplate.getForObject(REST_ACTION_GET_CUSTOMERS, String.class);
                    JSONArray json = new JSONArray(response);
                    mHandler.obtainMessage(StateMsgs.REST_RESPONSE_CUSTOMER_LIST.ordinal(), json)
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void deleteCustomer(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                try {
                    ResponseEntity response = restTemplate.exchange(REST_ACTION_DELETE_CUSTOMER, HttpMethod.DELETE, null, String.class, customerName);
                    mHandler.obtainMessage(StateMsgs.REST_DELETE_CUSTOMER_RESULT.ordinal(), response.getBody().toString())
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void deleteOrder(final String customerName, final int orderIndex) {
        new Thread(new Runnable() {
            public void run() {
                try {
                    ResponseEntity response = restTemplate.exchange(REST_ACTION_DELETE_ORDER, HttpMethod.DELETE, null, String.class, customerName, Integer.toString(orderIndex));
                    mHandler.obtainMessage(StateMsgs.REST_DELETE_ORDER_RESULT.ordinal(), response.getBody().toString())
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void getOrders(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                try {
                    String response = restTemplate.getForObject(REST_ACTION_GET_ORDERS, String.class, customerName);
                    JSONArray json = new JSONArray(response);
                    mHandler.obtainMessage(StateMsgs.REST_RESPONSE_ORDER_LIST.ordinal(), json)
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void addOrder(final String customerName, final List<String> order) {
        new Thread(new Runnable() {
            public void run() {
                try {
                    String workaroundOrder = GetWorkaroundOrderString(order);

                    String response = restTemplate.postForObject(REST_ACTION_ADD_ORDERS, null, String.class, customerName, workaroundOrder);
                    mHandler.obtainMessage(StateMsgs.REST_RESPONSE_ADD_ORDER.ordinal(), response)
                            .sendToTarget();
                }
                catch(Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    private String GetWorkaroundOrderString(List<String> order) {
        String workaroundOrder = "";
        for(String product: order){
            workaroundOrder += product.replace("|", "") + "|";
        }
        return workaroundOrder;
    }
}
