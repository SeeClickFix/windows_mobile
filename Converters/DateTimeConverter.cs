using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SeeClickFix.WP8.Converters
{
    public class DateTimeConverter : IValueConverter
    {
        static EpochToDateTimeConverter epochToDateTimeConverter = new EpochToDateTimeConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            DateTime dateTime;
            if (value is long)
            {
                dateTime = (DateTime)epochToDateTimeConverter.Convert(value, targetType, parameter, culture);
            }
            else
            {
                dateTime = (DateTime)value;
            }

            var now = DateTime.Now;
            if (now.Year == dateTime.Year)
            {
                if (now.Month == dateTime.Month)
                {
                    int days = (now.Day - dateTime.Day);
                    if (days == 0)
                    {
                        int hours = now.Hour - dateTime.Hour;
                        if (hours == 0)
                        {
                            var minutes = (now - dateTime).TotalMinutes;
                            if (minutes <= 1)
                            {
                                return "just now";
                            }
                            else if (minutes < 2)
                            {
                                return "about a minute ago";
                            }
                            else
                            {
                                return string.Format("about {0} minutes ago, at {1}", (int)minutes, dateTime.ToString("h:mm tt"));
                            }
                        }
                        else if (hours == 1)
                        {
                            return string.Format("about an hour ago, at {1}", hours, dateTime.ToString("h:mm tt"));
                        }
                        else if (hours < 6)
                        {
                            return string.Format("about {0} hours ago, at {1}", hours, dateTime.ToString("h:mm tt"));
                        }
                        else
                        {
                            return string.Format("today at {0}", dateTime.ToString("h:mm tt"));
                        }
                    }
                    else if (days == 1)
                    {
                        return string.Format("yesterday at {0}", dateTime.ToString("h:mm tt"));
                    }
                    else
                    {
                        return string.Format("{0} days ago, at {1}", days, dateTime.ToString("h:mm tt"));
                    }
                }
                else
                {
                    // not current month
                    return dateTime.ToString(@"MMMM d \a\t h:mm tt");
                }
            }
            else
            {
                return dateTime.ToString(@"MMMM d, yyyy \a\t h:mm tt");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
