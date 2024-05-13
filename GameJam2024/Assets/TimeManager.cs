using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public enum TimeZone
    {
        US,
        EU
    }
    public TimeZone timeZone;
    [SerializeField] float speed = 2;
    [SerializeField] bool secondsEqHours = false;
    [SerializeField] Clock clock;
    [SerializeField] Light2D globalLight;
    [SerializeField] bool dayNightCycle = false;

    [Header("TextFields")]
    [SerializeField] Text timeField;
    [SerializeField] Text dateField;
    [SerializeField] public date dateData;
    [SerializeField] RegionBehaviour[] regionBehaviours;
    [System.Serializable]
    public struct date
    {
        public bool day, month, year;
    };
    private void Start()
    {
        clock.month = Clock.Month.January;
        StartCoroutine(RecordTime());
    }
    private void Update()
    {
        if (dayNightCycle)
            StartCoroutine(DayNightCycle());
    }
    IEnumerator RecordTime()
    {
        while (true)
        {
            #region Days
            int days = 0;
            if (clock.month == Clock.Month.January || clock.month == Clock.Month.March || clock.month == Clock.Month.May || clock.month == Clock.Month.July || clock.month == Clock.Month.August || clock.month == Clock.Month.October || clock.month == Clock.Month.December)
                days = 31;
            else if (clock.month == Clock.Month.April || clock.month == Clock.Month.June || clock.month == Clock.Month.September || clock.month == Clock.Month.November)
                days = 30;
            else if (clock.month == Clock.Month.February)
            {
                if (IsLeapYear(clock.year))
                    days = 29;
                else days = 28;
            }
            #endregion
            if (secondsEqHours)
            {
                clock.minute += 30;
                if (clock.minute >= 60)
                {
                    clock.minute = 0;
                    clock.hour++;
                }
            }
            else
            {
                clock.minute++;
                if (clock.minute >= 60)
                {
                    clock.minute = 0;
                    clock.hour++;
                }
            }
            if (clock.hour >= 24)
            {
                clock.hour = 0;
                clock.day++;
                foreach (var regionBhvr in regionBehaviours)
                {
                    regionBhvr.GenerateMaterials();
                }
            }
            if (clock.month == Clock.Month.December && clock.day - 1 == days)
            {
                clock.month = Clock.Month.January;
                clock.day = 1;
                if (clock.bc)
                    clock.year++;
                else clock.year--;
                if (IsLeapYear(clock.year))
                    Debug.Log($"Year {clock.year} is a leap year");
            }
            if (clock.day - 1 == days)
            {
                clock.day = 1;
                clock.month++;
            }


            if (clock.year == 0 && !clock.bc)
                clock.bc = true;
            if (timeField != null)
                if (timeZone == TimeZone.US)
                {
                    // Convert clock.hour to 12-hour format
                    int hour12 = clock.hour % 12;
                    if (hour12 == 0)
                        hour12 = 12; // 0 hour should be 12 in 12-hour format

                    // Determine if it's AM or PM
                    string amPm = (clock.hour >= 12 && clock.hour < 24) ? "pm" : "am";

                    // Update the timeField text
                    timeField.text = $"{hour12}:{(clock.minute < 10 ? "0" : "")}{clock.minute}{amPm}";
                }
                else if (timeZone == TimeZone.EU)
                {
                    timeField.text = $"{(clock.hour % 24 < 10 ? "0" : "")}{clock.hour % 24}:{(clock.minute < 10 ? "0" : "")}{clock.minute}";
                }
            if (dateField != null)
            {
                if (dateData.day && dateData.month && dateData.year)
                {
                    dateField.text = $"{clock.day}, {clock.month}, {clock.year}, {(clock.bc ? "BC" : "AD")}";
                }
                else if (dateData.month && dateData.year)
                    dateField.text = $"{clock.month}, {clock.year}, {(clock.bc ? "BC" : "AD")}";
                else if (dateData.year)
                    dateField.text = $"{clock.year}, {(clock.bc ? "BC" : "AD")}";

            }

            float half = 1f;
            if (secondsEqHours)
                half = 2;
            if (speed <= 0)
                yield return new WaitForSeconds(1f / half);
            else
                yield return new WaitForSeconds((1f / half) / speed);
        }
    }
    public bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }
    float intensity = 0;
    IEnumerator DayNightCycle()
    {

        float t = (clock.hour + (clock.minute == 30 ? .5f : 0)) / 12f;

        if (t <= 1)
        {
            intensity = Mathf.Lerp(0.05f, 1, t);
        }
        else if (t > 1)
        {
            intensity = Mathf.Lerp(1, 0.05f, t - 1);
        }
        float initIntens = globalLight.intensity;

        float t1 = 0;

        while (globalLight.intensity != intensity)
        {
            globalLight.intensity = Mathf.Lerp(initIntens, intensity, t1);
            t1 += .05f;
            yield return new WaitForSeconds(.01f);
        }

    }
}

[System.Serializable]
public class Clock
{
    public int hour, minute, day = 1, year = 82;
    [System.Serializable]
    public enum Month
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
    public Month month;
    public bool bc = false;
}
