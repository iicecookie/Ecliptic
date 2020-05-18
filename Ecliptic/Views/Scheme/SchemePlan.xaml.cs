using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking;
using Ecliptic.Models;
using Android.InputMethodServices;
using System.Linq;
using Ecliptic.Views.WayFounder;

namespace Ecliptic.Views
{
    public partial class SchemePlanPage : ContentPage
    {
        TouchManipulationBitmap bitmap;

        List<long> touchIds = new List<long>();

        MatrixDisplay matrixDisplay = new MatrixDisplay();

        public SchemePlanPage()
        {
            InitializeComponent();

            SKBitmap bitmap = new SKBitmap(600, 600);

            this.bitmap = new TouchManipulationBitmap(bitmap);
            this.bitmap.TouchManager.Mode = TouchManipulationMode.ScaleRotate;

            FloorPicker.ItemsSource  = FloorData.Floors;
            FloorPicker.SelectedItem = FloorData.GetFloor(4);

            PointData.RoomPoints = PointData.Points
                        .Where(p => p.IsWaypoint == true)
                        .Where(c => c.Room != null).ToList();
        }

        protected override void OnAppearing()
        {   
            base.OnAppearing();
        }

        #region FloorChange
        void OnFloorPickerSelected(object sender, EventArgs args)
        {
            // загрузили стены
            EdgeData.CurrentFloorWalls = EdgeData.Edges
                .Where(e => e.PointFrom.IsWaypoint == false)
                .Where(c => c.PointTo.Floor.Level == ((Floor)FloorPicker.SelectedItem).Level)
                .ToList();

            // селектнули - отрисовали  
            canvasView.InvalidateSurface();
        }

        private void OnStepedDown (object sender, EventArgs args)
        {
            if (FloorPicker.SelectedItem == null)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int prevlevel = ((Floor)FloorPicker.SelectedItem).Level - 1;
            if (prevlevel == 0) prevlevel--;

            if (FloorData.GetFloor(prevlevel) != null)
            {
                FloorPicker.SelectedItem = FloorData.GetFloor(prevlevel);
            }
            else
            {
                DependencyService.Get<IToast>().Show("Вы на нижнем этаже");
            }
        }
                                  
        private void OnStepedUp   (object sender, EventArgs args)
        {
            if (FloorPicker.SelectedItem == null)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int nextlevel = ((Floor)FloorPicker.SelectedItem).Level + 1;
            if (nextlevel == 0) nextlevel++;

            if (FloorData.GetFloor(nextlevel) != null)
            {
                FloorPicker.SelectedItem = FloorData.GetFloor(nextlevel);
            }
            else
            {
                DependencyService.Get<IToast>().Show("Вы на последнем этаже");
            }
        }
        #endregion

        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width  * pt.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                   // if (bitmap.HitTest(point)) // открыть для перемещения по нажатию на битмап
                    {
                        touchIds.Add(args.Id);
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        break;
                    }
                   // break;

                case TouchActionType.Moved:
                    if (touchIds.Contains(args.Id))
                    {
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        canvasView.InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchIds.Contains(args.Id))
                    {
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        touchIds.Remove(args.Id);
                        canvasView.InvalidateSurface();
                    }
                    break;
            }
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Отображение рисунка
            if (FloorPicker.SelectedItem != null)
                bitmap.Paint(canvas, ((Floor)FloorPicker.SelectedItem).Level);


            // Отрисовка матрицы преобразования
            SKSize matrixSize = matrixDisplay.Measure(bitmap.Matrix);

            matrixDisplay.Paint(canvas, bitmap.Matrix,
                new SKPoint(info.Width  - matrixSize.Width,
                            info.Height - matrixSize.Height));
        }

        #region Toolbar
        void RefrashMatrix(object sender, EventArgs args)
        {
            // селектнули - отрисовали  
            bitmap.Matrix = SKMatrix.MakeIdentity();

            canvasView.InvalidateSurface();
        }
        #endregion
    }
}
