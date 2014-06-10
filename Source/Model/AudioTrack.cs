﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using VVVV.Core.Collections;
using VVVV.Core.Model;
using NAudio.Wave;

namespace Timeliner
{
    public class TLAudioTrack : TLTrackBase
    {
    	public EditableIDList<TLSample> Samples
        {
            get;
            private set;
        }
    	
        public TLAudioTrack()
        	: this(IDGenerator.NewID)
        {
        }

        public TLAudioTrack(string name)
            : base(name)
        {
        	Samples = new EditableIDList<TLSample>("Samples");
        }
        
        public void LoadFile()
        {
        	var wave = new WaveChannel32(new Mp3FileReader(@"C:\Users\joreg\Documents\My Dropbox\Public\twenty steps.mp3"));

        	var buffer = new byte[16384*2];
        	var read = 0;
        	
        	var i = 0;
        	while (wave.Position < wave.Length)
        	{
        		read = wave.Read(buffer, 0, 16384*2);
        		
        		var max = 0f;
        		var absMax = 0f;
        		for (int j = 0; j < read / 4; j++)
        		{
        			var s = BitConverter.ToSingle(buffer, j*4);
        			var abs = Math.Abs(s); 
        			if (abs > absMax)
        			{
        				absMax = abs;
        				max = s;
        			}
        		}
        		Samples.Add(new TLSample(i += 2, max));
        	}
        }
    	
		public override string GetCurrentValueAsString()
		{
			return "";
		}
    	
		public override object GetCurrentValueAsObject()
		{
			return null;
		}
    }

    public class TLSample : TLModelBase
    {
        public EditableProperty<float> Time { get; private set; }
        public EditableProperty<float> Value { get; private set; }
        
        public PointF Position
        {
        	get
        	{
        		return new PointF(Time.Value, Value.Value);
        	}
        }
        
        public TLSample()
            : this(IDGenerator.NewID)
        {
        }
        
        public TLSample(string name)
            : this(name, 0, 0)
        {
        }
        
        public TLSample(float time, float value)
            : this(IDGenerator.NewID, time, value)
        {
        }

        public TLSample(string name, float time, float value)
            : base(name)
        {
            Time = new EditableProperty<float>("Time", time);
            Value = new EditableProperty<float>("Value", value);
            Add(Time);
            Add(Value);
        }
    }
}
