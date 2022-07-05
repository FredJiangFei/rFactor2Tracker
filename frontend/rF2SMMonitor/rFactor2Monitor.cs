using rF2SMMonitor.rFactor2Data;

namespace rF2SMMonitor
{
  public class rFactor2Monitor
  {
    const int CONNECTION_RETRY_INTERVAL_MS = 1000;
    const int DISCONNECTED_CHECK_INTERVAL_MS = 15000;
    const float DEGREES_IN_RADIAN = 57.2957795f;
    const int LIGHT_MODE_REFRESH_MS = 500;
    const bool useStockCarRulesPlugin = false;
    bool connected = false;

    MappedBuffer<rF2Telemetry> telemetryBuffer = new MappedBuffer<rF2Telemetry>(rFactor2Constants.MM_TELEMETRY_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
    rF2Telemetry telemetry = new rF2Telemetry();

    public rF2Telemetry GetRF2Telemetry() {
        telemetryBuffer.ClearStats();
        while (!connected)
        {
            try
            {
                telemetryBuffer.Connect();
                connected = true;
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }

        while (connected)
        {
            telemetryBuffer.GetMappedData(ref telemetry);
            return telemetry;
        }
        return telemetry;
    } 

    void Disconnect()
    {
        telemetryBuffer.Disconnect();
        connected = false;
    }
  }
}