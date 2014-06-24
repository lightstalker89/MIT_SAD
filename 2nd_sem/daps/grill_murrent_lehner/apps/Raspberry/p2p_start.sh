##!/bin/sh
#
# Script for enabling wifi p2p
#

clear
# reset wlan0 to 802.11 wifi mode
sudo iwpriv wlan0 p2p_set enable=0

echo "Wifi p2p will be enabled..."

# define ifname
echo "Define name for Wifi direct AP:"
read ifname
echo "Interface name is $ifname!"
sudo iwpriv wlan0 p2p_set setDN=$ifname

# set intent (softap, paring device will broadcast ip adress)
sudo iwpriv wlan0 p2p_set intent=15

# set SSID (found in p2p_hostapd.conf)
sudo iwpriv wlan0 p2p_set ssid=DIRECT-RT

# set channel (found in p2p_hostapd.conf)
#sudo iwpriv wlan0 p2p_set 

# enable p2p functionality in chipset
sudo iwpriv wlan0 p2p_set enable=1

# scan for wifi direct devices
echo "Scanning for Wifi direct devices..."
sudo iwlist wlan0 scan

# start provisioning discovery
echo "Please insert Mac Adress for provisioning discovery:"
read macPairDevice
mode="_pbc"
concatString="$macPairDevice$mode"
echo $concatString
sudo iwpriv wlan0 p2p_set prov_disc=$concatString

# check provisioning state
check="00"

while [ "$check" != "07" ]
do
	echo "check p2p status"
	response=`sudo iwpriv wlan0 p2p_get status`
	check=`sudo echo $response| cut -d'=' -f 2`
	echo $check
done

if [ "$check" = "07" ]
then
	sudo iwpriv wlan0 p2p_set got_wpsinfo=3
else
	echo "Provisioning didn't work!!!"
fi


# start group negotiation
sudo iwpriv wlan0 p2p_set nego=$macPairDevice

# check negotiation process
check="00"
counter=0
while [ "$check" != "10" ]
do
	counter=$((counter+1))
	if [ "$counter" -gt 20 ]
	then
		sudo iwpriv wlan0 p2p_set nego=$macPairDevice
		echo "start negotiation again"
		counter=0
	fi
        echo "check p2p status"
        response=`sudo iwpriv wlan0 p2p_get status`
        check=`sudo echo $response| cut -d'=' -f 2`
        echo $check
done

if [ "$check" = "10" ]
then
        echo "Negotiation successful!!"

# 	Unfortunatelly not working at the moment wpa/hastapd problems on raspberry...
#	sudo ./wpa_supplicant -i wlan0 -c ./wpa_0_8.conf -B
#	sudo ./wpa_cli wps_pbc
else
        echo "Negotiation didn't work!!!" 
fi

# start udp server
#echo "Start UDP Server:"
#./udpReceive.py

# check signal strength
#while true;
#do
#	iwconfig wlan0 | grep -i quality
#done
