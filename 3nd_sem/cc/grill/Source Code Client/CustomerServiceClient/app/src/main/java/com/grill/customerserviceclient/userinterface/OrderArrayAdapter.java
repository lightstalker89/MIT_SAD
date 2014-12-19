package com.grill.customerserviceclient.userinterface;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.grill.customerserviceclient.R;
import com.grill.customerserviceclient.objects.Customer;
import com.grill.customerserviceclient.objects.ParcelableArrayList;

import java.util.List;

/**
 * Created by Flo on 16.12.2014.
 */
public class OrderArrayAdapter<T> extends ArrayAdapter<T> {

    private final Context context;
    private final List<T> list;

    public OrderArrayAdapter(Context context, int resource, List<T> list) {
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
            rowView = inflater.inflate(R.layout.order_details, null);

            // Hold the view objects in an object, that way the don't need to be "re-  finded"
            view = new ViewHolder();
            view.image = (ImageView) rowView.findViewById(R.id.imgIcon);
            view.customerOrder = (TextView) rowView.findViewById(R.id.customerOrder);

            rowView.setTag(view);
        } else {
            view = (ViewHolder) rowView.getTag();
        }

        /** Set data to your Views. */
        List<String> item = (List<String>)list.get(position);

        view.customerOrder.setText(getOrderString(item));
        view.image.setImageResource(R.drawable.shopping_trolley);
        return rowView;
    }

    private String getOrderString(List<String> orders){
        String orderString = "";
        for (String order: orders){
            orderString += order + ", ";
        }
        if(orderString.length() < 1){ return orderString; }
        return orderString.substring(0, orderString.length()-2);
    }

    protected static class ViewHolder{
        protected ImageView image;
        protected TextView customerOrder;
    }
}
