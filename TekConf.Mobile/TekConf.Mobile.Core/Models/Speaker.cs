using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;
using TekConf.Mobile.Core.Annotations;

namespace TekConf.Mobile.Core
{
	public class Speaker
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[Indexed]
		public string Slug { get; set; }

		public int SessionId { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Description { get; set; }
		public string BlogUrl { get; set; }
		public string TwitterName { get; set; }
		public string FacebookUrl { get; set; }
		public string LinkedInUrl { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string Url { get; set; }
		public string FullName { get; set; }
		public string ProfileImageUrl { get; set; }
		public string GooglePlusUrl { get; set; }
		public string VimeoUrl { get; set; }
		public string YoutubeUrl { get; set; }
		public string GithubUrl { get; set; }
		public string CoderWallUrl { get; set; }
		public string StackoverflowUrl { get; set; }
		public string BitbucketUrl { get; set; }
		public string CodeplexUrl { get; set; }
	}
	
}