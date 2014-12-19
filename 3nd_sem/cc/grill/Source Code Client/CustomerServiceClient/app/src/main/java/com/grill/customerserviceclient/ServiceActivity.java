package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Message;
import android.os.Vibrator;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.os.Handler;
import android.view.View;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.grill.customerserviceclient.connection.IConnection;
import com.grill.customerserviceclient.connection.RestConnection;
import com.grill.customerserviceclient.connection.SoapConnection;
import com.grill.customerserviceclient.helper.ActivityResults;
import com.grill.customerserviceclient.objects.ParcelableArrayList;
import com.grill.customerserviceclient.userinterface.CustomerArrayAdapter;
import com.grill.customerserviceclient.helper.IntentMsgs;
import com.grill.customerserviceclient.helper.StateMsgs;
import com.grill.customerserviceclient.objects.Customer;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;

import java.util.ArrayList;
import java.util.List;


public class ServiceActivity extends ActionBarActivity {

    private IConnection serviceConnection;
    private ActivityResults[] values;

    private TextView headlineFetching;
    private Button btnAddCustomer;
    private Button btnRefresh;

    private ListView listCustomer;
    private CustomerArrayAdapter<Customer> adapter;
    private List<Customer> customerList =  new ArrayList<>();

    // Vibrator object
    private Vibrator vib;
    private final int DURATION = 70;

    private String serviceType;
    private String ip;
    private boolean performLongClick;
    private int editCustomerIndex = -1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_service);
        values = ActivityResults.values();
        vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

        Intent info = getIntent();
        ip = info.getStringExtra(IntentMsgs.IP_ADDRESS.toString());
        serviceType = info.getStringExtra(IntentMsgs.SERVICE_TYPE.toString());
        if(serviceType.equals("SOAP")){
            serviceConnection = SoapConnection.getInstance();
            this.setTitle("SOAP Service");
        }
        else{
            serviceConnection = RestConnection.getInstance();
            this.setTitle("REST Service");

        }
        serviceConnection.setActivityHandler(mHandler);
        serviceConnection.setIpAddress(ip);

        headlineFetching = (TextView) findViewById(R.id.txtFetching);

        listCustomer = (ListView) findViewById(R.id.listViewCustomer);
        adapter = new CustomerArrayAdapter(this, R.layout.customer_details, customerList);
        listCustomer.setAdapter(adapter);
        listCustomer.setOnItemLongClickListener(customerOnLongClickListener);
        listCustomer.setOnItemClickListener(customerClickListener);

        btnAddCustomer = (Button)findViewById(R.id.btnAddCustomer);
        btnAddCustomer.setOnClickListener(addCustomer);

        btnRefresh = (Button)findViewById(R.id.btnRefreshCustomers);
        btnRefresh.setOnClickListener(refreshCustomers);

        serviceConnection.getCustomers();
        displayLoadingPanel();
    }

    View.OnClickListener addCustomer = new View.OnClickListener() {
        public void onClick(View v) {
            Intent myIntent = new Intent(v.getContext(),
                    AddCustomerActivity.class);
            startActivityForResult(myIntent,
                    ActivityResults.ADD_CUSTOMER.ordinal());
        }
    };

    View.OnClickListener refreshCustomers = new View.OnClickListener() {
        public void onClick(View v) {
            customerList.clear();
            adapter.notifyDataSetChanged();
            serviceConnection.getCustomers();
            displayLoadingPanel();
        }
    };

    private AdapterView.OnItemClickListener customerClickListener = new AdapterView.OnItemClickListener() {
        @Override
        public void onItemClick(AdapterView<?> v, final View view, int id,
                                long arg3) {
            if (!performLongClick) {
                editCustomerIndex = id;
                Intent myIntent = new Intent(ServiceActivity.this,
                        CustomerDetailsActivity.class);
                myIntent.putExtra(IntentMsgs.SERVICE_TYPE.toString(), serviceType);
                myIntent.putExtra(IntentMsgs.CUSTOMER_OBJECT.toString(), customerList.get(id));
                startActivityForResult(myIntent,
                        ActivityResults.CUSTOMER_DETAILS.ordinal());
            }
            performLongClick = false;
        }
    };

    private AdapterView.OnItemLongClickListener customerOnLongClickListener = new AdapterView.OnItemLongClickListener() {

        @Override
        public boolean onItemLongClick(AdapterView<?> v, final View view,
                                       int id, long arg3) {
            performLongClick = true;
            vib.vibrate(DURATION);

            Customer customer = customerList.get(id);

            Intent myIntent = new Intent(ServiceActivity.this,
                    DeleteCustomerActivity.class);
            myIntent.putExtra(IntentMsgs.CUSTOMER_NAME.toString(), customer.getCustomerName());
            myIntent.putExtra(IntentMsgs.CUSTOMER_INDEX.toString(), id);
            startActivityForResult(myIntent,
                    ActivityResults.DELETE_CUSTOMER.ordinal());

            return false;
        }
    };

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (values[requestCode]) {
            case ADD_CUSTOMER:
                if (resultCode == Activity.RESULT_OK) {
                    String name = data.getStringExtra(AddCustomerActivity.CUSTOMER_NAME);
                    serviceConnection.addCustomer(name);
                    customerList.add(new Customer(name, new ArrayList<ParcelableArrayList>()));
                    adapter.notifyDataSetChanged();
                }
                break;
            case DELETE_CUSTOMER:
                if (resultCode == Activity.RESULT_OK) {
                    int index = data.getIntExtra(IntentMsgs.CUSTOMER_INDEX.toString(), -1);
                    if (index != -1) {
                        Customer customer = customerList.get(index);
                        serviceConnection.deleteCustomer(customer.getCustomerName());
                        customerList.remove(customer);
                        adapter.notifyDataSetChanged();
                    }
                    else {
                        Toast.makeText(this, "Delete failed", Toast.LENGTH_SHORT)
                                .show();
                    }
                }
                break;
            case CUSTOMER_DETAILS:
                serviceConnection.setActivityHandler(mHandler);
                Customer customer = data.getParcelableExtra(IntentMsgs.CUSTOMER_OBJECT.toString());
                customerList.set(editCustomerIndex, customer);
                adapter.notifyDataSetChanged();
                editCustomerIndex = -1;
                break;
        }
    }

    // Todo: put object parsing in connection classes (REST and SOAP) to reduce different message handling
    private final Handler mHandler = new Handler(new Handler.Callback() {

        @Override
        public boolean handleMessage(Message msg) {
            if (msg.what == StateMsgs.SOAP_ADD_CUSTOMER_RESULT.ordinal()) {
                SoapPrimitive result = (SoapPrimitive) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(ServiceActivity.this, "Add successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(ServiceActivity.this, "Add failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            if (msg.what == StateMsgs.REST_ADD_CUSTOMER_RESULT.ordinal()) {
                String result = (String) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(ServiceActivity.this, "Add successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(ServiceActivity.this, "Add failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if (msg.what == StateMsgs.SOAP_DELETE_CUSTOMER_RESULT.ordinal()) {
                SoapPrimitive result = (SoapPrimitive) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(ServiceActivity.this, "Delete successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(ServiceActivity.this, "Delete failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if (msg.what == StateMsgs.REST_DELETE_CUSTOMER_RESULT.ordinal()) {
                String result = (String) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(ServiceActivity.this, "Delete successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(ServiceActivity.this, "Delete failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if (msg.what == StateMsgs.SOAP_RESPONSE_CUSTOMER_LIST.ordinal()) {
                SoapObject result = (SoapObject) msg.obj;
                for(int i=0; i < result.getPropertyCount(); i++){
                    List<ParcelableArrayList> orders = new ArrayList<>();
                    SoapObject customer = (SoapObject)result.getProperty(i);
                    SoapObject customerOrders = (SoapObject)customer.getProperty(1);
                    String customerName = customer.getProperty(0).toString();
                    for(int j=0; j < customerOrders.getPropertyCount(); j++){
                        SoapObject customerOrder = (SoapObject)customerOrders.getProperty(j);
                        ParcelableArrayList order = new ParcelableArrayList();
                        for(int k=0; k < customerOrder.getPropertyCount(); k++){
                            order.add(customerOrder.getProperty(k).toString());
                        }
                        orders.add(order);
                    }
                    customerList.add(new Customer(customerName, orders));
                    adapter.notifyDataSetChanged();
                }
                hideLoadingPanel();
                Toast.makeText(ServiceActivity.this, "Operation successful", Toast.LENGTH_LONG).show();
            }
            else if (msg.what == StateMsgs.REST_RESPONSE_CUSTOMER_LIST.ordinal()) {
                JSONArray result = (JSONArray) msg.obj;

                try {
                    for(int i=0; i < result.length(); i++){
                        List<ParcelableArrayList> orders = new ArrayList<>();
                        JSONObject customer = result.getJSONObject(i);
                        String customerName = customer.getString("CustomerName");
                        JSONArray customerOrders = customer.getJSONArray("CustomerOrders");

                        for(int j=0; j < customerOrders.length(); j++){
                            JSONArray customerOrder = customerOrders.getJSONArray(j);
                            ParcelableArrayList order = new ParcelableArrayList();
                            for(int k=0; k < customerOrder.length(); k++){
                                order.add(customerOrder.getString(k));
                            }
                            orders.add(order);
                        }
                        customerList.add(new Customer(customerName, orders));
                        adapter.notifyDataSetChanged();
                    }
                    Toast.makeText(ServiceActivity.this, "Operation successful", Toast.LENGTH_LONG).show();
                }
                catch (JSONException e) {
                    Toast.makeText(ServiceActivity.this, "Failed to parse json", Toast.LENGTH_LONG).show();
                    e.printStackTrace();
                }
                hideLoadingPanel();
            }
            else if(msg.what == StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal()){
                hideLoadingPanel();
                Toast.makeText(ServiceActivity.this, "SOAP response error", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.SOAP_XML_PARSE_ERROR.ordinal()){
                hideLoadingPanel();
                Toast.makeText(ServiceActivity.this, "Soap xml parse error", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.CONNECTION_ERROR.ordinal()){
                hideLoadingPanel();
                Toast.makeText(ServiceActivity.this, "Connection error occurred", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.GENERAL_ERROR.ordinal()){
                hideLoadingPanel();
                Toast.makeText(ServiceActivity.this, "General error occurred", Toast.LENGTH_LONG).show();
            }
            return true;
        }
    });

    private void displayLoadingPanel(){
        findViewById(R.id.loadingPanel).setVisibility(View.VISIBLE);
        headlineFetching.setVisibility(View.VISIBLE);
    }

    private void hideLoadingPanel(){
        findViewById(R.id.loadingPanel).setVisibility(View.GONE);
        headlineFetching.setVisibility(View.GONE);
    }
}
