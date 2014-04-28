package com.grill.car2car.interfaces;

import com.grill.car2car.connection.CommunicationManager;

public interface IMapFragment {
	public void setCommunicationManager(CommunicationManager manager);
	public void setMarkerOnPosition(double latitude, double longtitude, String macAddress);
}
