---
title: Events
---

**Events** in Doraemon SoS source represents special triggers that may
happen when some group of criteria are matched.

They are mainly linked to triggering cutscenes when reaching certain places
and having certain levels of friendship.

But they may also be used to control some internal game state, the more notable
use of them for state tracking is when player buys one-time sold items,
like backpack upgrades.

# Relevant structures/managers/classes

## `EventMasterCollection`
Main storage of Event's static data, has accessors and references to everything


## `SEventTitleText`
Text of event titles, it follows the default Text structure. It is loaded into `TextManager`.

```C#
struct SEventTitleText
{
	int mId; // Text ID
	string mTextData; // Text content
}
```

## `SEventText`
Unknown


## `EventGroupMasterModel`
Stores the groups of events in the game. This is used to group the events in **What If? Phone Booth** gadget

Original structure comes from `SEventGroupData` with the following structure:
```C#
struct SEventGroupData
{
	int mGroupId; // Group ID - Events are linked using it
	int mNameId; // Text ID for the name of this group (from TextManager)
	int mIconId; // ID of the Icon asset for this group
}
```


## `EventMasterModel`
Definition on an Event, including the trigger conditions and how it should execute.

The structure is derived from `SEventData`: (Since both structures are quite similar, differences are commented in the structure below)
```C#
struct SEventData
{
	int mId; // Event ID
	int mTitleId; // Event title Text ID (see SEventTitleText). -1 means no title
	bool mIsUnManaged; //
	bool mIsRecollection; //
	int mGroupId; // Event Group ID (see EventGroupMasterModel). -1 means no group
	int mEventType; //
	int mPriority; //
	int mMapId; //
	bool mIsPosition; //
	float mPositionX; //
	float mPositionY; //
	float mPositionZ; //
	string mFinishedEventCondition; //
	int mIntervalDayCount; //
	int mStartDate_Year; //
	int mStartDate_Season; //
	int mStartDate_Day; //
	int mStartDate_Week; //
	int mStartTime_Hour; //
	int mStartTime_Minutes; //
	int mEndDate_Year; //
	int mEndDate_Season; //
	int mEndDate_Day; //
	int mEndDate_Week; //
	int mEndTime_Hour; //
	int mEndTime_Minutes; //
	bool mIsDailyTime; //
	int mWeather; //
	bool mIsDontStartOnRainy; //
	int mNpcId; //
	int mNpcPresentItemId; //
	string mNpcLikabilityCondition; //
	string mNpcTotalLikabilityCondition; //
	int mHaveItemId; //
	string mPetLikabilityCondition; //
	int mAfterEventDate_Year; //
	int mAfterEventDate_Season; //
	int mAfterEventDate_Day; //
	int mAfterEventTime_Hour; //
	int mAfterEventTime_Minutes; //
}
```
