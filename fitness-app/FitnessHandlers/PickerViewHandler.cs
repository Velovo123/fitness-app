#if IOS
using System;
using System.Collections;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using fitness_app.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;
using Foundation;

namespace fitness_app.FitnessHandlers
{
    public partial class PickerViewHandler : ViewHandler<PickerView, UIPickerView>
    {
        MyDataModel _dataModel;
        CADisplayLink _displayLink; 

        protected override UIPickerView CreatePlatformView()
        {
            return new UIPickerView(CGRect.Empty);
        }

        public static IPropertyMapper<PickerView, PickerViewHandler> PickerViewMapper =
            new PropertyMapper<PickerView, PickerViewHandler>(ViewHandler.ViewMapper)
            {
                [nameof(PickerView.ItemsSource)] = MapItemsSource,
                [nameof(PickerView.SelectedIndex)] = MapSelectedIndex,
                [nameof(PickerView.FontFamily)] = MapFontFamily,
                [nameof(PickerView.FontSize)] = MapFontSize,
                [nameof(PickerView.RowHeight)] = MapRowHeight,
            };

        public PickerViewHandler() : base(PickerViewMapper)
        {
        }

        protected override void ConnectHandler(UIPickerView platformView)
        {
            base.ConnectHandler(platformView);

            UpdateItemsSource();
            UpdateSelectedIndex();

            CustomizeSelectionIndicator(platformView);
            StartDisplayLink(platformView);
        }

        protected override void DisconnectHandler(UIPickerView platformView)
        {
            _displayLink?.Invalidate();
            _displayLink = null;
            base.DisconnectHandler(platformView);
        }

        public static void MapItemsSource(PickerViewHandler handler, PickerView view)
        {
            handler.UpdateItemsSource();
        }

        public static void MapSelectedIndex(PickerViewHandler handler, PickerView view)
        {
            handler.UpdateSelectedIndex();
        }

        public static void MapRowHeight(PickerViewHandler handler, PickerView view)
        {
            handler.UpdateItemsSource();
        }

        public static void MapFontFamily(PickerViewHandler handler, PickerView view)
        {
            handler.UpdateItemsSource();
        }

        public static void MapFontSize(PickerViewHandler handler, PickerView view)
        {
            handler.UpdateItemsSource();
        }

        void UpdateItemsSource()
        {
            if (VirtualView == null || PlatformView == null)
                return;

            var items = VirtualView.ItemsSource;
            var fontSize = VirtualView.FontSize;
            var fontFamily = VirtualView.FontFamily;

            var nativeFont = string.IsNullOrEmpty(fontFamily)
                ? UIFont.SystemFontOfSize((nfloat)fontSize)
                : UIFont.FromName(fontFamily, (nfloat)fontSize) ?? UIFont.SystemFontOfSize((nfloat)fontSize);

            _dataModel = new MyDataModel(
                items,
                row => VirtualView.SelectedIndex = row,
                nativeFont,
                VirtualView.RowHeight
            );

            PlatformView.Model = _dataModel;
        }

        void UpdateSelectedIndex()
        {
            if (VirtualView == null || PlatformView == null || _dataModel == null)
                return;

            int selectedIndex = VirtualView.SelectedIndex;
            var rowCount = _dataModel.GetRowsInComponent(PlatformView, 0);

            if (selectedIndex >= 0 && selectedIndex < rowCount)
            {
                PlatformView.Select(selectedIndex, 0, true);
                _dataModel.SetSelectedIndex(selectedIndex);
            }
        }

        void CustomizeSelectionIndicator(UIPickerView pickerView)
        {
            if (pickerView.Subviews.Length > 1)
            {
                try
                {
                    var indicator = pickerView.Subviews[1];
                    indicator.BackgroundColor = UIColor.Black;
                    indicator.Layer.ZPosition = -1;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to change selection indicator color: {ex}");
                }
            }
        }
        
        void StartDisplayLink(UIPickerView pickerView)
        {
            _displayLink = CADisplayLink.Create(() =>
            {
                UpdateLabelColors(pickerView);
            });
            _displayLink.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Default);
        }

        void UpdateLabelColors(UIPickerView pickerView)
        {
            CGRect indicatorFrame = CGRect.Empty;
            if (pickerView.Subviews.Length > 1)
            {
                indicatorFrame = pickerView.Subviews[1].Frame;
            }
            
            foreach (UIView subview in pickerView.Subviews)
            {
                if (subview is UILabel label)
                {
                    if (!indicatorFrame.IsEmpty && indicatorFrame.IntersectsWith(label.Frame))
                    {
                        label.TextColor = UIColor.White;
                    }
                    else
                    {
                        label.TextColor = UIColor.Black;
                    }
                    label.SetNeedsDisplay();
                }
            }
            pickerView.LayoutIfNeeded();
        }
    }

    public class MyDataModel : UIPickerViewModel
    {
        private readonly IList<string> _list = new List<string>();
        private readonly Action<int> _selectedHandler;
        private readonly UIFont _font;
        private readonly double _rowHeight;
        private int _selectedIndex = -1;

        public MyDataModel(IEnumerable items, Action<int> selectedHandler, UIFont font, double rowHeight)
        {
            _selectedHandler = selectedHandler;
            _font = font;
            _rowHeight = rowHeight;

            if (items != null)
            {
                foreach (var item in items)
                {
                    _list.Add(item.ToString());
                }
            }
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, IntPtr component) => (nfloat)_rowHeight;

        public override nint GetComponentCount(UIPickerView pickerView) => 1;

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component) => _list.Count;

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            UILabel label = view as UILabel;
            if (label == null)
            {
                label = new UILabel(pickerView.Bounds)
                {
                    TextAlignment = UITextAlignment.Center
                };
            }

            label.Font = _font;
            string text = _list[(int)row];

            var boldFont = UIFont.BoldSystemFontOfSize(_font.PointSize);

            var attributes = new UIStringAttributes {
                StrokeWidth = -3f, 
                StrokeColor = UIColor.White,
                ForegroundColor = UIColor.Black,
            };
            var highlightedAttributes = new UIStringAttributes {
                StrokeWidth = -3f, 
                StrokeColor = UIColor.Black,
                ForegroundColor = UIColor.White,
                Font = boldFont
            };
            label.AttributedText = new NSAttributedString(text, attributes);
            
            if ((int)row == _selectedIndex)
            {
                label.AttributedText = new NSAttributedString(text, highlightedAttributes);
                label.TextColor = UIColor.White;
            }
         
    
            return label;
        }

        public void SetSelectedIndex(int selectedIndex)
        {
            _selectedIndex = selectedIndex;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            _selectedIndex = (int)row;
            _selectedHandler?.Invoke((int)row);
            pickerView.ReloadAllComponents();
        }
    }
}

#endif
