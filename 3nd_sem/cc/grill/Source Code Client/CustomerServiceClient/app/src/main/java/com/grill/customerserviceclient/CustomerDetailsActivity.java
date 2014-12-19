package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.os.Vibrator;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.grill.customerserviceclient.connection.IConnection;
import com.grill.customerserviceclient.connection.RestConnection;
import com.grill.customerserviceclient.connection.SoapConnection;
import com.grill.customerserviceclient.helper.IntentMsgs;
import com.grill.customerserviceclient.helper.StateMsgs;
import com.grill.customerserviceclient.objects.Customer;
import com.grill.customerserviceclient.objects.ParcelableArrayList;
import com.grill.customerserviceclient.userinterface.OrderArrayAdapter;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapPrimitive;

import java.util.ArrayList;
import java.util.List;


public class CustomerDetailsActivity extends Activity {

    private IConnection serviceConnection;
    private Customer customer;

    private ListView listOrders;
    private OrderArrayAdapter<List<String>> adapter;

    // Views, (alternative use Fragments)
    RelativeLayout belowStandard;
    RelativeLayout belowDeleteOrder;
    RelativeLayout belowAddOrder;
    ScrollView aboveAddOrder;
    RelativeLayout aboveStandard;

    // EditText add order
    EditText editTxtProduct1;
    EditText editTxtProduct2;
    EditText editTxtProduct3;
    EditText editTxtProduct4;

    // Below delete order ui
    Button btnDeleteOrder;
    Button btnCancelDelete;

    // Below Standard ui
    Button btnRefreshOrders;
    Button btnAddOrder;

    // Below Add order ui
    Button btnSaveOrder;
    Button btnCancelSaveOrder;

    // Vibrator object
    private Vibrator vib;
    private final int DURATION = 70;

    String serviceType;
    View selectedView;
    int selectedOrderIndex = -1;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_customer_details);
        vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

        Intent info = getIntent();
        customer = info.getParcelableExtra(IntentMsgs.CUSTOMER_OBJECT.toString());
        serviceType = info.getStringExtra(IntentMsgs.SERVICE_TYPE.toString());
        if(serviceType.equals("SOAP")){serviceConnection = SoapConnection.getInstance();}
        else{ serviceConnection = RestConnection.getInstance(); }
        serviceConnection.setActivityHandler(mHandler);
        setReturnValue();

        ((TextView) findViewById(R.id.txtCustomerName)).setText(
                ((TextView) findViewById(R.id.txtCustomerName)).getText().toString() +
                        "\n" + customer.getCustomerName()
        );

        listOrders = (ListView) findViewById(R.id.listViewOrders);
        adapter = new OrderArrayAdapter(this, R.layout.order_details, new ArrayList<String>());
        listOrders.setAdapter(adapter);
        listOrders.setOnItemLongClickListener(orderOnLongClickListener);
        fillOrderList(customer.getOrders());

        belowDeleteOrder = (RelativeLayout) findViewById(R.id.relativeLayoutBelowDelete);
        belowStandard = (RelativeLayout) findViewById(R.id.relativeLayoutBelowStandard);
        belowAddOrder = (RelativeLayout) findViewById(R.id.relativeLayoutBelowAddOrders);
        aboveAddOrder = (ScrollView) findViewById(R.id.scrollLayoutAboveAddOrder);
        aboveStandard = (RelativeLayout) findViewById(R.id.relativeLayoutAboveStandard);

        editTxtProduct1 = (EditText) findViewById(R.id.editTxtProduct1);
        editTxtProduct2 = (EditText) findViewById(R.id.editTxtProduct2);
        editTxtProduct3 = (EditText) findViewById(R.id.editTxtProduct3);
        editTxtProduct4 = (EditText) findViewById(R.id.editTxtProduct4);

        btnRefreshOrders = (Button) findViewById(R.id.btnRefreshOrders);
        btnAddOrder = (Button) findViewById(R.id.btnAddOrder);

        btnRefreshOrders.setOnClickListener(refreshOrder);
        btnAddOrder.setOnClickListener(addOrder);

        btnCancelDelete = (Button) findViewById(R.id.btnCancelDeleteOrder);
        btnDeleteOrder = (Button) findViewById(R.id.btnDeleteOrder);

        btnCancelDelete.setOnClickListener(cancelDelete);
        btnDeleteOrder.setOnClickListener(deleteOrder);

        btnSaveOrder = (Button) findViewById(R.id.btnCreateOrder);
        btnCancelSaveOrder = (Button) findViewById(R.id.btnCancelAddOrder);

        btnSaveOrder.setOnClickListener(saveOrder);
        btnCancelSaveOrder.setOnClickListener(cancelAddOrder);

    }

    private void fillOrderList(List<ParcelableArrayList> orders) {
        for (List<String> order: orders){
            adapter.add(order);
        }
    }

    private AdapterView.OnItemLongClickListener orderOnLongClickListener = new AdapterView.OnItemLongClickListener() {

        @Override
        public boolean onItemLongClick(AdapterView<?> v, final View view,
                                       int id, long arg3) {
            vib.vibrate(DURATION);
            view.setSelected(true);
            selectedView = view;
            selectedOrderIndex = id;
            showDeleteOrderView();
            return false;
        }
    };

    View.OnClickListener refreshOrder = new View.OnClickListener() {
        public void onClick(View v) {
            serviceConnection.getOrders(customer.getCustomerName());
        }
    };

    View.OnClickListener addOrder = new View.OnClickListener() {
        public void onClick(View v) {
            showAddOrderView();
        }
    };

    View.OnClickListener cancelDelete = new View.OnClickListener() {
        public void onClick(View v) {
            if(selectedView != null) selectedView.setSelected(false);
            showStandardView();
        }
    };

    View.OnClickListener deleteOrder = new View.OnClickListener() {
        public void onClick(View v) {
            if(selectedOrderIndex != -1){
                serviceConnection.deleteOrder(customer.getCustomerName(), selectedOrderIndex);
                adapter.remove(customer.getOrders().get(selectedOrderIndex));
                customer.getOrders().remove(selectedOrderIndex);
                selectedOrderIndex = -1;
                showStandardView();
            }
        }
    };

    View.OnClickListener cancelAddOrder = new View.OnClickListener() {
        public void onClick(View v) {
            showStandardView();
        }
    };

    View.OnClickListener saveOrder = new View.OnClickListener() {
        public void onClick(View v) {
            ParcelableArrayList order = getListOfOrder(new ParcelableArrayList(),aboveAddOrder);
            if(!order.isEmpty()){
                adapter.add(order);
                customer.getOrders().add(order);
                serviceConnection.addOrder(customer.getCustomerName(), order);
            }
            else{
                Toast.makeText(CustomerDetailsActivity.this, "Please add some products", Toast.LENGTH_LONG).show();
            }
        }
    };

    private ParcelableArrayList getListOfOrder(ParcelableArrayList order, ViewGroup aboveAddOrder) {;
        for( int i = 0; i < aboveAddOrder.getChildCount(); i++ ){
            Object child = aboveAddOrder.getChildAt(i);
            if (child instanceof EditText)
            {
                EditText e = (EditText)child;
                String product = e.getText().toString().trim();
                if(product.length() != 0)    // Whatever logic here to determine if valid.
                {
                    order.add(product);
                    e.getText().clear();
                }
            }
            else if(child instanceof ViewGroup)
            {
                order = getListOfOrder(order, (ViewGroup)child);
            }
        }
        return order;
    }

    private void showDeleteOrderView() {
        belowStandard.setVisibility(View.GONE);
        aboveAddOrder.setVisibility(View.GONE);
        belowAddOrder.setVisibility(View.GONE);
        aboveStandard.setVisibility(View.VISIBLE);
        belowDeleteOrder.setVisibility(View.VISIBLE);

    }

    private void showStandardView() {
        aboveAddOrder.setVisibility(View.GONE);
        belowAddOrder.setVisibility(View.GONE);
        belowDeleteOrder.setVisibility(View.GONE);
        aboveStandard.setVisibility(View.VISIBLE);
        belowStandard.setVisibility(View.VISIBLE);
    }

    private void showAddOrderView() {
        aboveStandard.setVisibility(View.GONE);
        belowStandard.setVisibility(View.GONE);
        belowDeleteOrder.setVisibility(View.GONE);
        aboveAddOrder.setVisibility(View.VISIBLE);
        belowAddOrder.setVisibility(View.VISIBLE);
    }

    // Todo: put object parsing in connection classes (REST and SOAP) to reduce different message handling
    private final Handler mHandler = new Handler(new Handler.Callback() {

        @Override
        public boolean handleMessage(Message msg) {
            if (msg.what == StateMsgs.SOAP_DELETE_ORDER_RESULT.ordinal()) {
                SoapPrimitive result = (SoapPrimitive) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(CustomerDetailsActivity.this, "Delete successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(CustomerDetailsActivity.this, "Delete failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            if (msg.what == StateMsgs.REST_DELETE_ORDER_RESULT.ordinal()) {
                String result = (String) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(CustomerDetailsActivity.this, "Delete successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(CustomerDetailsActivity.this, "Delete failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if(msg.what == StateMsgs.SOAP_RESPONSE_ORDER_LIST.ordinal()){
                SoapObject result = (SoapObject) msg.obj;
                List<ParcelableArrayList> orders = new ArrayList<>();
                for(int j=0; j < result.getPropertyCount(); j++){
                    SoapObject customerOrder = (SoapObject)result.getProperty(j);
                    ParcelableArrayList order = new ParcelableArrayList();
                    for(int k=0; k < customerOrder.getPropertyCount(); k++){
                        order.add(customerOrder.getProperty(k).toString());
                    }
                    orders.add(order);
                }
                adapter.clear();
                fillOrderList(orders);
                Toast.makeText(CustomerDetailsActivity.this, "Updated order list successful", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.REST_RESPONSE_ORDER_LIST.ordinal()){
                JSONArray result = (JSONArray) msg.obj;
                List<ParcelableArrayList> orders = new ArrayList<>();
                try {
                    for(int j=0; j < result.length(); j++){
                        JSONArray customerOrder = result.getJSONArray(j);
                        ParcelableArrayList order = new ParcelableArrayList();
                        for(int k=0; k < customerOrder.length(); k++){
                            order.add(customerOrder.getString(k));
                        }
                        orders.add(order);
                    }
                    adapter.clear();
                    fillOrderList(orders);
                    Toast.makeText(CustomerDetailsActivity.this, "Updated order list successful", Toast.LENGTH_LONG).show();
                }
                catch (JSONException e) {
                    Toast.makeText(CustomerDetailsActivity.this, "Failed to parse json", Toast.LENGTH_LONG).show();
                    e.printStackTrace();
                }
            }
            else if(msg.what == StateMsgs.SOAP_RESPONSE_ADD_ORDER.ordinal()){
                SoapPrimitive result = (SoapPrimitive) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(CustomerDetailsActivity.this, "Added order successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(CustomerDetailsActivity.this, "Add order failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if(msg.what == StateMsgs.REST_RESPONSE_ADD_ORDER.ordinal()){
                String result = (String) msg.obj;
                if(result.toString().equals("true")){
                    Toast.makeText(CustomerDetailsActivity.this, "Added order successful", Toast.LENGTH_LONG).show();
                }
                else{
                    Toast.makeText(CustomerDetailsActivity.this, "Add order failed, please refresh", Toast.LENGTH_LONG).show();
                }
            }
            else if(msg.what == StateMsgs.SOAP_GET_RESPONSE_ERROR.ordinal()){
                Toast.makeText(CustomerDetailsActivity.this, "SOAP response error", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.SOAP_XML_PARSE_ERROR.ordinal()){
                Toast.makeText(CustomerDetailsActivity.this, "Soap xml parse error", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.CONNECTION_ERROR.ordinal()){
                Toast.makeText(CustomerDetailsActivity.this, "Connection error occurred", Toast.LENGTH_LONG).show();
            }
            else if(msg.what == StateMsgs.GENERAL_ERROR.ordinal()){
                Toast.makeText(CustomerDetailsActivity.this, "General error occurred", Toast.LENGTH_LONG).show();
            }
            return true;
        }
    });

    private void setReturnValue() {
        Intent returnIntent = new Intent();
        returnIntent.putExtra(IntentMsgs.CUSTOMER_OBJECT.toString(), this.customer);
        setResult(RESULT_OK, returnIntent);
    }
}
