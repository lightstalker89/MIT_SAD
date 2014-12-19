package com.grill.customerserviceclient.connection;

import org.apache.http.client.HttpResponseException;
import org.ksoap2.SoapEnvelope;
import org.ksoap2.SoapFault;
import org.ksoap2.serialization.PropertyInfo;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import java.io.IOException;
import java.text.MessageFormat;
import java.util.List;

import android.os.Handler;

import com.grill.customerserviceclient.helper.StateMsgs;

/**
 * Created by Flo on 15.12.2014.
 */
public class SoapConnection implements IConnection{

    // Standard ip
    private String ipAddress = "10.0.0.6";

    private final String NAMESPACE = "http://grill.com/webservices/";
    private String URL = "http://{0}:8180/customerservice?wsdl";

    private final String SOAP_ACTION_ADD_CUSTOMER = "http://grill.com/webservices/CustomerService/AddCustomer";
    private final String METHOD_NAME_ADD_CUSTOMER = "AddCustomer";

    private final String SOAP_ACTION_GET_CUSTOMERS = "http://grill.com/webservices/CustomerService/GetCustomers";
    private final String METHOD_NAME_GET_CUSTOMERS = "GetCustomers";

    private final String SOAP_ACTION_DELETE_CUSTOMERS = "http://grill.com/webservices/CustomerService/DeleteCustomer";
    private final String METHOD_NAME_DELETE_CUSTOMERS = "DeleteCustomer";

    private final String SOAP_ACTION_DELETE_ORDER = "http://grill.com/webservices/CustomerService/DeleteOrder";
    private final String METHOD_NAME_DELETE_ORDER = "DeleteOrder";

    private final String SOAP_ACTION_GET_ORDERS = "http://grill.com/webservices/CustomerService/GetOrders";
    private final String METHOD_NAME_GET_ORDERS = "GetOrders";

    private final String SOAP_ACTION_ADD_ORDER = "http://grill.com/webservices/CustomerService/AddOrder";
    private final String METHOD_NAME_ADD_ORDER = "AddOrder";

    private SoapSerializationEnvelope envelope;
    private HttpTransportSE httpTransport;

    private static SoapConnection soapInstance;
    private Handler mHandler;

    private SoapConnection(){
        setIpAddress(); // Set standard
        // Initialize ksoap2 objects
        /*httpTransport = new HttpTransportSE(URL);
        httpTransport.debug = true;
        envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
        envelope.dotNet = true;*/
    }

    public static SoapConnection getInstance() {
        if (soapInstance == null) {
            soapInstance = new SoapConnection();
        }
        return soapInstance;
    }

    private void setIpAddress() {
        this.URL = MessageFormat.format("http://{0}:8180/customerservice?wsdl", this.ipAddress);
        httpTransport = new HttpTransportSE(this.URL);
        httpTransport.debug = true;
        envelope = new SoapSerializationEnvelope(SoapEnvelope.VER11);
        envelope.dotNet = true;
    }

    @Override
    public void setIpAddress(String ip) {
        this.ipAddress = ip;
        setIpAddress();
    }

    @Override
    public void setActivityHandler(Handler handler){
        this.mHandler = handler;
    }

    @Override
    public void addCustomer(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_ADD_CUSTOMER);
                request.addProperty("customerName", customerName);
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_ADD_CUSTOMER, envelope);
                    try {
                        mHandler.obtainMessage(StateMsgs.SOAP_ADD_CUSTOMER_RESULT.ordinal(), envelope.getResponse())
                                .sendToTarget();

                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                        e.printStackTrace();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void getCustomers() {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_GET_CUSTOMERS);
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_GET_CUSTOMERS, envelope);
                    try {
                        Thread.sleep(1000);
                        mHandler.obtainMessage(StateMsgs.SOAP_RESPONSE_CUSTOMER_LIST.ordinal(), envelope.getResponse())
                                .sendToTarget();

                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void deleteCustomer(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_DELETE_CUSTOMERS);
                request.addProperty("customerName", customerName);
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_DELETE_CUSTOMERS, envelope);
                    try {
                        mHandler.obtainMessage(StateMsgs.SOAP_DELETE_CUSTOMER_RESULT.ordinal(), envelope.getResponse())
                                .sendToTarget();
                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void deleteOrder(final String customerName, final int orderIndex) {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_DELETE_ORDER);
                request.addProperty("customerName", customerName);
                request.addProperty("orderIndex", orderIndex);
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_DELETE_ORDER, envelope);
                    try {
                        mHandler.obtainMessage(StateMsgs.SOAP_DELETE_ORDER_RESULT.ordinal(), envelope.getResponse())
                                .sendToTarget();

                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                        e.printStackTrace();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void getOrders(final String customerName) {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_GET_ORDERS);
                request.addProperty("customerName", customerName);
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_GET_ORDERS, envelope);
                    try {
                        mHandler.obtainMessage(StateMsgs.SOAP_RESPONSE_ORDER_LIST.ordinal(), envelope.getResponse())
                                .sendToTarget();

                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                        e.printStackTrace();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                }
            }
        }).start();
    }

    @Override
    public void addOrder(final String customerName, final List<String> order) {
        new Thread(new Runnable() {
            public void run() {
                SoapObject request = new SoapObject(NAMESPACE, METHOD_NAME_ADD_ORDER);
                request.addProperty("customerName", customerName);
                request.addSoapObject(convertListToSoapObject(order));
                envelope.setOutputSoapObject(request);
                try {
                    httpTransport.call(SOAP_ACTION_ADD_ORDER, envelope);
                    try {
                        mHandler.obtainMessage(StateMsgs.SOAP_RESPONSE_ADD_ORDER.ordinal(), envelope.getResponse())
                                .sendToTarget();

                    } catch (SoapFault e) {
                        mHandler.obtainMessage(StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal())
                                .sendToTarget();
                    } catch (Exception e){
                        mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                                .sendToTarget();
                        e.printStackTrace();
                    }
                } catch (HttpResponseException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (IOException e) {
                    mHandler.obtainMessage(StateMsgs.CONNECTION_ERROR.ordinal())
                            .sendToTarget();
                } catch (XmlPullParserException e) {
                    mHandler.obtainMessage(StateMsgs.SOAP_XML_PARSE_ERROR.ordinal())
                            .sendToTarget();
                } catch (Exception e){
                    mHandler.obtainMessage(StateMsgs.GENERAL_ERROR.ordinal())
                            .sendToTarget();
                    e.printStackTrace();
                }
            }
        }).start();
    }

    private SoapObject convertListToSoapObject(List<String> order) {
        SoapObject listObject = new SoapObject(NAMESPACE, "order");

        for(int i=0; i < order.size(); i++){
            PropertyInfo p = new PropertyInfo();
            p.setNamespace("http://schemas.microsoft.com/2003/10/Serialization/Arrays");
            p.setName("string");
            p.setValue(order.get(i));
            listObject.addProperty(p);
        }
        return listObject;
    }
}
