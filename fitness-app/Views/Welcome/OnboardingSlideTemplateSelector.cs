using fitness_app.Models;

namespace fitness_app.Views.Welcome;

public class OnboardingSlideTemplateSelector : DataTemplateSelector
{
    public required DataTemplate Welcome02Template { get; set; }
    public required DataTemplate Welcome03Template { get; set; }
    public required DataTemplate Welcome04Template { get; set; }
    
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is OnboardingSlide slide)
        {
            switch (slide.SlideType)
            {
                case OnboardingSlideType.Welcome02:
                    return Welcome02Template;
                case OnboardingSlideType.Welcome03:
                    return Welcome03Template;
                case OnboardingSlideType.Welcome04:
                    return Welcome04Template;
            }
        }
        return Welcome02Template;
    }
}