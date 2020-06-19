using Android.App;
using Android.Widget;
using Android.OS;

namespace PhysikComputer_Android
{
    [Activity(Label = "PhysikComputer_Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        EditText edittext1;
        TextView textview1;
        Button button1;
        System.Timers.Timer timer1;
        System.Timers.Timer timer2;
        int value;
        int startvalue;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);            
            SetContentView (Resource.Layout.Main);
            edittext1 = FindViewById<EditText>(Resource.Id.editText1);
            textview1 = FindViewById<TextView>(Resource.Id.textView1);
            button1 = FindViewById<Button>(Resource.Id.button1);
            timer1 = new System.Timers.Timer();
            timer2 = new System.Timers.Timer();
            button1.Click += delegate{
                edittext1.Enabled = false;
                timer1.Interval = int.Parse(edittext1.Text);
                textview1.Text = edittext1.Text;
                value = int.Parse(edittext1.Text);
                startvalue = int.Parse(edittext1.Text);
                timer1.Elapsed += delegate
                {
                    
                };
                timer1.Enabled = true;
                timer1.Start();
                timer2.Interval = 1000;                
                timer2.Elapsed += delegate
                {
                    RunOnUiThread(delegate { 
                        textview1.Text = (value -1).ToString();
                        value -= 1;
                        if (value <= 0) { value = startvalue; }
                    });
                };
                timer2.Enabled = true;
                timer2.Start();
            };
        }
    }
}

