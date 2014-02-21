using System.Drawing;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using TekConf.Mobile.Core.ViewModels;
using Cirrious.MvvmCross.Binding.Touch.Views;

namespace TekConf.Mobile.iOS.Views
{
//	[Register("ConferencesView")]
//	public class ConferencesView : MvxViewController
//    {
//        public override void ViewDidLoad()
//        {
//            View = new UIView(){ BackgroundColor = UIColor.White};
//            base.ViewDidLoad();
//
//			// ios7 layout
//            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
//               EdgesForExtendedLayout = UIRectEdge.None;
//			   
//            var label = new UILabel(new RectangleF(10, 10, 300, 40));
//            Add(label);
//            var textField = new UITextField(new RectangleF(10, 50, 300, 40));
//            Add(textField);
//
//			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel>();
////            set.Bind(label).To(vm => vm.Hello);
////            set.Bind(textField).To(vm => vm.Hello);
//            set.Apply();
//        }
//    }

	[Register("ConferencesView")]
	public class ConferencesView : MvxTableViewController
	{
		public ConferencesView()
		{
			Title = "Conferences";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				EdgesForExtendedLayout = UIRectEdge.None;

			var source = new MvxStandardTableViewSource(TableView, "TitleText Name");
			TableView.Source = source;

			var set = this.CreateBindingSet<ConferencesView, ConferencesViewModel>();
			set.Bind(source).To(vm => vm.Conferences);
			//set.Bind(source).For(s => s.SelectionChangedCommand).To(vm => vm.ShowDetailCommand);
			set.Apply();
			(this.DataContext as ConferencesViewModel).Init ();
			TableView.ReloadData();
		}
	}
}