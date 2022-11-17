using kzModUtils.ItemData;

#nullable enable

namespace kzModUtils.EventData
{
	public class EventConfig
	{
		public string EventModId { get; set; }

		public int EventId { get; internal set; }

		public string? Title { get; set; } = null;

		public int TitleId { get; internal set; } = -1;

		public IdHolder<CustomItemConfig>? RequiredItem { get; set; } = null;

		public EventConfig(string id)
		{
			this.EventModId = id;
		}

		internal EventMasterModel ToEventMasterModel(int eventId)
		{
			this.EventId = eventId;
			return new EventMasterModel(new CEventData.SEventData()
			{
				mId = eventId,
				mTitleId = TitleId,
				mHaveItemId = this.RequiredItem == null ? -1 : this.RequiredItem.Id,
				mIsUnManaged = false,
				mIsRecollection = false,
				mGroupId = -1,
				mEventType = 0,
				mPriority = 70,
				mMapId = -1,
				mIsPosition = false,
				mPositionX = 0,
				mPositionY = 0,
				mPositionZ = 0,
				mFinishedEventCondition = "",
				mIntervalDayCount = -1,
				mStartDate_Year = -1,
				mStartDate_Season = -1,
				mStartDate_Day = -1,
				mStartDate_Week = -1,
				mStartTime_Hour = -1,
				mStartTime_Minutes = -1,
				mEndDate_Year = -1,
				mEndDate_Season = -1,
				mEndDate_Day = -1,
				mEndDate_Week = -1,
				mEndTime_Hour = -1,
				mEndTime_Minutes = -1,
				mIsDailyTime = false,
				mWeather = -1,
				mIsDontStartOnRainy = false,
				mNpcId = -1,
				mNpcPresentItemId = -1,
				mNpcLikabilityCondition = "",
				mNpcTotalLikabilityCondition = "",
				mPetLikabilityCondition = "",
				mAfterEventDate_Day = -1,
				mAfterEventDate_Season = -1,
				mAfterEventDate_Year = -1,
				mAfterEventTime_Hour = -1,
				mAfterEventTime_Minutes = -1,
			});
		}
	}
}
