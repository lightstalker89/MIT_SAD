package com.grill.car2car;

import android.app.ListFragment;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.net.wifi.p2p.WifiP2pDevice;
import android.net.wifi.p2p.WifiP2pDeviceList;
import android.net.wifi.p2p.WifiP2pManager.PeerListListener;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.AdapterView.OnItemLongClickListener;

import java.util.ArrayList;
import java.util.List;

import com.grill.car2car.helpers.WiFiP2pServiceContainer;
import com.grill.car2car.interfaces.DeviceActionListener;

/**
 * A ListFragment that displays available peers on discovery and requests the
 * parent activity to handle user interaction events
 */
public class DeviceListFragment extends ListFragment implements
		PeerListListener {

	private List<WiFiP2pServiceContainer> peers = new ArrayList<WiFiP2pServiceContainer>();
	ProgressDialog progressDialog = null;
	View mContentView = null;
	private WifiP2pDevice device;

	@Override
	public void onActivityCreated(Bundle savedInstanceState) {
		super.onActivityCreated(savedInstanceState);
		this.setListAdapter(new WiFiPeerListAdapter(getActivity(),
				R.layout.row_devices, peers));

		getListView().setOnItemLongClickListener(disconnectFromPeer);

	}

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
		mContentView = inflater.inflate(R.layout.device_list, null);
		return mContentView;
	}

	/**
	 * @return this device
	 */
	public WifiP2pDevice getDevice() {
		return device;
	}

	private static String getDeviceStatus(int deviceStatus) {
		Log.d(MainActivity.TAG, "Peer status :" + deviceStatus);
		switch (deviceStatus) {
		case WifiP2pDevice.AVAILABLE:
			return "Available";
		case WifiP2pDevice.INVITED:
			return "Invited";
		case WifiP2pDevice.CONNECTED:
			return "Connected";
		case WifiP2pDevice.FAILED:
			return "Failed";
		case WifiP2pDevice.UNAVAILABLE:
			return "Unavailable";
		default:
			return "Unknown";

		}
	}

	/**
	 * Initiate a connection with the peer.
	 */
	@Override
	public void onListItemClick(ListView l, View v, int position, long id) {
		((DeviceClickListener) getActivity()).connectP2p((WiFiP2pServiceContainer) l
				.getItemAtPosition(position));
		((TextView) v.findViewById(R.id.device_details)).setText("Connecting");
	}

	private OnItemLongClickListener disconnectFromPeer = new OnItemLongClickListener() {

		@Override
		public boolean onItemLongClick(AdapterView<?> parent, View view,
				int position, long id) {
			((DeviceActionListener) getActivity()).disconnect();
			return false;
		}
	};

	interface DeviceClickListener {
		public void connectP2p(WiFiP2pServiceContainer wifiP2pService);
	}

	/**
	 * Array adapter for ListFragment that maintains WifiP2pDevice list.
	 */
	public class WiFiPeerListAdapter extends
			ArrayAdapter<WiFiP2pServiceContainer> {

		private List<WiFiP2pServiceContainer> items;

		/**
		 * @param context
		 * @param textViewResourceId
		 * @param objects
		 */
		public WiFiPeerListAdapter(Context context, int textViewResourceId,
				List<WiFiP2pServiceContainer> objects) {
			super(context, textViewResourceId, objects);
			items = objects;

		}

		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			View v = convertView;
			if (v == null) {
				LayoutInflater vi = (LayoutInflater) getActivity()
						.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
				v = vi.inflate(R.layout.row_devices, null);
			}
			WiFiP2pServiceContainer service = items.get(position);
			if (service != null) {
				TextView top = (TextView) v.findViewById(R.id.device_name);
				TextView bottom = (TextView) v
						.findViewById(R.id.device_details);
				if (top != null) {
					top.setText(service.device.deviceName);
				}
				if (bottom != null) {
					bottom.setText(getDeviceStatus(service.device.status));
				}
			}

			return v;

		}
	}

	/**
	 * Update UI for this device.
	 * 
	 * @param device
	 *            WifiP2pDevice object
	 */
	public void updateThisDevice(WifiP2pDevice device) {
		this.device = device;
		TextView view = (TextView) mContentView.findViewById(R.id.my_name);
		view.setText(device.deviceName);
		view = (TextView) mContentView.findViewById(R.id.my_status);
		view.setText(getDeviceStatus(device.status));
	}

	public void clearPeers() {
		peers.clear();
		((WiFiPeerListAdapter) getListAdapter()).notifyDataSetChanged();
	}

	/**
     * 
     */
	public void onInitiateDiscovery() {
		if (progressDialog != null && progressDialog.isShowing()) {
			progressDialog.dismiss();
		}
		progressDialog = ProgressDialog.show(getActivity(),
				"Press back to cancel", "finding peers", true, true,
				new DialogInterface.OnCancelListener() {

					@Override
					public void onCancel(DialogInterface dialog) {

					}
				});
	}

	@Override
	public void onPeersAvailable(WifiP2pDeviceList peerList) {
		if (progressDialog != null && progressDialog.isShowing()) {
            progressDialog.dismiss();
        }
        peers.clear();
        for (WifiP2pDevice deviceItem : peerList.getDeviceList()) {
        	WiFiP2pServiceContainer device = new WiFiP2pServiceContainer();
        	device.device = deviceItem;
        	device.instanceName = deviceItem.deviceName;
        	device.serviceRegistrationType = deviceItem.primaryDeviceType;
        	peers.add(device);
		}
        
        ((WiFiPeerListAdapter) getListAdapter()).notifyDataSetChanged();
        if (peers.size() == 0) {
            Log.d(MainActivity.TAG, "No devices found");
            return;
        }
	}
}
