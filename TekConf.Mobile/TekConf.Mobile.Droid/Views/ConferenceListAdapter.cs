using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Droid.Views;
using TekConf.Mobile.Core.ViewModels;
using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using TekConf.Mobile.Core.Messages;
using Android.Widget;
using Android.Content;
using Android.Views;
using System.Collections.Generic;
using TekConf.Mobile.Core;
using System.Linq;
using Android.Graphics.Drawables;
using UrlImageViewHelper;

namespace TekConf.Mobile.Droid.Views
{

	public class ConferenceListAdapter : BaseAdapter<Conference>
	{
		Activity _activity;
		List<Conference> _conferences;

		public ConferenceListAdapter (Activity activity, IEnumerable<Conference> conferences)
		{
			_conferences = conferences.OrderBy (c => c.Start).ToList ();
			_activity = activity;
		}

		public override int Count {
			get { return _conferences.Count; }
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override Conference this [int index] {
			get { return _conferences [index]; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView;
			if (view == null) {
				view = _activity.LayoutInflater.Inflate (Resource.Layout.Item_Conference, parent, false);
				view.SetTag(Resource.Id.conferenceImage, view.FindViewById(Resource.Id.conferenceImage));
				view.SetTag (Resource.Id.conferenceName, view.FindViewById (Resource.Id.conferenceName));
			}

			var conference = _conferences [position];

			TextView conferenceNameTextView = view.FindViewById<TextView> (Resource.Id.conferenceName);
			if (conferenceNameTextView != null)
				conferenceNameTextView.Text = conference.Name;

//			TextView conferenceDateRangeTextView = view.FindViewById<TextView> (Resource.Id.conferenceDateRange);
//			if (conferenceDateRangeTextView != null)
//				conferenceDateRangeTextView.Text = conference.DateRange ();

			ImageView imageView = view.FindViewById<SquareImage> (Resource.Id.conferenceImage);
			if (imageView != null) {
				imageView.SetUrlDrawable (conference.ImageUrl, Resource.Drawable.DefaultConference);
			}
			return view;
		}

	}
}