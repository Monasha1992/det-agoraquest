# Arduino ESP32

This is the arduino code that listens to the server's mqtt topic and input or output hardware devices' responses according
to them.

## Getting Started

***Verify and Upload*** the code to the ESP32 board using Arduino IDE.

## Prerequisites

* install [PubSubClient](https://pubsubclient.knolleary.net/) dependency in Arduino IDE.

* Fill the required values for the following variables in the code:

```arduino
    // WiFi and MQTT Configuration
    const char* ssid = "~mnsh-asus-2.4Ghz";       // Replace with your WiFi SSID
    const char* password = "ZXHNF660"; // Replace with your WiFi Password
    const char* mqtt_server = "broker.emqx.io"; // Replace with your MQTT broker address
    const int mqtt_port = 1883;
    const char* mqtt_topic = "esp32/control";  // MQTT Topic to subscribe
```