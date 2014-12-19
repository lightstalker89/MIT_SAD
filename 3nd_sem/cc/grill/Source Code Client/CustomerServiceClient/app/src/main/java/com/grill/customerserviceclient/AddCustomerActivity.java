package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;


public class AddCustomerActivity extends Activity {

    private Button btnCancel;
    private Button btnAddCustomer;
    private EditText editTxtCustomerName;

    public final static String CUSTOMER_NAME = "customer_name_result";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_customer);

        btnCancel = (Button)findViewById(R.id.btnCancel);
        btnAddCustomer = (Button)findViewById(R.id.btnSaveCustomer);
        editTxtCustomerName = (EditText)findViewById(R.id.editTextCustomerName);

        btnCancel.setOnClickListener(cancel);
        btnAddCustomer.setOnClickListener(addCustomer);
    }

    View.OnClickListener cancel = new View.OnClickListener() {
        public void onClick(View v) {
            finish();
        }
    };

    View.OnClickListener addCustomer = new View.OnClickListener() {
        public void onClick(View v) {
            if(!editTxtCustomerName.getText().toString().equals("")){
                String customerName = editTxtCustomerName.getText().toString();
                Intent returnIntent = new Intent();
                returnIntent.putExtra(CUSTOMER_NAME, customerName);
                setResult(RESULT_OK, returnIntent);
                finish();
            }
            else{
                Toast.makeText(AddCustomerActivity.this, "Please enter a valid name!", Toast.LENGTH_LONG).show();
            }
        }
    };
}
