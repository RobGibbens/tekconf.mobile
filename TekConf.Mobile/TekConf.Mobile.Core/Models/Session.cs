﻿using System;
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
        public string Slug { get; set; }
        public int ConferenceId { get; set; }
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
        public bool IsAddedToSchedule { get; set; }

        public string StartDescription()
        {
            if (Start == default(DateTime))
            {
                return "Not scheduled yet";
            }

            return Start.ToString("dddd h:mm tt");
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