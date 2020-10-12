﻿using System;
using UnityEngine;

namespace Juce.Feedbacks
{
    public class Vector3Element : Element
    {
        [SerializeField] [HideInInspector] private bool useStartValue = default;
        [SerializeField] [HideInInspector] private bool useStartX = default;
        [SerializeField] [HideInInspector] private bool useStartY = default;
        [SerializeField] [HideInInspector] private bool useStartZ = default;
        [SerializeField] [HideInInspector] private bool useEndX = default;
        [SerializeField] [HideInInspector] private bool useEndY = default;
        [SerializeField] [HideInInspector] private bool useEndZ = default;
        [SerializeField] [HideInInspector] private float startValueX = default;
        [SerializeField] [HideInInspector] private float startValueY = default;
        [SerializeField] [HideInInspector] private float startValueZ = default;
        [SerializeField] [HideInInspector] private float endValueX = default;
        [SerializeField] [HideInInspector] private float endValueY = default;
        [SerializeField] [HideInInspector] private float endValueZ = default;

        public bool UseStartValue => useStartValue;
        public bool UseStartX => useStartX;
        public bool UseStartY => useStartY;
        public bool UseStartZ => useStartZ;
        public bool UseEndX => useEndX;
        public bool UseEndY => useEndY;
        public bool UseEndZ => useEndZ;
        public float StartValueX => startValueX;
        public float StartValueY => startValueY;
        public float StartValueZ => startValueZ;
        public float EndValueX => endValueX;
        public float EndValueY => endValueY;
        public float EndValueZ => endValueZ;
    }
}