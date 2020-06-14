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
using Xamarin.Essentials;

namespace Ecliptic.Views
{
    public partial class SchemePlanPage : ContentPage
    {
        TouchManipulationBitmap bitmap;
        List<long>    touchIds      = new List<long>();
        MatrixDisplay matrixDisplay = new MatrixDisplay();

        public SchemePlanPage()
        {
            InitializeComponent();

            SKBitmap bitmap = new SKBitmap(600, 600);

            this.bitmap = new TouchManipulationBitmap(bitmap);
            this.bitmap.TouchManager.Mode = TouchManipulationMode.ScaleRotate;

            // if (PointData.Points.Count == 0)
            // {
            //     Shell.Current.GoToAsync($"buildings");
            // }
        }
        public SchemePlanPage(Room room) : this()
        {
            FloorPicker.ItemsSource = FloorData.Floors;
            CurrentFloor = room.Floor;
            FloorPicker.SelectedItem = CurrentFloor;

            // селектнули - двинули  
            PointM point = PointData.Find(room);
            float height = (float)DeviceDisplay.MainDisplayInfo.Height / 4;
            float width  = (float)DeviceDisplay.MainDisplayInfo.Width / 2;

            bitmap.Matrix = SKMatrix.MakeTranslation((float)(-point.X + width),
                                                     (float)(-point.Y + height));
        }

        Floor CurrentFloor = null;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //  if (FloorPicker.ItemsSource == null)
            FloorPicker.ItemsSource = FloorData.Floors;

            if (BuildingPage.isUpdate && FloorPicker.ItemsSource.Count > 0)
            {
                FloorPicker.SelectedIndex = 1;
            }

            PointData.RoomPoints = PointData.Points.Where(p => p.Room != null).ToList();

            if (FloorPicker.SelectedItem == null && FloorData.Floors.Count > 0)
            {
                CurrentFloor = FloorData.Floors.First();
                FloorPicker.SelectedItem = CurrentFloor;
            }

            if (FloorPicker.SelectedItem != null)
            {
                LoadFloorMap(CurrentFloor);
            }
        }

        private void LoadFloorMap(Floor floor)
        {                
            // загрузили стены
            EdgeData.CurrentFloorWalls = EdgeData.Edges
                .Where(e => e.PointFrom.IsWaypoint == false)
                .Where(c => c.PointTo.Floor.Level == floor?.Level)
                .ToList();

            // 
            PointData.CurrentFloorRoomPoints = PointData.Points
                .Where(p => p.Room != null)
                .Where(p => p.Floor.Level == floor?.Level).ToList();
        }

        #region FloorChange
        void OnFloorPickerSelected(object sender, EventArgs args)
        {
            CurrentFloor = FloorPicker?.SelectedItem as Floor;
            int? selectedfloor = CurrentFloor?.Level;
            if ( selectedfloor == null) return;

            LoadFloorMap(CurrentFloor);

            // селектнули - отрисовали  
            canvasView.InvalidateSurface();
        }

        private void OnStepedDown (object sender, EventArgs args)
        {
            if (FloorPicker.ItemsSource.Count == 0)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int? prevlevel = CurrentFloor?.Level - 1;
            if ( prevlevel == 0) prevlevel--;

            CurrentFloor = FloorData.GetFloor(prevlevel);

            if (CurrentFloor != null)
            {
                FloorPicker.SelectedItem = CurrentFloor;
            }
            else
            {
                DependencyService.Get<IToast>().Show("Вы на нижнем этаже");
            }
        }
                                  
        private void OnStepedUp   (object sender, EventArgs args)
        {
            if (FloorPicker.ItemsSource.Count == 0)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int? nextlevel = CurrentFloor?.Level + 1;
            if ( nextlevel == 0) nextlevel++;

            CurrentFloor = FloorData.GetFloor(nextlevel);

            if (CurrentFloor != null)
            {
                FloorPicker.SelectedItem = CurrentFloor;
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

                        Room room = bitmap.HitTest(point);
                        if (room != null) { Shell.Current.GoToAsync($"roomdetails?name={room.Name}"); }
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
            if (CurrentFloor != null)
                bitmap.Paint(canvas, CurrentFloor.Level);

            // Отрисовка матрицы преобразования
            // SKSize matrixSize = matrixDisplay.Measure(bitmap.Matrix);

            // matrixDisplay.Paint(canvas, bitmap.Matrix,
            //     new SKPoint(info.Width  - matrixSize.Width,
            //                 info.Height - matrixSize.Height));
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
