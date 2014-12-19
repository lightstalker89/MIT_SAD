package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import com.grill.customerserviceclient.helper.IntentMsgs;


public class DeleteCustomerActivity extends Activity {

    private String customerName;
    private int index;

    Button yes;
    Button no;
    TextView heading;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_delete_customer);

        Resources res = getResources();

        Intent info = getIntent();
        customerName = info.getStringExtra(IntentMsgs.CUSTOMER_NAME.toString());
        index = info.getIntExtra(IntentMsgs.CUSTOMER_INDEX.toString(), -1);

        yes = (Button) findViewById(R.id.btnYes);
        no = (Button) findViewById(R.id.btnNo);
        heading = (TextView) findViewById(R.id.textDeleteHeading);

        yes.setOnClickListener(deleteCustomer);
        no.setOnClickListener(cancelDelete);
        heading.setText(res.getString(R.string.delete_customer).concat("\n"+customerName));
    }

    View.OnClickListener cancelDelete = new View.OnClickListener() {
        public void onClick(View v) {
            finish();
        }
    };

    View.OnClickListener deleteCustomer = new View.OnClickListener() {
        public void onClick(View v) {
            Intent returnIntent = new Intent();
            returnIntent.putExtra(IntentMsgs.CUSTOMER_INDEX.toString(), index);
            setResult(RESULT_OK, returnIntent);
            finish();
        }
    };

}
