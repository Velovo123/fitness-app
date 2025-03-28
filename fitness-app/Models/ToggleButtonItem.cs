using PropertyChanged;

namespace fitness_app.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ToggleButtonItem
    {
        public bool IsSelected { get; set; }
        public string? Text { get; set; }
        public string? ImageSource { get; set; }
        
        [DependsOn(nameof(IsSelected), nameof(ImageSource))]
        public string? DisplayImageSource => 
            string.IsNullOrWhiteSpace(ImageSource) ? null : (IsSelected ? TintImage(ImageSource, "White") : ImageSource);
        
        private string TintImage(string source, string tint)
        {
            return source + "_" + tint.ToLower();
        }
    }
}