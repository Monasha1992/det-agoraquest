#include <WiFi.h>
#include <PubSubClient.h>

// WiFi and MQTT Configuration
const char* ssid = "OnePlus";       // Replace with your WiFi SSID
const char* password = "n67gk59t"; // Replace with your WiFi Password
const char* mqtt_server = "broker.emqx.io"; // Replace with your MQTT broker address
const int mqtt_port = 1883;
const char* mqtt_topic = "esp32/control";  // MQTT Topic to subscribe

#define LED_PIN 13  // GPIO2 for built-in LED (or change to your pin)

int motorPin = 8;

// MQTT and WiFi Clients
WiFiClient espClient;
PubSubClient client(espClient);

// Connect to WiFi
void setup_wifi() {
  Serial.print("Connecting to WiFi...");
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("Connected!");
}

// Callback function for incoming MQTT messages
void callback(char* topic, byte* message, unsigned int length) {
  Serial.print("Message received: ");
  String receivedMessage = "";

  for (int i = 0; i < length; i++) {
    receivedMessage += (char)message[i];
  }

  Serial.println(receivedMessage);

  // Control LED based on message
  if (receivedMessage == "activate") {
    digitalWrite(LED_PIN, HIGH);
    Serial.println("LED ON");
    analogWrite(motorPin, 255);
  } else if (receivedMessage == "deactivate") {
    digitalWrite(LED_PIN, LOW);
    Serial.println("LED OFF");
    analogWrite(motorPin, 0);
  }
}

// Connect to MQTT broker
void reconnect() {
  // Get the ESP32 unique MAC address (chip ID)
  uint64_t chipid = ESP.getEfuseMac();
  
  // Convert chip ID to a hex string
  char clientID[20];  
  snprintf(clientID, sizeof(clientID), "ESP32_%04X%08X", 
           (uint16_t)(chipid >> 32), (uint32_t)chipid);
  
  Serial.print("ESP32 Unique Client ID: ");
  Serial.println(clientID);

  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
    if (client.connect(clientID)) {
      Serial.println("Connected!");
      client.subscribe(mqtt_topic);
    } else {
      Serial.println(client.state());
      Serial.print(" ");
      Serial.println("Failed, retrying in 5 seconds...");
      delay(5000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW); // Start with LED OFF

  setup_wifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(callback);
}

void loop() {
  if (!client.connected()) {
    reconnect();
  }
  client.loop();
}
