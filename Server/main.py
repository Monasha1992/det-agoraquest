import asyncio
import struct
import paho.mqtt.client as mqtt
from bleak import BleakClient

# BLE and MQTT Configuration
BLE_DEVICE_ADDRESS = "FF07AE86-5F0E-CA26-5A58-48930409DB8F"  # Replace with your BLE heart rate sensor address
HEART_RATE_CHARACTERISTIC_UUID = "00002A37-0000-1000-8000-00805f9b34fb"  # Standard heart rate UUID
MQTT_BROKER = "broker.emqx.io"  # Replace with your MQTT broker address
MQTT_PORT = 1883
MQTT_TOPIC_HR = "sensor/heart_rate"
ESP32_MQTT_TOPIC = "esp32/control"

# Threshold for heart rate to trigger ESP32 action
HR_THRESHOLD = 85

# MQTT Client Setup
mqtt_client = mqtt.Client()
mqtt_client.connect(MQTT_BROKER, MQTT_PORT, 60)
mqtt_client.loop_start()


async def heart_rate_callback(sender: int, data: bytearray):
    """Handles incoming heart rate data and sends it via MQTT."""
    heart_rate = struct.unpack("B", data[1:2])[0]
    if heart_rate > HR_THRESHOLD:
        print(f"\033[31mHeart Rate: {heart_rate} BPM\033[0m")
    else:
        print(f"\033[32mHeart Rate: {heart_rate} BPM\033[0m")

    # Publish heart rate to MQTT
    mqtt_client.publish(MQTT_TOPIC_HR, heart_rate)

    # If heart rate exceeds threshold, send command to ESP32
    if heart_rate > HR_THRESHOLD:
        mqtt_client.publish(ESP32_MQTT_TOPIC, "activate")
    else:
        mqtt_client.publish(ESP32_MQTT_TOPIC, "deactivate")


async def main():
    """Main function to connect to BLE device and subscribe to heart rate notifications."""
    async with BleakClient(BLE_DEVICE_ADDRESS) as client:
        if await client.is_connected():
            print("Connected to BLE Heart Rate Sensor")
            await client.start_notify(HEART_RATE_CHARACTERISTIC_UUID, heart_rate_callback)

            # Keep the loop running
            while True:
                await asyncio.sleep(0.1)
        else:
            print("Failed to connect to BLE device")


if __name__ == "__main__":
    asyncio.run(main())
