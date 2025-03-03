import asyncio
from bleak import BleakClient, BleakScanner


async def scan_devices():
    devices = await BleakScanner.discover()
    for device in devices:
        print(f"Device: {device.name}, Address: {device.address}")


# SCOSCHE_ADDRESS = "FF07AE86-5F0E-CA26-5A58-48930409DB8F"  # Replace with your device's address
# HEART_RATE_UUID = "00002A37-0000-1000-8000-00805f9b34fb"
#
#
# def heart_rate_callback(sender, data):
#     heart_rate = data[1]  # Extract the heart rate value
#     print(f"Heart Rate: {heart_rate} bpm")
#
#
# async def connect_to_scocche():
#     async with BleakClient(SCOSCHE_ADDRESS) as client:
#         if await client.is_connected():
#             print(f"Connected to {SCOSCHE_ADDRESS}")
#
#             # Subscribe to heart rate notifications
#             await client.start_notify(HEART_RATE_UUID, heart_rate_callback)
#
#             # Keep the script running to listen for data
#             await asyncio.sleep(30)  # Adjust duration as needed
#             await client.stop_notify(HEART_RATE_UUID)


asyncio.run(scan_devices())
# asyncio.run(connect_to_scocche())
