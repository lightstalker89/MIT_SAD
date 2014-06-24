##!/bin/sh

while true;
do
	iwconfig wlan0 | grep -i quality
done
