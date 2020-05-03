using System;
using System.Collections.Generic;

using SkiaSharp;

using TouchTracking;

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

            Matrix = SKMatrix.MakeIdentity();

            TouchManager = new TouchManipulationManager
            {
                Mode = TouchManipulationMode.ScaleRotate
            };
        }

        public void Paint(SKCanvas canvas, int floor)
        {
            canvas.Save();
            SKMatrix matrix = Matrix;

            SKPaint paint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 75,
                TextAlign = SKTextAlign.Center
            };

            canvas.Concat(ref matrix);
            canvas.DrawBitmap(bitmap, 0, 0);


            if (floor == 1)
            {
                canvas.DrawText("213", 250, 250, paint);
                canvas.DrawLine(0, 0, 500, 0, paint);
                canvas.DrawLine(500, 0, 500, 500, paint);
                canvas.DrawLine(500, 500, 0, 500, paint);
                canvas.DrawLine(0, 500, 0, 0, paint);
                canvas.DrawLine(0, 200, 500, 200, paint);
                canvas.DrawLine(250, 200, 250, 0, paint);
            }
            else
            {
                canvas.DrawLine(0, 0, 500, 0, paint);
                canvas.DrawLine(500, 0, 500, 500, paint);
                canvas.DrawLine(500, 500, 0, 500, paint);
                canvas.DrawLine(0, 500, 0, 0, paint);
                canvas.DrawLine(0, 200, 500, 200, paint);
                canvas.DrawLine(250, 200, 250, 0, paint);

                canvas.DrawLine(500, 500, 500, 700, paint);
                canvas.DrawLine(500, 700, 300, 700, paint);
                canvas.DrawLine(300, 700, 300, 500, paint);
            }

            canvas.Restore();
        }

        internal void Paint(TouchManipulationBitmap bitmap, int selectedIndex)
        {
            throw new NotImplementedException();
        }

        public bool HitTest(SKPoint location)
        {
            // Инвертировать матрицу
            SKMatrix inverseMatrix;

            if (Matrix.TryInvert(out inverseMatrix))
            {
                // Преобразование точки, используя инвертированную матрицу
                SKPoint transformedPoint = inverseMatrix.MapPoint(location);

                // Проверка, находится ли она в нетрансформированном прямоугольнике
                SKRect rect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                return rect.Contains(transformedPoint);
            }
            return false; 
        }

        public void ProcessTouchEvent(long id, TouchActionType type, SKPoint location)
        {
            switch (type)
            {
                case TouchActionType.Pressed:
                    touchDictionary.Add(id, new TouchManipulationInfo
                    {
                        PreviousPoint = location,
                        NewPoint = location
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

            if (infos.Length == 1)
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
            // осторожно
            // if (matrix.ScaleX < 0.01) { matrix.ScaleX = 0.01F; }
            // if (matrix.ScaleY < 0.01) { matrix.ScaleY = 0.01F; }

            Matrix = matrix;
        }
    }

}
