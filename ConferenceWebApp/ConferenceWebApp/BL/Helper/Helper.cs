using ConferenceWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace ConferenceWebApp.BL.Helper
{
    public class DateAndDay
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
    }


    public static class Helper
    {
        public static int GetConferenceId()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                return DBContext.Conference.FirstOrDefault().ID;
            }
        }


        public static DateTime getConferenceDate()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int ConferenceId = GetConferenceId();
                return (DateTime)DBContext.Conference.First(x => x.ID == ConferenceId).ConferenceDate;
            }
        }

        public static SelectList GetProgramDays()
        {

            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int ConferenceId = Helper.GetConferenceId();
                Conference Conference = DBContext.Conference.FirstOrDefault(x => x.ID == ConferenceId);

                DateTime ConferenceStartDate = (DateTime)Conference.ConferenceDate;
                DateTime ConferenceEndDate = (DateTime)Conference.EndDate;

                int DaysDifference = Convert.ToInt32((ConferenceEndDate - ConferenceStartDate).TotalDays);

                List<SelectListItem> ProgramDaysList = new List<SelectListItem>();
                //if (DaysDifference == 0)
                //{
                //    ProgramDaysList.Add(new SelectListItem() { Text = "Day 1", Value = "1" });
                //}

                //for (int i = 0; i <= DaysDifference + 1; i++)
                //{
                //    ProgramDaysList.Add(new SelectListItem() { Text = "Day " + i, Value = i.ToString()/*, Selected=true*/ });
                //}


                for (int i = 0; i <= DaysDifference; i++)
                {
                    DateAndDay AddDateAndDay = new DateAndDay();
                    if (i == 0)
                    {

                        AddDateAndDay.Date = ConferenceStartDate;
                        AddDateAndDay.Day = "Day 1";
                        ProgramDaysList.Add(new SelectListItem() { Text = "Day 1", Value = "1" }); ;
                    }
                    else
                    {

                        AddDateAndDay.Date = ConferenceStartDate.AddDays(i);
                        AddDateAndDay.Day = "Day " + (i + 1).ToString();
                        ProgramDaysList.Add(new SelectListItem() { Text = "Day " + (i + 1), Value = (i + 1).ToString()/*, Selected=true*/ });
                    }
                }

                //if (i == 0)
                //{

                //    AddDateAndDay.Date = ConferenceStartDate;
                //    AddDateAndDay.Day = "Day 1";
                //    ProgramDaysList.Add(AddDateAndDay);
                //}
                //else
                //{

                //    AddDateAndDay.Date = ConferenceStartDate.AddDays(i);
                //    AddDateAndDay.Day = "Day " + (i + 1).ToString();
                //    ProgramDaysList.Add(AddDateAndDay);
                //}

                return (new SelectList(ProgramDaysList, "Value", "Text"));

            }

        }

        public static async Task<List<DateAndDay>> GetProgramDaysWithDate()
        {
            using (ConferenceAppEntities DBContext = new ConferenceAppEntities())
            {
                int ConferenceId = Helper.GetConferenceId();
                Conference Conference = await DBContext.Conference.FirstOrDefaultAsync(x => x.ID == ConferenceId);

                DateTime ConferenceStartDate = (DateTime)Conference.ConferenceDate;
                DateTime ConferenceEndDate = (DateTime)Conference.EndDate;

                int DaysDifference = Convert.ToInt32((ConferenceEndDate - ConferenceStartDate).TotalDays);

                List<DateAndDay> ProgramDaysList = new List<DateAndDay>();


                for (int i = 0; i <= DaysDifference; i++)
                {
                    DateAndDay AddDateAndDay = new DateAndDay();
                    if (i == 0)
                    {

                        AddDateAndDay.Date = ConferenceStartDate;
                        AddDateAndDay.Day = "Day 1";
                        ProgramDaysList.Add(AddDateAndDay);
                    }
                    else
                    {

                        AddDateAndDay.Date = ConferenceStartDate.AddDays(i);
                        AddDateAndDay.Day = "Day " + (i + 1).ToString();
                        ProgramDaysList.Add(AddDateAndDay);
                    }
                }

                return (ProgramDaysList);
            }

        }

        public static string TruncateAtWord(this string input)
        {
            if (input == null || input.Length < 99)
                return input;
            int iNextSpace = input.LastIndexOf(" ", 99);
            return string.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : 99).Trim());
        }

        public static void SendEmail(string ToEmailAddress, string ToName, string Password)
        {

            string myGmailAddress = "noreply.conferenceapp@gmail.com";
            string appSpecificPassword = "google1234!";

            SmtpClient smtp =
               new SmtpClient("smtp.gmail.com");

            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Credentials = new
               NetworkCredential(myGmailAddress,
               appSpecificPassword);

            MailMessage message = new MailMessage();
            message.Sender = new MailAddress(myGmailAddress,
               "Conference App");
            message.From = new MailAddress(myGmailAddress,
               "Conference App");

            message.To.Add(new MailAddress(ToEmailAddress,
               ToName));

            message.Subject = "New password for Conference App login";
            message.Body = @"<h1>Conference App Password Reset</h1>
                            <p><strong>Dear " + ToName + @", </strong></p>
                            <p>Your new password for Conference App login is : <strong>" + Password + "</strong></p>";

            message.IsBodyHtml = true;
            smtp.Send(message);
        }



    }
}