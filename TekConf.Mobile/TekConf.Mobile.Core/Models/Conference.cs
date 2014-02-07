using System;

namespace TekConf.Mobile.Core
{
	public class Conference
	{
		public Conference ()
		{
		}

        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
	}
}

