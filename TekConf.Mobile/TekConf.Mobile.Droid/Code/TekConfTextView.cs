using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace TekConf.Mobile.Droid
{
    public class TekConfTextView : TextView
    {
        readonly Context _context;
        public TekConfTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _context = context;
            Init();
        }
        public TekConfTextView(Context context)
            : base(context)
        {
            _context = context;
            Init();
        }

        private void Init()
        {
            Typeface tf = Typeface.CreateFromAsset(_context.Assets, "fonts/OpenSans-Light.ttf");
            this.Typeface = tf;
            this.SetTextColor(Color.Black);
        }
    }
}