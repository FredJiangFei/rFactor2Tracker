using rF2SMMonitor;
using rF2SMMonitor.rFactor2Data;

    const int CONNECTION_RETRY_INTERVAL_MS = 1000;
    const int DISCONNECTED_CHECK_INTERVAL_MS = 15000;
    const float DEGREES_IN_RADIAN = 57.2957795f;
    const int LIGHT_MODE_REFRESH_MS = 500;
    const bool useStockCarRulesPlugin = false;
    bool connected = false;

    // Read buffers:
    MappedBuffer<rF2Telemetry> telemetryBuffer = new MappedBuffer<rF2Telemetry>(rFactor2Constants.MM_TELEMETRY_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
    MappedBuffer<rF2Scoring> scoringBuffer = new MappedBuffer<rF2Scoring>(rFactor2Constants.MM_SCORING_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
    MappedBuffer<rF2Rules> rulesBuffer = new MappedBuffer<rF2Rules>(rFactor2Constants.MM_RULES_FILE_NAME, true /*partial*/, true /*skipUnchanged*/);
    MappedBuffer<rF2ForceFeedback> forceFeedbackBuffer = new MappedBuffer<rF2ForceFeedback>(rFactor2Constants.MM_FORCE_FEEDBACK_FILE_NAME, false /*partial*/, false /*skipUnchanged*/);
    MappedBuffer<rF2Graphics> graphicsBuffer = new MappedBuffer<rF2Graphics>(rFactor2Constants.MM_GRAPHICS_FILE_NAME, false /*partial*/, false /*skipUnchanged*/);
    MappedBuffer<rF2PitInfo> pitInfoBuffer = new MappedBuffer<rF2PitInfo>(rFactor2Constants.MM_PITINFO_FILE_NAME, false /*partial*/, true /*skipUnchanged*/);
    MappedBuffer<rF2Weather> weatherBuffer = new MappedBuffer<rF2Weather>(rFactor2Constants.MM_WEATHER_FILE_NAME, false /*partial*/, true /*skipUnchanged*/);
    MappedBuffer<rF2Extended> extendedBuffer = new MappedBuffer<rF2Extended>(rFactor2Constants.MM_EXTENDED_FILE_NAME, false /*partial*/, true /*skipUnchanged*/);

    // Write buffers:
    MappedBuffer<rF2HWControl> hwControlBuffer = new MappedBuffer<rF2HWControl>(rFactor2Constants.MM_HWCONTROL_FILE_NAME);
    MappedBuffer<rF2WeatherControl> weatherControlBuffer = new MappedBuffer<rF2WeatherControl>(rFactor2Constants.MM_WEATHER_CONTROL_FILE_NAME);
    MappedBuffer<rF2RulesControl> rulesControlBuffer = new MappedBuffer<rF2RulesControl>(rFactor2Constants.MM_RULES_CONTROL_FILE_NAME);
    MappedBuffer<rF2PluginControl> pluginControlBuffer = new MappedBuffer<rF2PluginControl>(rFactor2Constants.MM_PLUGIN_CONTROL_FILE_NAME);

    // Marshalled views:
    rF2Telemetry telemetry = new rF2Telemetry();
    rF2Scoring scoring  = new rF2Scoring();
    rF2Rules rules = new rF2Rules();
    rF2ForceFeedback forceFeedback;
    rF2Graphics graphics;
    rF2PitInfo pitInfo;
    rF2Weather weather;
    rF2Extended extended = new rF2Extended();

    // Marashalled output views:
    rF2HWControl hwControl;
    rF2WeatherControl weatherControl;
    rF2RulesControl rulesControl;
    rF2PluginControl pluginControl;

    telemetryBuffer.ClearStats();
    scoringBuffer.ClearStats();
    extendedBuffer.ClearStats();
    rulesBuffer.ClearStats();

    while (!connected)
    {
        try
        {
            extendedBuffer.Connect();
            telemetryBuffer.Connect();
            scoringBuffer.Connect();
            rulesBuffer.Connect();
            forceFeedbackBuffer.Connect();
            graphicsBuffer.Connect();
            pitInfoBuffer.Connect();
            weatherBuffer.Connect();
            connected = true;
        }
        catch (Exception ex)
        {
           Disconnect();
        }
    }

    if (!connected)
        return;

    try
    {     
        extendedBuffer.GetMappedData(ref extended);
        scoringBuffer.GetMappedData(ref scoring);
        telemetryBuffer.GetMappedData(ref telemetry);
        rulesBuffer.GetMappedData(ref rules);
    }
    catch (Exception)
    {
        Disconnect();
    }
 
    // Track rF2 transitions.
    TransitionTracker tracker = new TransitionTracker();

    // Config
    bool logPhaseAndState = true;
    bool logDamage = true;
    bool logTiming = true;
    bool logRules = true;

    while (connected)
    {
        tracker.TrackPhase(ref scoring, ref telemetry, ref extended, logPhaseAndState);
        tracker.TrackDamage(ref scoring, ref telemetry, ref extended, logDamage);
        tracker.TrackTimings(ref scoring, ref telemetry, ref rules, ref extended, logTiming);
        tracker.TrackRules(ref scoring, ref telemetry, ref rules, ref extended, logRules);

        // connected = false;
    }

    void Disconnect()
    {
        extendedBuffer.Disconnect();
        scoringBuffer.Disconnect();
        rulesBuffer.Disconnect();
        telemetryBuffer.Disconnect();
        forceFeedbackBuffer.Disconnect();
        pitInfoBuffer.Disconnect();
        weatherBuffer.Disconnect();
        graphicsBuffer.Disconnect();
        hwControlBuffer.Disconnect();
        weatherControlBuffer.Disconnect();
        rulesControlBuffer.Disconnect();
        pluginControlBuffer.Disconnect();
        connected = false;
    }
