using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TidyPhoto.ViewModel.Converter
{
    public class FontNameToKoreanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cond = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentUICulture.Name);

            ReadOnlyCollection<FontFamily> ENGFontList = (ReadOnlyCollection<FontFamily>)value;
            List<string> KORFontList = new List<string>();

            foreach (FontFamily font in ENGFontList)
            {
                if (font.FamilyNames.ContainsKey(cond))
                    KORFontList.Add(font.FamilyNames[cond]);
                else
                    KORFontList.Add(font.ToString());
            }
            KORFontList.Sort();
            return KORFontList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
