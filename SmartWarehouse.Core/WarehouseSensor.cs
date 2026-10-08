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

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            IsAlertTriggered = false;
        }

        public void RecordReading(double newTemperature)
        {
            if (IsActive == false)
            {
                throw new InvalidOperationException("Sensor must be active.");
            }

            if (newTemperature < -50.0 || newTemperature > 80.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newTemperature), "Temperature must be between -50 and 80.");
            }

            CurrentTemperature = newTemperature;

            if (CurrentTemperature >= CriticalThresholdCelsius)
            {
                IsAlertTriggered = true;
            }
            else
            {
                IsAlertTriggered = false;
            }
        }

        public void UpdateThreshold(double newThreshold)
        {
            if (newThreshold < -30.0 || newThreshold > 50.0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newThreshold), "Threshold must be between -30 and 50.");
            }

            CriticalThresholdCelsius = newThreshold;

            if (CurrentTemperature >= CriticalThresholdCelsius)
            {
                IsAlertTriggered = true;
            }
            else
            {
                IsAlertTriggered = false;
            }
        }
    }
}