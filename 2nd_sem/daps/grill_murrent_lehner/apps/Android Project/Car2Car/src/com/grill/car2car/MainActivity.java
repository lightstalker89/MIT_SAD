package com.grill.car2car;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

import com.grill.car2car.DeviceListFragment.DeviceClickListener;
import com.grill.car2car.DeviceListFragment.WiFiPeerListAdapter;
import com.grill.car2car.connection.ClientSocketHandler;
import com.grill.car2car.connection.CommunicationManager;
import com.grill.car2car.connection.GroupOwnerSocketHandler;
import com.grill.car2car.connection.WifiDirectBroadcastReceiver;
import com.grill.car2car.helpers.WiFiP2pServiceContainer;
import com.grill.car2car.helpers.WifiActivity;
import com.grill.car2car.interfaces.DeviceActionListener;
import com.grill.car2car.interfaces.IMapFragment;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.net.wifi.WpsInfo;
import android.net.wifi.p2p.WifiP2pConfig;
import android.net.wifi.p2p.WifiP2pDevice;
import android.net.wifi.p2p.WifiP2pInfo;
import android.net.wifi.p2p.WifiP2pManager;
import android.net.wifi.p2p.WifiP2pManager.ActionListener;
import android.net.wifi.p2p.WifiP2pManager.Channel;
import android.net.wifi.p2p.WifiP2pManager.ConnectionInfoListener;
import android.net.wifi.p2p.WifiP2pManager.DnsSdServiceResponseListener;
import android.net.wifi.p2p.WifiP2pManager.DnsSdTxtRecordListener;
import android.net.wifi.p2p.nsd.WifiP2pDnsSdServiceInfo;
import android.net.wifi.p2p.nsd.WifiP2pDnsSdServiceRequest;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.os.Vibrator;
import android.provider.Settings;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.widget.Toast;

public class MainActivity extends WifiActivity implements DeviceActionListener, DeviceClickListener,
	ConnectionInfoListener {

	private MapDetailFragment mapFragment;
	private String macAddress;
	
	// TXT RECORD properties
    public static final String TXTRECORD_PROP_AVAILABLE = "available";
    public static final String SERVICE_INSTANCE = "_wificar2car";
    public static final String SERVICE_REG_TYPE = "_presence._tcp";
	
    public static final int SERVER_PORT = 4545;
    
	public static final int MESSAGE_READ = 0x401;
	public static final int MY_HANDLE = 0x402;

	public static final String TAG = "wifidirectdemo";
	private boolean isWifiP2pEnabled;

	private final IntentFilter intentFilter = new IntentFilter();
	private WifiP2pManager manager;
	private Channel channel;
	private BroadcastReceiver receiver;
	private WifiP2pDnsSdServiceRequest serviceRequest;

	private Vibrator vib;
	private final int DURATION = 70;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_main);
		vib = (Vibrator) getSystemService(Context.VIBRATOR_SERVICE);

		WifiInfo info = ((WifiManager) getSystemService(Context.WIFI_SERVICE)).getConnectionInfo();
		macAddress = info.getMacAddress();
		
		// Indicates whether Wi-Fi P2P is enabled
		intentFilter.addAction(WifiP2pManager.WIFI_P2P_STATE_CHANGED_ACTION);
		// Indicates that the available peer list has changed.
		intentFilter.addAction(WifiP2pManager.WIFI_P2P_PEERS_CHANGED_ACTION);
		// Indicates the state of Wi-Fi P2P connectivity has changed.
		intentFilter
				.addAction(WifiP2pManager.WIFI_P2P_CONNECTION_CHANGED_ACTION);
		// Indicates this device's configuration details have changed.
		intentFilter
				.addAction(WifiP2pManager.WIFI_P2P_THIS_DEVICE_CHANGED_ACTION);

		manager = (WifiP2pManager) getSystemService(Context.WIFI_P2P_SERVICE);
		channel = manager.initialize(this, getMainLooper(), null);
		startRegistrationAndDiscovery();
	}

	/**
     * Registers a local service and then initiates a service discovery
     */
    private void startRegistrationAndDiscovery() {
        Map<String, String> record = new HashMap<String, String>();
        record.put(TXTRECORD_PROP_AVAILABLE, "visible");

        WifiP2pDnsSdServiceInfo service = WifiP2pDnsSdServiceInfo.newInstance(SERVICE_INSTANCE, SERVICE_REG_TYPE, record);
        manager.addLocalService(channel, service, new ActionListener() {

            @Override
            public void onSuccess() {
            	Toast.makeText(MainActivity.this, "Added Local Service", Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onFailure(int error) {
            	Toast.makeText(MainActivity.this, "Failed to add a service", Toast.LENGTH_SHORT).show();
            }
        });

        discoverService();
    }
	
    private void discoverService() {

        /*
         * Register listeners for DNS-SD services. These are callbacks invoked
         * by the system when a service is actually discovered.
         */

        manager.setDnsSdResponseListeners(channel,
                new DnsSdServiceResponseListener() {

                    @Override
                    public void onDnsSdServiceAvailable(String instanceName,
                            String registrationType, WifiP2pDevice srcDevice) {

                        // A service has been discovered. Is this our app?

                        if (instanceName.equalsIgnoreCase(SERVICE_INSTANCE)) {

                            // update the UI and add the item the discovered
                            // device.
                            DeviceListFragment fragment = (DeviceListFragment) getFragmentManager()
                                    .findFragmentById(R.id.frag_list);
                            if (fragment != null) {
                            	WiFiPeerListAdapter adapter = ((WiFiPeerListAdapter) fragment.getListAdapter());
                                WiFiP2pServiceContainer service = new WiFiP2pServiceContainer();
                                service.device = srcDevice;
                                service.instanceName = instanceName;
                                service.serviceRegistrationType = registrationType;
                                adapter.add(service);
                                adapter.notifyDataSetChanged();
                                Log.d(TAG, "onBonjourServiceAvailable "
                                        + instanceName);
                            }
                        }

                    }
                }, new DnsSdTxtRecordListener() {

                    /**
                     * A new TXT record is available. Pick up the advertised
                     * buddy name.
                     */
                    @Override
                    public void onDnsSdTxtRecordAvailable(
                            String fullDomainName, Map<String, String> record,
                            WifiP2pDevice device) {
                        Log.d(TAG,
                                device.deviceName + " is "
                                        + record.get(TXTRECORD_PROP_AVAILABLE));
                    }
                });

        // After attaching listeners, create a service request and initiate
        // discovery.
        serviceRequest = WifiP2pDnsSdServiceRequest.newInstance();
        manager.addServiceRequest(channel, serviceRequest,
                new ActionListener() {

                    @Override
                    public void onSuccess() {
                    	Toast.makeText(MainActivity.this, "Added service discovery request", Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onFailure(int arg0) {
                    	Toast.makeText(MainActivity.this, "Failed adding service discovery request", Toast.LENGTH_SHORT).show();
                    }
                });
        manager.discoverServices(channel, new ActionListener() {

            @Override
            public void onSuccess() {
            	Toast.makeText(MainActivity.this, "Service discovery initiated", Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onFailure(int arg0) {
            	Toast.makeText(MainActivity.this, "Service discovery failed", Toast.LENGTH_SHORT).show();
            }
        });
    }
    
	@Override
	protected void onResume() {
		super.onResume();
		receiver = new WifiDirectBroadcastReceiver(manager, channel, this);
		registerReceiver(receiver, intentFilter);
	}

	@Override
	public void onPause() {
		super.onPause();
		unregisterReceiver(receiver);
	}

	@Override
	public void setIsWifiP2pEnabled(boolean value) {
		this.isWifiP2pEnabled = value;

	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		MenuInflater inflater = getMenuInflater();
		inflater.inflate(R.menu.action_items, menu);
		return true;
	}

	/*
	 * (non-Javadoc)
	 * 
	 * @see android.app.Activity#onOptionsItemSelected(android.view.MenuItem)
	 */
	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		switch (item.getItemId()) {
		case R.id.atn_direct_enable:
			if (manager != null && channel != null) {

				// Since this is the system wireless settings activity, it's
				// not going to send us a result. We will be notified by
				// WiFiDeviceBroadcastReceiver instead.

				startActivity(new Intent(Settings.ACTION_WIRELESS_SETTINGS));
			} else {
				Log.e(TAG, "channel or manager is null");
			}
			return true;

		case R.id.atn_direct_discover:
			if (!isWifiP2pEnabled) {
				Toast.makeText(MainActivity.this, R.string.p2p_off_warning,
						Toast.LENGTH_SHORT).show();
				return true;
			}
			final DeviceListFragment fragment = (DeviceListFragment) getFragmentManager()
					.findFragmentById(R.id.frag_list);
			fragment.onInitiateDiscovery();
			manager.discoverPeers(channel, new WifiP2pManager.ActionListener() {

				@Override
				public void onSuccess() {
					Toast.makeText(MainActivity.this, "Discovery Initiated",
							Toast.LENGTH_SHORT).show();
				}

				@Override
				public void onFailure(int reasonCode) {
					Toast.makeText(MainActivity.this,
							"Discovery Failed : " + reasonCode,
							Toast.LENGTH_SHORT).show();
				}
			});
			return true;
		default:
			return super.onOptionsItemSelected(item);
		}
	}

	@Override
	public void resetData() {
		DeviceListFragment fragmentList = (DeviceListFragment) getFragmentManager()
				.findFragmentById(R.id.frag_list);

		if (fragmentList != null) {
			fragmentList.clearPeers();
		}
	}

	@Override
	public void cancelDisconnect() {
		/*
		 * A cancel abort request by user. Disconnect i.e. removeGroup if
		 * already connected. Else, request WifiP2pManager to abort the ongoing
		 * request
		 */
		if (manager != null) {
			final DeviceListFragment fragment = (DeviceListFragment) getFragmentManager()
					.findFragmentById(R.id.frag_list);
			if (fragment.getDevice() == null
					|| fragment.getDevice().status == WifiP2pDevice.CONNECTED) {
				disconnect();
			} else if (fragment.getDevice().status == WifiP2pDevice.AVAILABLE
					|| fragment.getDevice().status == WifiP2pDevice.INVITED) {

				manager.cancelConnect(channel, new ActionListener() {

					@Override
					public void onSuccess() {
						Toast.makeText(MainActivity.this,
								"Aborting connection", Toast.LENGTH_SHORT)
								.show();
					}

					@Override
					public void onFailure(int reasonCode) {
						Toast.makeText(
								MainActivity.this,
								"Connect abort request failed. Reason Code: "
										+ reasonCode, Toast.LENGTH_SHORT)
								.show();
					}
				});
			}
		}
	}

	// Alter versuch
	@Override
	public void connect(WifiP2pConfig config) {
		manager.connect(channel, config, new ActionListener() {

			@Override
			public void onSuccess() {
				// WiFiDirectBroadcastReceiver will notify us. Ignore for now.
			}

			@Override
			public void onFailure(int reason) {
				Toast.makeText(MainActivity.this, "Connect failed. Retry.",
						Toast.LENGTH_SHORT).show();
			}
		});
	}

	@Override
	public void disconnect() {
		/*
		 * final DeviceDetailFragment fragment = (DeviceDetailFragment)
		 * getFragmentManager() .findFragmentById(R.id.frag_detail);
		 * fragment.resetViews();
		 */
		manager.removeGroup(channel, new ActionListener() {

			@Override
			public void onFailure(int reasonCode) {
				Log.d(TAG, "Disconnect failed. Reason :" + reasonCode);

			}

			@Override
			public void onSuccess() {
				vib.vibrate(DURATION);
				Toast.makeText(MainActivity.this, "Successfully disconnected",
						Toast.LENGTH_SHORT).show();
			}
		});
	}

	// Eigenes interface
	@Override
    public void connectP2p(WiFiP2pServiceContainer service) {
        WifiP2pConfig config = new WifiP2pConfig();
        config.deviceAddress = service.device.deviceAddress;
        config.wps.setup = WpsInfo.PBC;
        if (serviceRequest != null)
            manager.removeServiceRequest(channel, serviceRequest,
                    new ActionListener() {

                        @Override
                        public void onSuccess() {
                        }

                        @Override
                        public void onFailure(int arg0) {
                        }
                    });

        manager.connect(channel, config, new ActionListener() {

            @Override
            public void onSuccess() {
            	Toast.makeText(MainActivity.this, "Connecting to service",
						Toast.LENGTH_SHORT).show();
            }

            @Override
            public void onFailure(int errorCode) {
                //appendStatus("Failed connecting to service");
            }
        });
    }
	
	protected final Handler mHandler = new Handler() {
		@Override
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case MESSAGE_READ:
					byte[] readBuf = (byte[]) msg.obj;
					int count = msg.arg1;
					
					byte[] test1 = Arrays.copyOfRange(readBuf, 0, 8);
					byte[] test2 = Arrays.copyOfRange(readBuf, 8, 16);
					byte[] test3 = Arrays.copyOfRange(readBuf, 16, count);
					double currentLatitude = ByteBuffer.wrap(test1).getDouble();
					double currentLongitude = ByteBuffer.wrap(test2).getDouble();
					String macAddress = new String(test3);
					((IMapFragment) mapFragment).setMarkerOnPosition(currentLatitude, currentLongitude, macAddress);
				break;

			case MY_HANDLE:
				Toast.makeText(MainActivity.this, "Communication manager successfully registered", Toast.LENGTH_LONG).show();
				mapFragment = (MapDetailFragment) getFragmentManager()
	                    .findFragmentById(R.id.frag_maps);
				Object obj = msg.obj;
	        	((IMapFragment) mapFragment).setCommunicationManager((CommunicationManager)obj);
			}
		}
	};
	
	@Override
    public void onConnectionInfoAvailable(WifiP2pInfo p2pInfo) {
        Thread handler = null;
        /*
         * The group owner accepts connections using a server socket and then spawns a
         * client socket for every client. This is handled by {@code
         * GroupOwnerSocketHandler}
         */

        if (p2pInfo.isGroupOwner) {
            Log.d(TAG, "Connected as group owner");
            try {
                handler = new GroupOwnerSocketHandler(
                        mHandler, macAddress);
                handler.start();
            } catch (IOException e) {
                Log.d(TAG,
                        "Failed to create a server thread - " + e.getMessage());
                return;
            }
        } else {
            Log.d(TAG, "Connected as peer");
            handler = new ClientSocketHandler(
                    mHandler,
                    p2pInfo.groupOwnerAddress, macAddress);
            handler.start();
        }
    }
}
