package com.grill.customerserviceclient.userinterface;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.grill.customerserviceclient.R;
import com.grill.customerserviceclient.objects.Customer;
import com.grill.customerserviceclient.objects.ParcelableArrayList;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Flo on 16.12.2014.
 */
public class CustomerArrayAdapter<T> extends ArrayAdapter<T> {

    private final Context context;
    private final List<T> list;

    public CustomerArrayAdapter(Context context, int resource, List<T> list) {
        super(context, resource, list);
        this.context = context;
        this.list = list;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        View rowView = convertView;
        ViewHolder view;

        if(rowView == null)
        {
            // Get a new instance of the row layout view
            LayoutInflater inflater = LayoutInflater.from(context);
            rowView = inflater.inflate(R.layout.customer_details, null);

            // Hold the view objects in an object, that way the don't need to be "re-  finded"
            view = new ViewHolder();
            view.customerName = (TextView) rowView.findViewById(R.id.customerName);
            view.customerOrders= (TextView) rowView.findViewById(R.id.customerOrders);

            rowView.setTag(view);
        } else {
            view = (ViewHolder) rowView.getTag();
        }

        /** Set data to your Views. */
        Customer item = (Customer)list.get(position);
        view.customerName.setText(item.getCustomerName());
        if(!item.getOrders().isEmpty()){

            view.customerOrders.setText(getOrderString(item.getOrders()));
        }
        else{
            view.customerOrders.setText("- no orders -");
        }

        return rowView;
    }

    private String getOrderString(List<ParcelableArrayList> orders){
        String orderString = "";
        for(int i=0; i < orders.get(0).size(); i++){
            orderString += orders.get(0).get(i) + ", ";
            if(i == 2){ orderString+= "..."; break; };
        }
        return orderString;
    }

    protected static class ViewHolder{
        protected TextView customerName;
        protected TextView customerOrders;
    }
}
