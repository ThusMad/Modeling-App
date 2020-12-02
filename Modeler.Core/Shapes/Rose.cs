﻿using System;
using System.Collections.Generic;
using System.Linq;
using Modeler.Core.Enums;
using Modeler.Core.Models;
using Modeler.Core.Utilities;
using SharpDX.Mathematics.Interop;

namespace Modeler.Core.Shapes
{
    public class Rose : ShapeBase
    {
        private float _t1;
        private float _t2;
        private float _step;
        private int _k;
        private int _size;

        public int Size
        {
            get => _size;
            set
            {
                _size = value;
                Rebuild();
            }
        }

        public float Step
        {
            get => _step;
            set
            {
                _step = value;
                Rebuild();
            }
        }

        public float T1
        {
            get => _t1;
            set
            {
                _t1 = value;
                Rebuild();
            }
        }

        public float T2
        {
            get => _t2;
            set
            {
                _t2 = value;
                Rebuild();
            }
        }

        public int K
        {
            get => _k;
            set
            {
                _k = value;
                Rebuild();
            }
        }

        public Rose(int centerX, int centerY, float t1, float t2, int k, int size, RawColor4 color, float thickness = 1f, float step = 0.1f) : base(centerX, centerY, ShapeType.Roze, color, thickness)
        {
            _t1 = t1;
            _t2 = t2;
            _k = k;
            _size = size;
            _step = step;

            for (var i = _t1; i < _t2; i+= _step)
            {
                Data.Add(new RawVector2(
                    centerX + _size * Original(i).X,
                    centerY + _size * Original(i).Y
                    ));
            }
        }

        public Rose(RoseCreationModel model, RawColor4 color, float thickness = 1f) : base(model.CenterX, model.CenterY, ShapeType.Roze, color, thickness)
        {
            _t1 = 0;
            _t2 = model.K % 2 == 0 ? 2 * (float)Math.PI : (float)Math.PI;
            _k = model.K;
            _size = model.Size;
            _step = model.Step;

            for (var i = _t1; i < _t2; i += _step)
            {
                Data.Add(new RawVector2(
                    model.CenterX + _size * Original(i).X,
                    model.CenterY + _size * Original(i).Y
                ));
            }

            CalculateOuterBox();
        }


        public void Rebuild()
        {
            for (var i = _t1; i < _t2; i += _step)
            {
                Data.Add(new RawVector2(
                    CenterX + _size * (float)(Math.Cos(_k * i) * Math.Cos(i)),
                    CenterY + _size * (float)(Math.Cos(_k * i) * Math.Sin(i))
                ));
            }

            CalculateOuterBox();
        }

        public override float CalculateArea(int a)
        {
            if (_k % 2 == 0)
            {
                return (float)(a * a * Math.PI) / 2;
            }

            return (float)(a * a * Math.PI) / 4;
        }

        public override float CalculateArcLength()
        {
            var sum = 0f;

            for(var i = 1; i < Data.Count; i++)
            {
                var a = Data[i];
                var b = Data[i - 1];
                sum += Utility.GetDistance(a.X, a.Y, b.X, b.Y);
            }
            
            return sum * 2;
        }

        public override float CalculateCurvative(RawVector2 point)
        {
            var K = 0f;
            var sum = 0f;
            var theta = 0f;
            var dict = new Dictionary<float, float>();
            for (var i = _t1; i < _t2; i += _step)
            {
                var orig = Original(i);

                var nativePoint = new RawVector2(
                    CenterX + _size * orig.X,
                    CenterY + _size * orig.Y
                );

                dict.Add(i, Utility.GetDistance(nativePoint.X, nativePoint.Y, point.X, point.Y));
            }

            var order = dict.ToList().OrderBy(a => a.Value);
            theta = order.ToList().First().Key;

            var x0 = theta;

            K = (float) ((Math.Pow(OrigPolar(x0), 2) + 2 * Math.Pow(OrigPolarD1(x0), 2) -
                          OrigPolar(x0) * OrigPolarD2(x0)) /
                         Math.Pow(Math.Pow(OrigPolar(x0), 2) + Math.Pow(OrigPolarD1(x0), 2), 3 / 2));


            return 1 / K;
        }

        public override ICollection<RawVector2> BuildTangen(RawVector2 point, int length)
        {
            var theta = 0f;
            var dict = new Dictionary<float, float>();
            for (var i = _t1; i < _t2; i += _step)
            {
                var orig = Original(i);

                var nativePoint = new RawVector2(
                    CenterX + _size * orig.X,
                    CenterY + _size * orig.Y
                );

                dict.Add(i, Utility.GetDistance(nativePoint.X, nativePoint.Y, point.X, point.Y));
            }

            var order = dict.ToList().OrderBy(a => a.Value);
            theta = order.ToList().First().Key;

            var range = GetRange(point, length, 2);

            var x0 = theta;
            var y0 = Original(x0).Y;
            var yd0 = DerivativeYt(x0).Y;

            var points = range.Select(x => new RawVector2(x, (yd0 * (x - x0) + y0))).ToList();

            var pointMin = points.Max(p => p.Y);
            var pointMax = points.Min(p => p.Y);

            var pointCenter = pointMax - ((pointMax - pointMin) / 2);

            var dist = point.Y - pointCenter;
            var result = points.Select(p => new RawVector2(p.X, p.Y + dist));

            return (result.Where(res => Utility.GetDistance(res.X, res.Y, point.X, point.Y) < 100)).ToList();
        }

        public override ICollection<RawVector2> BuildNormal(RawVector2 point, int length)
        {
            var theta = 0f;
            var dict = new Dictionary<float, float>();
            for (var i = _t1; i < _t2; i += _step)
            {
                var orig = Original(i);

                var nativePoint = new RawVector2(
                    CenterX + _size * orig.X,
                    CenterY + _size * orig.Y
                );

                dict.Add(i, Utility.GetDistance(nativePoint.X, nativePoint.Y, point.X, point.Y));
            }

            var order = dict.ToList().OrderBy(a => a.Value);
            theta = order.ToList().First().Key;

            var range = GetRange(point, length, 2);

            var x0 = theta;
            var y0 = Original(x0).Y;
            var yd0 = DerivativeYt(x0).Y;

            var points = range.Select(x => new RawVector2(x, y0 - ((1 / yd0) * (x - x0)))).ToList();

            var pointMin = points.Max(p => p.Y);
            var pointMax = points.Min(p => p.Y);

            var pointCenter = pointMax - ((pointMax - pointMin) / 2);

            var dist = point.Y - pointCenter;
            var result = points.Select(p => new RawVector2(p.X, p.Y + dist));

            return (result.Where(res => Utility.GetDistance(res.X, res.Y, point.X, point.Y) < 100)).ToList();
        }

        private ICollection<float> GetRange(RawVector2 middlePoint, int length, int precision = 100)
        {
            var points = new List<float>();

            var a = new RawVector2(middlePoint.X - length / 2, 0);
            var b = new RawVector2(middlePoint.X + length / 2, 0);

            var count = (int)(length / precision);

            var d = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y)) / count;
            var fi = Math.Atan2(b.Y - a.Y, b.X - a.X);

            for (int j = 0; j <= count; ++j)
                points.Add((int)(a.X + j * d * Math.Cos(fi)));

            return points;
        }

        private RawVector2 DerivativeYt(float t)
        {
            return new RawVector2(
                t,
                (float)(-1 * ((1 - _k) * Math.Cos(t - _k * t) + (_k + 1) * Math.Cos(_k * t + t)) /
                               ((1 - _k) * Math.Sin(t - _k * t) + (_k + 1) * Math.Sin(_k * t + t)))
            );
        }

        private RawVector2 Original(float theta)
        {
            return new RawVector2(
                (float)(Math.Cos(_k * theta) * Math.Cos(theta)),
                (float)(Math.Cos(_k * theta) * Math.Sin(theta))
            );
        }

        private float OrigPolar(float theta)
        {
            return _size * (float)Math.Cos(_k * theta);
        }

        private float OrigPolarD1(float theta)
        {
            return _size * (float)(-1 * _k * Math.Sin(_k * theta));
        }

        private float OrigPolarD2(float theta)
        {
            return _size * (float)(-1 * Math.Pow(_k, 2) * Math.Cos(_k * theta));
        }

    }
}