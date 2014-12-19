package com.grill.customerserviceclient.objects;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;

/**
 * Created by Flo on 17.12.2014.
 */
public class ParcelableArrayList extends ArrayList<String> implements Parcelable {

    private static final long serialVersionUID = -8516873361351845306L;

    public ParcelableArrayList() {
        super();
    }

    private ParcelableArrayList(Parcel in) {
        in.readList(this, String.class.getClassLoader());
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeList(this);
    }

    public static final Parcelable.Creator<ParcelableArrayList> CREATOR =
            new Parcelable.Creator<ParcelableArrayList>() {
                public ParcelableArrayList createFromParcel(Parcel in) {
                    return new ParcelableArrayList(in);
                }

                public ParcelableArrayList[] newArray(int size) {
                    return new ParcelableArrayList[size];
                }
            };
}
