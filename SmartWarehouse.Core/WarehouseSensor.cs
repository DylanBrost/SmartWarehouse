using System;
using System.Collections.Generic;
using System.Text;

namespace SmartWarehouse.Core
{
    public class WarehouseSensor
    {
        public string SensorId { get; private set; }
        public string LocationTag { get; private set; }
        public double CurrentTemperature { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsAlertTriggered { get; private set; }
        public double CriticalThresholdCelsius { get; private set; }

        public WarehouseSensor(string sensorId, string locationTag,
            double criticalThreshold = 4.0)
        {
            if (string.IsNullOrWhiteSpace(sensorId))
            {
                throw new ArgumentException("Sensor ID cannot be blank.");
            }

            if (string.IsNullOrWhiteSpace(locationTag))
            {
                throw new ArgumentException("Location cannot be blank.");
            }

            SensorId = sensorId;
            LocationTag = locationTag;
            CurrentTemperature = 0.0;
            IsActive = false;
            IsAlertTriggered = false;
            CriticalThresholdCelsius = criticalThreshold;
        }
    }
}