using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;

namespace TekConf.Mobile.Core
{
	public class Conference : INotifyPropertyChanged
	{

		[PrimaryKey, AutoIncrement]
		public virtual int Id { get; set; }

		[Indexed]
		public string Name { get; set; }

		[Indexed]
		public string Slug { get; set; }
		private DateTime _start;
		public DateTime Start
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
					OnPropertyChanged("Start");
					SetDateRange();
				}
			}
		}

		private DateTime _end;
		private string _dateRange;

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
					OnPropertyChanged("End");
					SetDateRange();
				}
			}
		}

		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string ImageUrlSquare { get; set; }

		public string DateRange
		{
			get { return _dateRange; }
			set
			{
				if (_dateRange != value)
				{
					_dateRange = value;
					OnPropertyChanged("DateRange");
				}
			}
		}

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


		public string FormattedCity
		{
			get
			{
				string formattedAddress;
				if (IsOnline == true)
				{
					formattedAddress = "online";
				}
				else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(State))
				{
					formattedAddress = City + ", " + State;
				}
				else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Country))
				{
					formattedAddress = City + ", " + Country;
				}
				else if (!string.IsNullOrWhiteSpace(City))
				{
					formattedAddress = City;
				}
				else
				{
					formattedAddress = "No location set";
				}

				return formattedAddress;
			}

		}

		public bool? IsOnline { get; set; }
		public int StreetNumber { get; set; }
		public string StreetName { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalArea { get; set; }
		public string Country { get; set; }
		public double Longitude { get; set; }
		public double Latitude { get; set; }


		public string FormattedAddress
		{
			get
			{
				string formattedAddress;
				if (IsOnline == true)
				{
					formattedAddress = "online";
				}
				else if (StreetNumber != default(int) && !string.IsNullOrWhiteSpace(StreetName) && !string.IsNullOrWhiteSpace(City) &&
								 !string.IsNullOrWhiteSpace(State))
				{
					formattedAddress = StreetNumber + " " + StreetName + "\n" + City + ", " + State + " " + PostalArea;
				}
				else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(State))
				{
					formattedAddress = City + ", " + State;
				}
				else if (!string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Country))
				{
					formattedAddress = City + ", " + Country;
				}
				else if (!string.IsNullOrWhiteSpace(City))
				{
					formattedAddress = City;
				}
				else
				{
					formattedAddress = "No location set";
				}

				return formattedAddress;
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

