using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float speed = 2;
    [SerializeField] bool secondsEqHours = false;
    [SerializeField] Clock clock;
    private void Start()
    {
        clock.month = Clock.Month.January;
        StartCoroutine(RecordTime());
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
                clock.hour++;
                clock.minute = 0;
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
            if (clock.hour > 24)
            {
                clock.hour = 0;
                clock.day++;
            }
            if (clock.month == Clock.Month.December && clock.day - 1 == days)
            {
                clock.month = Clock.Month.January;
                clock.day = 1;
                if (clock.afterH)
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


            if (clock.year == 0 && !clock.afterH)
                clock.afterH = true;
            if (speed <= 0)
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitForSeconds(1f / speed);
        }
    }
    public bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
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
    public bool afterH = false;
}
