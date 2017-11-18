﻿using System;

namespace Darker.Health
{
    public class Meter
    {
        private int _maximum;
        private int _value;

        public Meter(int maximum)
        {
           
            Maximum = maximum;
            Value = Maximum;
        }

        public int Maximum
        {
            get => _maximum;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value),
                        $"Meter maximum value must be greater than zero. Cannot set to: {value}");
                _maximum = value; 
                
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                if(value > Maximum) throw new ArgumentOutOfRangeException(nameof(value),$"Maximum Value is {Maximum}. Cannot set to: {value}");
                if(value < 0) throw new ArgumentOutOfRangeException(nameof(value), $"Meter value must be a positive number. Cannot set to: {value}");
                _value = value; 
                if(_value == 0) OnDepleted();
            }
        }

        public event EventHandler Depleted;

        protected virtual void OnDepleted()
        {
            Depleted?.Invoke(this, EventArgs.Empty);
        }
    }
}