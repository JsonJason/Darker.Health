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

        [Test]
        public void Decrease_Lowers_Meter_Value()
        {
            _health.Value = 50;
            _health.Decrease(6);

            Assert.AreEqual(44,_health.Value);
        }

        [Test]
        public void Decrease_Value_Cannot_Be_Negative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _health.Decrease(-6));
        }

        [Test]
        public void Decrease_Of_More_Than_Maximum_Clamp_Value_To_Zero()
        {
            _health.Decrease(MaximumHealth + 3);
        }

        [Test]
        public void Decrease_Of_More_Than_Value_Returns_Remainder()
        {
            var remainder = _health.Decrease(_health.Value + 3);
            Assert.AreEqual(3,remainder);
        }

        [Test]
        public void Decrease_Of_Exact_Value_Return_Zero_Remainder()
        {
            var remainder = _health.Decrease(_health.Value);
            Assert.AreEqual(0,remainder);
        }

    }
}