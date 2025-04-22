using PropertyChanged;

namespace fitness_app.Models;

[AddINotifyPropertyChangedInterface]

public class CategoryItem
{
    public string? Title { get; set; }

    public string? ImageSource { get; set; }

    public bool IsSelected { get; set; }
}