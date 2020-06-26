using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamTrack
{
    public class AnimatedProgressBehaviour: Behavior<ProgressBar>
    {        
        protected override void OnAttachedTo(ProgressBar progressBar)
        {
            progressBar.PropertyChanged += ProgressBar_PropertyChanged; 
            base.OnAttachedTo(progressBar);
        }

        protected override void OnDetachingFrom(ProgressBar progressBar)
        {
            progressBar.PropertyChanged -= ProgressBar_PropertyChanged;
            base.OnDetachingFrom(progressBar);
        }

        private async void ProgressBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var progressBar = (ProgressBar)sender;
            if ((e.PropertyName == "Progress") && progressBar.Progress == 1.0)
            {
                progressBar.RotateTo(0, 1);
                progressBar.ScaleXTo(1, 200, Easing.BounceIn);
                await progressBar.ScaleYTo(1, 200, Easing.BounceOut);
                await progressBar.ProgressTo(0, 3000, Easing.Linear);
                progressBar.ScaleYTo(5, 200, Easing.BounceIn);
                await progressBar.ScaleXTo(0.01, 200, Easing.BounceIn);
                progressBar.RotateTo(360, 1000);
            }
        }
    }
}
