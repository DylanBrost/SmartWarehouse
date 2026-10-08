using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartWarehouse.Core;

namespace SmartWarehouse.Tests
{
    [TestClass]
    public class WarehouseSensorTests
    {
        [TestMethod]
        public void ConstructorSetsDefaultValues()
        {
            // Arrange
            string sensorId = "S001";
            string locationTag = "Cold-Vault-01";

            // Act
            WarehouseSensor sensor = new WarehouseSensor(sensorId, locationTag);

            // Assert
            Assert.AreEqual(sensorId, sensor.SensorId);
            Assert.AreEqual(locationTag, sensor.LocationTag);
            Assert.AreEqual(0.0, sensor.CurrentTemperature);
            Assert.IsFalse(sensor.IsActive);
            Assert.IsFalse(sensor.IsAlertTriggered);
            Assert.AreEqual(4.0, sensor.CriticalThresholdCelsius);
        }

        [TestMethod]
        public void ConstructorCustomThreshold()
        {
            // Arrange
            double threshold = 10.0;

            // Act
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01", threshold);

            // Assert
            Assert.AreEqual(threshold, sensor.CriticalThresholdCelsius);
        }

        [TestMethod]
        public void ActivateSetsActive()
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");

            // Act
            sensor.Activate();

            // Assert
            Assert.IsTrue(sensor.IsActive);
        }
        [TestMethod]
        public void ActivateTwice()
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();

            // Act
            sensor.Activate();

            // Assert
            Assert.IsTrue(sensor.IsActive);
        }

        [TestMethod]
        public void DeactivateAlert()
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();
            sensor.RecordReading(10.0);

            // Act
            sensor.Deactivate();

            // Assert
            Assert.IsFalse(sensor.IsActive);
            Assert.IsFalse(sensor.IsAlertTriggered);
        }

        [DataTestMethod]
        [DataRow(3.0, false)]
        [DataRow(4.0, true)]
        [DataRow(5.0, true)]
        public void RecordChecksAlert(double temperature, bool expectedAlert)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();

            // Act
            sensor.RecordReading(temperature);

            // Assert
            Assert.AreEqual(temperature, sensor.CurrentTemperature);
            Assert.AreEqual(expectedAlert, sensor.IsAlertTriggered);
        }

        [DataTestMethod]
        [DataRow(-50.0)]
        [DataRow(80.0)]
        public void RecordAllowsLimits(double temperature)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();

            // Act
            sensor.RecordReading(temperature);

            // Assert
            Assert.AreEqual(temperature, sensor.CurrentTemperature);
        }

        [DataTestMethod]
        [DataRow(-50.1)]
        [DataRow(80.1)]
        public void RecordInvalidTemperature(double temperature)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();
            bool exceptionThrown = false;

            // Act
            try
            {
                sensor.RecordReading(temperature);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }


        [TestMethod]
        public void RecordWhileInactive()
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            bool exceptionThrown = false;

            // Act
            try
            {
                sensor.RecordReading(5.0);
            }
            catch (InvalidOperationException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void InvalidSensorId(string? sensorId)
        {
            // Arrange
            string location = "Cold-Vault-01";
            bool exceptionThrown = false;

            // Act
            try
            {
                WarehouseSensor sensor = new WarehouseSensor(sensorId!, location);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void InvalidLocation(string? location)
        {
            // Arrange
            string sensorId = "S001";
            bool exceptionThrown = false;

            // Act
            try
            {
                WarehouseSensor sensor = new WarehouseSensor(sensorId, location!);
            }
            catch (ArgumentException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
        }


        [DataTestMethod]
        [DataRow(4.0, 6.0, false)]
        [DataRow(10.0, 5.0, true)]
        [DataRow(10.0, 3.0, true)]
        public void ChangeThreshold(
            double startThreshold, double threshold, bool expectedAlert)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01", startThreshold);
            sensor.Activate();
            sensor.RecordReading(5.0);

            // Act
            sensor.UpdateThreshold(threshold);

            // Assert
            Assert.AreEqual(threshold, sensor.CriticalThresholdCelsius);
            Assert.AreEqual(expectedAlert, sensor.IsAlertTriggered);
        }

        [DataTestMethod]
        [DataRow(-30.0)]
        [DataRow(50.0)]
        public void ThresholdLimits(double threshold)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();

            // Act
            sensor.UpdateThreshold(threshold);

            // Assert
            Assert.AreEqual(threshold, sensor.CriticalThresholdCelsius);
        }

        [DataTestMethod]
        [DataRow(-30.1)]
        [DataRow(50.1)]
        public void InvalidThreshold(double threshold)
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();
            bool exceptionThrown = false;

            // Act
            try
            {
                sensor.UpdateThreshold(threshold);
            }
            catch (ArgumentOutOfRangeException)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsTrue(exceptionThrown);
            Assert.AreEqual(4.0, sensor.CriticalThresholdCelsius);
        }

        [TestMethod]
        public void LowerReadingClearsAlert()
        {
            // Arrange
            WarehouseSensor sensor = new WarehouseSensor(
                "S001", "Cold-Vault-01");
            sensor.Activate();
            sensor.RecordReading(10.0);

            // Act
            sensor.RecordReading(2.0);

            // Assert
            Assert.AreEqual(2.0, sensor.CurrentTemperature);
            Assert.IsFalse(sensor.IsAlertTriggered);
        }
    }
}
