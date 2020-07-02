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
        }
        public SchemePlanPage(Room room) : this() // открытие схемы с фокусом на помещение
        {
            FloorPicker.ItemsSource = FloorData.Floors;
            FloorData.CurrentFloor = room.Floor;
            FloorPicker.SelectedItem = FloorData.CurrentFloor;

            // переместить матрицу на координаты помещения  
            PointM point = PointData.Find(room);
            float height = (float)DeviceDisplay.MainDisplayInfo.Height / 4;
            float width  = (float)DeviceDisplay.MainDisplayInfo.Width / 2;

            bitmap.Matrix = SKMatrix.MakeTranslation((float)(-point.X + width),
                                                     (float)(-point.Y + height));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            FloorPicker.ItemsSource = FloorData.Floors;

            if (BuildingPage.isUpdate && FloorPicker.ItemsSource.Count > 0)
            {
                // если здание обновлено сбросить индекс до 1
                FloorPicker.SelectedIndex = 1; 
                BuildingPage.isUpdate = false;
            }

            if (FloorPicker.SelectedItem == null && FloorData.Floors.Count > 0)
            {
                // загрузить этажи в селектор если там пусто
                FloorData.CurrentFloor   = FloorData.Floors.First();
                FloorPicker.SelectedItem = FloorData.CurrentFloor;
            }

            if (FloorPicker.SelectedItem != null)
            {
                LoadFloorData(FloorData.CurrentFloor);
            }
            PointData.RoomPoints = PointData.Points.Where(p => p.Room != null).ToList();
        }

        // выборка стен и помещений для работы с меньшим объемом данных при отрисовки
        private void LoadFloorData(Floor floor)
        {                
            // загрузили стены этажа
            EdgeData.CurrentFloorWalls = EdgeData.Edges
                .Where(e => e.PointFrom.IsWaypoint == false)
                .Where(c => c.PointTo.Floor.Level == floor?.Level)
                .ToList();

            // загрузили помещения этажа
            PointData.CurrentFloorRoomPoints = PointData.Points
                .Where(p => p.Room != null)
                .Where(p => p.Floor.Level == floor?.Level).ToList();
        }

        #region FloorChange
        // событие выбора элемента в контроллере выбора этажа
        void OnFloorPickerSelected(object sender, EventArgs args)
        {
            FloorData.CurrentFloor = FloorPicker?.SelectedItem as Floor;
            int? selectedfloor = FloorData.CurrentFloor?.Level;
            if ( selectedfloor == null) return;

            LoadFloorData(FloorData.CurrentFloor);

            // выбрали этаж - отрисовали  
            canvasView.InvalidateSurface();
        }

        // нажатие кнопки переключения на предыдущий этаж
        private void OnStepedDown (object sender, EventArgs args)
        {
            if (FloorPicker.ItemsSource.Count == 0)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int? prevlevel = FloorData.CurrentFloor?.Level - 1;
            if ( prevlevel == 0) prevlevel--;

            Floor Nextfloor = FloorData.GetFloor(prevlevel);

            if (Nextfloor != null)
            {
                FloorData.CurrentFloor = Nextfloor;
                FloorPicker.SelectedItem = FloorData.CurrentFloor;
            }
            else
            {
                DependencyService.Get<IToast>().Show("Вы на нижнем этаже");
            }
        }
          
        // нажатие кнопки переключения на следующий этаж
        private void OnStepedUp   (object sender, EventArgs args)
        {
            if (FloorPicker.ItemsSource.Count == 0)
            {
                DependencyService.Get<IToast>().Show("Здание не загружено");
                return;
            }

            int? nextlevel = FloorData.CurrentFloor?.Level + 1;
            if ( nextlevel == 0) nextlevel++;

            Floor Nextfloor = FloorData.GetFloor(nextlevel);

            if (Nextfloor != null)
            {
                FloorData.CurrentFloor = Nextfloor;
                FloorPicker.SelectedItem = FloorData.CurrentFloor;
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

        // вызов отрисовки схемы при взаимодействии со схемой
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            
            // Отображение рисунка
            if (FloorData.CurrentFloor != null)
                bitmap.Paint(canvas, FloorData.CurrentFloor.Level);

            // Отрисовка матрицы преобразования
            // SKSize matrixSize = matrixDisplay.Measure(bitmap.Matrix);

            // matrixDisplay.Paint(canvas, bitmap.Matrix,
            //     new SKPoint(info.Width  - matrixSize.Width,
            //                 info.Height - matrixSize.Height));
        }

        #region Toolbar
        // возвращение в начальное положение схемы
        void RefrashMatrix(object sender, EventArgs args)
        {
            bitmap.Matrix = SKMatrix.MakeIdentity();

            canvasView.InvalidateSurface();
        }
        #endregion
    }
}
