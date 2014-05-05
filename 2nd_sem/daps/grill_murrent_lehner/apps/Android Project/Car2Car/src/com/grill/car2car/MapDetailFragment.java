package com.grill.car2car;


import java.util.HashMap;
import java.util.Map;

import com.google.android.gms.maps.CameraUpdate;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.Marker;
import com.google.android.gms.maps.model.MarkerOptions;
import com.grill.car2car.connection.CommunicationManager;
import com.grill.car2car.interfaces.IMapFragment;

import android.app.Fragment;
import android.content.Context;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationManager;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageButton;
import android.widget.Toast;

public class MapDetailFragment extends Fragment implements LocationListener, IMapFragment {

	Map<String, Marker> markerList = new HashMap<String, Marker>();
	
	private CommunicationManager manager;
	private double currentLongitude;
	private double currentLatitude;
	
	// Google Map
    private GoogleMap googleMap;
    private LocationManager locationManager;
    private ImageButton btnSend;
    
    // Mapp settings
    private final long MIN_TIME = 400;
    private final float MIN_DISTANCE = 1000;
	
    @Override
	public void onActivityCreated(Bundle savedInstanceState) {
		super.onActivityCreated(savedInstanceState);
		
		btnSend = (ImageButton) getView().findViewById(R.id.btnGoogle);
		btnSend.setOnClickListener(sendInfo);
		
		locationManager = (LocationManager) getActivity().getSystemService(Context.LOCATION_SERVICE);
	    locationManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, MIN_TIME, MIN_DISTANCE, (LocationListener) this);
		// locationManager.requestLocationUpdates(LocationManager.NETWORK_PROVIDER, MIN_TIME, MIN_DISTANCE, (LocationListener) this);
		
		try {
            // Loading map
            initilizeMap();
 
        } catch (Exception e) {
            e.printStackTrace();
        }
	}
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container,
            Bundle savedInstanceState) {
        return inflater.inflate(R.layout.google_maps, null);
    }
	
	/**
     * function to load map. If map is not created it will create it for you
     * */
    private void initilizeMap() {
        if (googleMap == null) {
            googleMap = ((MapFragment) getFragmentManager().findFragmentById(
                    R.id.map)).getMap();
 
            // check if map is created successfully or not
            if (googleMap == null) {
                Toast.makeText(getActivity().getApplicationContext(),
                        "Sorry! unable to create maps", Toast.LENGTH_SHORT)
                        .show();
            }
            else {
            	googleMap.setMyLocationEnabled(true);
            	
            	/*Location location = googleMap.getMyLocation();
            	LatLng coordinate = new LatLng(location.getLatitude(), location.getLongitude());
            	CameraUpdate yourLocation = CameraUpdateFactory.newLatLngZoom(coordinate, 5);
            	googleMap.animateCamera(yourLocation);*/
			}
        }
    }
 
    @Override
	public void onResume() {
        super.onResume();
        initilizeMap();
    }

	@Override
	public void onLocationChanged(Location location) {
		LatLng latLng = new LatLng(location.getLatitude(), location.getLongitude());
	    CameraUpdate cameraUpdate = CameraUpdateFactory.newLatLngZoom(latLng, 18);
	    googleMap.animateCamera(cameraUpdate);
	    locationManager.removeUpdates((LocationListener) this);
	    
	    currentLatitude = location.getLatitude();
	    currentLongitude = location.getLongitude();
	    if (this.manager != null) {
	    	sendCoordinates(location.getLatitude(), location.getLongitude());
		}
	}

	View.OnClickListener sendInfo = new View.OnClickListener() {
		public void onClick(View v) {
			if (MapDetailFragment.this.manager != null) {
				
		    	sendCoordinates(currentLatitude, currentLongitude);
			}
		}
	};
	
	private void sendCoordinates(double latitude, double longitude) {
		byte[] latitudeBytes = new byte[8];
		java.nio.ByteBuffer.wrap(latitudeBytes).putDouble(latitude);
		byte[] longitudeBytes = new byte[8];
		java.nio.ByteBuffer.wrap(longitudeBytes).putDouble(longitude);

		byte[] coordinates = new byte[latitudeBytes.length + longitudeBytes.length];
		for (int i = 0; i < coordinates.length; ++i) {
			coordinates[i] = i < latitudeBytes.length ? latitudeBytes[i] : longitudeBytes[i - latitudeBytes.length];
		}
    	manager.write(coordinates);
	}
	
	@Override
	public void onStatusChanged(String provider, int status, Bundle extras) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onProviderEnabled(String provider) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onProviderDisabled(String provider) {
		// TODO Auto-generated method stub
	}

	@Override
	public void setCommunicationManager(CommunicationManager manager) {
		this.manager = manager;
	}

	@Override
	public void setMarkerOnPosition(double latitude, double longitude, String macAddress) {
		
		if (markerList.containsKey(macAddress)) {
			Marker currentMarker = markerList.get(macAddress);
			currentMarker.remove();
			markerList.remove(macAddress);
		}
		Marker marker = googleMap.addMarker(new MarkerOptions().position(new LatLng(latitude, longitude)));
		markerList.put(macAddress, marker);
	}
}
