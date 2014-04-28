package com.grill.car2car.helpers;

import com.grill.car2car.interfaces.IWifiP2P;

import android.app.Activity;

public abstract class WifiActivity extends Activity implements IWifiP2P{

	@Override
	public abstract void setIsWifiP2pEnabled(boolean value);
	@Override
	public abstract void resetData();

}
