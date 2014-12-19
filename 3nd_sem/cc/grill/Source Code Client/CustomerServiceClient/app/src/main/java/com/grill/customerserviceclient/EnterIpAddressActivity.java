package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.grill.customerserviceclient.helper.IntentMsgs;

import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class EnterIpAddressActivity extends Activity {

    Button btnSaveIp;
    Button btnCancel;
    EditText editTxtIpAddress;

    private Pattern pattern;
    private Matcher matcher;
    private static final String IPADDRESS_PATTERN = "^([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
            + "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
            + "([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\."
            + "([01]?\\d\\d?|2[0-4]\\d|25[0-5])$";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_enter_ip_address);
        pattern = Pattern.compile(IPADDRESS_PATTERN);

        btnSaveIp = (Button) findViewById(R.id.btnSetIp);
        btnCancel = (Button) findViewById(R.id.btnSetIpCancel);
        editTxtIpAddress = (EditText) findViewById(R.id.editTextCustomerName);

        btnSaveIp.setOnClickListener(setIp);
        btnCancel.setOnClickListener(cancelSetIp);
    }

    private View.OnClickListener cancelSetIp = new View.OnClickListener() {
        public void onClick(View v) {
            finish();
        }
    };

    private View.OnClickListener setIp = new View.OnClickListener() {
        public void onClick(View v) {
            if ((validate(editTxtIpAddress.getText().toString()))) {
                String ipAddress = editTxtIpAddress.getText().toString();
                Intent returnIntent = new Intent();
                returnIntent.putExtra(IntentMsgs.IP_ADDRESS.toString(), ipAddress);
                setResult(RESULT_OK, returnIntent);
                finish();
            }
            else{
                Toast.makeText(EnterIpAddressActivity.this, "Please enter a valid Ip address", Toast.LENGTH_LONG).show();
            }
        }
    };

    /**
     * Validate ip address with regular expression
     *
     * @param ip
     *            ip address for validation
     * @return true valid ip address, false invalid ip address
     */
    public boolean validate(final String ip) {
        matcher = pattern.matcher(ip);
        return matcher.matches();
    }
}
