using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishbook.Entities
{
	[SerializeField]
	internal class FishbookCondition
	{
		private readonly FishPointCondtion AllSeasons = FishPointCondtion.Spring | FishPointCondtion.Summer | FishPointCondtion.Autumn | FishPointCondtion.Winter;

		public FishingPointMasterModel ConditionModel { get; set; }

		public FishPointCondtion CompleteFlag { get; set; }

		public FishPointCondtion ProgressFlag {
			get { return this.mProgressFlag; }
			set { this.mProgressFlag = value; }
		}

		[SerializeField]
		private FishPointCondtion mProgressFlag;

		private bool IsTargetSeason(FishPointCondtion cond)
		{
			return ((this.CompleteFlag & cond) == cond);
		}

		private bool IsSeasonAchieved(FishPointCondtion cond)
		{
			// This season is not even valid
			if ((this.CompleteFlag & cond) != cond)
				return false;

			return ((this.ProgressFlag & cond) == cond);
		}

		public string[] GetConditionTexts()
		{
			string timeRange = "any time";
			if (this.ConditionModel.StartTime != -1 && this.ConditionModel.EndTime != -1) {
				timeRange = $"{this.ConditionModel.StartTime} ~ {this.ConditionModel.EndTime}";
			}

			string weather;
			switch (this.ConditionModel.Weather)
			{
				case (int) Define.Weather.TypeEnum.Cloudy:
					weather = "Cloudy";
					break;

				case (int) Define.Weather.TypeEnum.Rainy:
					weather = "Rainy";
					break;

				case (int) Define.Weather.TypeEnum.Shower:
					weather = "Shower";
					break;

				case (int) Define.Weather.TypeEnum.Sunny:
					weather = "Sunny";
					break;

				default:
					weather = "";
					break;
			}

			if (weather.Length > 0)
				weather = $", {weather} days";


			if ((this.ProgressFlag & AllSeasons) == AllSeasons)
				return new string[] { $"Every season, {timeRange}{weather}" };

			var conditions = new List<string>();
			if (IsTargetSeason(FishPointCondtion.Spring)) {
				if (IsSeasonAchieved(FishPointCondtion.Spring))
					conditions.Add($"Spring, {timeRange}{weather}");
				else
					conditions.Add("???");
			}

			if (IsTargetSeason(FishPointCondtion.Summer)) {
				if (IsSeasonAchieved(FishPointCondtion.Summer))
					conditions.Add($"Summer, {timeRange}{weather}");
				else
					conditions.Add("???");
			}

			if (IsTargetSeason(FishPointCondtion.Autumn)) {
				if (IsSeasonAchieved(FishPointCondtion.Autumn))
					conditions.Add($"Autumn, {timeRange}{weather}");
				else
					conditions.Add("???");
			}

			if (IsTargetSeason(FishPointCondtion.Winter)) {
				if (IsSeasonAchieved(FishPointCondtion.Winter))
					conditions.Add($"Winter, {timeRange}{weather}");
				else
					conditions.Add("???");
			}

			return conditions.ToArray();
		}

		public void UpdateProgress()
		{
			var season = SingletonMonoBehaviour<UserManager>.Instance.User.Time.Season;
			switch (season) {
			case Define.Time.SeasonEnum.Spring:
				this.ProgressFlag |= FishPointCondtion.Spring;
				break;
			case Define.Time.SeasonEnum.Summer:
				this.ProgressFlag |= FishPointCondtion.Summer;
				break;
			case Define.Time.SeasonEnum.Autumn:
				this.ProgressFlag |= FishPointCondtion.Autumn;
				break;
			case Define.Time.SeasonEnum.Winter:
				this.ProgressFlag |= FishPointCondtion.Winter;
				break;
			}
		}

		public int GetConditionsCount()
		{
			int count = 0;
			foreach (var flag in Enum.GetValues(typeof(FishPointCondtion)) as FishPointCondtion[])
			{
				if (flag == FishPointCondtion.None)
					continue;

				if ((this.CompleteFlag & flag) == flag)
					count++;
			}

			return count;
		}

		public int GetCompletedConditionsCount()
		{
			int count = 0;
			foreach (var flag in Enum.GetValues(typeof(FishPointCondtion)) as FishPointCondtion[])
			{
				if (flag == FishPointCondtion.None)
					continue;

				if ((this.ProgressFlag & flag) == flag)
					count++;
			}

			return count;
		}
	}
}
