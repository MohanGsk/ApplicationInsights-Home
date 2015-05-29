using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace WorkerRoleB.Telemetry
{
    public class RequestTelemetryHelper
    {
        private static TelemetryClient AI_CLIENT = new TelemetryClient();
        private static string SUCCESS_CODE = "200";
        private static string FAILURE_CODE = "500";

        public static RequestTelemetry StartNewRequest(string name, DateTimeOffset startTime)
        {
            var request = new RequestTelemetry();
            request.Name = name;
            request.Timestamp = startTime;
            return request;
        }

        public static void DispatchRequest(RequestTelemetry request, TimeSpan duration, bool success)
        {
            request.Duration = duration;
            request.Success = success;
            request.ResponseCode = (success) ? SUCCESS_CODE : FAILURE_CODE;
            AI_CLIENT.TrackRequest(request);
        }
    }
}