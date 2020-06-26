using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamTrack
{
    public class AnimatedProgressBehaviour: Behavior<ProgressBar>
    {
        bool _isAnimating;
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
                if (!_isAnimating)
                {
                    _isAnimating = true;
                    await progressBar.ProgressTo(0, 5000, Easing.Linear);
            
                    _isAnimating = false;
                    await progressBar.ProgressTo(1, 1, Easing.Linear);

                }
            }
        }
    }
}
