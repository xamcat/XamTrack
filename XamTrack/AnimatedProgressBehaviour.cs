using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamTrack
{
    public class AnimatedProgressBehaviour: Behavior<View>
    {        
        protected override void OnAttachedTo(View view)
        {
            view.PropertyChanged += ProgressBar_PropertyChanged; 
            base.OnAttachedTo(view);
        }

        protected override void OnDetachingFrom(View view)
        {
            view.PropertyChanged -= ProgressBar_PropertyChanged;
            base.OnDetachingFrom(view);
        }

        private async void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var view = (View)sender;
            if (e.PropertyName == "IsVisible")
            {
                while (view.IsVisible)
                {
                    await view.TranslateTo(-200,0, 1000, Easing.BounceIn);
                    await view.ScaleXTo(1, 1000, Easing.BounceIn);
                    await view.TranslateTo(100, 0, 1000, Easing.BounceIn);
                    await view.ScaleXTo(0.1, 1000, Easing.BounceIn);
                }
            }
        }
    }
}
