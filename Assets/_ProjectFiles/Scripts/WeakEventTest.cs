using System;
using System.Threading;
using Egsp.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    public class WeakEventTest : MonoBehaviour
    {
        private WeakEvent<int> _weakEvent = new WeakEvent<int>();

        private int _x;
        
        private void Update()
        {
            Profiler.BeginSample("WEAK");

            for (var i = 0; i < 100; i++)
            {
                var obj = new WeakObjTest();
                _x = 1;
                _weakEvent.Subscribe(obj.ZPLus);
                _weakEvent.Raise(32);
                obj = null;
                GC.Collect();
                _weakEvent.Raise(32);
            }
            
            Profiler.EndSample();
            
            Profiler.BeginSample("AFTER WEAK");
            
            Profiler.EndSample();
            EditorApplication.isPlaying = false;
        }

        private void XPlus(int y)
        {
            Debug.Log(_x + y);
            _x += y;
        }
    }

    public class WeakObjTest
    {
        private int z;
        public void ZPLus(int y)
        {
            z += y;
        }
    }
}