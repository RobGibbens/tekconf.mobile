using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;
using TekConf.Mobile.Core.Annotations;

namespace TekConf.Mobile.Core
{
	public class Session : INotifyPropertyChanged
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public string Slug { get; set; }
		public int ConferenceId { get; set; }
		
		[Indexed]
		public string Title { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		private string _room;
		public string Room
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(_room) && _room.Length == 1)
					_room = "Room " + _room;

				return _room;
			}
			set
			{
				_room = value;
			}
		}

		public string Difficulty { get; set; }
		public string Description { get; set; }
		public string TwitterHashTag { get; set; }
		public string SessionType { get; set; }

		private bool _isAddedToSchedule;
		public bool IsAddedToSchedule
		{
			get { return _isAddedToSchedule; }
			set
			{
				if (_isAddedToSchedule != value)
				{
					_isAddedToSchedule = value;
					OnPropertyChanged("IsAddedToSchedule");
					OnPropertyChanged("State");
				}
			}
		}

		private SessionScheduleState _state;
		public SessionScheduleState State
		{
			get
			{
				_state = _isAddedToSchedule ? SessionScheduleState.Added : SessionScheduleState.NotAdded;
				return _state;
			}
		}

		public string StartDescription
		{
			get
			{
				if (Start == default(DateTime))
				{
					return "Not scheduled yet";
				}

				return Start.ToString("dddd h:mm tt");
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}