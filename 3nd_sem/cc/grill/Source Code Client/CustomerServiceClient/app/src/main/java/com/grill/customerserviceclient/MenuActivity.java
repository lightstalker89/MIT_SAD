package com.grill.customerserviceclient;

import android.app.Activity;
import android.content.Intent;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageButton;

import com.grill.customerserviceclient.helper.ActivityResults;
import com.grill.customerserviceclient.helper.IntentMsgs;


public class MenuActivity extends ActionBarActivity {

    ImageButton soapButton;
    ImageButton restButton;
    ImageButton settingsButton;

    private ActivityResults[] values;

    private String ipAddress = "";
    private boolean showIpActvity = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_menu);
        values = ActivityResults.values();

        soapButton = (ImageButton)findViewById(R.id.btnSoap);
        restButton = (ImageButton)findViewById(R.id.btnRest);
        settingsButton = (ImageButton)findViewById(R.id.btnSettings);

        soapButton.setOnClickListener(openSoapActivity);
        restButton.setOnClickListener(openRestActivity);
        settingsButton.setOnClickListener(openSettingsActivty);
        if(!showIpActvity){
            showIpActvity = true;
            openSettings();
        }
    }

    private View.OnClickListener openSoapActivity = new View.OnClickListener() {
        public void onClick(View v) {
            Intent myIntent = new Intent(v.getContext(),
                    ServiceActivity.class);
            myIntent.putExtra(IntentMsgs.SERVICE_TYPE.toString(), "SOAP");
            myIntent.putExtra(IntentMsgs.IP_ADDRESS.toString(), ipAddress);
            startActivity(myIntent);
        }
    };

    private View.OnClickListener openRestActivity = new View.OnClickListener() {
        public void onClick(View v) {
            Intent myIntent = new Intent(v.getContext(),
                    ServiceActivity.class);
            myIntent.putExtra(IntentMsgs.SERVICE_TYPE.toString(), "REST");
            myIntent.putExtra(IntentMsgs.IP_ADDRESS.toString(), ipAddress);
            startActivity(myIntent);
        }
    };

    private View.OnClickListener openSettingsActivty = new View.OnClickListener() {
        public void onClick(View v) {
            openSettings();
        }
    };

    private void openSettings(){
        Intent myIntent = new Intent(MenuActivity.this,
                EnterIpAddressActivity.class);
        startActivityForResult(myIntent,
                ActivityResults.GET_IP.ordinal());
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        switch (values[requestCode]) {
            case GET_IP:
                if (resultCode == Activity.RESULT_OK) {
                    ipAddress = data.getStringExtra(IntentMsgs.IP_ADDRESS.toString());
                }
                break;
        }
    }
}
