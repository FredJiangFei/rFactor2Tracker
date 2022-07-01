// See https://aka.ms/new-console-template for more information
using MQTT;
using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;

var monitor = new rFactor2Monitor();
var telemetry = new rF2Telemetry {
   mBytesUpdatedHint = 10,
   mNumVehicles = 20
}; // monitor.GetRF2Telemetry();

var mqttTest = new MQTTTest();
await mqttTest.Send(telemetry);