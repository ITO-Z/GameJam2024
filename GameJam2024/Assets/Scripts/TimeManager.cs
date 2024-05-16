using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    bool ff = false;
    [Tooltip("duration in seconds")] [SerializeField] float duration = 5;
    [SerializeField] int paymentsPerMonth = 1;
    [SerializeField] bool timeInDays = false;
    [SerializeField] bool timeInMonths = false;
    [SerializeField] bool timeInYears = false;
    public Clock clock;
    [Header("TextFields")]
    [SerializeField] Text timeField;
    [SerializeField] Text dateField;
    [SerializeField] public date dateData;
    [SerializeField] RegionBehaviour[] regionBehaviours;
    [SerializeField] LogMessages log;
    float durationCopy;

    private SoundManager soundManager;
    private int times = 0;

    [System.Serializable]
    public struct date
    {
        public bool day, month, year;
    };
    private void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        durationCopy = duration;
        clock.month = Clock.Month.January;
        UpdateDateField();
    }
    private void Update()
    {
        StartCoroutine(RecordTime()); //records time in months
        StartCoroutine(Generate());
    }
    bool generating = false;
    IEnumerator Generate()
    {
        if (!generating)
        {
            generating = true;
            yield return new WaitForSeconds(durationCopy / paymentsPerMonth);
            foreach (var reg in regionBehaviours)
            {
                if (reg.conquered)
                {
                    reg.GenerateMaterials();
                    if(times%Random.Range(30, 50) == 0){
                        soundManager.Play(5);
                        times++;
                    }
                }
            }

            generating = false;
        }
        else yield return null;
    }
    bool startedRec = false;
    IEnumerator RecordTime()
    {
        if (!startedRec)
        {
            Events();
            startedRec = true;
            durationCopy = duration / (ff ? 2f : 1f);
            yield return new WaitForSeconds(durationCopy);
            Camera.main.GetComponent<GameManager>().SaveGame();
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
            if (timeInDays)
                clock.day++;
            if (timeInMonths)
                clock.month++;
            if (timeInYears)
                if (!clock.bc)
                    clock.year++;
                else clock.year--;

            if (clock.day >= days)
                clock.month++;

            if ((int)clock.month >= 12)
            {
                clock.month = Clock.Month.January;
                if (!clock.bc)
                    clock.year++;
                else clock.year--;

                if (IsLeapYear(clock.year))
                    log.SendMessageInLog($"Year {clock.year} is a leap year.", LogMessages.typeOfLogMessage.eveniment);
            }

            if (clock.year == 0 && clock.bc)
                clock.bc = false;
            UpdateDateField();

            startedRec = false;
        }
        else yield return null;
    }
    void UpdateDateField()
    {
        if (dateField != null)
        {
            if (dateData.day && dateData.month && dateData.year)
            {
                dateField.text = $"{clock.day}, {clock.month}, {clock.year}, {(clock.bc ? "BC" : "AC")}";
            }
            else if (dateData.month && dateData.year)
                dateField.text = $"{clock.month}, {clock.year}, {(clock.bc ? "BC" : "AC")}";
            else if (dateData.year)
                dateField.text = $"{clock.year}, {(clock.bc ? "BC" : "AC")}";
        }
    }
    public bool IsLeapYear(int year)
    {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    bool[] events = { false, false, false, false, false, false };
    public void Events()
    {
        if (clock.year == 82 && !events[0] && clock.bc)
        {
            events[0] = true;
            log.SendMessageInLog("Year 82 BC. Burebista rises to power and starts centralizing the Dacian tribes.", LogMessages.typeOfLogMessage.eveniment);
        }
        if (clock.year == 75 && !events[1] && clock.bc)
        {
            events[1] = true;
            log.SendMessageInLog("Year 75 BC. Burebista consolidates his power and implements reforms within the Dacian Kingdom.", LogMessages.typeOfLogMessage.eveniment);
        }
        if (clock.year == 61 && !events[2] && clock.bc)
        {
            events[2] = true;
            log.SendMessageInLog("Year 61 BC. Burebista establishes diplomatic relations with the Roman Republic to secure his kingdom's borders.", LogMessages.typeOfLogMessage.eveniment);
        }
        if (clock.year == 55 && !events[3] && clock.bc)
        {
            events[3] = true;
            log.SendMessageInLog("Year 55 BC. Relations between the Dacian and Roman Republics deteriorate due to territorial disputes and tensions along the Danube frontier.", LogMessages.typeOfLogMessage.eveniment);
        }
        if (clock.year == 50 && !events[4] && clock.bc)
        {
            events[4] = true;
            log.SendMessageInLog("Year 50 BC. Burebista strengthens Dacia's military and fortifies its borders in preparation for potential conflicts with the Roman Republic.", LogMessages.typeOfLogMessage.eveniment);
        }
        if (clock.year == 44 && !events[5] && clock.bc)
        {
            events[5] = true;
            log.SendMessageInLog("Year 44 BC. Burebista is assassinated, leading to a power vacuum and internal strife within Dacia as rival factions compete for control.", LogMessages.typeOfLogMessage.eveniment);
            StartCoroutine(DelayEnd());
        }
    }
    public void FastForwardTime()
    {
        ff = !ff;
    }
    IEnumerator DelayEnd()
    {
        yield return new WaitForSeconds(2.5f);
        Camera.main.GetComponent<GameManager>().EndGame(false);
    }
}

[System.Serializable]
public class Clock
{
    public int day = 1, year = 82;
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
    public bool bc = true;
}
