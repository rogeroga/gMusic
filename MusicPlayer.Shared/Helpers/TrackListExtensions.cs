﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicPlayer.Api;
using MusicPlayer.Data;
using MusicPlayer.Models;

namespace MusicPlayer.Helpers
{
	internal static class TrackListExtensions
	{
		public static List<Track> SortByPriority(this List<Track> tracks)
		{
			return tracks.OrderByDescending(x => x, new TrackPriorityComparer()).ToList();
		}
	}

	public class TrackPriorityComparer : IComparer<Track>
	{
		public int Compare(Track x, Track y)
		{
			var priority = x.Priority.CompareTo(y.Priority);
			if (priority != 0)
				return priority;

			var servicePriority = Priority(x.ServiceType).CompareTo(Priority(y.ServiceType));
			if (servicePriority != 0)
				return servicePriority;

			var mediaPriority = Priority(x.MediaType).CompareTo(Priority(y.MediaType));
			return mediaPriority;
		}

		public int Priority(MediaType media)
		{
			switch (media)
			{
				case MediaType.Audio:
					return 100;
				case MediaType.Video:
					return Settings.PreferVideos ? 150 : 50;
			}
			return 0;
		}

		public int Priority(ServiceType serviceType)
		{
			switch (serviceType)
			{
				case ServiceType.FileSystem:
					return 100;
				case ServiceType.iPod:
					return 90;
				case ServiceType.Google:
				case ServiceType.Amazon:
					return 80;
				default:
					return 0;
			}
		}
	}
}