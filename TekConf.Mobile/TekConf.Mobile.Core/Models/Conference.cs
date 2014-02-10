using System;
using Cirrious.MvvmCross.ViewModels;

namespace TekConf.Mobile.Core
{
	public class Conference : MvxViewModel
	{
	    private DateTime _start;
	    private DateTime _end;

	    public Conference ()
		{
		}

        public string Name { get; set; }

	    public new DateTime Start
	    {
	        get
	        {
	            return _start;
	        }
	        set
	        {
                if (_start != value)
                {
                    _start = value;
                    RaisePropertyChanged(() => Start);
                    SetDateRange();
                }
	        }
	    }

	    public DateTime End
	    {
	        get
	        {
	            return _end;
	        }
	        set
	        {
                if (_end != value)
                {
                    _end = value;
                    RaisePropertyChanged(() => End);
                    SetDateRange();
                }
	        }
	    }

	    public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string DateRange { get; set; }

        public void SetDateRange()
        {
            string range;
            if (Start == default(DateTime) || End == default(DateTime))
            {
                range = "No Date Set";
            }
            else if (Start.Month == End.Month && Start.Year == End.Year)
            {
                // They begin and end in the same month
                if (Start.Date == End.Date)
                {
                    range = Start.ToString("MMMM") + " " + Start.Day + ", " + Start.Year;
                }
                else
                    range = Start.ToString("MMMM") + " " + Start.Day + " - " + End.Day + ", " + Start.Year;
            }
            else
            {
                // They begin and end in different months
                if (Start.Year == End.Year)
                {
                    range = Start.ToString("MMMM") + " " + Start.Day + " - " + End.ToString("MMMM") + " " + End.Day + ", " + Start.Year;
                }
                else
                {
                    range = Start.ToString("MMMM") + " " + Start.Day + ", " + Start.Year + " - " + End.ToString("MMMM") + " " + End.Day + ", " + End.Year;
                }

            }

            DateRange = range;
        }

	}
}

