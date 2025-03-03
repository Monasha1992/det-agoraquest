# Python server

This is the python server that connects to a BLE device and sends the data to the unity project and Esp32 over MQTT
broker.

## Getting Started

### Install dependencies

```shell
  pip install bleak paho-mqtt
```

### Run the server

```shell
  python main.py
```

## Prerequisites

Run the device_scanner.py to get the device address and name of the BLE device you want to connect to.
then replace the address in the main.py file with the address of the device you want to connect to.

```python
BLE_DEVICE_ADDRESS = "FF07AE86-5F0E-CA26-5A58-48930409DB8F"  # Replace with your BLE heart rate sensor address
HEART_RATE_CHARACTERISTIC_UUID = "00002A37-0000-1000-8000-00805f9b34fb"  # Standard heart rate UUID
```

and then run the device_scanner.py file to get the address of the BLE device you want to connect to.

```shell
  python device_scanner.py
```

Example result:

```text
  ...
  Device: Rhythm 24 9905, Address: FF07AE86-5F0E-CA26-5A58-48930409DB8F
  ...
```