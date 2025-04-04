using System.Collections;

namespace fitness_app.Controls;

public class PickerView : View
{
    public event EventHandler<SelectedIndexChangedEventArgs>? SelectedIndexChanged;
    
    public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(PickerView),
                default(IEnumerable),
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
                nameof(SelectedIndex),
                typeof(int),
                typeof(PickerView),
                defaultValue: -1,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: OnSelectedIndexChanged);

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(
                nameof(FontFamily),
                typeof(string),
                typeof(PickerView),
                default(string));

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(
                nameof(FontSize),
                typeof(double),
                typeof(PickerView),
                14.0);

        public static readonly BindableProperty RowHeightProperty =
            BindableProperty.Create(nameof(RowHeight),
                typeof(double),
                typeof(PickerView),
                50.0);


        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        
        public double RowHeight
        {
            get => (double)GetValue(RowHeightProperty);
            set => SetValue(RowHeightProperty, value);
        }
    
        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerView)?.UpdateItemsSource();
        }

        private static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (PickerView)bindable;
            control.UpdateSelectedIndex();
            control.SelectedIndexChanged?.Invoke(control, new SelectedIndexChangedEventArgs((int)oldValue, (int)newValue));
        }

        private void UpdateItemsSource()
        {
            Handler?.UpdateValue(nameof(ItemsSource));
        }

        private void UpdateSelectedIndex()
        {
            Handler?.UpdateValue(nameof(SelectedIndex));
        }
}

public class SelectedIndexChangedEventArgs : EventArgs
{
    public int OldIndex { get; }
    public int NewIndex { get; }

    public SelectedIndexChangedEventArgs(int oldIndex, int newIndex)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
    }
}