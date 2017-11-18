using System;
using NUnit.Framework;

namespace Darker.Health.Tests
{
    [TestFixture]
    public class MeterTests
    {
        [SetUp]
        public void SetUp()
        {
            _health = new Meter(MaximumHealth);
        }

        private Meter _health;
        public int MaximumHealth = 100;

        [Test]
        public void Maximum_Value_Must_Be_Greater_Than_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Maximum = -5);
            Assert.Throws<ArgumentOutOfRangeException>(() => _health = new Meter(-5));
        }

        [Test]
        public void Meter_Value_Starts_At_Maximum()
        {
            Assert.AreEqual(MaximumHealth, _health.Value);
        }

        [Test]
        public void Meter_Value_Cannot_Be_Set_Higher_Than_Maximum()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Value = MaximumHealth + 1);
            
        }

        [Test]
        public void Meter_Value_Reaching_Zero_Depletes_Meter()
        {
            int timesDepletedEventFired = 0;
            _health.Depleted += (o, a) => timesDepletedEventFired++;

            _health.Value = 0;

            Assert.AreEqual(1,timesDepletedEventFired);
        }

        [Test]
        public void Meter_Value_Cannot_Be_Less_Than_Zero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Value =-6);

        }

    }
}