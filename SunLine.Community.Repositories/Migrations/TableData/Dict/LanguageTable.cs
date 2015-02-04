using System;
using System.Linq;
using SunLine.Community.Entities.Dict;

namespace SunLine.Community.Repositories.Migrations.TableData.Dict
{
    public static class LanguageTable
    {
        public static void Initialize(IDatabaseContext context)
        {
            AddLanguage(context, "Abkhazian", "AB");
            AddLanguage(context, "Afar", "AA");
            AddLanguage(context, "Afrikaans", "AF");
            AddLanguage(context, "Albanian", "SQ");
            AddLanguage(context, "Amharic", "AM");
            AddLanguage(context, "Arabic", "AR");
            AddLanguage(context, "Armenian", "HY");
            AddLanguage(context, "Assamese", "AS");
            AddLanguage(context, "Aymara", "AY");
            AddLanguage(context, "Azerbaijani", "AZ");
            AddLanguage(context, "Bashkir", "BA");
            AddLanguage(context, "Basque", "EU");
            AddLanguage(context, "Bengali, Bangla", "BN");
            AddLanguage(context, "Bhutani", "DZ");
            AddLanguage(context, "Bihari", "BH");
            AddLanguage(context, "Bislama", "BI");
            AddLanguage(context, "Breton", "BR");
            AddLanguage(context, "Bulgarian", "BG");
            AddLanguage(context, "Burmese", "MY");
            AddLanguage(context, "Byelorussian", "BE");
            AddLanguage(context, "Cambodian", "KM");
            AddLanguage(context, "Catalan", "CA");
            AddLanguage(context, "Chinese", "ZH");
            AddLanguage(context, "Corsican", "CO");
            AddLanguage(context, "Croatian", "HR");
            AddLanguage(context, "Czech", "CS");
            AddLanguage(context, "Danish", "DA");
            AddLanguage(context, "Dutch", "NL");
            AddLanguage(context, "English, American", "EN");
            AddLanguage(context, "Esperanto", "EO");
            AddLanguage(context, "Estonian", "ET");
            AddLanguage(context, "Faeroese", "FO");
            AddLanguage(context, "Fiji", "FJ");
            AddLanguage(context, "Finnish", "FI");
            AddLanguage(context, "French", "FR");
            AddLanguage(context, "Frisian", "FY");
            AddLanguage(context, "Gaelic (Scots Gaelic)", "GD");
            AddLanguage(context, "Galician", "GL");
            AddLanguage(context, "Georgian", "KA");
            AddLanguage(context, "German", "DE");
            AddLanguage(context, "Greek", "EL");
            AddLanguage(context, "Greenlandic", "KL");
            AddLanguage(context, "Guarani", "GN");
            AddLanguage(context, "Gujarati", "GU");
            AddLanguage(context, "Hausa", "HA");
            AddLanguage(context, "Hebrew", "IW");
            AddLanguage(context, "Hindi", "HI");
            AddLanguage(context, "Hungarian", "HU");
            AddLanguage(context, "Icelandic", "IS");
            AddLanguage(context, "Indonesian", "IN");
            AddLanguage(context, "Interlingua", "IA");
            AddLanguage(context, "Interlingue", "IE");
            AddLanguage(context, "Inupiak", "IK");
            AddLanguage(context, "Irish", "GA");
            AddLanguage(context, "Italian", "IT");
            AddLanguage(context, "Japanese", "JA");
            AddLanguage(context, "Javanese", "JW");
            AddLanguage(context, "Kannada", "KN");
            AddLanguage(context, "Kashmiri", "KS");
            AddLanguage(context, "Kazakh", "KK");
            AddLanguage(context, "Kinyarwanda", "RW");
            AddLanguage(context, "Kirghiz", "KY");
            AddLanguage(context, "Kirundi", "RN");
            AddLanguage(context, "Korean", "KO");
            AddLanguage(context, "Kurdish", "KU");
            AddLanguage(context, "Laothian", "LO");
            AddLanguage(context, "Latin", "LA");
            AddLanguage(context, "Latvian, Lettish", "LV");
            AddLanguage(context, "Lingala", "LN");
            AddLanguage(context, "Lithuanian", "LT");
            AddLanguage(context, "Macedonian", "MK");
            AddLanguage(context, "Malagasy", "MG");
            AddLanguage(context, "Malay", "MS");
            AddLanguage(context, "Malayalam", "ML");
            AddLanguage(context, "Maltese", "MT");
            AddLanguage(context, "Maori", "MI");
            AddLanguage(context, "Marathi", "MR");
            AddLanguage(context, "Moldavian", "MO");
            AddLanguage(context, "Mongolian", "MN");
            AddLanguage(context, "Nauru", "NA");
            AddLanguage(context, "Nepali", "NE");
            AddLanguage(context, "Norwegian", "NO");
            AddLanguage(context, "Occitan", "OC");
            AddLanguage(context, "Oriya", "OR");
            AddLanguage(context, "Oromo, Afan", "OM");
            AddLanguage(context, "Pashto, Pushto", "PS");
            AddLanguage(context, "Persian", "FA");
            AddLanguage(context, "Polish", "PL");
            AddLanguage(context, "Portuguese", "PT");
            AddLanguage(context, "Punjabi", "PA");
            AddLanguage(context, "Quechua", "QU");
            AddLanguage(context, "Rhaeto-Romance", "RM");
            AddLanguage(context, "Romanian", "RO");
            AddLanguage(context, "Russian", "RU");
            AddLanguage(context, "Samoan", "SM");
            AddLanguage(context, "Sangro", "SG");
            AddLanguage(context, "Sanskrit", "SA");
            AddLanguage(context, "Serbian", "SR");
            AddLanguage(context, "Serbo-Croatian", "SH");
            AddLanguage(context, "Sesotho", "ST");
            AddLanguage(context, "Setswana", "TN");
            AddLanguage(context, "Shona", "SN");
            AddLanguage(context, "Sindhi", "SD");
            AddLanguage(context, "Singhalese", "SI");
            AddLanguage(context, "Siswati", "SS");
            AddLanguage(context, "Slovak", "SK");
            AddLanguage(context, "Slovenian", "SL");
            AddLanguage(context, "Somali", "SO");
            AddLanguage(context, "Spanish", "ES");
            AddLanguage(context, "Sudanese", "SU");
            AddLanguage(context, "Swahili", "SW");
            AddLanguage(context, "Swedish", "SV");
            AddLanguage(context, "Tagalog", "TL");
            AddLanguage(context, "Tajik", "TG");
            AddLanguage(context, "Tamil", "TA");
            AddLanguage(context, "Tatar", "TT");
            AddLanguage(context, "Tegulu", "TE");
            AddLanguage(context, "Thai", "TH");
            AddLanguage(context, "Tibetan", "BO");
            AddLanguage(context, "Tigrinya", "TI");
            AddLanguage(context, "Tonga", "TO");
            AddLanguage(context, "Tsonga", "TS");
            AddLanguage(context, "Turkish", "TR");
            AddLanguage(context, "Turkmen", "TK");
            AddLanguage(context, "Twi", "TW");
            AddLanguage(context, "Ukrainian", "UK");
            AddLanguage(context, "Urdu", "UR");
            AddLanguage(context, "Uzbek", "UZ");
            AddLanguage(context, "Vietnamese", "VI");
            AddLanguage(context, "Volapuk", "VO");
            AddLanguage(context, "Welsh", "CY");
            AddLanguage(context, "Wolof", "WO");
            AddLanguage(context, "Xhosa", "XH");
            AddLanguage(context, "Yiddish", "JI");
            AddLanguage(context, "Yoruba", "YO");
            AddLanguage(context, "Zulu", "ZU");
        }

        private static void AddLanguage(IDatabaseContext context, string name, string code)
        {
            if (!context.Languages.Any(x => x.Code == code))
            {
                context.Languages.Add(new Language { Code = code, Name = name, CreationDate = DateTime.UtcNow, Version = 1 });
            }
        }
    }
}
