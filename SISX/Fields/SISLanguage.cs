using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SISX.Fields
{
    public class SISLanguage : SISField
    {
        public UInt32 language; // TLanguage


        public SISLanguage(BinaryReader br)
            : base(br)
        {
        }

        protected override void ReadValue(BinaryReader br)
        {
            language = br.ReadUInt32();
        }

        public override string ToString()
        {
            TLanguage lang = (TLanguage) language;
            return lang.ToString();
        }
    }


    public enum TLanguage
    {
        /**
        Enumerated value used for testing - does not represent a language.
        */
        ELangTest = 0,

        /** UK English. */
        ELangEnglish = 1,

        /** French. */
        ELangFrench = 2,

        /** German. */
        ELangGerman = 3,

        /** Spanish. */
        ELangSpanish = 4,

        /** Italian. */
        ELangItalian = 5,

        /** Swedish. */
        ELangSwedish = 6,

        /** Danish. */
        ELangDanish = 7,

        /** Norwegian. */
        ELangNorwegian = 8,

        /** Finnish. */
        ELangFinnish = 9,

        /** American. */
        ELangAmerican = 10,

        /** Swiss French. */
        ELangSwissFrench = 11,

        /** Swiss German. */
        ELangSwissGerman = 12,

        /** Portuguese. */
        ELangPortuguese = 13,

        /** Turkish. */
        ELangTurkish = 14,

        /** Icelandic. */
        ELangIcelandic = 15,

        /** Russian. */
        ELangRussian = 16,

        /** Hungarian. */
        ELangHungarian = 17,

        /** Dutch. */
        ELangDutch = 18,

        /** Belgian Flemish. */
        ELangBelgianFlemish = 19,

        /** Australian English. */
        ELangAustralian = 20,

        /** Belgian French. */
        ELangBelgianFrench = 21,

        /** Austrian German. */
        ELangAustrian = 22,

        /** New Zealand English. */
        ELangNewZealand = 23,

        /** International French. */
        ELangInternationalFrench = 24,

        /** Czech. */
        ELangCzech = 25,

        /** Slovak. */
        ELangSlovak = 26,

        /** Polish. */
        ELangPolish = 27,

        /** Slovenian. */
        ELangSlovenian = 28,

        /** Taiwanese Chinese. */
        ELangTaiwanChinese = 29,

        /** Hong Kong Chinese. */
        ELangHongKongChinese = 30,

        /** Peoples Republic of China's Chinese. */
        ELangPrcChinese = 31,

        /** Japanese. */
        ELangJapanese = 32,

        /** Thai. */
        ELangThai = 33,

        /** Afrikaans. */
        ELangAfrikaans = 34,

        /** Albanian. */
        ELangAlbanian = 35,

        /** Amharic. */
        ELangAmharic = 36,

        /** Arabic. */
        ELangArabic = 37,

        /** Armenian. */
        ELangArmenian = 38,

        /** Tagalog. */
        ELangTagalog = 39,

        /** Belarussian. */
        ELangBelarussian = 40,

        /** Bengali. */
        ELangBengali = 41,

        /** Bulgarian. */
        ELangBulgarian = 42,

        /** Burmese. */
        ELangBurmese = 43,

        /** Catalan. */
        ELangCatalan = 44,

        /** Croatian. */
        ELangCroatian = 45,

        /** Canadian English. */
        ELangCanadianEnglish = 46,

        /** International English. */
        ELangInternationalEnglish = 47,

        /** South African English. */
        ELangSouthAfricanEnglish = 48,

        /** Estonian. */
        ELangEstonian = 49,

        /** Farsi. */
        ELangFarsi = 50,

        /** Canadian French. */
        ELangCanadianFrench = 51,

        /** Gaelic. */
        ELangScotsGaelic = 52,

        /** Georgian. */
        ELangGeorgian = 53,

        /** Greek. */
        ELangGreek = 54,

        /** Cyprus Greek. */
        ELangCyprusGreek = 55,

        /** Gujarati. */
        ELangGujarati = 56,

        /** Hebrew. */
        ELangHebrew = 57,

        /** Hindi. */
        ELangHindi = 58,

        /** Indonesian. */
        ELangIndonesian = 59,

        /** Irish. */
        ELangIrish = 60,

        /** Swiss Italian. */
        ELangSwissItalian = 61,

        /** Kannada. */
        ELangKannada = 62,

        /** Kazakh. */
        ELangKazakh = 63,

        /** Khmer. */
        ELangKhmer = 64,

        /** Korean. */
        ELangKorean = 65,

        /** Lao. */
        ELangLao = 66,

        /** Latvian. */
        ELangLatvian = 67,

        /** Lithuanian. */
        ELangLithuanian = 68,

        /** Macedonian. */
        ELangMacedonian = 69,

        /** Malay. */
        ELangMalay = 70,

        /** Malayalam. */
        ELangMalayalam = 71,

        /** Marathi. */
        ELangMarathi = 72,

        /** Moldovian. */
        ELangMoldavian = 73,

        /** Mongolian. */
        ELangMongolian = 74,

        /** Norwegian Nynorsk. */
        ELangNorwegianNynorsk = 75,

        /** Brazilian Portuguese. */
        ELangBrazilianPortuguese = 76,

        /** Punjabi. */
        ELangPunjabi = 77,

        /** Romanian. */
        ELangRomanian = 78,

        /** Serbian. */
        ELangSerbian = 79,

        /** Sinhalese. */
        ELangSinhalese = 80,

        /** Somali. */
        ELangSomali = 81,

        /** International Spanish. */
        ELangInternationalSpanish = 82,

        /** American Spanish. */
        ELangLatinAmericanSpanish = 83,

        /** Swahili. */
        ELangSwahili = 84,

        /** Finland Swedish. */
        ELangFinlandSwedish = 85,

        /** Reserved for future use. */
        ELangReserved1 = 86,		// reserved for future use

        /** Tamil. */
        ELangTamil = 87,

        /** Telugu. */
        ELangTelugu = 88,

        /** Tibetan. */
        ELangTibetan = 89,

        /** Tigrinya. */
        ELangTigrinya = 90,

        /** Cyprus Turkish. */
        ELangCyprusTurkish = 91,

        /** Turkmen. */
        ELangTurkmen = 92,

        /** Ukrainian. */
        ELangUkrainian = 93,

        /** Urdu. */
        ELangUrdu = 94,

        /** Reserved for future use. */
        ELangReserved2 = 95,		// reserved for future use

        /** Vietnamese. */
        ELangVietnamese = 96,

        /** Welsh. */
        ELangWelsh = 97,

        /** Zulu. */
        ELangZulu = 98,

        /**
        @deprecated
	
        Use of this value is deprecated.
        */
        ELangOther = 99,

        /**
        Indicates the final language in the language downgrade path.
	
        @see BaflUtils::NearestLanguageFile
        @see BaflUtils::GetDowngradePath
        */
        ELangNone = 0xFFFF, //up to 1023 languages
        ELangMaximum = ELangNone // This must always be equal to the last (largest) TLanguage enum.
    };
}
