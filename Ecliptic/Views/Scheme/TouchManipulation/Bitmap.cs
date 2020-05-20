﻿using System;
using System.Collections.Generic;
using Android.Graphics;
using Ecliptic.Models;
using Ecliptic.Views.WayFounder;
using SkiaSharp;

using TouchTracking;
using Xamarin.Essentials;

namespace Ecliptic.Views
{
    class TouchManipulationBitmap
    {
        public SKBitmap bitmap;
        public SKMatrix Matrix { set; get; }

        public TouchManipulationManager TouchManager { set; get; }

        Dictionary<long, TouchManipulationInfo> touchDictionary =
                         new Dictionary<long, TouchManipulationInfo>();

        public TouchManipulationBitmap(SKBitmap bitmap)
        {
            this.bitmap = bitmap;

            // Matrix = SKMatrix.MakeIdentity();

            float height = (float)DeviceDisplay.MainDisplayInfo.Height / 1280;

            Matrix = SKMatrix.MakeScale(height, height);

            TouchManager = new TouchManipulationManager
            {
                Mode = TouchManipulationMode.ScaleRotate
            };
        }

        public void Paint(SKCanvas canvas, int floor)
        {
            canvas.Save();

            SKMatrix matrix = Matrix;

            SKPaint wallpaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 75,
                StrokeWidth = 10,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Square,
                TextAlign = SKTextAlign.Center
            };
            SKPaint textpaint = new SKPaint
            {
                TextSize = 36,
                IsAntialias = true,
                Color = SKColors.Blue,
                TextScaleX = 1.0f,
                TextAlign = SKTextAlign.Center,
            };
            SKPaint waypaint  = new SKPaint
            {
                TextSize = 64.0f,
                IsAntialias = true,
                Color = SKColors.Red,
                TextScaleX = 1.0f,
                StrokeWidth = 5,
                TextAlign = SKTextAlign.Center,
            };

            canvas.Concat(ref matrix);
            canvas.DrawBitmap(bitmap, 0, 0);

            // рисуем стены
            foreach (var edge in EdgeData.CurrentFloorWalls)
            {
                canvas.DrawLine((float)edge.PointFrom.X, (float)edge.PointFrom.Y,
                                (float)edge.PointTo.X,   (float)edge.PointTo.Y,   wallpaint);
            }

            // рисуем маршрут если есть
            foreach (var edge in EdgeData.Ways)
            {
                if (edge.PointFrom.Floor.Level == floor)
                    canvas.DrawLine((float)edge.PointFrom.X, (float)edge.PointFrom.Y,
                                    (float)edge.PointTo.X,   (float)edge.PointTo.Y, waypaint);
            }

            // рисуем имяна помещений
            foreach (var point in PointData.RoomPoints)
            {
                if (point.Floor.Level == floor)
                {
                    canvas.DrawText(point.Room.Name, (float)point.X, (float)point.Y, textpaint);
                }
            }

            canvas.Restore();
        }

        internal void Paint(TouchManipulationBitmap bitmap, int selectedIndex)
        {
            throw new NotImplementedException();
        }

        public Room HitTest(SKPoint location)
        {
            SKMatrix inverseMatrix;

            if (Matrix.TryInvert(out inverseMatrix))
            {
                SKPoint transformedPoint = inverseMatrix.MapPoint(location);

                foreach (var r in PointData.CurrentFloorRoomPoints)
                {
                    // сделать скелинг от матрицы, иначе при разных масштабах хер попадешь
                    SKRect rect = SKRect.Create((float)r.X - 25, (float)r.Y - 25, 50, 50);

                    if (rect.Contains(transformedPoint))
                    {
                        return r.Room;
                    }
                }
            }
            return null;
        }

        public void ProcessTouchEvent(long id, TouchActionType type, SKPoint location)
        {
            switch (type)
            {
                case TouchActionType.Pressed:
                    touchDictionary.Add(id, new TouchManipulationInfo
                    {
                        PreviousPoint = location,
                        NewPoint      = location
                    });
                    break;

                case TouchActionType.Moved:
                    TouchManipulationInfo info = touchDictionary[id];
                    info.NewPoint = location;
                    Manipulate();
                    info.PreviousPoint = info.NewPoint;
                    break;

                case TouchActionType.Released:
                    touchDictionary[id].NewPoint = location;
                    Manipulate();
                    touchDictionary.Remove(id);
                    break;

                case TouchActionType.Cancelled:
                    touchDictionary.Remove(id);
                    break;
            }
        }

        void Manipulate()
        {
            TouchManipulationInfo[] infos = new TouchManipulationInfo[touchDictionary.Count];
            touchDictionary.Values.CopyTo(infos, 0);
            SKMatrix touchMatrix = SKMatrix.MakeIdentity();

            if      (infos.Length == 1)
            {
                SKPoint prevPoint = infos[0].PreviousPoint;
                SKPoint newPoint = infos[0].NewPoint;
                SKPoint pivotPoint = Matrix.MapPoint(bitmap.Width / 2, bitmap.Height / 2);

                touchMatrix = TouchManager.OneFingerManipulate(prevPoint, newPoint, pivotPoint);
            }
            else if (infos.Length >= 2)
            {
                int pivotIndex = infos[0].NewPoint == infos[0].PreviousPoint ? 0 : 1;
                SKPoint pivotPoint = infos[pivotIndex].NewPoint;
                SKPoint newPoint = infos[1 - pivotIndex].NewPoint;
                SKPoint prevPoint = infos[1 - pivotIndex].PreviousPoint;

                touchMatrix = TouchManager.TwoFingerManipulate(prevPoint, newPoint, pivotPoint);
            }

            SKMatrix matrix = Matrix;
            SKMatrix.PostConcat(ref matrix, touchMatrix);

            // что бы не уменьшить ниже нижнего
            if (Math.Abs(matrix.ScaleX) < 0.1 &&
                Math.Abs(matrix.ScaleY) < 0.1 &&
                Math.Abs(matrix.SkewX ) < 0.1 &&
                Math.Abs(matrix.SkewY ) < 0.1)
            { return; }

            // что бы не поднивать выше верхнего
            if (Math.Abs(matrix.ScaleX) > 4.0 &&
                Math.Abs(matrix.ScaleY) > 4.0 ||
                Math.Abs(matrix.SkewX)  > 4.0 &&
                Math.Abs(matrix.SkewY)  > 4.0)
            { return; }

            Matrix = matrix;
        }
    }
}