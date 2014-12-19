package com.grill.customerserviceclient.objects;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Flo on 16.12.2014.
 */
public class Customer implements Parcelable {

    private String name;

    public String getCustomerName(){
        return this.name;
    }

    private List<ParcelableArrayList> orders;

    // Allow to change form outside
    public List<ParcelableArrayList> getOrders(){
        return orders;
    }

    public Customer(String name, List<ParcelableArrayList> orders){
        this.name = name;
        this.orders = orders;
    }

    public Customer(){

    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(name);
        dest.writeTypedList(orders);
    }

    // this is used to regenerate your object. All Parcelables must have a CREATOR that implements these two methods
    public static final Parcelable.Creator<Customer> CREATOR = new Parcelable.Creator<Customer>() {
        @Override
        public Customer createFromParcel(Parcel source) {
            return new Customer(source);
        }

        @Override
        public Customer[] newArray(int size) {
            return new Customer[size];
        }
    };

    // example constructor that takes a Parcel and gives you an object populated with it's values
    private Customer(Parcel source) {
        this.name = source.readString();
        this.orders = new ArrayList<ParcelableArrayList>();
        source.readTypedList(orders, ParcelableArrayList.CREATOR);
    }
}
