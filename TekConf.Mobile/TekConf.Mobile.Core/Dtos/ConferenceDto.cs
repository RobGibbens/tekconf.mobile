namespace TekConf.Mobile.Core.Dtos
{
	using System;
	using System.Collections.Generic;

	public class ConferenceDto
	{
		public string Name { get; set; }

		public string Slug { get; set; }

		public DateTime Start { get; set; }

		public DateTime End { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		string _imageUrlSquare;
		public string ImageUrlSquare {
			get {
				if (string.IsNullOrWhiteSpace(_imageUrlSquare))
				{
					return ImageUrl;
				}
				return _imageUrlSquare;
			}
			set {
				_imageUrlSquare = value;
			}
		}

		public List<SessionDto> Sessions { get; set; }
	}
}